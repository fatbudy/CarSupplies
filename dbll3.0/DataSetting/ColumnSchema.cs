using System;
using System.Collections.Generic;
using System.Text;

namespace DataSetting
{
    /// <summary>
    /// 数据表列设置
    /// </summary>
    [Serializable]
    public class ColumnSchema:ICloneable 
    {
        /// <summary>
        /// 别名
        /// </summary>
        private string _alisaname=string.Empty ;
        private string _name=string.Empty ;
        private bool _visiable=true ;
        private int _width=60;
        /// <summary>
        /// 数据表列名称
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
        /// 别名，Datagrid或其他方式显示名称
        /// </summary>
        public string AlisaName
        {
            get
            {
                return _alisaname;
            }
            set
            {
                _alisaname = value;
            }
        }

        /// <summary>
        /// 列显示宽度
        /// </summary>
        public int Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
            }
        }

        public bool Visiable
        {
            get
            {
                return _visiable;
            }
            set
            {
                _visiable = value;
            }
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
