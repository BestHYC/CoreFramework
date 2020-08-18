using System;
using System.ComponentModel;

namespace Framework
{
    /// <summary>
    /// 方法特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple =false)]
    public class ActionDetailAttribute : Attribute
    {
        public ActionDetailModel Model { get; }
        public ActionDetailAttribute(ActionDetailModel detail)
        {
            Model = detail;
        }
        public ActionDetailAttribute(ActionDetailEnum info):this((Int32)info, info.GetName(), info.GetAttribute<DescriptionAttribute>()?.Description)
        {

        }
        /// <summary>
        /// 默认是name=url 比如 Home/index中的Index,那么name = Index url= index
        /// 注意:在此处的id是可以重复的,比如 Home/Index 与Login/index中的Index都是1,
        /// 通过Id与menuid组成的值为唯一值
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="menuid"></param>
        /// <param name="des"></param>
        public ActionDetailAttribute(Int32 id, String name, String des = "") : this(id, name, des, name)
        {
            
        }
        public ActionDetailAttribute(ActionId id, String name, String des = "") : this((Int32)id, name, des, name)
        {

        }
        public ActionDetailAttribute(ActionId id, String name, String des, String url) : this((Int32)id, name, des, url)
        {

        }
        public ActionDetailAttribute(Int32 id, String name, String des, String url)
        {
            this.Model = new ActionDetailModel();
            this.Model.Id = id;
            this.Model.Name = name;
            this.Model.Description = des;
            this.Model.Url = url;
        }
    }
    public enum ActionDetailEnum
    {
        /// <summary>
        /// 默认页面展示
        /// </summary>
        [Description("首页")]
        Index =1,
        /// <summary>
        /// 新增页面
        /// </summary>
        [Description("新增")]
        Create =2,
        /// <summary>
        /// 修改功能
        /// </summary>
        [Description("修改")]
        Update =3,
        /// <summary>
        /// 删除
        /// </summary>
        [Description("删除")]
        Delete =4,
        /// <summary>
        /// 详情
        /// </summary>
        [Description("详情")]
        Detail =5,
    }
    /// <summary>
    /// 菜单Id
    /// </summary>
    public enum ActionId
    {
        None = 0,
        First = 1, Second, Third, Forth, Fifth,
        Sixth, Seventh, Eighth, Ninth, Tenth, 
        Eleventh, Twelfth, Thirteenth, Fourteenth, 
        Fifteenth, Sixteenth
    }

}
