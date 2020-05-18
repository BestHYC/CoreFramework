using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.ORM.Dapper
{
    public interface IBatch
    {
        String SqlBuilder { get; set; }
        DynamicParameters DynamicParameters { get; set; }
    }
}
