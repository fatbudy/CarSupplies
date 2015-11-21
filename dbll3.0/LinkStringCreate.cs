using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace dbll3
{
    //[Serializable]
    //public abstract class LinkStringCreate : ILinkStringCreate
    //{
    //    private byte[] pw_bytes = new byte[] { };

    //    [NonSerialized]
    //    private string _pw = string.Empty;
    //    [NonSerialized]
    //    private bool _change = false;

    //    public string DatabaseName { get; set; }
    //    public string User { get; set; }
    //    public string ServerName { get; set; }
    //    public string Password
    //    {
    //        get
    //        {
    //            if (_change)
    //            {
    //                _pw = Encoding.UTF8.GetString(Endcode(pw_bytes));
    //                _change = false;
    //            }
    //            return _pw;
    //        }
    //        set
    //        {
    //            _pw = value;
    //            _change = true;
    //            pw_bytes = Endcode(Encoding.UTF8.GetBytes(value));
    //        }
    //    }
    //    public ushort Port { get; set; }
    //    /// <summary>
    //    /// 分析字符串，是否可以转换成连接信息
    //    /// </summary>
    //    public abstract bool Parse(string linkstring);

    //    public static bool TryParse(string linkstring, ref LinkStringCreate lsc)
    //    {
    //        throw new NotImplementedException();
    //    }
    //    public static bool Load(string filename, ref LinkStringCreate ilsc)
    //    {
    //        try
    //        {
    //            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
    //            {
    //                byte[] tmp = new byte[fs.Length];
    //                fs.Read(tmp, 0, tmp.Length);
    //                using (MemoryStream ms = new MemoryStream(Endcode(tmp)))
    //                {
    //                    BinaryFormatter bf = new BinaryFormatter();
    //                    ilsc = bf.Deserialize(ms) as LinkStringCreate;
    //                }
    //            }
    //            return true;
    //        }
    //        catch
    //        {
    //            return false;
    //        }
    //    }
    //    public static bool Save(string filename, LinkStringCreate ilsc)
    //    {
    //        try
    //        {
    //            using (FileStream sw = new FileStream(filename, FileMode.Create, FileAccess.Write))
    //            {
    //                using (MemoryStream ms = new MemoryStream())
    //                {
    //                    BinaryFormatter bf = new BinaryFormatter();
    //                    bf.Serialize(ms, ilsc);
    //                    byte[] tmp = Endcode(ms.ToArray());
    //                    sw.Write(tmp, 0, tmp.Length);
    //                }
    //            }

    //            return true;
    //        }
    //        catch
    //        {
    //            return false;
    //        }
    //    }
    //    public static bool Load(Stream s, ref LinkStringCreate ils)
    //    {
    //        if (s == null)
    //        {
    //            return false;
    //        }
    //        try
    //        {
    //            //待考量，是否需要添加单文件流内保存多种信息的情况
    //            //s.Seek(0,SeekOrigin.Begin );
    //            byte[] tmp = new byte[s.Length];
    //            s.Read(tmp, 0, tmp.Length);
    //            using (MemoryStream ms = new MemoryStream(Endcode(tmp)))
    //            {
    //                BinaryFormatter bf = new BinaryFormatter();
    //                ils = bf.Deserialize(ms) as LinkStringCreate;
    //            }

    //            return true;
    //        }
    //        catch
    //        {
    //            return false;
    //        }
    //    }
    //    public static bool Save(Stream s, LinkStringCreate ils)
    //    {
    //        if (s == null)
    //        {
    //            return false;
    //        }
    //        try
    //        {
    //            using (MemoryStream ms = new MemoryStream())
    //            {
    //                BinaryFormatter bf = new BinaryFormatter();
    //                bf.Serialize(ms, ils);
    //                byte[] tmp = Endcode(ms.ToArray());
    //                s.Write(tmp, 0, tmp.Length);
    //            }

    //            return true;
    //        }
    //        catch
    //        {
    //            return false;
    //        }
    //    }
    //    /// <summary>
    //    /// 一次编码,二次解码
    //    /// </summary>
    //    /// <param name="data"></param>
    //    /// <returns>简单的字符替换法</returns>
    //    private static byte[] Endcode(byte[] data) // 一次编码,二次解码
    //    {
    //        try
    //        {
    //            using (MemoryStream ms = new MemoryStream())
    //            {
    //                byte v = 0;
    //                foreach (byte k in data)
    //                {
    //                    v = (byte)(byte.MaxValue ^ k);
    //                    ms.WriteByte(v);
    //                }

    //                return ms.ToArray();
    //            }
    //        }
    //        catch
    //        {
    //            return new byte[] { };
    //        }
    //    }
    //    public static byte[] Encode(byte[] data)
    //    {
    //        try
    //        {
    //            using (MemoryStream ms = new MemoryStream())
    //            {
    //                byte v = 0;
    //                foreach (byte k in data)
    //                {
    //                    v = (byte)(byte.MaxValue ^ k);
    //                    ms.WriteByte(v);
    //                }

    //                return ms.ToArray();
    //            }
    //        }
    //        catch
    //        {
    //            return new byte[] { };
    //        }
    //    }
    //    public abstract string LinkString { get; }
    //    public override string ToString()
    //    {
    //        return string.Format("{0} on {1}({2})", DatabaseName, ServerName, User);
    //    }

    //    public abstract bool CreateLinkData(ref System.Data.Common.DbConnection conn, 
    //        ref System.Data.Common.DbCommand cmd, 
    //        ref System.Data.Common.DbDataAdapter dataAdapter);
        
    //    public abstract bool GetDatareader(ref System.Data.Common.DbDataReader reader,string sqlQuery);
                
    //    /// <summary>
    //    /// 测试链接信息是否有效
    //    /// </summary>
    //    /// <returns></returns>
    //    public virtual bool Checked()
    //    {
    //        return false;
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <returns></returns>
    //    public virtual string Summary 
    //    {
    //        get
    //        {
    //            return "";
    //        }
    //    }

    //    /// <summary>
    //    /// 反射加载数据库连接设置信息建造器
    //    /// </summary>
    //    /// <param name="ilsc"></param>
    //    /// <param name="file">关联建造器的库文件</param>
    //    /// <returns></returns>
    //    public static bool LoadLinkStringCreate(ref ILinkStringCreate ilsc, string file)
    //    {
    //        // 使用接口，通过反射和配置文件来决定数据库连接信息是否使用。

    //        //检查库文件是否存在
    //        if (!File.Exists(file))
    //        {
    //            return false;
    //        }


    //        return true;
    //    }


    //    public bool Load(Stream s, int len)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public bool Save(Stream s)
    //    {
    //        throw new NotImplementedException();
    //    }


    //    public System.Data.Common.DbConnection GetDbConnection()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public System.Data.Common.DbCommand GetDbCommand()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public System.Data.Common.DbDataAdapter GetDbDataAdapter()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
