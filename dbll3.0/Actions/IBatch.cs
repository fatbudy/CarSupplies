using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dbll3.Actions
{
    public interface IBatch:IAction 
    {
    
        Action[] Actions { get; }

        bool IsBatchExecute
        {
            get;
            set;
        }
    }
}
