using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Data;
using System.Xml.Serialization;
using System.IO;
using System.Globalization;
using System.Data.Common;

namespace dbll3
{
    public enum SchemaType
    {
        /// <summary>
        /// 数据库所有表
        /// </summary>
        Tables,
        /// <summary>
        /// 指定表列信息
        /// </summary>
        Columns,
        /// <summary>
        /// 制定表索引
        /// </summary>
        Indexes,
        /// <summary>
        /// 索引列
        /// </summary>
        IndexColumns,
        /// <summary>
        /// 数据库所有存储过程
        /// </summary>
        Procedures
    }

    [Serializable]
    public sealed partial class GetDataClass : IGetDataClass
    {
        #region "基本变量，事件，跨线程事件委托"
        /// <summary>
        /// 数据库访问时错误信息反馈事件
        /// </summary>
        public event DataOperationErrorMessageHandler Message;

        private DbConnection _Idbcon = null;
        private DbCommand _Idbcmd = null;
        private DbDataAdapter _DA = null;
        private DbCommandBuilder _ldbcbuid = null;
        //private DbTransaction _ldbTran = null;

        /// <summary>
        /// 数据库批查询完成时事件
        /// </summary>
        public event SQLExecuteCompletedHandler ExecuteCompleted;// 数据库批查询完成时事件
        /// <summary>
        /// 跨线程与GUI交互时，将线程同步操作
        /// </summary>
        private SynchronizationContext _sc = null;

        /// <summary>
        /// 触发数据库访问时的相关信息，主要是错误信息
        /// </summary>
        /// <param name="sql">执行错误的SQL脚本</param>
        /// <param name="errmsg">执行时的错误信息</param>
        private void RaiseEvent(DataOperationErrorMessageArgs e)  // 触发数据库访问时的相关信息，主要是错误信息
        {
            if (Message != null)
            {
                if (_sc != null)
                {
                    SendOrPostCallback sd = delegate(object data)
                    {
                        Message.Invoke(this, e);
                    };
                    //线程同步处理
                    _sc.Send(sd, e);
                }
                else
                {
                    Message.Invoke(this, e);
                }
            }
        }

        /// <summary>
        /// 批查询完成时的事件触发处理过程
        /// </summary>
        /// <param name="e"></param>
        private void raiseevent_sqlec(SQLExecuteCompleteArgs e)
        {
            if (ExecuteCompleted != null)
            {
                if (_sc != null)
                {
                    SendOrPostCallback sd = delegate(object data)
                    {
                        ExecuteCompleted.Invoke(this, e);
                    };
                    //线程同步处理
                    _sc.Send(sd, e);
                }
                else
                {
                    ExecuteCompleted.Invoke(this, e);
                }
            }
        }
        private void Dispose(bool disp)
        {
            if (_Idbcmd != null)
            {
                if (_Idbcmd.Connection != null)
                {
                    if (_Idbcmd.Connection.State != ConnectionState.Closed)
                    {
                        _Idbcmd.Connection.Close();
                    }
                }
                _Idbcmd.Dispose();
            }
            if (_Idbcon != null)
            {
                if (_Idbcon.State != ConnectionState.Closed)
                {
                    _Idbcon.Close();
                }
                _Idbcon.Dispose();
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// 分析SQL语句的结果是否正确返回事件触发
        /// </summary>
        /// <param name="e"></param>
        private void raiseevent_ParseResult(ParseResultArgs e)  // 分析SQL语句的结果是否正确返回事件触发
        {
            if (ParseResult != null)
            {
                if (_sc != null)
                {
                    SendOrPostCallback sd = delegate(object data)
                    {
                        ParseResult.Invoke(this, e);
                    };
                    //与gui同步处理
                    _sc.Send(sd, e);
                }
                else
                {
                    ParseResult.Invoke(this, e);
                }
            }
        }
        /// <summary>
        /// SQL语句错误分析事件
        /// </summary>
        public event ParseResultHandler ParseResult;


        #endregion

        public GetDataClass()
        {
            // TODO: Complete member initialization
        }

        public bool CheckLink()
        {
            if (_Idbcon == null)
                return false;

            try
            {
                if (_Idbcon.State != ConnectionState.Open)
                {
                    _Idbcon.Open();
                }
                System.Threading.Thread.Sleep(10);
                if (_Idbcon.State != ConnectionState.Closed)
                {
                    _Idbcon.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                RaiseEvent(new DataOperationErrorMessageArgs("链接测试错误", ex.Message));
                return false;
            }
        }

        /// <summary>
        /// 验证数据库连接配置是否有效
        /// </summary>
        /// <returns></returns>
        public bool IsNoVaild() // 验证数据库连接配置是否有效
        {
            return _Idbcon == null || _Idbcmd == null || _DA == null;
        }

        public bool GetData(string sqlstring, ref System.Data.DataTable data)
        {
            if (_Idbcmd == null)
                return false;

            try
            {
                _Idbcmd.CommandText = sqlstring;
                if (_Idbcmd.Connection.State != ConnectionState.Open)
                {
                    _Idbcmd.Connection.Open();
                }
               
                _DA.Fill(data);

                return true;
            }
            catch (Exception ex)
            {
                RaiseEvent(new DataOperationErrorMessageArgs(sqlstring, ex.Message));
                return false;
            }
        }

        public bool GetData(string sqlstring, ref System.Data.DataTable data, int pageindex, int pagesize)
        {
            if (_Idbcmd == null)
                return false;

            try
            {
                _Idbcmd.CommandText = sqlstring;
                if (_Idbcmd.Connection.State != ConnectionState.Open)
                {
                    _Idbcmd.Connection.Open();
                }
                if (pagesize < 1)
                {
                    _DA.Fill(data);
                }
                else
                {
                    _DA.Fill(pagesize * pageindex, pagesize, data);
                }
                return true;
            }
            catch (Exception ex)
            {
                RaiseEvent(new DataOperationErrorMessageArgs(sqlstring, ex.Message));
                return false;
            }
        }

        public bool ExecuteQuery(string sqlstring, out int count)
        {
            count = 0;
            if (_Idbcmd == null)
            {
                //errStr = "";
                return false;
            }

            try
            {
                _Idbcmd.CommandText = sqlstring;

                if (_Idbcmd.Connection.State != ConnectionState.Open)
                {
                    _Idbcmd.Connection.Open();
                }
                //实物处理开始
                _Idbcmd.Transaction = _Idbcmd.Connection.BeginTransaction();

                count = _Idbcmd.ExecuteNonQuery();
                if (_Idbcmd.Transaction != null)
                {
                    //提交事务
                    _Idbcmd.Transaction.Commit();
                }
                //errStr = "";
                return true;
            }
            catch (Exception ex)
            {
                if (_Idbcmd.Transaction != null)
                {
                    //回滚事务
                    _Idbcmd.Transaction.Rollback();
                }
                RaiseEvent(new DataOperationErrorMessageArgs(sqlstring, ex.Message));
                //errStr = ex.Message ;
                return false;
            }
        }

        /// <summary>
        /// 获取数据库表信息，或者表结构信息，具体信息参照SchemaType查阅
        /// </summary>
        /// <param name="table">Tables,Columns,Primary_Keys</param>
        /// <param name="SchemaType"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public bool GetTableNames(ref DataTable table, SchemaType st = SchemaType.Tables, string tablename = "")    // 获取数据库表信息，或者表结构信息，具体信息参照SchemaType查阅
        {
            //•Databases
            //•ForeignKeys
            //•Indexes
            //•IndexColumns
            //•Procedures
            //•ProcedureParameters
            //•Tables
            //•Columns
            //•Users
            //•Views
            //•ViewColumns
            //•UserDefinedTypes

            //Oracle .net
            //•Columns
            //•Indexes
            //•IndexColumns
            //•Procedures
            //•Sequences
            //•Synonyms
            //•Tables
            //•Users
            //•Views
            //•Functions
            //•Packages
            //•PackageBodies
            //•Arguments
            //•UniqueKeys
            //•PrimaryKeys
            //•ForeignKeys
            //•ForeignKeyColumns
            //•ProcedureParameters
            try
            {
                string[] vstmp = new string[] { null, null, null, null };
                table.Clear();
                if (_Idbcon.State != ConnectionState.Open)
                {
                    _Idbcon.Open();
                }
                switch (st)
                {
                    case SchemaType.Tables:
                        table = _Idbcon.GetSchema("Tables");
                        break;
                    case SchemaType.Columns:
                        vstmp[2] = tablename;
                        table = _Idbcon.GetSchema("Columns", vstmp);
                        break;
                    case SchemaType.Indexes:
                        vstmp[2] = tablename;
                        table = _Idbcon.GetSchema("Indexes", vstmp);
                        break;
                    case SchemaType.IndexColumns:
                        vstmp[2] = tablename;
                        table = _Idbcon.GetSchema("IndexColumns", vstmp);
                        break;
                    case SchemaType.Procedures:
                        table = _Idbcon.GetSchema("Procedures");
                        break;
                    default:
                        return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                RaiseEvent(new DataOperationErrorMessageArgs("获取表名信息错误", ex.Message));
                return false;
            }
        }
        /// <summary>
        /// 将DataTable 数据序列化成xml文本数据
        /// </summary>
        /// <param name="table"></param>
        /// <param name="tablename"></param>
        /// <param name="ci"></param>
        /// <returns></returns>
        public static string TableToXMLString(DataTable table, CultureInfo ci)// 将DataTable 数据序列化成xml文本数据
        {
            if (string.IsNullOrEmpty(table.TableName))
            {
                table.TableName = "tmp_table";
            }
            if (ci == null)
            {
                ci = new CultureInfo("zh-CHS");
            }
            using (StringWriter sw = new StringWriter(ci))
            {
                XmlSerializer xmls = new XmlSerializer(table.GetType());
                xmls.Serialize(sw, table);
                sw.Flush();
                return sw.GetStringBuilder().ToString();
            }
        }

        /// <summary>
        /// 返回数据库内所有表名称的XML格式字符串
        /// </summary>
        /// <param name="st"></param>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public string GetTableNames(SchemaType st = SchemaType.Tables, string tablename = "")
        {
            DataTable dt = new DataTable();
            GetTableNames(ref dt, st, tablename);
            return TableToXMLString(dt, null);
        }

        /// <summary>
        /// 返回执行页数据的XML格式字符串
        /// </summary>
        /// <param name="sqlstring">查询的sql语句</param>
        /// <param name="pageindex">指定页索引</param>
        /// <param name="pagesize">指定分页的大小</param>
        /// <returns></returns>
        public string GetData(string sqlstring, int pageindex, int pagesize)
        {
            DataTable dt = new DataTable();
            GetData(sqlstring, ref dt, pageindex, pagesize);
            return TableToXMLString(dt, null);
        }

        public bool GetDataByProc(string selectSQL, ref DataTable data, params string[] parArgs)
        {
            if (_Idbcmd == null)
                return false;
            try
            {
                _Idbcmd.CommandText = selectSQL;
                if (_Idbcmd.Connection.State != ConnectionState.Open)
                {
                    _Idbcmd.Connection.Open();
                }
                _DA.Fill(data);
                return true;
            }
            catch (Exception ex)
            {
                RaiseEvent(new DataOperationErrorMessageArgs(selectSQL, ex.Message));
                return false;
            }
        }

        /// <summary>
        /// 设置线程同步因子
        /// </summary>
        /// <param name="sc"></param>
        public void SetSynchronizationContext(SynchronizationContext sc)
        {
            _sc = sc;
        }

        #region "异步执行部分"
        public void GetData(string sqlstr, long sid)
        {
            if (_Idbcmd == null)
            {
                raiseevent_sqlec(new SQLExecuteCompleteArgs(sid, new DataSet(), false));
                return;
            }

            Action<object> objA = delegate(object data)
            {
                DataSet ds = new DataSet();
                bool comp = false;
                try
                {
                    _Idbcmd.CommandText = sqlstr;
                    if (_Idbcmd.Connection.State != ConnectionState.Open)
                    {
                        _Idbcmd.Connection.Open();
                    }
                    _DA.Fill(ds);
                    comp = true;
                }
                catch (Exception ex)
                {
                    RaiseEvent(new DataOperationErrorMessageArgs(sqlstr, ex.Message));
                }
                SQLExecuteCompleteArgs sqlec = new SQLExecuteCompleteArgs(sid, ds, comp);
                raiseevent_sqlec(sqlec);
            };

            ThreadPool.QueueUserWorkItem(new WaitCallback(objA));
        }

        public void GetData(string sqlstr, int pageindex, int pagesize, long sid, string tablename = "")
        {
            if (_Idbcmd == null)
            {
                raiseevent_sqlec(new SQLExecuteCompleteArgs(sid, new DataTable(), false));
                return;
            }

            Action<object> objA = delegate(object data)
            {
                DataTable dt = new DataTable() { TableName = string.IsNullOrEmpty(tablename) ? "Table" : tablename };
                bool comp = false;
                try
                {

                    _Idbcmd.CommandText = sqlstr;
                    if (_Idbcmd.Connection.State != ConnectionState.Open)
                    {
                        _Idbcmd.Connection.Open();
                    }
                    if (pagesize < 1)
                    {
                        _DA.Fill(dt);
                    }
                    else
                    {
                        _DA.Fill(pagesize * pageindex, pagesize, dt);
                    }
                    comp = true;
                }
                catch (Exception ex)
                {
                    RaiseEvent(new DataOperationErrorMessageArgs(sqlstr, ex.Message));
                }
                SQLExecuteCompleteArgs sqlec = new SQLExecuteCompleteArgs(sid, dt, comp);
                raiseevent_sqlec(sqlec);
            };

            ThreadPool.QueueUserWorkItem(new WaitCallback(objA));
        }

        public void ExecuteQuery(string sqlstr, long sid)
        {
            Action<object> objA = delegate(object data)
            {
                int count = 0;
                bool comp = false;
                if (_Idbcmd != null)
                {
                    try
                    {

                        _Idbcmd.CommandText = sqlstr;

                        if (_Idbcmd.Connection.State != ConnectionState.Open)
                        {
                            _Idbcmd.Connection.Open();
                        }
                        //实物处理开始
                        _Idbcmd.Transaction = _Idbcmd.Connection.BeginTransaction();

                        count = _Idbcmd.ExecuteNonQuery();
                        if (_Idbcmd.Transaction != null)
                        {
                            //提交事务
                            _Idbcmd.Transaction.Commit();
                        }
                        comp = true;
                    }
                    catch (Exception ex)
                    {
                        if (_Idbcmd.Transaction != null)
                        {
                            //回滚事务
                            _Idbcmd.Transaction.Rollback();
                        }
                        RaiseEvent(new DataOperationErrorMessageArgs(sqlstr, ex.Message));
                        comp = false;
                    }
                }
                SQLExecuteCompleteArgs sqlec = new SQLExecuteCompleteArgs(sid, count, comp);
                raiseevent_sqlec(sqlec);
            };
            ThreadPool.QueueUserWorkItem(new WaitCallback(objA));
        }

        /// <summary>
        /// 异步分析SQL语句
        /// </summary>
        /// <param name="sqlStr"></param>
        public void ParseAs(string sqlStr)  // 异步分析SQL语句
        {
            string errStr = string.Empty;
            Action<object> objA = delegate(object data)
            {
                bool comp = false;
                if (_Idbcmd != null)
                {

                    if (_Idbcmd.Connection.State != ConnectionState.Open)
                    {
                        _Idbcmd.Connection.Open();
                    }

                    try
                    {
                        //设置环境为分析模式
                        _Idbcmd.CommandText = "SET PARSEONLY ON";
                        _Idbcmd.ExecuteNonQuery();

                        _Idbcmd.CommandText = sqlStr;
                        _Idbcmd.ExecuteNonQuery();
                        comp = true;
                    }
                    catch (Exception ex)
                    {
                        errStr = ex.Message;
                        //RaiseEvent(sqlStr, ex.Message);
                        comp = false;
                    }
                    finally
                    {
                        //关闭分析模式
                        _Idbcmd.CommandText = "SET PARSEONLY OFF";
                        _Idbcmd.ExecuteNonQuery();
                    }
                }
                raiseevent_ParseResult(new ParseResultArgs(sqlStr, comp, errStr));
            };

            ThreadPool.QueueUserWorkItem(new WaitCallback(objA));
        }
        #endregion

        /// <summary>
        /// 分析SQL语句
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <returns></returns>
        public bool Parse(string sqlStr)    // 分析SQL语句
        {
            bool comp = false;

            if (_Idbcmd.Connection.State != ConnectionState.Open)
            {
                _Idbcmd.Connection.Open();
            }

            try
            {
                //设置环境为分析模式
                _Idbcmd.CommandText = "SET PARSEONLY ON";
                _Idbcmd.ExecuteNonQuery();

                _Idbcmd.CommandText = sqlStr;
                _Idbcmd.ExecuteNonQuery();
                comp = true;
            }
            catch //(Exception ex)
            {
                //RaiseEvent(sqlStr, ex.Message);
                comp = false;
            }
            finally
            {
                //关闭分析模式
                _Idbcmd.CommandText = "SET PARSEONLY OFF";
                _Idbcmd.ExecuteNonQuery();
            }

            return comp;
        }



        public bool GetData(string sqlstring, ref DataSet ds)
        {
            if (_Idbcmd == null)
                return false;

            try
            {
                _Idbcmd.CommandText = sqlstring;
                if (_Idbcmd.Connection.State != ConnectionState.Open)
                {
                    _Idbcmd.Connection.Open();
                }

                _DA.Fill(ds);

                return true;
            }
            catch (Exception ex)
            {
                RaiseEvent(new DataOperationErrorMessageArgs(sqlstring, ex.Message));
                return false;
            }
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="sqlstring"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool UpdateData(string sqlstring, System.Data.DataTable data)
        {
            if (_Idbcmd == null)
                return false;

            try
            {
                _Idbcmd.CommandText = sqlstring;
                if (_Idbcmd.Connection.State != ConnectionState.Open)
                {
                    _Idbcmd.Connection.Open();
                }
                //此举可解决连续多个不同表在插入或更新是抛出的异常
                //Table不存在*列等，即使DataAdapter更换了Select，但是Insert，Update却无法更新
                if (_DA != null && _ilsc != null)
                {
                    _DA.Dispose();
                    _DA = _ilsc.GetDbDataAdapter();
                }
                else
                {
                    return false;
                }

                _DA.SelectCommand = _Idbcmd;
                _ldbcbuid.DataAdapter = _DA;
               
                _DA.InsertCommand = _ldbcbuid.GetInsertCommand();
                _DA.UpdateCommand = _ldbcbuid.GetUpdateCommand();
                _DA.Update(data);
                data.AcceptChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// DataSet 更新是需要注意各个表之间的关系
        /// </summary>
        /// <param name="sqlstring"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool UpdateData(string sqlstring, System.Data.DataSet data)
        {
            if (_Idbcmd == null)
                return false;

            try
            {
                _Idbcmd.CommandText = sqlstring;
                if (_Idbcmd.Connection.State != ConnectionState.Open)
                {
                    _Idbcmd.Connection.Open();
                }
                int i = 0;
                foreach (DataTable dt in data.Tables)
                {
                    if (i == 0)
                    {
                        _DA.TableMappings.Add("Table", dt.TableName);
                    }
                    else
                    {
                        _DA.TableMappings.Add(string.Format("Table{0}", i), dt.TableName);
                    }
                    i++;
                }
                _ldbcbuid.DataAdapter = _DA;
                _DA.InsertCommand = _ldbcbuid.GetInsertCommand();
                _DA.UpdateCommand = _ldbcbuid.GetUpdateCommand();
                //_DA.DeleteCommand = _ldbcbuid.GetDeleteCommand();

                for ( i=0;i<_DA.TableMappings.Count ;i++)
                {
                    _DA.Update(data,_DA.TableMappings[i].SourceTable  );
                }
                data.AcceptChanges();
                _DA.TableMappings.Clear();
                return true;
            }
            catch(Exception ex)
            {
                Message.Invoke(this, new DataOperationErrorMessageArgs(ex.Message));
                return false;
            }
        }

        public bool GetTableSchema(string sqlstring,ref DataTable data)
        {
            if (_Idbcmd == null)
                return false;

            try
            {
                _Idbcmd.CommandText = sqlstring;
                if (_Idbcmd.Connection.State != ConnectionState.Open)
                {
                    _Idbcmd.Connection.Open();
                }

                _DA.FillSchema(data,System.Data.SchemaType.Mapped );
                return true;
            }
            catch (Exception ex)
            {
                RaiseEvent(new DataOperationErrorMessageArgs(sqlstring, ex.Message));
                return false;
            }
        }
        public void GetTableSchema(string sqlstring, long sid)
        {
        }
    }
}
