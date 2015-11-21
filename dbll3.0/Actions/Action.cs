using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dbll3.Actions
{
    public class Action : IAction
    {
        protected string _exeSQlString;
        /// <summary>
        /// 执行结果类型
        /// </summary>
        public ResultType ResultType
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public bool Execute()
        {
            throw new NotImplementedException();
        }

        public bool Analyse()
        {
            throw new NotImplementedException();
        }

        protected void _InstSQLString()
        {
            throw new System.NotImplementedException();
        }
    }
}
