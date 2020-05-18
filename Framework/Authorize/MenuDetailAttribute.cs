using System;
using System.Collections.Generic;
using System.Text;

namespace Framework
{
    /// <summary>
    /// 添加特性,只能用在Controller类上使用
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple =false)]
    public class MenuDetailAttribute : Attribute
    {
        private MenuDetailModel m_model;
        public MenuDetailModel GetModel()
        {
            return m_model;
        }
        public MenuDetailAttribute(MenuDetailModel model)
        {
            m_model = model;
        }
        /// <summary>
        /// 默认中 name为url的一部分,如HomeController,Name =home,url=home
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="classname"></param>
        /// <param name="pid"></param>
        /// <param name="des"></param>
        public MenuDetailAttribute(Int32 id, String name, String classname, Int32 pid, String des = "") : this(id, name, classname, pid, des, name)
        {

        }
        public MenuDetailAttribute(Int32 id, String name, String classname, Int32 pid, String des, String url)
        {
            m_model = new MenuDetailModel();
            m_model.Id = id;
            m_model.Name = name;
            m_model.Description = des;
            m_model.Url = url;
            m_model.Classname = classname;
            m_model.Pid = pid;
        }
        public override bool Equals(object obj)
        {
            MenuDetailAttribute attribute = obj as MenuDetailAttribute;
            return Equals(attribute);
        }
        public Boolean Equals(MenuDetailAttribute attribute)
        {
            if (ReferenceEquals(attribute, null)) return false;
            return attribute.GetModel().Id == this.m_model.Id;
        }
        public override int GetHashCode()
        {
            return this.m_model.Id.GetHashCode();
        }
    }
}
