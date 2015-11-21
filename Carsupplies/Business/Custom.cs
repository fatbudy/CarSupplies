using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dbl;

namespace Carsupplies.Business
{
    public partial class Custom : Carsupplies.fbParent
    {
        private static Custom _default = null;
        public static Custom Default
        {
            get
            {
                if (_default == null || _default.IsDisposed)
                {
                    _default = new Custom();
                }
                return _default;
            }
        }

        public Custom()
        {
            InitializeComponent();
        }

        private void Custom_Load(object sender, EventArgs e)
        {
            if (!_dblClass.ActionList.ContainsKey("CM_NONE"))
            {
                //错误
            }
            _sqlcommand = _dblClass.ActionList["CM_NONE"];
            reload();
        }
        private void reload()
        {
            if (_dblClass.ReadData("CM_NONE", ref _sqlcommand, new List<object[]>()) != 0)
            {
                //错误
            }
            //this.CustomdataGridView.DataBindings.Clear();
            //this.CustomdataGridView.DataBindings.Add(
            //    new Binding("DataSource", _sqlcommand.SQLDataSet, _sqlcommand.SQLList[0].Data.TableName));
            this.CustomdataGridView.DataSource = _sqlcommand.SQLDataSet;
            this.CustomdataGridView.DataMember = _sqlcommand.SQLList[0].Data.TableName;
            _dblClass.set_dataColumn(_sqlcommand.ColumnSetting, this.CustomdataGridView);
        }


        private void Savebutton_Click(object sender, EventArgs e)
        {
            _sqlcommand.SQLList[0].OnUpdated = true;
            _dblClass.Update(_sqlcommand);
            //reload();
        }


    }
}
