using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using dbll3;
using System.IO;
using dbll3.LinkManage;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace dbl
{
    /// <summary>
    /// 
    /// </summary>
    public class DBLClass : IdblClass
    {
        private GetDataClass _GDC=null;
        /// <summary>
        /// 
        /// </summary>
        public event ShowMessageEventHandler ShowMessage;

        private bool __OnERROR = false;
        /// <summary>
        /// 执行的动作集合
        /// </summary>
        private Dictionary<string, SqlCmd> _sqlKey = new Dictionary<string, SqlCmd>();
        /// <summary>
        /// 消息提示集合
        /// </summary>
        private Dictionary<int, CustemMessageShowArgs> _cms_list = new Dictionary<int, CustemMessageShowArgs>();

        /// <summary>
        /// 正则表达式，取{0}格式的匹配
        /// </summary>
        private Regex _break_comp = new Regex(@"\{\d\}", RegexOptions.Compiled);

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, SqlCmd> ActionList { get { return _sqlKey; } }

        public bool get_ColumnSetting(string colname, out ColumnSetting cs)
        {
            foreach (string key in _sqlKey.Keys)
            {
                if (_sqlKey[key].ColumnSetting.ContainsKey (colname ))
                {
                    cs = _sqlKey[key].ColumnSetting[colname];
                    return true ;
                }
            }
            cs = null;
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqid"></param>
        /// <param name="dcs"></param>
        /// <param name="sqli"></param>
        /// <returns></returns>
        public bool get_ColumnSetting(int sqid, ref Dictionary<string, ColumnSetting> dcs, out SqlListItem sqli)
        {
             sqli = null;
            foreach (string k in _sqlKey.Keys )
            {
                if (_sqlKey[k].ContainsID(sqid, out sqli))
                {
                    dcs=_sqlKey[k].ColumnSetting;
                    return true;
                }
            }
            SqlCmd sqlc = null;
            if (!_sqlKey.ContainsKey("__BASE_SETTING_OTHER"))
            {
                sqlc = new SqlCmd();
                _sqlKey.Add("__BASE_SETTING_OTHER", sqlc);
            }
            else
            {
                 sqlc = _sqlKey["__BASE_SETTING_OTHER"];
            }
            DataTable dt = new DataTable();
            if (!_GDC.GetData(string.Format("SELECT [ID],[ActionName],[StepSQL],[paramnumber] from ActionStepSQLItem where id='{0}'", sqid), ref dt))
            {
                return false;
            }
            if (dt.Rows.Count < 1)
            {
                return false ;
            }

            int param_count = 0;

            foreach (DataRow dr in dt.Rows)
            {//增加了参数数量对比
                if (Convert.IsDBNull(dr["paramnumber"]) || !int.TryParse(dr["paramnumber"].ToString(), out param_count))
                {
                    continue;
                }
                //使用正则表达式，获取sql中的{}对数，与param_count作比较，不等下一条记录对比
                if (!CompParamCount(dr["stepsql"].ToString(), param_count))
                {
                    continue;
                }
                sqli = new SqlListItem()
                {
                    ActionName = dr["actionname"].ToString(),
                    SQLString = dr["stepsql"].ToString(),
                    ParamNumber = param_count,
                    ID = Convert.ToInt32(dr["id"])
                };
                sqlc.SQLList.Add(sqli    );
                break;
            }
            sqlc.CompleteParamItemCount();  //计算拥有参数的子项数量
            sqlc.Inistaled = true;
            dt = new DataTable();
            if (_GDC.GetData(string.Format ("select * from dbo.get_ActionSettingByASSID('{0}')",sqid), ref dt))  //获取数据列设置
            {
                 sqlc.SetColumnSettingDataByTable(dt);
                 getColumnSettingEx(sqlc);
            }
            dcs = sqlc.ColumnSetting;
            return true;
        }
        /// <summary>
        /// 获取扩展设置,数据关联列的数据获取
        /// </summary>
        /// <param name="sqlc"></param>
        private void getColumnSettingEx(SqlCmd sqlc)//获取扩展设置,数据关联列的数据获取
        {
            if (sqlc != null)
            {
                DataTable dt = new DataTable();
                foreach (string  cs in sqlc.ColumnSetting.Keys )
                {
                    if (sqlc.ColumnSetting[cs].LinkData)
                    {
                        dt.Clear();
                        Dictionary<string, object> tmplist = new Dictionary<string, object>();
                        if (_GDC.GetData(string.Format("SELECT [id],[acsid],[colname] from ColumnSettingLinkEx where acsid='{0}'", sqlc.ColumnSetting[cs].id), ref dt)
                            && dt.Rows.Count  > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (!Convert.IsDBNull(dr["colname"]))
                                {
                                    tmplist.Add(Convert.ToString(dr["colname"]),Convert.DBNull );
                                }
                            }
                        }
                        sqlc.ColumnSetting[cs].LinkDataColumnEx = tmplist;
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void get_Data(string sql, ref DataTable dt)
        {
            _GDC.GetData(sql, ref dt);
        }
        private bool _get_sqlbyID(int id,out string sql)
        {
            SqlListItem sqli = null;
            foreach (string k in _sqlKey.Keys)
            {
                if (_sqlKey[k].ContainsID(id, out sqli))
                {
                    sql= sqli.SQLString;
                    return true;
                }
            }
            sql = string.Empty;
            return false;
        }
        /// <summary>
        /// 更新列设置的管理数据
        /// </summary>
        /// <param name="sqlc"></param>
        /// <param name="colname">列名称</param>
        public void RefreshColumnSettingLinkData(ref SqlCmd sqlc, string colname)
        {
            if (sqlc.ColumnSetting.ContainsKey(colname))
            {
                ColumnSetting cs = sqlc.ColumnSetting[colname];
                DataTable dt = new DataTable();
                string sql = string.Empty;
                if (_get_sqlbyID(cs.LinkDataSQLString, out sql))
                {
                    if (!_GDC.GetData(sql, ref dt))
                    {
                        return; //错误
                    }
                    cs.LinkDataTable = dt;
                }
            }
        }
        /// <summary>
        /// 设置表格列显示
        /// </summary>
        /// <param name="dcs"></param>
        /// <param name="dgv"></param>
        public  void set_dataColumn(Dictionary<string, ColumnSetting> dcs,DataGridView dgv)
        {
            ColumnSetting cs = null;
            foreach (DataGridViewColumn dc in dgv.Columns)
            {
                if (dcs.ContainsKey(dc.Name))
                {
                    cs = dcs[dc.Name];
                    dc.HeaderText =cs.HeadText;
                    dc.Width = cs.Width;
                    dc.Visible = cs.Visiable;
                }
            }
        }
        private void _set_dataColumn(SqlCmd sqlc, DataTable descTable)
        {
            ColumnSetting cs = null;
            foreach (DataColumn dc in descTable.Columns)
            {
                if (sqlc.ColumnSetting.ContainsKey(dc.ColumnName))
                {
                    cs = sqlc.ColumnSetting[dc.ColumnName];

                    if (cs.HaveDefaultValue())
                    {
                        dc.DefaultValue = cs.DefaultValue;
                    }
                    if (cs.IdentityIS)
                    {
                        dc.AutoIncrement = true;
                        dc.AutoIncrementSeed = 0;
                        dc.AutoIncrementStep = -1;
                    }
                    //计算列设置，待处理
                    if (cs.HaveComputeExpression())
                    {
                        dc.Expression =Convert .ToString( cs.Expression );
                    }
                }
            }
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="key">操作的关键字</param>
        /// <param name="sqlc"></param>
        /// <param name="param">sql语句将带入的参数，id=-1时，如果有参数，注意参数顺序</param>
        /// <param name="id">默认为-1</param>
        /// <returns>id如果大于-1，确保param有值，且id小于param的数量</returns>
        public int ReadData(string key, ref SqlCmd sqlc, List<object[]> param, int id = -1)        // 读取数据
        {     
            //*执行前先分析SQL语法

            if (__OnERROR || _GDC == null)
                return -1;

            if (_sqlKey.ContainsKey(key))       //确认关键字是否存在
            {
                sqlc = _sqlKey[key];        //取得相关数据实例
                string sql = string.Empty ;
                if (!sqlc.Inistaled)                //相关子项是否载入
                {
                    __Get_ItemData(key, ref sqlc);  //载入子项数据
                }

                if (sqlc.SQLList.Count < 1)         //关键字实例下无相关操作
                {
                    return -1;
                }

                if (id > -1)      //执行指定id的sql语句
                {
                    if (id >= sqlc.SQLList.Count)   //索引数据不在数据集范围内
                    {
                        return -1;
                    }

                    SqlListItem sqli = sqlc.SQLList[id];

                    if (sqli.ParamNumber > 0)   //#M 此处逻辑待整理
                    {
                        if (param != null && param.Count > 0)
                        {
                            //sqli.Param = param.Count > 0 ? param[0] : new object[] { }; //赋值sql语句的参数
                            sqli.Param = param[0];
                        }
                        else
                        {
                            return -1;
                        }
                    }   //end #M

                    sql = sqli.ToString();
                    if (!_GDC.Parse(sql))      //验证sql语句语法
                        return -1;

                    //取得数据，并设置表列在DataTable中的设置
                    bool isNewTable = false;    //scope IsNewTable Start
                    DataTable dt = null;
                    if (sqli.Data == null)
                    {
                        dt = new DataTable();
                        sqli.Data = dt;
                        if (!_GDC.GetTableSchema(sqli.ToString(), ref dt))
                        {
                            return -1;
                        }
                        isNewTable = true;
                    }
                    else
                    {
                        dt = sqli.Data;
                    }
                    if (!_GDC.GetData(sql, ref dt))
                    {
                        return -1;
                    }

                    // sqli.Data = dt;
                    if (isNewTable)
                    {
                        _set_dataColumn(sqlc,dt);
                    }
                    //scope IsNewTable Finish
                    return 0;
                }
                else
                {
                    //执行所有sql语句
                    if (!sqlc.HaveParamItemCount.Equals(param.Count))
                    {
                        return -1;
                    }
                    int param_index = 0;
                    for (int i = 0; i < sqlc.SQLList.Count; i++)
                    {
                        SqlListItem s = sqlc.SQLList[i];
                        if (s.ParamNumber > 0)  //有参数的载入参数
                        {
                            s.Param = param[param_index];     //载入参数
                            param_index += 1;
                        }
                    }

                    sql = sqlc.SQLString();        //将所有sql语句汇总
                    if (!_GDC.Parse(sql))      //验证sql语句语法
                        return -1;
                    bool isNewTable = false; //scope IsNewTable Start
                    DataTable dttmp = null;
                    foreach (SqlListItem sli in sqlc.SQLList)
                    {
                        //取得数据，并设置表列在DataTable中的设置
                        isNewTable = false;    //新表标志重置
                        if (sli.Data == null)
                        {
                            dttmp = new DataTable();
                            sli.Data = dttmp;
                            if (!_GDC.GetTableSchema(sli.ToString(), ref dttmp))
                            {
                                return -1;
                            }
                            //设置列默认值及自增列
                            isNewTable = true;
                        }
                        else
                        {
                            dttmp = sli.Data;
                        }
                       
                        if (!_GDC.GetData(sli.ToString(), ref dttmp))
                        {
                            //读取错误,清楚所有数据
                            sqlc.ClearData();
                            return -1;
                        }
                        //dttmp.TableName = sli.ActionName;
                        //sli.Data = dttmp;       //赋值读取的数据
                        if (isNewTable)
                        {
                            _set_dataColumn(sqlc, dttmp);
                        }
                        //scope IsNewTable Finish
                    }

                    return 0;
                }
            }
            //错误消息管理

            return -1;
        }

        /// <summary>
        /// 更新编辑数据，插入数据
        /// </summary>
        /// <param name="sqlc"></param>
        /// <returns></returns>
        public int Update(SqlCmd sqlc)      // 更新编辑数据，插入数据
        {
            if (__OnERROR || _GDC == null || sqlc == null)
                return -1;

            foreach (SqlListItem sli in sqlc.SQLList)
            {
                if (sli.OnUpdated)
                {
                    if (!_GDC.UpdateData(sli.SQLString, sli.Data))
                    {
                        //错误,这里需要做调整，增加批量更新时失败回滚的操作
                        break;
                    }
                    sli.OnUpdated = false;
                }
            }

            return 0;
        }

        /// <summary>
        /// 从远程数据库载入操作数据字典
        /// </summary>
        /// <returns></returns>
        public int LoadKey() // 从远程数据库载入操作数据字典
        {
            if (__OnERROR || _GDC == null)
                return -1;

            DataTable dt = new DataTable();
            _GDC.GetData("select [key],setsql from ActionKeys order by [key]", ref dt);
            if (dt.Rows.Count <1)
            {
                return -1;
            }

            _sqlKey.Clear();

            foreach (DataRow  dr in dt.Rows)
            {
                if (Convert.IsDBNull(dr["key"]) )//|| Convert.IsDBNull(dr["setsql"])
                    continue;
                SqlCmd sqcmd = new SqlCmd();
                sqcmd.Key =Convert.ToString ( dr["key"]);
                sqcmd.ColumnSettingSQL = Convert.ToString(dr["setsql"]);
                _sqlKey.Add(sqcmd.Key,sqcmd);
            }
            return 0;
        }
        /// <summary>
        /// 获取动作的子项sql
        /// </summary>
        /// <param name="key"></param>
        /// <param name="sqlc"></param>
        private void __Get_ItemData(string key, ref SqlCmd sqlc)// 获取动作的子项sql
        {
            DataTable dt = new DataTable();
            if (!_GDC.GetData(sqlc.ColumnSettingSQL, ref dt))  //获取数据列设置
            {
                return;
            }
            sqlc.SetColumnSettingDataByTable(dt);
            getColumnSettingEx(sqlc);   //获取扩展设置,数据关联列的数据获取
            if (!_GDC.GetData(string.Format("SELECT  ID,ActionName, StepSQL, paramnumber FROM  groupaction where [key]='{0}'", key), ref dt))
            {
                //获取子项有错误
                return;
            }
            if (dt.Rows.Count < 0)
            {
                sqlc.Inistaled = true;
                return;
            }
            int param_count = 0;
            
            foreach (DataRow dr in dt.Rows)
            {//增加了参数数量对比
                if (Convert.IsDBNull (dr["paramnumber"]) ||  !int.TryParse(dr["paramnumber"].ToString(), out param_count))
                {
                    continue;
                }
                //使用正则表达式，获取sql中的{}对数，与param_count作比较，不等下一条记录对比
                if (!CompParamCount(dr["stepsql"].ToString(), param_count))
                {
                    continue;
                }
                sqlc.SQLList.Add(new SqlListItem() { ActionName = dr["actionname"].ToString(), 
                    SQLString = dr["stepsql"].ToString() ,
                    ParamNumber =param_count ,ID =Convert.ToInt32 ( dr["id"])}
                    );
            }
           
            sqlc.CompleteParamItemCount();  //计算拥有参数的子项数量
            sqlc.Inistaled = true;
        }
        private void __LoadSqlCmdItem(object data)
        {
            SqlCmd sqlctmp = null;
            foreach (string k in _sqlKey.Keys )
            {
                sqlctmp = _sqlKey[k];
                __Get_ItemData(k, ref sqlctmp );
            }
        }
        public void LoadSqlCmdItems()
        {
            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(__LoadSqlCmdItem));
        }
        /// <summary>
        /// 将{0}格式参数字符转换序列数字
        /// </summary>
        /// <param name="val"></param>
        /// <returns>错误返回-1</returns>
        private int _comp_getIndex(string val)// 将{0}格式参数字符转换序列数字
        {
            int tmp = -1;
            if (int.TryParse(val.Replace("{", "").Replace("}", "").Trim(), out tmp))
            {
                return tmp;
            }
            return -1;
        }
        /// <summary>
        /// 比较参数数量与设定的是否相等
        /// </summary>
        /// <param name="val"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        private bool CompParamCount(string val, int num)        // 比较参数数量与设定的是否相等
        {
            MatchCollection mc=_break_comp.Matches(val);
            int icom = -1;
            foreach (Match m in mc)
            {
                icom += 1;
                if (_comp_getIndex(m.Value) != icom )
                {
                    return false;
                }
            }
            return mc.Count .Equals(num);
        }
        /// <summary>
        /// 从远程数据库载入消息字典
        /// </summary>
        /// <returns></returns>
        public int LoadMessageList()            // 从远程数据库载入消息字典
        {
            if (__OnERROR ||  _GDC == null )
                return -1;

            DataTable dt = new DataTable();
            _GDC.GetData("select id,caption,text,icon,button from CustomMessageList order by id", ref dt);
            if (dt.Rows.Count < 1)
            {
                return -1;
            }

            _cms_list.Clear();
            int id = 0;

            foreach (DataRow dr in dt.Rows)
            {
                CustemMessageShowArgs tmp = new CustemMessageShowArgs(
                    dr["text"].ToString(),  //消息文本
                    dr["caption"].ToString(),       //消息标题
                    int.Parse(dr["icon"].ToString()),       //图标
                    int.Parse(dr["buttion"].ToString()));   //按钮

                id = int.Parse(dr["id"].ToString());

                if (_cms_list.ContainsKey(id))
                    continue;

                _cms_list.Add(id, tmp);
            }
            return 0;
        }

        /// <summary>
        /// 测试数据库连接有效性
        /// </summary>
        /// <returns></returns>
        public bool CheckDBLinkState()          // 测试数据库连接有效性
        {
            if (__OnERROR && _GDC != null)
                return false ;

            return _GDC.CheckLink();
        }

        public int LinkSetting(string filename ="")
        {
            LinkStringCreate lsc = new LinkStringCreate();

            if (!string.IsNullOrEmpty(filename))
            {
                lsc.FileName = filename;
            }

            if (lsc.ShowDialog() == DialogResult.OK)
            {
                return 0;
            }
            return -1;
        }

        public int InitSetting()
        {
            string dbfile = string.Format(@"{0}\dbsetting.db", AppDomain.CurrentDomain.BaseDirectory);
            return InitSetting(dbfile);
        }
        /// <summary>
        /// 载入数据库连接设置并初始化连接
        /// </summary>
        /// <returns></returns>
        public int InitSetting(string filename)     // 载入数据库连接设置并初始化连接
        {
            __OnERROR = true;
            _GDC = new GetDataClass();

            if (!File.Exists(filename))
            {
                //设置数据库连接，错误返回-1；
                if (MessageBox.Show("未找到数据配置文件，是否现在设置数据连接？", "提示",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    LinkStringCreate lsc = new LinkStringCreate();
                    lsc.FileName = filename;
                    if (lsc.ShowDialog() != DialogResult.OK)
                    {
                        return -1;
                    }
                }
                else
                {
                    return -1;
                }

            }
            //设定数据库连接实例
            if (!_GDC.Load_ILinkStringCreate(filename))
                return -1;
            //关联消息事件
            _GDC.Message += new DataOperationErrorMessageHandler(_GDC_public_Message);
            _GDC.ExecuteCompleted += new SQLExecuteCompletedHandler(_GDC_public_ExecuteCompleted);
            _GDC.ParseResult += new ParseResultHandler(_GDC_public_ParseResult);
            __OnERROR = false;
            return 0;
        }
        private void _GDC_public_Message(object sender, DataOperationErrorMessageArgs e)
        {
            //GetDataClass 消息处理
            MessageBox.Show(e.Message);
        }
        private void _GDC_public_ExecuteCompleted(object sender, SQLExecuteCompleteArgs e)
        {
            //switch (e.SessionID)
            //{
            //    case 8888:      //查询分析
            //    default:
            //        break;
            //}
        }
        private void _GDC_public_ParseResult(object sender, ParseResultArgs e)
        {
            //分析完成，语句{0}{1}", e.Result ? "正确" : "有错误", !e.Result ? "(" + e.ErrorMessage + ")" :""
        }
       public  string getLinkDataSerialNumber()
        {
            return ItemLineNumber.GetSerialNumber();
        }
    }
}
