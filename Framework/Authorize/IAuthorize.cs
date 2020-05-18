using System;
using System.Collections.Generic;
using System.Text;

namespace Framework
{
    /// <summary>
    /// 用作权限反射中的统一标识,只有继承这个接口的才会出现在菜单中
    /// 并不是继承Controller就能够展现,请注意
    /// </summary>
    public interface IAuthorize
    {
    }
}
