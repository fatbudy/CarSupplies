using System;
using System.Collections.Generic;
using System.Text;

namespace DataSetting
{
    /// <summary>
    /// 数据库数据表配置信息
    /// </summary>
    [Serializable ]
    public class DataSchema
    {
        private ColumnCollection _columns=new ColumnCollection ();
        private OutColumnCollection _outsetting = new OutColumnCollection();
        private InColumnCollection _insetting = new InColumnCollection();
        private string _name="table";

        /// <summary>
        /// 数据表数据列基本设置
        /// </summary>
        public ColumnCollection Columns
        {
            get
            {
                return _columns;
            }
            set
            {
                _columns = value;
            }
        }

        /// <summary>
        /// 数据表名称
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        /// <summary>
        /// 数据导出设置
        /// </summary>
        public DataSetting.OutColumnCollection OutSetting
        {
            get
            {
                return _outsetting;
            }
            set
            {
                _outsetting = value;
            }
        }

        /// <summary>
        /// 数据导入设置
        /// </summary>
        public DataSetting.InColumnCollection InSetting
        {
            get
            {
                return _insetting;
            }
            set
            {
                _insetting = value;
            }
        }
    }
}
