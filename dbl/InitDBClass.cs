using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace dbl
{
    /// <summary>
    /// 初始化数据 
    /// </summary>
    public class InitDBClass
    {
        private string _sqlString = string.Empty;
        /// <summary>
        /// 载入数据库创建脚本
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public bool LoadFile(string filename)   // 载入数据库创建脚本
        {
            if (!File.Exists(filename))
            {
                return false;
            }

            try
            {
                using (StreamReader sr = new StreamReader(filename, Encoding.UTF8))
                {
                    _sqlString = sr.ReadToEnd();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 初始化数据库 
        /// </summary>
        /// <returns></returns>
        public int InitDataBase(dbll3.GetDataClass gdc)  // 初始化数据库
        {
            if (_sqlString.Length.Equals(0) || gdc==null)
                return -1;
            //分析载入的sql语句（建立数据的语句）
            if(gdc.Parse (_sqlString ))
            {
                int count = 0;
                //执行sql
                if (gdc.ExecuteQuery(_sqlString, out count))
                {
                    //执行成功
                    return 0;
                }
            }

            return -1;
        }
        /// <summary>
        /// 插入基础数据
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="gdc"></param>
        /// <returns></returns>
        public int InsertBaseData(string filename, dbll3.GetDataClass gdc)  // 插入基础数据
        {
            if (!File.Exists(filename) || gdc==null)
            {
                return -1;
            }

            if (!gdc.CheckLink())
            {
                return -1;
            }

            try
            {
                string sqltmp = string.Empty;
                using (StreamReader sr = new StreamReader(filename, Encoding.UTF8))
                {
                    sqltmp = sr.ReadToEnd();
                }
                if (gdc.Parse(sqltmp))
                {
                    int count = 0;
                    if (gdc.ExecuteQuery(sqltmp, out count))
                    {
                        return 0;
                    }
                }
            }
            catch
            {
            }
            return -1;
        }
    }
}
