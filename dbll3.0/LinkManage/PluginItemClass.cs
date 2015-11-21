using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dbll3.LinkManage
{
    public class PluginItemClass
    {
        public string AssemblyFileName { get; set; }
        public string AssemblyName { get; set; }
        public string Version { get; set; }
        public override bool Equals(object obj)
        {
            if (obj is PluginItemClass)
            {
                PluginItemClass p = obj as PluginItemClass;
                return AssemblyFileName.Equals(p.AssemblyFileName) && AssemblyName.Equals(p.AssemblyName) && Version .Equals (p.Version );
            }
            return false ;
        }
        public override int GetHashCode()
        {
            return (AssemblyFileName + AssemblyName+Version ).GetHashCode(); 
        }
        public override string ToString()
        {
            return string.Format ("{0}/{1}/{2}",AssemblyFileName,AssemblyName,Version );
        }
    }
    public class PluginItems : IDictionary<string, PluginItemClass>
    {
        private Dictionary<string, PluginItemClass> _dic_pic = new Dictionary<string, PluginItemClass>();
        public void Add(string key, PluginItemClass value)
        {
            _dic_pic.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return _dic_pic.ContainsKey(key);
        }

        public ICollection<string> Keys
        {
            get { return _dic_pic .Keys ; }
        }

        public bool Remove(string key)
        {
            return _dic_pic .Remove (key);
        }

        public bool TryGetValue(string key, out PluginItemClass value)
        {
            return _dic_pic .TryGetValue (key,out value);
        }

        public ICollection<PluginItemClass> Values
        {
            get { return _dic_pic.Values ; }
        }

        public PluginItemClass this[string key]
        {
            get
            {
                return _dic_pic[key];
            }
            set
            {
                _dic_pic[key]=value;
            }
        }

        public void Add(KeyValuePair<string, PluginItemClass> item)
        {
            _dic_pic.Add(item.Key ,item.Value );
        }

        public void Clear()
        {
            _dic_pic.Clear();
        }

        public bool Contains(KeyValuePair<string, PluginItemClass> item)
        {
            return _dic_pic.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, PluginItemClass>[] array, int arrayIndex)
        {
            
        }

        public int Count
        {
            get { return _dic_pic.Count ; }
        }

        public bool IsReadOnly
        {
            get { return false ; }
        }

        public bool Remove(KeyValuePair<string, PluginItemClass> item)
        {
            return _dic_pic.Remove(item.Key );
        }

        public IEnumerator<KeyValuePair<string, PluginItemClass>> GetEnumerator()
        {
            return _dic_pic.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _dic_pic.GetEnumerator();
        }
    }
}
