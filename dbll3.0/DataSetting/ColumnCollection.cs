using System;
using System.Collections.Generic;
using System.Text;

namespace DataSetting
{
    [Serializable ]
    public class ColumnCollection:ICollection<ColumnSchema>
    {
        private IList<ColumnSchema> ilcs = new List<ColumnSchema>();

        public ColumnSchema GetColumnSchema(string name)
        {
            ColumnSchema cs = null;
            foreach (ColumnSchema cs_tmp in ilcs)
            {
                if (cs_tmp.Name.Equals(name))
                {
                    cs = cs_tmp;
                    break;
                }
            }
            return cs;
        }
        public void Add(ColumnSchema item)
        {
            ilcs.Add(item);
        }

        public void Clear()
        {
            ilcs.Clear();
        }

        public bool Contains(ColumnSchema item)
        {
            return ilcs.Contains(item);
        }

        public void CopyTo(ColumnSchema[] array, int arrayIndex)
        {
            if (array.Length - arrayIndex > ilcs.Count)
            {
                for (int i = 0; i < ilcs.Count; i++)
                {
                    array[arrayIndex + i] = ilcs[i];
                }
            }
        }

        public int Count
        {
            get { return ilcs.Count ; }
        }

        public bool IsReadOnly
        {
            get { return false ; }
        }

        public bool Remove(ColumnSchema item)
        {
            return ilcs.Remove (item);
        }

        public IEnumerator<ColumnSchema> GetEnumerator()
        {
            return ilcs.GetEnumerator ();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ilcs.GetEnumerator();
        }
    }
    [Serializable]
    public class InColumnCollection : ICollection<InColumnSchema>
    {
        private IList<InColumnSchema> ilcs = new List<InColumnSchema>();

        public InColumnSchema GetColumnSchema(string name)
        {
            InColumnSchema cs = null;
            foreach (InColumnSchema cs_tmp in ilcs)
            {
                if (cs_tmp.Name.Equals(name))
                {
                    cs = cs_tmp;
                    break;
                }
            }
            return cs;
        }
        public void Add(InColumnSchema item)
        {
            ilcs.Add(item);
        }

        public void Clear()
        {
            ilcs.Clear();
        }

        public bool Contains(InColumnSchema item)
        {
            return ilcs.Contains(item);
        }

        public void CopyTo(InColumnSchema[] array, int arrayIndex)
        {
            if (array.Length - arrayIndex > ilcs.Count)
            {
                for (int i = 0; i < ilcs.Count; i++)
                {
                    array[arrayIndex + i] = ilcs[i];
                }
            }
        }

        public int Count
        {
            get { return ilcs.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(InColumnSchema item)
        {
            return ilcs.Remove(item);
        }

        public IEnumerator<InColumnSchema> GetEnumerator()
        {
            return ilcs.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ilcs.GetEnumerator();
        }
    }
    [Serializable]
    public class OutColumnCollection : ICollection<OutColumnSchema>
    {
        private IList<OutColumnSchema> ilcs = new List<OutColumnSchema>();

        public OutColumnSchema GetColumnSchema(string name)
        {
            OutColumnSchema cs = null;
            foreach (OutColumnSchema cs_tmp in ilcs)
            {
                if (cs_tmp.Name.Equals(name))
                {
                    cs = cs_tmp;
                    break;
                }
            }
            return cs;
        }
        public void Add(OutColumnSchema item)
        {
            ilcs.Add(item);
        }

        public void Clear()
        {
            ilcs.Clear();
        }

        public bool Contains(OutColumnSchema item)
        {
            return ilcs.Contains(item);
        }

        public void CopyTo(OutColumnSchema[] array, int arrayIndex)
        {
            if (array.Length - arrayIndex > ilcs.Count)
            {
                for (int i = 0; i < ilcs.Count; i++)
                {
                    array[arrayIndex + i] = ilcs[i];
                }
            }
        }

        public int Count
        {
            get { return ilcs.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(OutColumnSchema item)
        {
            return ilcs.Remove(item);
        }

        public IEnumerator<OutColumnSchema> GetEnumerator()
        {
            return ilcs.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ilcs.GetEnumerator();
        }
    }
}
