using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Carsupplies.Business;

namespace Carsupplies
{
    public partial class FormMain : Form
    {
        private bool SubWindowsOnTableModels= false;
        private bool _login = false;
        public FormMain()
        {
            InitializeComponent();
        }
       private  fbParent _CurrentItem = null;   //当前操作的业务窗体

        private void FormMain_Load(object sender, EventArgs e)
        {
            Login();
        }
        private void Login(bool reload=false )    //用户登录
        {
            if (reload)
            {
                Program._DBL_MAIN.LoadKey();
                Program._DBL_MAIN.LoadMessageList();
                Program._DBL_MAIN.LoadSqlCmdItems();
            }
            if (_CurrentItem != null)
            {
                this.BodysplitContainer.Panel1.Controls.Remove(_CurrentItem);
            }
            tabControl1.TabPages.Clear();
            hide_menu();
            _login = SubWindowsOnTableModels;
            SubWindowsOnTableModels = false;
            tabControl1.Visible = false;
            UserLogin fbp = new UserLogin();
            fbp.Parent = this.BodysplitContainer.Panel1;
            _CurrentItem = fbp;
            fbp.Closed += new ClosedEventHandler(fbp_Closed);
            fbp.DBLClass = Program._DBL_MAIN;
            fbp.Show();
            窗体模式ToolStripMenuItem.Checked = SubWindowsOnTableModels;
        }
        private void NewWindow(fbParent fbp) //新窗体或新选项卡
        {
            TabPage tp = new TabPage(fbp.Name);
            tp.Name = fbp.Name;
            tp.BackColor = this.BackColor;
            fbp.CloseButtonShow = false;
            tp.Controls.Add(fbp);
            tabControl1.TabPages.Add(tp);
            tabControl1.SelectedTab = tp;
            tp.Show();
        }
        private void fbp_Closed(object sender, ClosedArgs e)
        {
            if (e.Closing && e.DialogResult == DialogResult.OK)
            {
                set_premission(e.Data);
                this.BodysplitContainer.Panel1.Controls.Remove(_CurrentItem);
                SubWindowsOnTableModels = _login;
                if(SubWindowsOnTableModels )
                {
                    tabControl1.Visible = true;
                }
                _CurrentItem = null;
            }
            else
            {
                Application.Exit();
            }
        }
        private void Item_Closed(object sender, ClosedArgs e)
        {
            if (e.Closing && _CurrentItem !=null)
            {
                this.BodysplitContainer.Panel1.Controls.Remove(_CurrentItem);
            }
        }
        private void Closelabel_Click(object sender, EventArgs e)
        {
            tabControl1.TabPages.RemoveAt (tabControl1.SelectedIndex);
            if (tabControl1.TabPages.Count== 0)
            {
                //Closelabel.Visible = false ;  //设置关闭按钮显示
            } 
        }
        private void Add_Control(fbParent c, string tableName = "") 
        {
            if (SubWindowsOnTableModels && tabControl1.TabPages.ContainsKey(tableName))
                {
                    tabControl1.SelectTab(tableName);
                    return;
            }
            c.DBLClass = Program._DBL_MAIN;
            c.Closed += new ClosedEventHandler(Item_Closed);
            c.Dock = DockStyle.Fill;

            //如果是TABPage 处理。。。
            if (SubWindowsOnTableModels)
            {
                if (tabControl1.TabPages.Count > 0)
                {
                    //Closelabel.Visible = true;  //设置关闭按钮显示
                }
                c.Name = tableName;
                NewWindow(c);
            }
            else
            {
                this.BodysplitContainer.Panel1.Controls.Remove(_CurrentItem);
                c.Parent = this.BodysplitContainer.Panel1;
                c.CloseButtonShow = true;
            }
            //不是
            _CurrentItem = c;
            c.Name = tableName;
            c.Show();
        }
        private void 窗体模式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            窗体模式ToolStripMenuItem.Checked = !窗体模式ToolStripMenuItem.Checked;
            SubWindowsOnTableModels = 窗体模式ToolStripMenuItem.Checked;
            if (SubWindowsOnTableModels)
            {
                if (_CurrentItem != null)
                {
                    this.BodysplitContainer.Panel1.Controls.Remove(_CurrentItem);
                    NewWindow(_CurrentItem);
                }
                tabControl1.Visible = true;
            }
            else
            {
                tabControl1.Visible = false;
                tabControl1.TabPages.Clear();
                if (_CurrentItem != null)
                {
                    _CurrentItem.Parent = this.BodysplitContainer.Panel1;
                    _CurrentItem.CloseButtonShow = true;
                }
            }
        }
        private void hide_menu()
        {
            menuStrip1.Visible = false;
        }
        private void set_premission(string data)
        {
            //设置菜单

            //设置工具条

            //显示菜单
            menuStrip1.Visible = true;

        }

        private void 测试ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UserLogin bf = new UserLogin();
            bf.DBLClass = Program._DBL_MAIN;
            bf.Parent = this.BodysplitContainer.Panel1;
            bf.Closed += new ClosedEventHandler(Item_Closed);
            bf.Show();
        }
        #region 
        private void 工具条ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            工具条ToolStripMenuItem.Checked = !工具条ToolStripMenuItem.Checked;
            toolStrip1.Visible = 工具条ToolStripMenuItem.Checked;
        }

        private void 状态栏ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            状态栏ToolStripMenuItem.Checked = !状态栏ToolStripMenuItem.Checked;
            statusStrip1.Visible = 状态栏ToolStripMenuItem.Checked;
        }

        private void 左侧栏ToolStripMenuItem_Click(object sender, EventArgs e)
        {    
            this.ParentsplitContainer.Panel1Collapsed = 左侧栏ToolStripMenuItem.Checked;
            左侧栏ToolStripMenuItem.Checked = !左侧栏ToolStripMenuItem.Checked;

        }

        private void 右侧栏ToolStripMenuItem_Click(object sender, EventArgs e)
        {           
            this.BodysplitContainer.Panel2Collapsed = 右侧栏ToolStripMenuItem.Checked;
            右侧栏ToolStripMenuItem.Checked = !右侧栏ToolStripMenuItem.Checked;

        }

        private void 关于AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 abone = new AboutBox1();
            abone.ShowDialog();
        }

        #endregion
        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //其他事宜处理

            Application.DoEvents();
            Application.Exit();
        }

        private void 重新登陆ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Login(true);
        }

        private void BodysplitContainer_Panel1_Resize(object sender, EventArgs e)
        {
            if (!SubWindowsOnTableModels)
            {
                if (BodysplitContainer.Panel1.Controls.Count > 0 && BodysplitContainer.Panel1.Controls[0] != null)
                {
                    fbParent f = null;
                    foreach (Control c in BodysplitContainer.Panel1.Controls)
                    {
                        f = c as fbParent;
                        if (f!=null && f.Dock != DockStyle.Fill)
                        {
                            c.Left = (BodysplitContainer.Panel1.Width - c.Width) / 2;
                            c.Top = (BodysplitContainer.Panel1.Height - c.Height) / 2;
                        }
                    }
                }
            }
        }

        private void 客户管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Custom bf = new Custom();
            Add_Control(bf, "客户管理");
        }

        private void 产品管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Products bf = new Products();
            Add_Control(bf, "产品管理");
        }

        private void FormMain_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && _CurrentItem !=null)
            {
                _CurrentItem.AcceptButton();
            }
        }

        private void 业务ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Business.Business bus = new Business.Business();
            Add_Control(bus,"业务");
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage != null)
            {
                _CurrentItem = e.TabPage.Controls[0] as fbParent;
            }
        }

        private void 业务类型设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BusinessType bt = new BusinessType();
            Add_Control(bt, "业务类型设置");
        }

        private void 切换用户ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Login();
        }

        private void tabControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Y < 20 &&tabControl1.SelectedTab !=null)
            {
                _CurrentItem = null;
                tabControl1.TabPages.Remove(tabControl1.SelectedTab);
            }
        }



    }
}
