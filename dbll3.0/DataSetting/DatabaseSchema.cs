using System;
using System.Collections.Generic;
using System.Text;
using DataSetting;

namespace dbll3
{
    [Serializable()]
    public class DatabaseSchema
    {
        /// <summary>
        /// 数据库表成员集合
        /// </summary>
        private List<DataSchema> dataschemaList;

        /// <summary>
        /// 数据库表成员集合
        /// </summary>
        public System.Collections.Generic.List<DataSchema> DataSchemaList
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

    }
}
