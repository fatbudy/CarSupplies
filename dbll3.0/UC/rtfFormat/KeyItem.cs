using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dbll3.UC.rtfFormat
{
    [Serializable()]
    public class KeyType
    {
        private bool _border;
        private string _keyname;
        private bool _toend;
        private Itembase _ib = null;
        public KeyType(string kn, bool b = false, bool te = false)
        {
            this._keyname = kn;
            this._border = b;
            this._toend = te;
        }

        public string KeyName
        {
            get
            {
                return this._keyname;
            }
        }

        public bool Border
        {
            get
            {
                return this._border;
            }
        }

        public bool TOEnd { get { return this._toend; } }
        public Itembase Item
        {
            get
            {
                return this._ib;
            }
            set
            {
                this._ib = value;
            }
        }
    }
    [Serializable]
    public abstract class Itembase
    {
        protected System.Drawing.Color _color;
        protected System.Drawing.Font _font;
        protected System.Collections.Generic.List<KeyType> _keys = new List<KeyType>();
        protected System.Drawing.Color _bcolor;
        protected string _summary;
        protected bool _key;
        /// <summary>
        /// richtextbox中要查找的文本
        /// </summary>
        public KeyType[] Keys
        {
            get
            {
                return this._keys.ToArray();
            }
        }

        public System.Drawing.Color Color
        {
            get
            {
                return this._color;
            }
        }

        public System.Drawing.Font Font
        {
            get
            {
                return this._font;
            }
        }

        public System.Drawing.Color BackColor
        {
            get
            {
                return this._bcolor;
            }
        }

        public string Summary
        {
            get
            {
                return this._summary;
            }
        }

        public bool Key
        {
            get
            {
                return this._key;
            }
        }

        public void addKey(KeyType key)
        {
            if (this._keys.IndexOf(key) != -1)
            {
                this._keys.Add(key);
            }
        }

        public void removeKey(KeyType key)
        {
            this._keys.Remove(key);
            //throw new System.NotImplementedException();
        }

        public void clear()
        {
            this._keys.Clear();
        }

        public bool Contains(KeyType key)
        {
            if (this._keys.IndexOf(key) > -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool Contains(string key)
        {
            foreach (KeyType kt in this._keys)
            {
                if (kt.KeyName == key)
                {
                    return true;
                }
            }
            return false;
        }
    }
    [Serializable()]
    public class ItemCollections
    {
        protected Dictionary<string, Itembase> _items = new Dictionary<string, Itembase>();
        //public Itembase[] Items
        //{
        //    get
        //    {
        //        return _items.Values.ToArray();
        //    }
        //}

        /// <param name="text">需要修改样式的文本或关键字</param>
        public bool get_style(string text, out Itembase ib)
        {
            foreach (string key in this._items.Keys)
            {
                if (this._items[key].Contains(text))
                {
                    ib = this._items[text];
                    return true;
                }
            }
            ib = null;
            return false;
        }
        public bool get_style(string text, out Itembase ib, out string type)
        {
            foreach (string key in this._items.Keys)
            {
                if (this._items[key].Contains(text))
                {
                    ib = this._items[text];
                    type = key;
                    return true;
                }
            }
            ib = null;
            type = "";
            return false;
        }
        public bool get_style_key(string key, out Itembase ib)
        {
            foreach (string k in this._items.Keys)
            {
                if (key.Equals(k))
                {
                    ib = this._items[key];
                    return true;
                }
            }
            ib = null;
            return false;
        }
        public KeyType[] Keys()
        {
            List<KeyType> lkt = new List<KeyType>();
            foreach (string k in this._items.Keys)
            {
                lkt.AddRange(this._items[k].Keys);
            }
            return lkt.ToArray();
        }
    }
    public class Item : Itembase
    {
        public Item(System.Drawing.Color color,
            System.Drawing.Color bcolor,
            string suma)
        {
            this._bcolor = bcolor;
            this._color = color;
            this._font = new System.Drawing.Font("Arial", 24);
            this._summary = suma;
        }
        public Item(System.Drawing.Color color,
            System.Drawing.Color bcolor,
            System.Drawing.Font f,
            string suma)
        {
            this._bcolor = bcolor;
            this._color = color;
            this._font = f;
            this._summary = suma;
        }
        public Item(System.Drawing.Color color,
            System.Drawing.Color bcolor,
            string suma,
            params KeyType[] text)
        {
            this._bcolor = bcolor;
            this._color = color;
            this._font = new System.Drawing.Font("Arial", 24);
            this._summary = suma;
            foreach (KeyType kt in text)
            {
                kt.Item = this;
                this._keys.Add(kt);
            }
            //Item(color, bcolor, new System.Drawing.Font("Arial", 24), suma, text);
        }

        public Item(System.Drawing.Color color,
            System.Drawing.Color bcolor,
            System.Drawing.Font f,
            string suma,
            params KeyType[] text)
        {
            this._bcolor = bcolor;
            this._color = color;
            this._font = f;
            this._font = new System.Drawing.Font("Arial", 24);
            this._summary = suma;
            foreach (KeyType kt in text)
            {
                kt.Item = this;
                this._keys.Add(kt);
            }

        }

    }
}
