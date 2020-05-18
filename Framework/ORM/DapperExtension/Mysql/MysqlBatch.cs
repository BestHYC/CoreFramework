using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.ORM.Dapper
{
    public class MysqlBatch : IBatch
    {
        public String SqlBuilder { get; set; }
        public DynamicParameters DynamicParameters { get; set; }
    }
}
