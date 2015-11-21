using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dbl;
using System.IO;

namespace Carsupplies.Business
{
    public partial class Business : Carsupplies.fbParent
    {
        private BindingSource topbs = null;
        private DataView itembs=null;
        /// <summary>
        /// 子表关联列的数据
        /// </summary>
        private object _Linkdata = Convert.DBNull ,_LastLinkData=Convert.DBNull ;
        /// <summary>
        /// 子表关联的列名
        /// </summary>
        private string _FilterItemColumn = string.Empty;    
        /// <summary>
        /// 父表关联的列名，架构设计是自增列，建议使用，不然要考虑应用与后台数据库上的设计关联
        /// </summary>
        private string _FilterTopColumn = string.Empty;
        /// <summary>
        /// 父表临时数据存储列，用于子表数据更新
        /// </summary>
        private string _FilterTopLinkColumn = string.Empty;
        public Business()
        {
            InitializeComponent();
        }
        private static Business _default = null;
        public static Business Default
        {
            get
            {
                if (_default == null || _default.IsDisposed)
                {
                    _default = new Business();
                }
                return _default;
            }
        }
        private void Business_Load(object sender, EventArgs e)
        {
            ModelName = "业务";
            if (!_dblClass.ActionList.ContainsKey("BM_NONE"))
            {
                //错误
            }
            _sqlcommand = _dblClass.ActionList["BM_NONE"];
            reload();
        }

        private void reload()
        {
            _sqlcommand.ClearData();
            if (_dblClass.ReadData("BM_NONE", ref _sqlcommand, new List<object[]>()) != 0)
            {
                //错误

            }
            topbs = new BindingSource(_sqlcommand.SQLDataSet, _sqlcommand.SQLList[0].Data.TableName);
            topbs.CurrentChanged += new EventHandler(topbs_CurrentChanged);
            this.bindingNavigator1.BindingSource = topbs;
            //itembs = new BindingSource(_sqlcommand.SQLDataSet, _sqlcommand.SQLList[1].Data.TableName);
            itembs = new DataView(_sqlcommand.SQLList[1].Data);

            _FilterItemColumn = "bGUID";
            _FilterTopColumn = "bGUID";
            _FilterTopLinkColumn = "id";
            itembs.Sort = string.Format("{0} asc", _FilterItemColumn);
            //DataColumn dcLinkA= _sqlcommand.SQLList[0].Data.Columns[_FilterTopLinkColumn];
            //dcLinkA.AutoIncrement = true;
            //dcLinkA.AutoIncrementSeed = 0;
            //dcLinkA.AutoIncrementStep = -1;
            //DataColumn dcLinkB = _sqlcommand.SQLList[1].Data.Columns[_FilterTopLinkColumn];
            //dcLinkB.AutoIncrement = true;
            //dcLinkB.AutoIncrementSeed = 0;
            //dcLinkB.AutoIncrementStep = -1;

            //DataColumn dca = _sqlcommand.SQLList[0].Data.Columns[_FilterTopColumn];
            ////dca.DefaultValue = _dblClass.getLinkDataSerialNumber();
            //DataColumn dcb = _sqlcommand.SQLList[1].Data.Columns[_FilterItemColumn];
            //ForeignKeyConstraint fkc = new ForeignKeyConstraint("", dca, dcb);

            ////'Set null values when a value is deleted.
            //fkc.DeleteRule = Rule.Cascade ;
            //fkc.AcceptRejectRule = AcceptRejectRule.Cascade;
            //'Add the constraint, and set EnforceConstraints to true.
            //_sqlcommand.SQLList[1].Data.Constraints.Add(fkc);
            //_sqlcommand.SQLDataSet.EnforceConstraints = true;

            //DataRelation drtmp = new DataRelation("", dca, dcb);
            //_sqlcommand.SQLDataSet.Relations.Add(drtmp);


            get_LinkColData();

            this.ItemdataGridView.DataSource = itembs;
            set_TopDataColumn(_sqlcommand.ColumnSetting, _sqlcommand.SQLList[0].Data);
            _dblClass.set_dataColumn(_sqlcommand.ColumnSetting, this.ItemdataGridView);
        }

        private void set_TopDataColumn(Dictionary<string, ColumnSetting> dcs, DataTable dt)
        {
            TopflowLayoutPanel.SuspendLayout();
            ColumnSetting cs = null;

            foreach (DataColumn dc in dt.Columns)
            {
                if (dcs.ContainsKey(dc.ColumnName) && dcs[dc.ColumnName].Visiable)
                {
                    cs = dcs[dc.ColumnName];
                    TableLayoutPanel tlp = new TableLayoutPanel();

                    tlp.RowCount = 1;
                    tlp.Parent = TopflowLayoutPanel;
                    tlp.Height = 32;
                    tlp.Show();

                    Label lleft = new Label();
                    lleft.Text = dcs[dc.ColumnName].HeadText;
                    lleft.Anchor = AnchorStyles.None;
                    lleft.AutoSize = true;
                    lleft.Show();

                    Control rtb = null;
                    if (cs.ReadOnly)
                    {
                        rtb = new Label() {  BorderStyle= System.Windows.Forms.BorderStyle.FixedSingle };                  
                    }
                    else
                    {
                        rtb = new TextBox();
                    }

                    rtb.DataBindings.Add(new Binding("Text", topbs, dc.ColumnName, true));
                    rtb.Width = dcs[dc.ColumnName].Width;
                    rtb.Anchor = AnchorStyles.Left;
                    tlp.ColumnCount = 2;

                    rtb.Show();

                    tlp.Controls.AddRange(new Control[] { lleft, rtb });
                    tlp.SetColumn(lleft, 0);
                    tlp.SetColumn(rtb, 1);

                    //start list data link
                    if (cs.LinkData && !cs.ReadOnly)    //列表数据类型
                    {
                        rtb.Tag = cs;
                        //这种委托不知道是否是共用，还是每个实例一个，待确定
                        MouseEventHandler rtb_right = delegate(object sender, MouseEventArgs e)
                        {
                            if (e.Button == MouseButtons.Left)
                            {
                                //弹出窗体对话框，用于选择数据
                                SelectItemValue siv = SelectItemValue.Default;
                                TextBox rtb_tmp = (TextBox)sender;
                                ColumnSetting cs_rtb = (ColumnSetting)rtb_tmp.Tag;

                                siv.Text = string.Format("选择 {0} ...", cs_rtb.HeadText);
                                siv.SetColumnSetting(_dblClass, cs_rtb, ModelName, cs_rtb.HeadText);
                                if (siv.ShowDialog() == DialogResult.OK)
                                {
                                    rtb_tmp.Text = siv.SelectValue;
                                }
                            }
                        };
                        rtb.MouseDoubleClick += rtb_right;

                        //tlp.ColumnCount = 3;
                        //ComboBox cb = new ComboBox();
                        //cb.Width = 20;
                        //cb.DropDownWidth = 200;
                        //cb.DropDownStyle = ComboBoxStyle.DropDownList;
                        //cb.TabIndex = 0;
                        //_dblClass.RefreshColumnSettingLinkData(ref _sqlcommand, dc.ColumnName); //更新数据
                        //cb.DataSource = cs.LinkDataTable;
                        //cb.DisplayMember = cs.LinkColumnName ;
                        ////cb.DataBindings.Add("Text", cs.LinkDataTable, "Value");
                        //EventHandler _Commit = delegate(object sender, EventArgs e)
                        // {
                        //     rtb.Text = cb.Text;
                        // };
                        //cb.SelectionChangeCommitted += _Commit;
                        //cb.Show();

                        //tlp.Controls.Add(cb);
                        //tlp.SetColumn(cb, 2);
                    }
                    //end list data link
                    tlp.AutoSize = true;
                    tlp.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                }

                //有些列后添加的，但是需要显示，只是没有配置信息的情况  
                //此情况未处理
            }
            TopflowLayoutPanel.ResumeLayout();
        }

        private void ItemdataGridView_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ////弹出窗体对话框，用于选择数据
            //SelectItemValue siv = new SelectItemValue();
            //TextBox rtb_tmp = (TextBox)sender;
            //ColumnSetting cs_rtb = (ColumnSetting)rtb_tmp.Tag;

            //siv.Text = string.Format("选择 {0} ...", cs_rtb.HeadText);
            //siv.SetColumnSetting(_dblClass, cs_rtb, ModelName, cs_rtb.HeadText);
            //if (siv.ShowDialog() == DialogResult.OK)
            //{
            //    rtb_tmp.Text = siv.SelectValue;
            //}
           // MessageBox.Show(string.Format ("col:{0} \t row:{1}",e.ColumnIndex, e.RowIndex ));
        }

        private void ItemdataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > 0)
            {
                string colname = ItemdataGridView.Columns[e.ColumnIndex].Name;
                ColumnSetting cs = null;
                if (_dblClass.get_ColumnSetting(colname, out cs) && cs.LinkData)
                {
                    if (ItemdataGridView.Rows[e.RowIndex].IsNewRow)
                    {
                        itembs.AddNew();
                        set_ColumnDefaultValue(this.ItemdataGridView.Rows[e.RowIndex ]);
                        ItemdataGridView.Rows[e.RowIndex].Cells[_FilterItemColumn].Value =_Linkdata;
                    }
                    SelectItemValue siv = SelectItemValue.Default;
                    siv.Text = string.Format("选择 {0} ...", cs.HeadText);
                    siv.SetColumnSetting(_dblClass, cs, ModelName, cs.HeadText);
                    if (siv.ShowDialog() == DialogResult.OK)
                    {
                        ItemdataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = siv.SelectValue;
                        if (cs.LinkData)
                        {
                            foreach (DataGridViewColumn dc in ItemdataGridView.Columns)
                            {
                                if (cs.LinkDataColumnEx.ContainsKey(dc.Name) )//&& !Convert.IsDBNull(cs.LinkDataColumnEx[dc.Name]))
                                {
                                    ItemdataGridView.Rows[e.RowIndex].Cells[dc.Name].Value = cs.LinkDataColumnEx[dc.Name];
                                }
                            }
                        }
                    }
                }
            }
        }

        private void ItemdataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                DataGridViewRow currentRow = this.ItemdataGridView.Rows[e.RowIndex];
                currentRow.HeaderCell.Value = Convert.ToString(e.RowIndex + 1);
            }
        }

        private void NewtoolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void FindtoolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void SavetoolStripButton_Click(object sender, EventArgs e)
        {
            //数据保存只保存当前项数据，有错误提示，与父项数据保存时操作有区别。
            //首先是检查带保存的数据
            //有错误或问题数据时提示
            if (this.topbs.Current != null)
            {
                DataRowView drv = this.topbs.Current as DataRowView;
                if (drv.IsNew)
                {
                    this.topbs.EndEdit();
                }
                get_xml(_sqlcommand.SQLDataSet);
                fromNoteShow fns = new fromNoteShow();
                fns.Set_NoteText(Clipboard.GetText());
                fns.Show();
            }
            _save_data();
        }
        private void _save_data()
        {
            _sqlcommand.SQLList[0].OnUpdated = true;
            _sqlcommand.SQLList[1].OnUpdated = true;
            CheckCurrentData();
            _dblClass.Update(_sqlcommand);
        }
        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            if (topbs.Count > 1)
            {
                _save_data();
            }
            DataRowView drv = topbs.Current as DataRowView;
            if (drv != null)
            {
                drv.Row[_FilterTopColumn]=_dblClass.getLinkDataSerialNumber();
                _Linkdata = drv.Row[_FilterTopColumn];
                string tmpf = string.Format("{0}={1}", _FilterItemColumn, _Linkdata);
                //this.itembs.Filter = tmpf;
                itembs.RowFilter = tmpf;
            }
        }

        private void topbs_CurrentChanged(object sender, EventArgs e)
        {
            //数据保存只保存当前项数据，有错误提示，与子项数据保存时操作有区别。
            //首先是检查带保存的数据
            //有错误或问题数据时，不是新添加数据时，提示，否则直接删除
            _LastLinkData = _Linkdata;
            get_LinkColData();

        }
        /// <summary>
        /// 设置当前数据与子表的关联信息
        /// </summary>
        private void get_LinkColData() // 设置当前数据与子表的关联信息
        {
            DataRowView drv = topbs.Current as DataRowView;
            if (drv != null && ! Convert.IsDBNull (drv.Row[_FilterTopColumn]))
            {
                _Linkdata = drv.Row[_FilterTopColumn];
                    string tmpf = string.Format("{0}={1}", _FilterItemColumn, _Linkdata);
                    //this.itembs.Filter = tmpf;
                    itembs.RowFilter = tmpf;
            }
        }
        /// <summary>
        /// 检查当前即将保存的数据是否符合存储要求
        /// </summary>
        /// <returns></returns>
        private bool CheckCurrentData() //检查当前即将保存的数据是否符合存储要求
        {
            string tmp = string.Format("{0} is NULL",  _FilterItemColumn);
           DataRow[] rows= _sqlcommand.SQLList[1].Data.Select(tmp);
           foreach (DataRow dr in rows)
           {
               _sqlcommand.SQLList[1].Data.Rows.Remove(dr);
           }
            return true;
        }
        private void get_xml(DataSet ds)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                foreach (DataTable dt in ds.Tables)
                {
                    dt.WriteXml(ms);
                }
                ms.Seek(0, SeekOrigin.Begin);
                using (StreamReader sr = new StreamReader(ms))
                {
                    Clipboard.SetText(sr.ReadToEnd());
                    
                }
            }
        }

        private void RefreshtoolStripButton_Click(object sender, EventArgs e)
        {
            _sqlcommand.ClearData();
            if (_dblClass.ReadData("BM_NONE", ref _sqlcommand, new List<object[]>()) != 0)
            {
                //错误

            }
        }

        /// <summary>
        /// 设置列默认值
        /// </summary>
        /// <param name="dgvr"></param>
        private void set_ColumnDefaultValue(DataGridViewRow dgvr) // 设置列默认值
        {
            object value = null;

            foreach (DataGridViewColumn dgvc in ItemdataGridView.Columns)
            {
                if (_sqlcommand.get_ColumnDefault(dgvc.Name, out value)     //取列默认值
                        &&! _sqlcommand.ColumnSetting[dgvc.Name ].IdentityIS //此列不是自增列
                    && Convert.IsDBNull ( dgvr.Cells[dgvc.Name].Value) )
                {
                    
                    dgvr.Cells[dgvc.Name].Value = value;
                }
            }
        }
        private void ItemdataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (e.RowCount > 0 && this.ItemdataGridView.Columns.Contains(_FilterItemColumn))
            {
                for (int i = 0; i < e.RowCount-1; i++)
                {
                    set_ColumnDefaultValue(this.ItemdataGridView.Rows[e.RowIndex + i]);
                    this.ItemdataGridView.Rows[e.RowIndex + i].Cells[_FilterItemColumn].Value =  _Linkdata;
                }
            }
        }

        private void ItemdataGridView_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            //有列值是有其他列计算而来的，在这里设置

        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            if (!Convert.IsDBNull(_LastLinkData))
            {
                string tmpf = string.Format("{0}={1}", _FilterItemColumn, _LastLinkData);
                itembs.RowFilter = tmpf;
                int i=-1;
                while (true)
                {
                    i = itembs.Find(_LastLinkData);
                    if (i > -1)
                    {
                        itembs.Delete(i);
                    }
                    else
                    {
                        break;
                    }
                }
                tmpf = string.Format("{0}={1}", _FilterItemColumn, _Linkdata );
                itembs.RowFilter = tmpf;
            }
        }
    }
}
