using Dapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Framework.ORM.Dapper
{
    public static class DapperExtensionMiddlerware
    {
        public static IServiceCollection AddContext(this IServiceCollection services, Action<DapperDbContext> action)
        {
            //注入dapper操作的数据库
            action.Invoke(DapperDbContext.Instance);
            return services;
        }
    }
}
