using System;
using System.Collections.Generic;
using System.Text;

namespace Framework
{
    public interface ITriggerExecute
    {
        /// <summary>
        /// 定时执行
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        Boolean Execute(IServiceProvider serviceProvider);
        /// <summary>
        /// 相同ID不允许同时进入多个,如果想执行所有的触发器,设置Guid即可
        /// 必须设置,否则报错
        /// </summary>
        /// <returns></returns>
        String GetId();
    }
}
