using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using idblink;
using System.Data.OleDb;
using System.Data.Common;
using System.Data;
using System.IO;
namespace OutPutData
{
    public class ExcelClass : ILinkStringCreate
    {
        public ExcelClass(string filename)
        {
            _pv2003 = checkACE();
            if (_pv2003 && filename.ToLower().EndsWith(".xlsx") )
            {
                _databaseName = filename.Substring(0, filename.Length - 1);
            }
            else
            {
                _databaseName = filename;
            }
            create_link_string();
        }
        public ExcelClass(string filename, bool past2003 = false)
        { 
            _pv2003 = past2003 ? checkACE() : false;
            _databaseName = filename;

            create_link_string();
        }

        public ExcelClass(string filename,string user,string pwd, bool past2003 = false)
        {
            this._databaseName = filename;
            this._user = user;
            this._password = pwd;
            _pv2003 = past2003 ? checkACE() : false;
            create_link_string();
        }

        private bool _pv2003;
        /// <summary>
        /// 是否是高于2003版本的Excel
        /// </summary>
        /// <remarks>
        /// 高于2003版本的需要安装ACE高版本的ODBC驱动，当前版本内不考虑。
        /// ACE：Provider=Microsoft.ACE.OLEDB.12.0;
        /// http://www.microsoft.com/zh-CN/download/details.aspx?id=13255 2010
        /// http://www.microsoft.com/zh-cn/download/details.aspx?id=23734 2007
        /// </remarks>
        public bool Past2003Ver
        {
            get { return _pv2003; }
            set
            {
                _pv2003 = value ? checkACE() : false;
            }
        }
        /// <summary>
        /// 检查ACE.OLEDB是否安装，已安装，返回真，否则假
        /// </summary>
        /// <returns></returns>
        public static bool checkACE()   // 检查ACE.OLEDB是否安装，已安装，返回真，否则假
        {
            String sConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}tmpcheckexcel.xlsx;Extended Properties=\"Excel 12.0 Xml;HDR=YES\";",
                    AppDomain.CurrentDomain.SetupInformation.ApplicationBase);
            try
            {
                using (OleDbConnection ole_conn = new OleDbConnection(sConnectionString))
                {
                    ole_conn.Open();
                }
                //操作成功后临时删除文件
                return true;
            }
            catch
            {
                return false;
            }
        }
        private static bool _only_ver80 = false;
        public static bool OnlyVer80
        {
            get { return _only_ver80; }
            set { _only_ver80 = value; }
        }
        public static string GetExcelFilter()
        {
            return !_only_ver80 && checkACE() ? "Excel 97~2003文件(*.xls)|*.xls|Excel 文件(*.xlsx)|*.xlsx" : "Excel 97~2003文件(*.xls)|*.xls";
        }


        private string _databaseName = string.Empty;
        private string _user = string.Empty;
        private string _servername = string.Empty;
        private string _password = string.Empty;
        private string _linkstring = string.Empty;
        private const string _summary = "Excel";
        private const string _assemblyName = "dbll3.OutPutData.ExcelClass"; //这里可以给改成自动生成的变量信息


        public string DatabaseName
        {
            get
            {
                return _databaseName ;
            }
            set
            {
                if (!_databaseName.Equals(value))
                {
                    _databaseName = value;
                    create_link_string();
                }
            }
        }

        public string User
        {
            get
            {
                return _user ;
            }
            set
            {
                if (!_user.Equals(value))
                {
                    _user = value;
                    create_link_string();
                }
            }
        }

        public string ServerName
        {
            get
            {
                return _servername ;
            }
            set
            {
                if (!_servername.Equals(value))
                {
                    _servername = value;
                    create_link_string();
                }
            }
        }

        public string Password
        {
            get
            {
                return _password ;
            }
            set
            {
                if (!_password.Equals(value))
                {
                    _password = value;
                    create_link_string();
                }
            }
        }

        public string LinkString
        {
            get { 
                return _linkstring ;
            }
        }

        /// <summary>
        /// 生成链接字符串信息
        /// </summary>
        private void create_link_string()
        {
            StringBuilder sb = new StringBuilder();
            if (_pv2003)
            {
                string ver = "5.0";
                if (_databaseName.EndsWith(".xlsx"))
                {
                    ver = "\"Excel 12.0 Xml;HDR=Yes\"";
                }
                else
                {
                    ver = "\"Excel 8.0;HDR=Yes\"";
                }
                sb.AppendFormat("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties={1}", _databaseName, ver);
            }
            else
            {
                sb.AppendFormat("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended properties=\"Excel 5.0;HDR=Yes;IMEX=1\"", _databaseName);
            }

            _linkstring = sb.ToString();
        }
        public bool CreateLinkData(ref DbConnection conn, ref System.Data.Common.DbCommand cmd, ref System.Data.Common.DbDataAdapter dataAdapter, ref DbCommandBuilder dbcb)
        {
            ///返回值
            bool result = false;    //返回值
                conn =new  OleDbConnection(_linkstring);
                cmd = new OleDbCommand();
                cmd.Connection = new OleDbConnection(_linkstring);
                cmd.CommandType = CommandType.Text;
                dataAdapter = new OleDbDataAdapter();
                dbcb = new OleDbCommandBuilder();
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                System.Threading.Thread.Sleep(10);
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public bool GetDatareader(ref System.Data.Common.DbDataReader reader,string sqlQuery)
        {            
            DbCommand dbcmd = null;
            bool result = false;
            try
            {
                dbcmd = new OleDbCommand(sqlQuery, new OleDbConnection(_linkstring));

                if (dbcmd.Connection  .State != ConnectionState.Open)
                {
                    dbcmd.Connection.Open();
                }
                reader = dbcmd.ExecuteReader();
                result = true;
            }
            catch
            {
                result = false;
            }

            return result;
        }

        public bool Checked()
        {
            DbConnection dbconn = null;
            ///返回值
            bool result = false;    //返回值
                dbconn = new OleDbConnection(_linkstring);
            try
            {
                if (dbconn.State != ConnectionState.Open)
                {
                    dbconn.Open();
                }
                System.Threading.Thread.Sleep(10);
                if (dbconn.State != ConnectionState.Closed)
                {
                    dbconn.Close();
                }
                result = true;
            }
            catch
            {
                result = false;
            }
            finally
            {
                dbconn.Dispose();
                dbconn = null;
            }
            return result;
        }

        public string Summary
        {
            get { return _summary ; }
        }

        public bool Load(Stream s,int len)
        {
            if (s == null || len <24)//24,当前实例的bytes最小占用的字节数,实际最小字节数应该高于这个数
            {
                return false;
            }
            try
            {
                //待考量，是否需要添加单文件流内保存多种信息的情况
                //s.Seek(0,SeekOrigin.Begin );
                byte[] tmp = new byte[len];
                s.Read(tmp, 0, tmp.Length);

                BinaryReader ms = new BinaryReader(new MemoryStream(tmp));
                    byte[] value = null;
                    len = ms.ReadInt32() ;
                    value = ms.ReadBytes ( len);
                    this._servername = Encoding.UTF8.GetString(value);

                    len = ms.ReadInt32();
                    value = ms.ReadBytes(len);
                    this._databaseName = Encoding.UTF8.GetString(value);

                    len = ms.ReadInt32();
                    value = ms.ReadBytes(len);
                    this._user = Encoding.UTF8.GetString(value);

                    len = ms.ReadInt32();
                    value = ms.ReadBytes(len);
                    this._password = Encoding.UTF8.GetString(Encode(value));
            
                create_link_string();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Save(Stream s)
        {
            if (s == null)
            {
                return false;
            }
            try
            {
                byte[] tmp_value = null;

                BinaryWriter bw = new BinaryWriter(s);
                    tmp_value = Encoding.UTF8.GetBytes(this._servername);
                    bw.Write(tmp_value.Length);
                    bw.Write(tmp_value);

                    tmp_value = Encoding.UTF8.GetBytes(this._databaseName);
                    bw.Write(tmp_value.Length);
                    bw.Write(tmp_value);

                    tmp_value = Encoding.UTF8.GetBytes(this._user);
                    bw.Write(tmp_value.Length);
                    bw.Write(tmp_value);

                    tmp_value = Encode(Encoding.UTF8.GetBytes(this._password));
                    bw.Write(tmp_value.Length);
                    bw.Write(tmp_value);

                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 一次编码,二次解码
        /// </summary>
        /// <param name="data"></param>
        /// <returns>简单的字符替换法</returns>
        private static byte[] Encode(byte[] data) // 一次编码,二次解码
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    byte v = 0;
                    foreach (byte k in data)
                    {
                        v = (byte)(byte.MaxValue ^ k);
                        ms.WriteByte(v);
                    }

                    return ms.ToArray();
                }
            }
            catch
            {
                return new byte[] { };
            }
        }

        public DbConnection GetDbConnection()
        {
            return new OleDbConnection(_linkstring);

        }

        public DbCommand GetDbCommand()
        {
            DbCommand cmd = null;
                cmd = new OleDbCommand();
                cmd.Connection = new OleDbConnection(_linkstring);
                cmd.CommandType = CommandType.Text;
            return cmd;
        }

        public DbDataAdapter GetDbDataAdapter()
        {
            return new OleDbDataAdapter();
        }

        public DbCommandBuilder GetDbCommandBuilder()
        {
            return new OleDbCommandBuilder();
        }
        /// <summary>
        /// 命名空间名及实例名
        /// </summary>
        /// <remarks >用于在实例信息保存时使用</remarks>
        public string AssemblyName
        {
            get { return _assemblyName; }
        }

        public event SetItemDataHandler SetItemData;

        public void SetItem()
        {

        }
        public object GetItemValue(string propertyName)
        {
            switch (propertyName)
            {
                default:
                    return null;
            }
        }
        public void SetItemValue(string propertyName, object  value)
        {

        }

        public string LinkSummary
        {
            get { return string.Format("{0} on {1}({2})", DatabaseName, ServerName, User);  }
        }

        private UInt16 _port = 0;
        public ushort Port
        {
            get
            {
                return _port;
            }
            set
            {
                if (!_port.Equals(value))
                {
                    _port = value;
                    create_link_string();
                }
            }
        }



    }
}
