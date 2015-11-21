using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Carsupplies.Business
{
    public partial class BusinessType : Carsupplies.fbParent
    {
        public BusinessType()
        {
            InitializeComponent();
        }

        private void BusinessType_Load(object sender, EventArgs e)
        {
            ModelName = "业务类型设置";

            if (!_dblClass.ActionList.ContainsKey("BTS_NONE"))
            {
                //错误
            }
            _sqlcommand = _dblClass.ActionList["BTS_NONE"];
            reload();
        }
        private void reload()
        {
            if (_dblClass.ReadData("BTS_NONE", ref _sqlcommand, new List<object[]>()) != 0)
            {
                //错误
            }
            this.TypedataGridView.DataSource = _sqlcommand.SQLDataSet;
            this.TypedataGridView.DataMember = _sqlcommand.SQLList[0].Data.TableName;
            _dblClass.set_dataColumn(_sqlcommand.ColumnSetting, this.TypedataGridView);
        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
            _sqlcommand.SQLList[0].OnUpdated = true;
            _dblClass.Update(_sqlcommand);
            //reload();
        }
    }
}
