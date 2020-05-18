using System;
using System.Collections.Generic;
using System.Text;

namespace Framework
{
    /// <summary>
    /// 菜单详细
    /// 菜单分为几个部分 
    /// 当前菜单的id值,
    /// 名称:菜单英文名
    /// 路由url,
    /// 描述:菜单中文名
    /// 当前菜单中显示的class小图标
    /// 父级菜单id,可以串联
    /// </summary>
    public class MenuDetailModel
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }
        public String Namespace { get; set; }
        public String Description { get; set; }
        public String Url { get; set; }
        public String Classname { get; set; }
        public Int32 Pid { get; set; }
    }
}
