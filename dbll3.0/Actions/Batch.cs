using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dbll3.Actions
{
    public class Batch : IBatch 
    {
        public Action[] Actions
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        public bool IsBatchExecute
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public void AddAction(Action a)
        {
            throw new System.NotImplementedException();
        }

        public void Remove(Action a)
        {
            throw new System.NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new System.NotImplementedException();
        }

        public void AddActions(Action[] alist)
        {
            throw new System.NotImplementedException();
        }

        public bool Execute()
        {
            throw new NotImplementedException();
        }

        public bool Analyse()
        {
            throw new NotImplementedException();
        }
    }
}
