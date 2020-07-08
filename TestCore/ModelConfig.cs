using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestCore
{
    public class ModelConfig
    {
        public string DBConnectionString { get; set; }

        public AppSettings AppSettings { get; set; }
    }

    public class AppSettings
    {
        public RedisCache RedisCaching { get; set; }

        public SQLConfig SqlServer { get; set; }

        public DateTime Date { get; set; }

        public string Author { get; set; }
    }

    public class RedisCache
    {
        public bool Enabled { get; set; }
        public string ConnectionString { get; set; }

    }

    public class SQLConfig
    {
        public string SQLType { get; set; }
        public string SqlServerConnection { get; set; }
        public string ProviderName { get; set; }
    }
}
