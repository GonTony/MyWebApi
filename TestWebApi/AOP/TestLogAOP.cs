using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebApi.AOP
{
    public class TestLogAOP : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var dateInterceptor = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}" +
                $"当前执行的方法是：{invocation.Method.Name}" +
                $"参数是：{string.Join(",",invocation.Arguments.Select(o=>(o??"").ToString()).ToArray())} \r\n";
            invocation.Proceed();//被拦截方法执行完后 继续执行当前方法

            dateInterceptor += $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}\r 当前方法执行完毕,返回的结果为：{invocation.ReturnValue}";

            #region 输出到当前项目日志
            var path = Directory.GetCurrentDirectory() + @"\Log";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string fileName = path + $@"\InterceptLog-{DateTime.Now.ToString("yyyyMMddHHmmss")}.log";

            StreamWriter sw = File.AppendText(fileName);
            sw.WriteLine(dateInterceptor);
            sw.Close();
            #endregion
        }
    }
}
