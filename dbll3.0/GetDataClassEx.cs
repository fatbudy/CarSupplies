//#define INTERFACE_ON 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Data.Common;
using dbll3.LinkManage;
using idblink;

namespace dbll3
{
    public partial interface IGetDataClass
    {
        string LinkSummary { get; }
        bool Load_ILinkStringCreate();
        bool Load_ILinkStringCreate(string setfilename);
        bool Save_ILinkStringCreate();
        /// <summary>
        /// 保存数据库连接配置信息
        /// </summary>
        /// <param name="filename">保存配置数据的文件名</param>
        /// <returns></returns>
        bool Save_ILinkStringCreate(string filename);
        ILinkStringCreate LinkStringCreate { get; set; }
    }

    public sealed partial class GetDataClass : IGetDataClass
    {
        private ILinkStringCreate _ilsc = null;
        public ILinkStringCreate LinkStringCreate
        {
            get
            {
                return _ilsc;
            }
            set
            {
                if (_ilsc != value && LinkManagementClass.Default.Items.ContainsKey(value.AssemblyName ))
                {
                    if (!value.Checked())
                    {
                        return ;
                    }
                    _ilsc = value;

                    //取得数据连接实例
                    this._Idbcon = _ilsc.GetDbConnection();
                    //取得DBCommand实例
                    this._Idbcmd = _ilsc.GetDbCommand();

                    this._DA = _ilsc.GetDbDataAdapter(); ;
                    this._DA.SelectCommand = this._Idbcmd;

                    this._linksummary = _ilsc.LinkSummary;
                }
            }
        }
        private string _linksummary = string.Empty;
        public string LinkSummary { get { return _linksummary; } }
        public bool Load_ILinkStringCreate()
        {
            if (File.Exists(LinkManagementClass.DefaultDBSettingFilename))
            {
                return Load_ILinkStringCreate(LinkManagementClass.DefaultDBSettingFilename);
            }
            return false;
        }
        /// <summary>
        /// 加载数据库配置信息
        /// </summary>
        /// <param name="setfilename">配置文件名</param>
        /// <returns></returns>
        public bool Load_ILinkStringCreate(string setfilename)
        {
            if (!File.Exists(setfilename))
            {
                return false;
            }

            string filenamea = string.Empty, filenameb = string.Empty;
            Assembly a = null;
            Type tc = null;
#if(INTERFACE_ON ==false ) 
            MethodInfo mi_sub_invoke = null;
            object ma = null;      
            bool result = false;    //发射执行的结果
#endif
            try
            {
                FileInfo fi = new FileInfo(setfilename);
                using (BinaryReader br = new BinaryReader(new FileStream(setfilename, FileMode.Open, FileAccess.Read)))
                {
                    int len = 0;    //待读取的文件长度
                    byte[] value = null;    //缓冲数据

                    long index = 0;
                    string nameSpacestr = null; //等待处理的命名空间名.类名

                    //读取数据库库设置类库文件名
                    //读取反射类全名类型
                    len = br.ReadInt32();
                    value = br.ReadBytes(len);
                    nameSpacestr = Encoding.UTF8.GetString(value);
                    if (LinkManagementClass.Default.Items.ContainsKey(nameSpacestr) && File.Exists(LinkManagementClass.Default.Items[nameSpacestr].AssemblyFileName))
                    {
                        a = Assembly.LoadFile(LinkManagementClass.Default.Items[nameSpacestr].AssemblyFileName);
                    }
                    else
                    {
                        return false;
                    }

                    len = br.ReadInt32();// 读取配置存储信息长度
                    index = br.BaseStream.Position; //记录当前读取位置，

                    //实施反射实例创建
                    tc = a.GetType(nameSpacestr);
#if(INTERFACE_ON )
                    _ilsc = a.CreateInstance(nameSpacestr) as ILinkStringCreate;

                    if (!_ilsc.Load(br.BaseStream, len))
                    {
                        return false;
                    }

                    if (!_ilsc.Checked())
                    {
                        return false;
                    }

                    //取得数据连接实例
                    this._Idbcon = _ilsc.GetDbConnection();
                    //取得DBCommand实例
                    this._Idbcmd = _ilsc.GetDbCommand();

                    this._DA = _ilsc.GetDbDataAdapter(); ;
                    this._DA.SelectCommand = this._Idbcmd;

                    this._linksummary = _ilsc.LinkSummary;
                    return true;
#else
                   ma = a.CreateInstance(nameSpacestr);
                    _ilsc =(ILinkStringCreate ) ma;
                    //建立配置读取反射
                    mi_sub_invoke = tc.GetMethod("Load");
                    //执行，取得读取结果
                    result = (bool)mi_sub_invoke.Invoke(ma, new object[] { br.BaseStream, len });
                    if (!result)
                        return false;//信息读取失败

                    mi_sub_invoke = tc.GetMethod("Checked");
                    result = (bool)mi_sub_invoke.Invoke(ma, new object[] { });

                    if (!result)
                        return false;

                    //取得数据连接实例
                    mi_sub_invoke = tc.GetMethod("GetDbConnection");
                    this._Idbcon = (DbConnection)mi_sub_invoke.Invoke(ma, new object[] { });
                    //取得DBCommand实例
                    mi_sub_invoke = tc.GetMethod("GetDbCommand");
                    this._Idbcmd = (DbCommand)mi_sub_invoke.Invoke(ma, new object[] { });

                    mi_sub_invoke = tc.GetMethod("GetDbDataAdapter");
                    this._DA = (DbDataAdapter)mi_sub_invoke.Invoke(ma, new object[] { });
                    this._DA.SelectCommand = _Idbcmd;

                    mi_sub_invoke = tc.GetMethod("GetDbCommandBuilder");
                    this._ldbcbuid  = (DbCommandBuilder )mi_sub_invoke.Invoke(ma, new object[] { });

                    mi_sub_invoke = tc.GetMethod("get_LinkSummary");
                    this._linksummary = (string)mi_sub_invoke.Invoke(ma, new object[] { });
                    return result;
#endif
                }//end binaryreader
            }
            catch
            {
                this._DA = null;
                this._Idbcmd = null;
                this._Idbcon = null;
            }
            finally
            {
                a = null;
#if (INTERFACE_ON ==false)     
                mi_sub_invoke = null;
                ma = null;
#endif
                tc = null;
            }
            return false;
        }

        public bool Save_ILinkStringCreate()
        {
            return Save_ILinkStringCreate(LinkManagementClass.DefaultDBSettingFilename );
        }
        /// <summary>
        /// 保存数据库连接配置信息
        /// </summary>
        /// <param name="setfilename">保存配置数据的文件名</param>
        /// <returns></returns>
        public bool Save_ILinkStringCreate(string filename)
        {
            if (string.IsNullOrEmpty(filename ) || string.IsNullOrWhiteSpace(filename )  )
            {
                return false;
            }
            if (_ilsc == null)
            {
                return false;
            }

            try
            {
                using (BinaryWriter bw = new BinaryWriter(new FileStream(filename, FileMode.Create, FileAccess.Write)))
                {
                    byte[] value;
                    long index = 0;
                    int len = 0;
                    //写入库文件名
                    //写入库中引用的命名空间及实例名
                    value = Encoding.UTF8.GetBytes(this._ilsc.AssemblyName );
                    bw.Write(value.Length);
                    bw.Write(value);
                    //记下当前流的位置
                    index = bw.BaseStream.Position;
                    //写入当前流位置的占位空间
                    bw.Write(len);

                    //开始写入配置数据
                    if (!_ilsc.Save(bw.BaseStream))
                    {
                        return false;

                    }

                    //计算配置数据占用的空间长度
                    len = (int)(bw.BaseStream.Length - index - 4);
                    //移动数据流写入位置至index处
                    bw.BaseStream.Seek(index, SeekOrigin.Begin);
                    //写入数据
                    bw.Write(len);
                    bw.Flush();
                }
                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}
