using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using idblink;
using System.IO;

namespace dbll3.LinkManage
{
    public partial class LinkStringCreate : Form
    {
        PluginItemClass _pi = null;
        private string filename = string.Empty;
        public string FileName
        {
            get
            {
                return string.IsNullOrEmpty(filename) ? LinkManagementClass.DefaultDBSettingFilename : filename;
            }
            set
            {
                if (!value.Equals(filename))
                {
                    filename = value;
                }
            }
        }
        public LinkStringCreate()
        {
            InitializeComponent();
            InitializePlugin();
        }

        private void InitializePlugin()
        {            
            //设定了配置文件时，需要解决加载默认配置文件的问题
            LinkManagementClass.Default.Search();
            foreach (var tmp in LinkManagementClass.Default.Items)
            {
                TypecomboBox.Items.Add(tmp.Key);
            }
            if (LinkManagementClass.Default.Items.Count > 0)
            {
                string key = LinkManagementClass.Default.Items.Keys.ToArray()[0];
                _pi = LinkManagementClass.Default.Items[key];

 
                if (File.Exists(LinkManagementClass.DefaultDBSettingFilename ))
                {
                    editDBLink1.LoadSetting(LinkManagementClass.DefaultDBSettingFilename);
                }
                else
                {               
                    Assembly a = Assembly.LoadFile(_pi.AssemblyFileName);
                    editDBLink1.LinkStringCreate = (ILinkStringCreate)a.CreateInstance(_pi.AssemblyName);
                    if (a != null)
                    {
                        GC.SuppressFinalize(a);
                    }
                }
                TypecomboBox.Text =key;
            }
        }
        private void Savebutton_Click(object sender, EventArgs e)
        {
            editDBLink1.SaveItemValue();
            if (editDBLink1.LinkStringCreate.Checked())
            {                 
                try
                {
                    if(string.IsNullOrEmpty (filename ))
                    {
                        filename =LinkManagementClass.DefaultDBSettingFilename;
                    }
                    using (BinaryWriter bw = new BinaryWriter(new FileStream(filename , FileMode.Create, FileAccess.Write)))
                    {   
                        byte[] value =Encoding.UTF8.GetBytes(_pi.AssemblyName);
                        bw.Write(value.Length);
                        bw.Write(value);
                        using (MemoryStream ms = new MemoryStream())
                        {
                            editDBLink1.LinkStringCreate.Save(ms);
                            value = ms.ToArray();
                        }
                        bw.Write(value.Length);
                        bw.Write(value);
                    }
                    MessageBox.Show("配置信息已经保存！","成功",MessageBoxButtons.OK ,MessageBoxIcon.Information );
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("保存失败，参见错误消息：{0}", ex.Message),"失败",MessageBoxButtons.OK ,MessageBoxIcon.Error );
                }
            }
        }

        private void Cancelbutton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel ;
            this.Close();
        }

        private void TypecomboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (TypecomboBox.Text != null && LinkManagementClass.Default.Items.Keys .Contains(TypecomboBox.Text ))
            {
                _pi = LinkManagementClass.Default.Items[TypecomboBox.Text];
                Assembly a = Assembly.LoadFile(_pi.AssemblyFileName);
                editDBLink1.LinkStringCreate = (ILinkStringCreate)a.CreateInstance(_pi.AssemblyName);
                if (a != null)
                {
                    GC.SuppressFinalize(a);
                }
            }
        }

    }
}
