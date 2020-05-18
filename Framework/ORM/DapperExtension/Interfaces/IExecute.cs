using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.ORM.Dapper
{
    public interface IExecute
    {
        IBatch End();
    }
}
