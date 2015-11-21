using System;
using System.Collections.Generic;
using System.Text;

namespace DataSetting
{
    [Serializable ]
    public class InColumnSchema:ColumnSchema 
    {
        private bool _checkdatavaild;
        private Type _type;
        private int _length=-1;
    
        public bool CheckDataVaild
        {
            get
            {
                return _checkdatavaild;
            }
            set
            {
                _checkdatavaild = value;
            }
        }

        public Type DataType
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }

        public int Length
        {
            get
            {
                return _length;
            }
            set
            {
                if (value >= -1)
                { _length = value; }
                else
                {
                    _length = -1;
                }
            }
        }
    }
}
