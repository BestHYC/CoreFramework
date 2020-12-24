using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Framework
{
    /// <summary>
    /// 注意:默认最顶级节点是0
    /// </summary>
    public class MenuAuthorizeHelper
    {
        public static MenuAuthorizeHelper Instance = new MenuAuthorizeHelper();
        private readonly Object m_lock = new Object();
        /// <summary>
        /// 保存Controller的fullname对应关系
        /// </summary>
        private Dictionary<String, MenuDetailModel> m_controller;
        /// <summary>
        /// 保存Controller上特性Id对应关系,注意,即使是相同名字的Controller,在Id中也不允许相同
        /// 如HomeController与区域中的HomeController,其Id也不允许相同
        /// </summary>
        private Dictionary<Int32, MenuDetailModel> m_menu;
        /// <summary>
        /// 父对子的关联关系
        /// </summary>
        private Dictionary<Int32, HashSet<Int32>> m_menupid;
        /// <summary>
        /// Controller中方法的集合,由于一个Controller中方法比较少,
        /// 所以直接遍历查询即可,不需要重新添加对象
        /// </summary>
        private Dictionary<Int32, HashSet<ActionDetailModel>> m_menutoaction;
        private MenuAuthorizeHelper()
        {
            m_menu = new Dictionary<Int32, MenuDetailModel>();
            m_menupid = new Dictionary<Int32, HashSet<Int32>>();
            m_controller = new Dictionary<String, MenuDetailModel>();
            m_menutoaction = new Dictionary<Int32, HashSet<ActionDetailModel>>();
        }
        /// <summary>
        /// 默认顶级菜单为0
        /// </summary>
        /// <param name="pmenuid"></param>
        /// <returns></returns>
        public IEnumerable<MenuDetailModel> GetChildrenMenu(Int32 pmenuid=0)
        {
            List<MenuDetailModel> list = new List<MenuDetailModel>();
            if (m_menupid.ContainsKey(pmenuid))
            {
                foreach(var item in m_menupid[pmenuid])
                {
                    if (m_menu.ContainsKey(item))
                    {
                        list.Add(m_menu[item]);
                    }
                }
            }
            return list;
        }
        public IEnumerable<MenuDetailModel> GetAllMenuDetail()
        {
            return m_controller.Values;
        }
        public HashSet<String> GetAllMenuID()
        {
            HashSet<String> hs = new HashSet<String>();
            foreach(var i in m_controller.Values)
            {
                hs.Add(i.Id + "");
            }
            foreach(var i in m_menutoaction.Values)
            {
                foreach(var ii in i)
                {
                    hs.Add(ConvertToId(ii));
                }
            }
            return hs;
        }
        public IEnumerable<ActionDetailModel> GetAction(Int32 menuid)
        {
            if (m_menutoaction.ContainsKey(menuid)) return m_menutoaction[menuid];
            return null;
        }
        public String ConvertRouteToId(String space, String controller,String index)
        {
            String fullname = $"{space}.{controller}"; ;
            if (!controller.EndsWith("Controller"))
            {
                fullname += "Controller";
            }
            MenuDetailModel menu = m_controller[fullname];
            ActionDetailModel action = m_menutoaction[Convert.ToInt32(menu.Id)].FirstOrDefault(item => item.Name == index);
            return ConvertToId(action);
        }
        public String ConvertToId(ActionDetailModel detail)
        {
            return $"{detail.Menuid}!&{detail.Id}";
        }
        public Tuple<MenuDetailModel, ActionDetailModel> IdToMenu(String id)
        {
            String[] arr = id.Split(new String[] { "!&" }, StringSplitOptions.RemoveEmptyEntries);
            MenuDetailModel menu = m_menu[Convert.ToInt32(arr[0])];
            ActionDetailModel action = m_menutoaction[Convert.ToInt32(arr[0])].FirstOrDefault(item => item.Id == Convert.ToInt32(arr[1]));
            return new Tuple<MenuDetailModel, ActionDetailModel>(menu, action);
        }
        /// <summary>
        /// 遍历当前AppDomain中所有的程序集
        /// </summary>
        public void SetCurrentAssembly()
        {
            lock (m_lock)
            {
                Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (var item in assemblies)
                {
                    SetCurrentAssembly(item);
                }
            }
        }
        /// <summary>
        /// 获取单个程序集中的类型
        /// 将继承IAuthorize的所有的类型反射出来,条件是 
        /// 一个是public 
        /// 一个是 包含MenuDetailAttribute特性
        /// </summary>
        /// <param name="assembly"></param>
        public void SetCurrentAssembly(Assembly assembly)
        {
            lock (m_lock)
            {
                var all = from t in assembly.ExportedTypes
                          where typeof(IAuthorize).GetTypeInfo()
                          .IsAssignableFrom(t.GetTypeInfo()) && t.IsPublic
                          select t;
                if (all != null)
                {
                    foreach (var item in all)
                    {
                        SetMenuDetail(item);
                    }
                }
            }
        }
        private void SetMenuDetail(Type t)
        {
            MenuDetailAttribute menuattr = t?.GetCustomAttribute<MenuDetailAttribute>();
            MenuDetailModel menu = menuattr?.GetModel();
            if (menu == null) return;
            menu.Namespace = t.Namespace;
            if (m_menu.ContainsKey(menu.Id))
            {
                if (m_menu[menu.Id] == menu) return;
                throw new Exception($"{menu.Name}的Id值与{m_menu[menu.Id].Name}中的Id值重复");
            }
            m_controller.Add(t.FullName, menu);
            m_menu.Add(menu.Id, menu);
            if (!m_menupid.TryGetValue(menu.Pid, out HashSet<int> pids))
            {
                pids = new HashSet<Int32>();
                m_menupid.Add(menu.Pid, pids);
            }
            pids.Add(menu.Id);
            SetActionDetail(t, menu.Id);
        }
        private void SetActionDetail(Type t, Int32 menuid)
        {
            if (!m_menutoaction.TryGetValue(menuid, out HashSet<ActionDetailModel> actions))
            {
                actions = new HashSet<ActionDetailModel>();
                m_menutoaction.Add(menuid, actions);
            }
            foreach (var item in t.GetMethods())
            {
                ActionDetailAttribute attribute = item.GetCustomAttribute<ActionDetailAttribute>();
                if (attribute == null) continue;
                attribute.Model.Menuid = menuid;
                if (actions.Contains(attribute.Model))
                {
                    throw new Exception($"{attribute.Model.Name}的Id已经有重复,请重新选择");
                }
                actions.Add(attribute.Model);
            }
        }
    }
}
