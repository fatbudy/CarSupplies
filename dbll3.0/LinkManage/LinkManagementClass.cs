using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace dbll3.LinkManage
{
    public sealed class LinkManagementClass
    {
        private PluginItems _listpis = new PluginItems();
        /// <summary>
        /// 搜索文件夹列表
        /// </summary>
        private List<string> _listsp = new List<string>();// 搜索文件夹列表

        public PluginItems Items { get { return _listpis; } }
        /// <summary>
        /// 搜索文件夹列表
        /// </summary>
        public List<string> SearchPath { get { return _listsp; } }// 搜索文件夹列表
        private TypeFilter _mytype = new TypeFilter(myInterfaceFilter);

        private Assembly _assembly = null;
        private MethodInfo _methodinfo_invoke = null;
        private Type _type_client = null;
        private object _client_Instance = null;
        private Type[] _myInterfaces = null;
        private string _myInterface = "";
        /// <summary>
        /// 待搜索的接口类型
        /// </summary>
        public string SearchInterface// 待搜索的接口类型
        {
            get { return _myInterface; }
            set
            {
                if (!_myInterface.Equals(value))
                {
                    _myInterface = value;
                }
            }
        }
        public bool VaildItems(bool renoVaild)
        {

            return false;
        }
        /// <summary>
        /// 搜索指定目录是否包含plugin库文件
        /// </summary>
        /// <param name="path">指定路径，如果为空，重新搜索当前列表内所有路径</param>
        /// <param name="addon">是否添加至列表</param>
        /// <returns></returns>
        public bool Search(string path = "", bool addon = true)// 搜索指定目录是否包含plugin库文件
        {
            bool ischecked = false;
            if (!string.IsNullOrWhiteSpace(path))
            {
                if (!SearchPath.Contains(path))
                {
                    searchFileInPath(path);
                    if (addon)  //将搜索的目录添加至搜索列表
                    {
                        SearchPath.Add(path);
                    }
                }
            }
            else
            {
                foreach (string sp in SearchPath)
                {
                    searchFileInPath(sp);
                }
            }

            return ischecked;
        }
        private void searchFileInPath(string path)
        {
            if (Directory.Exists(path))
            {
                foreach (string fn in Directory.GetFiles(path, "*.dll", SearchOption.AllDirectories))
                {
                    vaildDllfile(fn);
                }
                //_assembly = null;
                //_methodinfo_invoke = null;
                //_type_client = null;
                //_client_Instance = null;
                //_myInterfaces = null;
            }
        }
        private void vaildDllfile(string filename)
        {
            string assemblyName = "";
            try
            {
                _assembly = null;
                _assembly = Assembly.LoadFile(filename);
                foreach (Type t in _assembly.GetTypes())
                {
                    assemblyName = t.Namespace + "." + t.Name;

                    _myInterfaces = t.FindInterfaces(_mytype, _myInterface);//t.Name + "." + 
                    if (_myInterfaces.Length > 0)   //找到约定的接口类ILinkStringCreate
                    {
                        _type_client = _assembly.GetType(assemblyName);
                        _client_Instance = _assembly.CreateInstance(assemblyName);
                        _methodinfo_invoke = _type_client.GetMethod("get_Summary");
                        try
                        {
                            string tmp_str = (string)_methodinfo_invoke.Invoke(_client_Instance, new object[] { });
                            if (t.Namespace.Equals(tmp_str))    //Summary属性按约定设置
                            {
                                //将包含约定接口类的文件及命名信息记录添加至列表
                                _listpis.Add(assemblyName, new PluginItemClass()
                                {
                                    AssemblyFileName = filename,
                                    AssemblyName = assemblyName,
                                    Version = t.FullName
                                });
                            }
                        }
                        catch
                        {
                            //如错误，继续比对下一个类别是否是。。。
                        }
                    }// end if by _myInterfaces.Length
                }   //end foreach

            }
            catch
            {
                //无法加载文件时错误处理
            }

        }

        ~LinkManagementClass()
        {
            if (_assembly != null)
            {
                GC.SuppressFinalize(_assembly);
            }
            if (_methodinfo_invoke != null)
            {
                GC.SuppressFinalize(_methodinfo_invoke);
            }
            if (_client_Instance != null)
            {
                GC.SuppressFinalize(_client_Instance);
            }
        }
        private static bool myInterfaceFilter(Type typeObj, object criteriObj)
        {
            if (typeObj.ToString() == criteriObj.ToString())
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 默认的数据库配置文件名
        /// </summary>
        private static string _default_dbsetting_filename = string.Format("{0}\\databasesetting.db", AppDomain.CurrentDomain.BaseDirectory);
        public static string DefaultDBSettingFilename
        {
            get
            {
                return _default_dbsetting_filename;
            }
        }
        private static LinkManagementClass __LMC = null;
        public static LinkManagementClass Default
        {
            get
            {
                if (__LMC == null)
                {
                    __LMC = new LinkManagementClass();
                    __LMC.SearchInterface = "idblink.ILinkStringCreate";
                    __LMC.Search(AppDomain.CurrentDomain.BaseDirectory);
                }
                return __LMC;
            }
        }
    }
}
