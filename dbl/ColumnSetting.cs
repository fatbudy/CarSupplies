using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data ;


namespace dbl
{
    public class ColumnSetting
    {
        public int id { get; set; }
        public string ColName { get; set; }
        public bool Visiable { get; set; }
        public string HeadText { get; set; }
        public int Width { get; set; }
        public int Localtion { get; set; }
        public bool LinkData { get; set; }
        public int LinkDataSQLString { get; set; }
        public DataTable LinkDataTable { get; set; }
        public string LinkColumnName { get; set; }
        public Dictionary <string,object > LinkDataColumnEx { get; set; }
        public string ActionName { get; set; }
        public object  DefaultValue { get; set; }
        public bool ReadOnly { get; set; }
        public bool IdentityIS { get; set; }
        public object Expression { get; set; }
        public bool HaveDefaultValue()
        {
            return !Convert.IsDBNull(DefaultValue); 
        }
        public bool HaveComputeExpression()
        {
            return !Convert.IsDBNull(Expression);
        }
    }

}
