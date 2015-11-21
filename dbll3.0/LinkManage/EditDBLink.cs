using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using idblink;
using System.IO;
using System.Reflection;


namespace dbll3.LinkManage
{
    public partial class EditDBLink : UserControl
    {

        public EditDBLink()
        {
            InitializeComponent();
        }
        private Dictionary<string, Control> dic_itemlist = new Dictionary<string, Control>();
        private void _ilsc_SetItemData(object sender, ItemDataArgs e)
        {
            int line = this.tableLayoutPanel2.RowCount;
            this.tableLayoutPanel2.Controls.Add(new Label() { Text = e.HeadText +":", Anchor= AnchorStyles.Right ,AutoSize=true},0,line);
            if (e.PropertyType.Equals(typeof(bool)))
            {
                CheckBox cb = new CheckBox(){ Anchor = AnchorStyles.Left };
                cb.Checked = (bool)_ilsc.GetItemValue(e.PropertyName); 
                this.tableLayoutPanel2.Controls.Add(cb , 1, line);
                dic_itemlist.Add(e.PropertyName, cb); 
            }
            else
            {
                TextBox tb=new TextBox { Dock = DockStyle.Fill};
                tb.Text = (string)_ilsc.GetItemValue(e.PropertyName);
                this.tableLayoutPanel2.Controls.Add(tb, 1, line);
                dic_itemlist.Add(e.PropertyName, tb); 
            }
            this.tableLayoutPanel2.RowStyles.Add(new RowStyle());
        }
        private ILinkStringCreate _ilsc = null;
        public ILinkStringCreate LinkStringCreate
        {
            get { return _ilsc; }
            set {

                if (value == null)
                {
                    return;
                }
                if (_ilsc != null)
                {
                    _ilsc.SetItemData -= new SetItemDataHandler(_ilsc_SetItemData);
                    dic_itemlist.Clear();
                }
                _ilsc = value;
                _ilsc.SetItemData += new SetItemDataHandler(_ilsc_SetItemData);
                ReloadItemValue();
            }
        }
        public void SaveItemValue()
        {
            if (_ilsc == null)
            {
                return;
            }
            _ilsc.ServerName = ServerNametextBox.Text;
            _ilsc.DatabaseName = DatabasetextBox.Text;
            _ilsc.User = UserNametextBox.Text;
            _ilsc.Password = PasswordtextBox.Text;
            foreach (string key in dic_itemlist.Keys )
            {
                if (dic_itemlist[key] is CheckBox)
                {
                    _ilsc.SetItemValue(key, ((CheckBox)dic_itemlist[key]).Checked);
                }
                else
                {
                    _ilsc.SetItemValue(key, dic_itemlist[key].Text);
                }
            }
        }
        public void ReloadItemValue()
        {
            if (_ilsc != null)
            {
                dic_itemlist.Clear();
                this.tableLayoutPanel2.Controls.Clear();
                this.tableLayoutPanel2.RowCount = 0;
                this.tableLayoutPanel2.ColumnCount = 2;
                this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
                this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
               ServerNametextBox.Text= _ilsc.ServerName ;
                DatabasetextBox.Text=_ilsc.DatabaseName ;
                UserNametextBox.Text= _ilsc.User;
                PasswordtextBox.Text= _ilsc.Password ;
                _ilsc.SetItem();
            }
        }
        public void LoadSetting(string filename)
        {
            try
            {
                using (BinaryReader br = new BinaryReader(new FileStream(filename, FileMode.Open, FileAccess.Read)))
                {
                    int len = 0;    //待读取的文件长度
                    byte[] value = null;    //缓冲数据
                    string assemblyName = string.Empty;
                    List<string> files = new List<string>();
                    len = br.ReadInt32(); //读取反射类全名类型
                    value = br.ReadBytes(len);
                    assemblyName = Encoding.UTF8.GetString(value);
                    
                    if (_ilsc == null)
                    {
                        if (LinkManagementClass.Default.Items.ContainsKey(assemblyName) && File.Exists (LinkManagementClass.Default.Items[assemblyName].AssemblyFileName))
                        {
                                Assembly a = Assembly.LoadFile(LinkManagementClass.Default.Items[assemblyName].AssemblyFileName);
                                _ilsc = (ILinkStringCreate)a.CreateInstance(assemblyName);
                                _ilsc.SetItemData += new SetItemDataHandler(_ilsc_SetItemData);
                        }
                        else
                        {
                            return;
                        }
                    }

                    len = br.ReadInt32();
                    _ilsc.Load(br.BaseStream,len);
                    ReloadItemValue();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
