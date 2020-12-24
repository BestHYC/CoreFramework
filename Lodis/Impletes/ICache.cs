using Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Lodis
{
    public interface ICache
    {
        Boolean Remove();
        Boolean Add();
        Boolean UpdateOrAdd();
    }
}
