using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DataSetting;

namespace OutPutData
{
    public delegate void ExportStateEventHandler(object sender, ExportArgs e);
    public delegate void ExportProgressCompletedEventHandler(object sender, ExportProgressCompletedArgs e);
    //public delegate void UpdateProgress(ExportProgressArgs e);
    public enum ExportTypeEnum
    {
        None,
        Text,
        Xml,
        officeXML,
        Excel,
        CSV
    }
    public enum ExportDirectionEnum
    {
        /// <summary>
        /// 数据操作，双向支持，需要数据关联配置信息
        /// </summary>
        DataSource, //DataBase
        //OnlyOfficeFile, //Excel,Access
        File    //normal file
    }
    public interface IData
    {
        /// <summary>
        /// 包含数据的数据表，用于交换数据。
        /// </summary>
        DataTable Data { get; set; }
        /// <summary>
        /// 关于数据表的摘要信息
        /// </summary>
        string Summary { get; set; }
    }
    public interface IExportData : IData
    {
        event ExportStateEventHandler ExportState;
        event ExportProgressCompletedEventHandler ExportProgressCompleted;
        /// <summary>
        /// 导出数据存于指定的文件中
        /// </summary>
        void Export();

        /// <summary>
        /// 输出指定数据连接中指定表的数据
        /// </summary>
        /// <param name="con">数据库连接</param>
        /// <param name="tn">表名称</param>
        /// <param name="et">导出的数据格式</param>
        /// <param name="replace">替换目标数据信息</param>
        void Export(IDbConnection con, string tn, ExportTypeEnum et, bool replace);// 变更数据源
        /// <summary>
        /// 数据输出方向
        /// </summary>
        ExportDirectionEnum Direction { get; set; }// 数据输出方向
        /// <summary>
        /// 设置输出数据
        /// </summary>
        /// <param name="dt"></param>
        void SetData(DataTable dt);// 设置输出数据
        /// <summary>
        /// 数据导入、导出时的架构参照信息
        /// </summary>
        DataSchema SchemaSetting { get; set; }

        /// <summary>
        /// 数据导入、导出时的架构参照信息
        /// </summary>
        string ExportLinkString
        {
            get;
            set;
        }

        ExportTypeEnum ExportType
        {
            get;
            set;
        }

        void ExportOtherType(string filename, ExportTypeEnum et);// 数据导入、导出时的架构参照信息

        //2015-03-05 new add
        void SetData(DataSet ds);
        void ExportQueryString(IDbConnection con, string sqlStr, ExportTypeEnum et, bool replace);
        bool IsDataSetModel { get; set; }
    }
}
