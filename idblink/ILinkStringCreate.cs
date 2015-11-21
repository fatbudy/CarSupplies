using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.IO;
using System.Diagnostics;
using System.ComponentModel;

namespace idblink
{
    public class ItemDataArgs : EventArgs
    {
        public string HeadText { get; private set; }
        public string ToolTipText { get; private set; }
        public string PropertyName { get; private set; }
        public Type PropertyType { get; private set; }
        public ItemDataArgs(string headText, string tooltipText, string propertyName,Type propertyType)
        {
            HeadText = headText;
            ToolTipText = tooltipText;
            PropertyName = propertyName;
            PropertyType = propertyType;
        }

    }
    public delegate void SetItemDataHandler(object sender,ItemDataArgs e);
    public interface ILinkStringCreate
    {        
        string LinkSummary { get; }
        event SetItemDataHandler SetItemData;
        void SetItem();
        void SetItemValue(string propertyName, object value);
        object GetItemValue(string propertyName);
        /// <summary>
        /// 数据库名称
        /// </summary>
        string DatabaseName { get; set; }
        /// <summary>
        /// 数据库用户名
        /// </summary>
        string User { get; set; }
        /// <summary>
        /// 服务器
        /// </summary>
        string ServerName { get; set; }
        /// <summary>
        /// 用户口令
        /// </summary>
        string Password { get; set; }
        /// <summary>
        /// 数据库连接端口
        /// </summary>
        UInt16 Port { get; set; }
        /// <summary>
        /// 数据库连接字符串，仅空查看
        /// </summary>
        [Browsable(false)]
        string LinkString { get; }
        /// <summary>
        /// 建立数据库连接变量
        /// </summary>
        /// <returns></returns>
        bool CreateLinkData(ref DbConnection conn,  // 建立数据库连接变量
            ref DbCommand cmd,
            ref DbDataAdapter dataAdapter,
            ref DbCommandBuilder dbcb);
        bool GetDatareader(ref DbDataReader reader, string sqlQuery);
        /// <summary>
        /// 测试链接信息是否有效
        /// </summary>
        /// <returns></returns>
        bool Checked();
        /// <summary>
        /// 完成此接口的摘要信息
        /// </summary>
        string Summary { get; }
        bool Load(Stream s, int len);
        bool Save(Stream s);
        DbConnection GetDbConnection();
        DbCommand GetDbCommand();
        DbDataAdapter GetDbDataAdapter();
        DbCommandBuilder GetDbCommandBuilder();
        /// <summary>
        /// 命名空间名及实例名
        /// </summary>
        string AssemblyName { get; }
    }
}
