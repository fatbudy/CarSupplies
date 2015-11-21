using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using System.Windows.Forms;

namespace dbl
{
    [Serializable]
    public class SqlListItem
    {
        private object[] _param = new object[] { };
        public SqlListItem()
        {
            Data = new DataTable();
        }
        public DataTable Data { get; set; }
        public string ActionName { get; set; }
        public string SQLString { get; set; }
        public object[] Param
        {
            get
            {
                return _param;
            }
            set
            {
                if (value != null && !value.Equals(_param) && value.Length == ParamNumber)
                {
                    _param = value;
                }
            }
        }
        public int ParamNumber { get;internal  set; }
        public override string ToString()
        {
            return (Param.Length ==ParamNumber  && ParamNumber ==0 )?SQLString :  string.Format (SQLString ,Param );
        }
        public bool OnUpdated { get; set; }
        public int ID { get; set; }
        }
    [Serializable]
    public class SqlCmd
    {
        /// <summary>
        /// SQL语句列表
        /// </summary>
        private List<SqlListItem> _sqlList = new List<SqlListItem>();
        /// <summary>
        /// 表格显示控制查询语句
        /// </summary>
        private string _listShowSetting = string.Empty;
        private DataTable _listShowDataTable = new DataTable();
        private DataSet _sqlDataSet =null;
        private string _key = string.Empty;
        private bool _isinistal = false;
        private int itemcount = 0;
        public List<SqlListItem> SQLList
        {
            get { return _sqlList; }
            set
            {
                if (!value.Equals(_sqlList))
                {
                    _sqlList = value;
                }
            }
        }
        public string ListShowSetting
        {
            get
            {
                return _listShowSetting;
            }
            set
            {
                if (!value.Equals(_listShowSetting))
                {
                    _listShowSetting = value;
                }
            }
        }
        public DataTable ListShowDataTable 
        {
            get
            {
                return _listShowDataTable;
            }
            set
            {
                if (!value.Equals(_listShowDataTable))
                {
                    _listShowDataTable = value;
                }
            }
        }
        public DataSet SQLDataSet
        {
            get
            {
                if (_sqlDataSet == null)
                {
                    _sqlDataSet = new DataSet();
                    foreach (SqlListItem si in _sqlList)
                    {
                        _sqlDataSet.Tables.Add(si.Data);
                    }
                }
                return _sqlDataSet;
            }
        }
        public string SQLString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (SqlListItem var in _sqlList)
            {
                sb.AppendLine(var.ToString ());
            }
            return sb.ToString();
        }
        /// <summary>
        /// 操作实例的关键字
        /// </summary>
        public string Key   // 操作实例的关键字
        {
            get { return _key; }
            set { _key = value; }
        }
        /// <summary>
        /// 子项数据载入标志
        /// </summary>
        public bool Inistaled       // 子项数据载入标志
        {
            get { return _isinistal; }
            set { _isinistal = value; }
        }
        /// <summary>
        /// 清理子项数据
        /// </summary>
        public void ClearData() // 清理子项数据
        {
            foreach (SqlListItem sli in _sqlList)
            {
                sli.Data.Clear();
            }
        }
        public int HaveParamItemCount
        {
            get
            {            
                return itemcount;
            }
        }
        /// <summary>
        /// 计算拥有参数的子项数量，在读取数据时，需要对比带入的参数数量
        /// </summary>
        /// <returns></returns>
        public int CompleteParamItemCount() //计算拥有参数的子项数量，在读取数据时，需要对比带入的参数数量
        {
            foreach (SqlListItem sli in _sqlList)
            {
                if (sli.ParamNumber > 0)
                {
                    itemcount += 1;
                }
            }
            return itemcount;
        }

        private Dictionary<string, ColumnSetting> _dcs = new Dictionary<string, ColumnSetting>();
        public Dictionary <string ,ColumnSetting > ColumnSetting{get{return _dcs ;}}
        /// <summary>
        /// 载入列设置数据
        /// </summary>
        /// <param name="dt"></param>
        public void SetColumnSettingDataByTable(DataTable dt)  //载入列设置数据
        {
            //注意
            //这里只检查了表格列数量，未作其他检查
            if (dt.Columns.Count != 14)
            {
                return;
            }
            foreach (DataRow dr in dt.Rows)
            {
                ColumnSetting cs = new ColumnSetting();
                cs.id = Convert.ToInt32(dr[0]);
                cs.ColName = Convert.ToString(dr[1]);
                cs.Visiable = Convert.ToBoolean(dr[2]);
                cs.HeadText = Convert.ToString(dr[3]);
                cs.Width = Convert.ToInt32(dr[4]);
                cs.Localtion  = Convert.ToInt32(dr[5]);
                cs.LinkData  = Convert.ToBoolean(dr[6]);
                cs.LinkDataSQLString  = Convert.ToInt32(dr[7]);
                cs.LinkColumnName = Convert.ToString(dr[8]);
                cs.ActionName = Convert.ToString(dr[9]);
                cs.DefaultValue  = dr[10];
                cs.ReadOnly = Convert.ToBoolean(dr[11]);
                cs.IdentityIS  = Convert.ToBoolean(dr[12]);
                cs.Expression = dr[13];
                if (!_dcs.ContainsKey(cs.ColName))
                {
                    _dcs.Add(cs.ColName, cs);
                }
            }
        }
        public string ColumnSettingSQL { get; set; }

        private Control _window = null;
        /// <summary>
        /// 数据管理的窗体
        /// </summary>
        public Control Window   //数据管理的窗体
        {
            get { return _window; }
            set { _window = value; }
        }

        public bool ContainsID(int sqlid, out SqlListItem sqli)
        {
            foreach (SqlListItem sli in _sqlList)
            {
                if (sli.ID.Equals(sqlid))
                {
                    sqli = sli; 
                    return true; }
            }
            sqli = null;
            return false;
        }
        /// <summary>
        /// 返回所有子项的SQL语句集合
        /// </summary>
        /// <returns></returns>
        public string get_QueryString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (SqlListItem s in this._sqlList)
            {
                sb.AppendLine(s.ToString ());
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取列默认值
        /// </summary>
        /// <param name="colname"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool get_ColumnDefault(string colname ,out object value)
        {
            if (_dcs.ContainsKey(colname) && _dcs[colname].HaveDefaultValue() )
            {
                value = _dcs[colname].DefaultValue;
                return true;
            }
            value = null;
            return false;
        }
    }
}
