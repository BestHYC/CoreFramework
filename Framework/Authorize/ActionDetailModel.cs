using System;
using System.Collections.Generic;
using System.Text;

namespace Framework
{
    /// <summary>
    /// 菜单权限中按钮权限,因为只是按钮,所以只涉及到路由 不包含class值
    /// 按钮的Id
    /// Name:英文名
    /// Description:描述
    /// Url:路由,一般默认是Name的值
    /// Menuid:菜单的Id,归属于哪个菜单
    /// 注意:默认Index是显示页面,或者
    /// </summary>
    public class ActionDetailModel
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public String Url { get; set; }
        public Int32 Menuid { get; set; }
    }
}
