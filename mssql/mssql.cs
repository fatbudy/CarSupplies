using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using idblink;
namespace MSSQL
{
    [Serializable]
    public sealed class mssql:ILinkStringCreate 
    {
        private string _databaseName = string.Empty;
        private string _user = string.Empty;
        private string _servername = string.Empty;
        private string _password = string.Empty;
        private UInt16 _port = 1433;
        private string _linkstring = string.Empty;
        private const string _summary = "MSSQL";    //这个值必须与当前命名空间名相等
        private const string _assemblyName = "MSSQL.mssql"; //这里可以给改成自动生成的变量信息
        private bool _linkOledb = false;

        public mssql(string server, string dbname, string user, string pw, UInt16 port = 1433, bool oledb = false)
        {
            _servername = server;
            _databaseName = dbname;
            _user = user;
            _password = pw;
            _linkOledb = oledb;
            _port = port;
            create_link_string();
        }
        public mssql()
        {

        }
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

        public ushort Port
        {
            get
            {
                return _port  ;
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

        public string LinkString
        {
            get { 
                return _linkstring ;
            }
        }

        /// <summary>
        /// Oledb方式连接数据库
        /// </summary>
        public bool LinkOledb   // Oledb方式连接数据库
        {
            get
            {
                return _linkOledb;
            }
            set
            {
                if (_linkOledb.Equals(value))
                {
                    _linkOledb = value;
                    create_link_string();
                }
            }
        }
        /// <summary>
        /// 生成链接字符串信息
        /// </summary>
        private void create_link_string()
        {
            StringBuilder sb = new StringBuilder();
            if (LinkOledb)
            {
                sb.Append("Provider=SQLOLEDB;");
            }
            sb.AppendFormat("Data Source={0}", this.ServerName);
            if (!this.Port.Equals(1433))
            {
                sb.AppendFormat(",{0};", this.Port.ToString());
            }
            else
            {
                sb.Append(";");
            }
            sb.AppendFormat("user id={0};", this.User);
            sb.AppendFormat("password={0};", this.Password);
            sb.AppendFormat("Initial Catalog={0};", this.DatabaseName);

            _linkstring = sb.ToString();
        }
        public bool CreateLinkData(ref DbConnection conn, ref System.Data.Common.DbCommand cmd, ref System.Data.Common.DbDataAdapter dataAdapter, ref DbCommandBuilder dbcb)
        {
            ///返回值
            bool result = false;    //返回值
            if (_linkOledb)
            {
                conn =new  OleDbConnection(_linkstring);
                cmd = new OleDbCommand();
                cmd.Connection = new OleDbConnection(_linkstring);
                cmd.CommandType = CommandType.Text;
                dataAdapter = new OleDbDataAdapter();
                dbcb = new OleDbCommandBuilder();
            }
            else
            {
                conn = new SqlConnection(_linkstring);
                cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.Connection = new SqlConnection(_linkstring);
                dataAdapter = new SqlDataAdapter();
                dbcb =new SqlCommandBuilder();
            }
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
                if (_linkOledb)
                {
                    dbcmd = new OleDbCommand(sqlQuery, new OleDbConnection(_linkstring));
                }
                else
                {
                    dbcmd = new SqlCommand(sqlQuery, new SqlConnection(_linkstring));
                }
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
            if (_linkOledb)     //Oledb方式连接数据库
            {
                dbconn = new OleDbConnection(_linkstring);
            }
            else        //SqlCommand 方式连接数据库
            {
                dbconn = new SqlConnection(_linkstring);
            }
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

                    this._port = ms.ReadUInt16();
                    this._linkOledb = ms.ReadBoolean ();
            
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

                    bw.Write(this._port);
                    bw.Write(this._linkOledb);

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
            if (_linkOledb)
            {
                return new OleDbConnection(_linkstring);
            }
            else
            {
                return new SqlConnection(_linkstring);
            }
        }

        public DbCommand GetDbCommand()
        {
            DbCommand cmd = null;
            if (_linkOledb)
            {
                
                cmd = new OleDbCommand();
                cmd.Connection = new OleDbConnection(_linkstring);
                cmd.CommandType = CommandType.Text;
            }
            else
            {
                cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.Connection = new SqlConnection(_linkstring);
            }
            return cmd;
        }

        public DbDataAdapter GetDbDataAdapter()
        {
            DbDataAdapter dataAdapter = null;
            if (_linkOledb)
            {
                dataAdapter = new OleDbDataAdapter();
            }
            else
            {
                dataAdapter = new SqlDataAdapter();
            }
            return dataAdapter;
        }

        public DbCommandBuilder GetDbCommandBuilder()
        {
            DbCommandBuilder dbcb = null;
            if (_linkOledb)
            {
                dbcb = new OleDbCommandBuilder ();
            }
            else
            {
                dbcb = new SqlCommandBuilder ();
            }
            return dbcb;
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
            if (SetItemData != null)
            {
                SetItemData.Invoke(this, new ItemDataArgs("OleDB","Oledb方式连接SQL","LinkOledb",this.LinkOledb.GetType()  )); 
            }
        }
        public object GetItemValue(string propertyName)
        {
            switch (propertyName)
            {
                case "LinkOledb":
                    return _linkOledb;
                default:
                    return null;
            }
        }
        public void SetItemValue(string propertyName, object  value)
        {
            if (propertyName.Equals("LinkOledb"))
            {
                _linkOledb =(bool)value;
                create_link_string();
            }
        }

        public string LinkSummary
        {
            get { return string.Format("{0} on {1}({2})", DatabaseName, ServerName, User);  }
        }



    }
}
