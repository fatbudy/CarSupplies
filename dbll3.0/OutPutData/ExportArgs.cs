using System;
using System.Collections.Generic;
using System.Text;

namespace OutPutData
{
    public class ExportArgs : EventArgs
    {
        private int _code = 0;
        private string _msg = string.Empty;
        public string Message
        {
            get
            {
                return _msg;
            }
        }
        public int Code
        {
            get { return _code; }
        }
        public ExportArgs(string msg, int code)
        {
            _code = code;
            _msg = msg;
        }

        public override string ToString()
        {
            return _msg;
        }
    }
}
