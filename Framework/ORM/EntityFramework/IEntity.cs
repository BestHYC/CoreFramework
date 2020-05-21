using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.ORM.EntityFramework
{
    public interface IEntity : IEntity<Int32>
    {
    }

    public interface IEntity<TPrimaryKey> : ITrack
    {
        TPrimaryKey Id { get; set; }
    }
}
