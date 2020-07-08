using Microsoft.Extensions.Configuration;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration.Json;

namespace Test.Core.Repository.SqlSugar
{
   public class DbContext
    {
        //ModelConfig config;
        private string dbConnectionString;
        public SqlSugarClient db_;
        public DbContext()
        {
            GetDBConnectionString();
            
            if (!string.IsNullOrWhiteSpace(dbConnectionString))
            {
                db_ = new SqlSugarClient(new ConnectionConfig (){
                    ConnectionString=dbConnectionString,
                    DbType=DbType.MySql,
                    IsAutoCloseConnection=true,
                    InitKeyType=InitKeyType.Attribute
                });
            }
            else {
                throw new Exception("数据库连接字符串为空");
            }
        }

        public void GetDBConnectionString() {
            var config = new ConfigurationBuilder()
                
               .AddJsonFile("ConfigApp.json")               
               .Build();
            dbConnectionString = config.GetSection("AppSettings:SqlServer:SqlServerConnection").Value;
        }

        #region 根据实体类生成数据库表
        /// <summary>
        /// 功能描述:根据实体类生成数据库表
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="blnBackupTable">是否备份表</param>
        /// <param name="lstEntitys">指定的实体</param>
        public void CreateTableByEntity<T>(bool blnBackupTable, params T[] lstEntitys) where T : class, new()
        {
            Type[] lstTypes = null;
            if (lstEntitys != null)
            {
                lstTypes = new Type[lstEntitys.Length];
                for (int i = 0; i < lstEntitys.Length; i++)
                {
                    T t = lstEntitys[i];
                    lstTypes[i] = typeof(T);
                }
            }
            CreateTableByEntity(blnBackupTable, lstTypes);
        }

        /// <summary>
        /// 功能描述:根据实体类生成数据库表
        /// 作　　者:Blog.Core
        /// </summary>
        /// <param name="blnBackupTable">是否备份表</param>
        /// <param name="lstEntitys">指定的实体</param>
        public void CreateTableByEntity(bool blnBackupTable, params Type[] lstEntitys)
        {
            if (blnBackupTable)
            {
                db_.CodeFirst.BackupTable().InitTables(lstEntitys); //change entity backupTable            
            }
            else
            {
                db_.CodeFirst.InitTables(lstEntitys);
            }
        }
        #endregion

        public SimpleClient<T> GetEntityDB<T>() where T : class, new()
        {
            return new SimpleClient<T>(db_);
        }

        public SimpleClient<T> GetEntityDB<T>(SqlSugarClient db) where T : class, new()
        {
            return new SimpleClient<T>(db);
        }

        public static DbContext GetDbContext() {
            return new DbContext();
        } 

    }
}
