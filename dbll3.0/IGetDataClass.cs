using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Data;

namespace dbll3
{
    public delegate void DataOperationErrorMessageHandler(object sender, DataOperationErrorMessageArgs e);
    public delegate void SQLExecuteCompletedHandler(object sender, SQLExecuteCompleteArgs e);
    public delegate void ParseResultHandler(object sender, ParseResultArgs e);

    public enum eErrorType
    {
        Link,
        Query
    }
    public class ParseResultArgs : EventArgs
    {
        private bool _result = false;
        public bool Result { get { return _result; } }
        private string _sqlStr = string.Empty;
        public string SQLString { get { return _sqlStr; } }
        private string _errStr = string.Empty;
        public string ErrorMessage { get { return _errStr; } }
        public ParseResultArgs(string sqlStr, bool rest = true, string emsg = "")
        {
            _sqlStr = sqlStr;
            _errStr = emsg;
            _result = rest;
        }
    }
    public class SQLExecuteCompleteArgs : EventArgs
    {
        private bool _completed = false;
        public bool ExecuteCompleted { get { return _completed; } }

        private int _count = 0;
        public int Count { get { return _count; } }

        private DataSet _ds = new DataSet();
        public DataSet Datas { get { return _ds; } }

        private DataTable _dt = null;
        public DataTable Data
        {
            get
            {
                if (_ds.Tables.Count > 0)
                {
                    _dt = _ds.Tables[0];
                }
                else if (_dt == null)
                {
                    _dt = new DataTable();
                }
                return _dt;
            }
        }

        private long _sid = 0;
        public long SessionID { get { return _sid; } }
        public SQLExecuteCompleteArgs(long sid, DataTable dt, bool comp = true)
        {
            _sid = sid;
            _ds.Tables.Add(dt);
            _count = dt.Rows.Count;
            _completed = comp;
        }
        public SQLExecuteCompleteArgs(long sid, DataSet ds, bool comp = true)
        {
            _sid = sid;
            _ds = ds;
            _completed = comp;
            foreach (DataTable dt in ds.Tables)
            {
                _count += dt.Rows.Count;
            }
        }
        //public SQLExecuteCompleteArgs(int count, bool comp)       2015-03-05 change
        public SQLExecuteCompleteArgs(long sid, int count, bool comp)
        {
            _sid = sid;
            _count = count;
            _completed = comp;
        }
    }

    /// <summary>
    /// 执行错误的SQL语句及返回的错误信息
    /// </summary>
    public class DataOperationErrorMessageArgs : EventArgs
    {
        public DataOperationErrorMessageArgs(string errMsg)
        {
            sqlString = string.Empty ;
            Message = errMsg;
            ErrorType = eErrorType.Link ;
        }
        public DataOperationErrorMessageArgs(string sqlStr, string errMsg)
        {
            sqlString = sqlStr;
            Message = errMsg;
            ErrorType = eErrorType.Query;
        }
        /// <summary>
        /// 错误的SQL语句
        /// </summary>
        public string sqlString { get; private set; }
        /// <summary>
        /// 执行错误的SQL返回的错误信息
        /// </summary>
        public string Message { get; private set; }
        public eErrorType ErrorType { get; private set; }
    }

    public partial interface IGetDataClass : IDisposable
    {
        event DataOperationErrorMessageHandler Message;
        event ParseResultHandler ParseResult;
        event SQLExecuteCompletedHandler ExecuteCompleted;

        bool CheckLink();
        bool GetData(string sqlstring, ref DataSet  ds);
        bool GetData(string sqlstring, ref DataTable data);
        bool GetData(string sqlstring, ref DataTable data, int pageindex, int pagesize);
        bool ExecuteQuery(string sqlstring, out int count);
        bool IsNoVaild();
        bool GetTableNames(ref DataTable table, SchemaType st = SchemaType.Tables, string tablename = "");
        bool GetDataByProc(string procName, ref DataTable data, params string[] parArgs);

        string GetData(string sqlstring, int pageindex, int pagesize);
        string GetTableNames(SchemaType st = SchemaType.Tables, string tablename = "");


        void GetData(string sqlstr, long sid);
        void GetData(string sqlstr, int pageindex, int pagesize, long sid, string tablename = "");
        void ExecuteQuery(string sqlstr, long sid);
        void SetSynchronizationContext(SynchronizationContext sc);
        void ParseAs(string sqlStr);
        bool Parse(string sqlStr);
        bool GetTableSchema(string sqlstring, ref DataTable data);
        void GetTableSchema(string sqlstring,long sid);

    }
}
