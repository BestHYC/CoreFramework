using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.ORM.EntityFramework
{
    public interface ITrack
    {

        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime Createtime { get; set; }


        /// <summary>
        /// 更新时间
        /// </summary>
        DateTime Updatetime { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        Int32 Isdelete { get; set; }
        Int32 Isvalid { get; set; }

    }
}
