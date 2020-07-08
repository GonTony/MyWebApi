using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TestCore;
using TestCore.Redis;
using TestWebApi.AOP;

namespace TestWebApi
{
    public class Startup
    {
        public AppContextWA AppContextWebApi = null;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("ConfigApp.json")
                .AddEnvironmentVariables()
                .Build();
            //services.AddSingleton<IConfigurationRoot>(config);
            // services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddMvc()
              .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            #region Swagger
            string CurrBasePath = AppContext.BaseDirectory;
            string xmlInterfacePath = Path.Combine(CurrBasePath, "TestWebApi.xml");
            string xmlModelPath = Path.Combine(CurrBasePath,"Test.Core.Model.xml");
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version="v0.1.0",
                    Title="MyWebApi",
                    Description="接口说明",
                    //TermsOfService=Uri
                    Contact=new Microsoft.OpenApi.Models.OpenApiContact { Name="MyWebApi",Email="xxxx"}
                });
                c.IncludeXmlComments(xmlInterfacePath, true);//true 为显示controller的注释
                c.IncludeXmlComments(xmlModelPath, true);
            });
            #endregion


            services.AddScoped<ICaching, MeMoryCache>();

            ModelConfig configModel = new ModelConfig();
            config.Bind(configModel);
            services.AddSingleton(configModel);
            //services.AddSingleton<IRedisManager, RedisManager>();
            AppContextWebApi = new AppContextWA(services);
            //AppContextWebApi.DBConString = configModel.DBConnectionString;
            AppContextWebApi.PluginFactory = new DefaultPluginFactory();
            string pluginPath = Path.Combine(AppContext.BaseDirectory, "Plugin");
            AppContextWebApi.PluginFactory.Load(pluginPath);//加载插件
            AppContextWebApi.PluginFactory.Init(AppContextWebApi);

            #region 将其他程序集中的控制器加入ApplicationPartManager
            var apppart = services.FirstOrDefault(x => x.ServiceType == typeof(ApplicationPartManager))?.ImplementationInstance;
            if (apppart != null)
            {
                ApplicationPartManager apm = apppart as ApplicationPartManager;
                //所有附件程序集
                //AppContextWA ac = AppContextBase.Current as AppContextWA;
                AppContextWebApi.PluginFactory.LoaderAssembly.ForEach((a) =>
                {
                    apm.ApplicationParts.Add(new AssemblyPart(a));
                });
            }
            #endregion

            var builder = new ContainerBuilder();
            services.AddSingleton<IRedisManager, RedisManager>();
            builder.RegisterType<TestLogAOP>();
            builder.RegisterType<TestCacheAOP>();
            string basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;
            string assemblyPath = Path.Combine(basePath, "Test.Core.Service.dll");
            var assemblyService = Assembly.LoadFile(assemblyPath);
            //var assemblyService = Assembly.Load("Test.Core.Server");
            builder.RegisterAssemblyTypes(assemblyService)
                .AsImplementedInterfaces()//指定已扫描程序集中的类型注册为提供所有其实现的接口。
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()//对目标类型启用接口拦截。拦截器将被确定，通过在类或接口上截取属性, 或添加
                .InterceptedBy(typeof(TestLogAOP));//替换并指定拦截器
            string repositoryAssemblyPath = Path.Combine(basePath, "Test.Core.Repository.dll");//加载Test.Core.Repository.dll,Test.Core.Service.dll 是为了层级解耦 不解耦的话可直接对以上两个程序集添加引用
            //var assemblyRepoistory = Assembly.Load("Test.Core.Repository");
            var assemblyRepoistory = Assembly.LoadFile(repositoryAssemblyPath);
            builder.RegisterAssemblyTypes(assemblyRepoistory)
                .AsImplementedInterfaces()//指定已扫描程序集中的类型注册为提供所有其实现的接口。
                .InstancePerLifetimeScope()//？
                .EnableInterfaceInterceptors()//指明在接口处拦截
                .InterceptedBy(typeof(TestCacheAOP));
            //builder.RegisterType<RedisManager>().As<IRedisManager>().InstancePerLifetimeScope();//等效为 services.AddTransient<IRedisManager, RedisManager>();   InstancePerLifetimeScope：同一个Lifetime生成的对象是同一个实例
            //builder.RegisterType<RedisManager>().As<IRedisManager>().SingleInstance();//等效为 services.AddSingleton<IRedisManager, RedisManager>();
            //builder.RegisterType<RedisManager>().As<IRedisManager>(); //等效为 services.AddScoped<IRedisManager, RedisManager>();     
            builder.Populate(services);
            var applicationContainer = builder.Build();
          

            //services.AddToolDefined();
            return new AutofacServiceProvider(applicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseCors(options =>
            {
                options.AllowAnyHeader();
                options.AllowAnyMethod();
                options.AllowAnyOrigin();
                options.AllowCredentials();
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Apihelp V1");
            });

            app.UseHttpsRedirection();
            app.UseMvc(
                routes => {
                    routes.MapRoute("test", "{controller=test}/{action}/{id?}");
                    routes.MapRoute("default", "{controller}/{action}/{id?}");
                   
                }
                );
        }
    }
}
