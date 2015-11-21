using System;
using System.Collections.Generic;
using System.Text;

namespace OutPutData
{
    public class ExportProgressArgs : EventArgs
    {
        private string _link = string.Empty;
        private int _currentIndex = 0;
        private int _count = 0;
        private string _percentage = string.Empty;
        private bool _comp = false;
        public string Percentage { get { return _percentage; } }
        public int Count { get { return _count; }
            internal set
            {
                if (value > 1)
                {
                    _count = value;
                    _percentage = string.Format("{0:P}",  (float)_currentIndex / _count);
                }

            }
        }
        public int Index
        {
            get { return _currentIndex; }
            internal set
            {
                if (value > 0)
                {
                    _currentIndex = value;
                }
                _percentage =string.Format("{0:P}", _count>0? (float)_currentIndex / _count:0);
            }
        }
        public bool Completed
        {
            get { return _comp; }
            internal set { _comp = value; }
        }
        public string Link { get { return _link; } }
        public ExportProgressArgs(int index, int count, bool clear = false)
        {
            _currentIndex = index;
            _count = count;
            if (!clear)
            {
                _percentage = string.Format("{0:P}", (float)_currentIndex / _count);
                _comp = _currentIndex >= _count;
            }
            else
            {
                _percentage = "";
                _comp = true;
            }
        }
        public ExportProgressArgs(int max, bool clear)
        {
            _currentIndex = 0;
            _count = max;
            _percentage = "";
        }
        public bool IsCompleted { get { return _currentIndex >= _count; } }
        internal ExportProgressArgs()
        {
        }
        //2015-03-05 new add
        private  uint _steps = 1;
        private  uint _stepIndex = 0;
        private  bool _batch = false;
        public  uint Steps
        {
            get { return _steps; }
            internal set
            {
                if (value > 0)
                {
                    _steps = value;
                    _batch = _steps>0?true:false;
                }                
            }
        }
        public  uint StepIndex
        {
            get { return _stepIndex; }
            internal set
            {
                _stepIndex = value;
            }
        }
        public  bool IsBatch
        {
            get
            {
                return _batch;
            }
            internal  set
            {
                _batch = value;
            }
        }
        private static ExportProgressArgs _e = new ExportProgressArgs();
        public static ExportProgressArgs CurrentProgress
        {
            get
            {
                if (_e == null)
                {
                    _e = new ExportProgressArgs();
                }
                return _e;
            }
        }
        public  bool IsBatchCompleted { get { return _stepIndex >= _steps; } }
    }

    public class ExportProgressCompletedArgs : EventArgs
    {
        private bool _completed = false;
        public bool Completed { get { return _completed; } }
        private DateTime _compdt = DateTime.Now;
        public DateTime CompletedTime { get { return _compdt; } }
        private bool _clear = false;
        public bool ClearProgressInfor { get { return _clear; } set { _clear = value; } }
        public ExportProgressCompletedArgs(bool comp)
        {
            _completed = comp;
        }
    }
}
