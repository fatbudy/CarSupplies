using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dbll3.Actions
{
    public interface IAction
    {

        bool Execute();

        bool Analyse();
    }
}
