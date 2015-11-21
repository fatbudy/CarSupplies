using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Threading;
using System.IO;
using System.Reflection;
namespace OutPutData
{
    /// <summary>
    /// 导出DataTable数据至文件
    /// </summary>
    /// <remarks>待添加导出DataSet数据的方法  2015-03-03</remarks>
    public sealed class ExportData : IExportData
    {
        /// <summary>
        /// 前台UI线程
        /// </summary>
        private SynchronizationContext _sc = null;
        /// <summary>
        /// 前台用于UI属性更新的为过函数
        /// </summary>
        private SendOrPostCallback  _up_e = null;
        
        /// <summary>
        /// 设置前台UI线程及更新委托
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="spc"></param>
        public void set_sc(SynchronizationContext sc,SendOrPostCallback spc)
        {
            _sc = sc;
            _up_e = spc;
        }

        /// <summary>
        /// 数据导出进度事件
        /// </summary>
        public event ExportProgressCompletedEventHandler ExportProgressCompleted;
        /// <summary>
        /// 数据导出状态消息
        /// </summary>
        public event ExportStateEventHandler ExportState;

        /// <summary>
        /// 导出数据进度主控制
        /// </summary>
        private bool pe_on = true, sub_on = false;// 导出数据进度主控制
        /// <summary>
        /// 数据表导出时当前导出的数据行位置
        /// </summary>
        private int _index = -1, _count = -1;// 数据表导出时当前导出的数据行位置
        private uint _stepindex = 0, _stepcount = 0; //多个数据表导出时使用
        private bool _isBatchExport = false;
        /// <summary>
        /// 导入导出的数据存储表
        /// </summary>
        private DataTable _data = null;// 导入导出的数据存储表
        /// <summary>
        /// 待导出数据的数据集
        /// </summary>
        private DataSet _dataset = null;    // 待导出数据的数据集
        /// <summary>
        /// 是否是数据集数据导出
        /// </summary>
        private bool _isdataset = false;// 是否是数据集数据导出

        /// <summary>
        /// 待输出的数据集
        /// </summary>
        public DataTable Data// 待输出的数据集
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
            }
        }
        /// <summary>
        /// 待导出数据的数据集
        /// </summary>
        public DataSet DataSet { get { return _dataset; } }// 待导出数据的数据集
        public ExportData()
        {
            pe_on = true;
            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(progress_event));
            _filter_string = "文本文件(*.txt)|*.txt|XML 文件(*.xml)|*.xml";
            //_filter_string = string.Format ("文本文件(*.txt)|*.txt|XML 文件(*.xml)|*.xml|{0}",ExcelClass.GetExcelFilter());
        }
        ~ExportData()
        {
            pe_on = false;
        }
        private void raise_ExportState(ExportArgs e)
        {
            if (ExportState != null)
            {
                ExportState.Invoke(this, e);
            }
        }
        /// <summary>
        /// 数据进度委托，以便在UI线程中同步
        /// </summary>
        private void set_pe(ExportProgressArgs e)// 数据进度委托，以便在UI线程中同步
        {
            if (_sc != null && _up_e != null)
            {
                _sc.Send(_up_e, e);
            }
        }
        /// <summary>
        /// 数据输出进度显示
        /// </summary>
        private void progress_event(object data)// 数据输出进度显示
        {
            while (pe_on)
            {
                if (sub_on)//&& _count > 0 && _index > -1)
                {
                    ExportProgressArgs e = new ExportProgressArgs();
                    e.Index = _index;
                    e.Count = _count;
                    e.StepIndex = _stepindex;
                    e.Steps = _stepcount;

                    set_pe(e);

                    if (((_isBatchExport && e.IsBatchCompleted) //批量导出模式
                            ||
                               !_isBatchExport)    //非批量，单数据集模式
                            && (_index >= _count))
                    {
                        sub_on = false;
                        if (ExportProgressCompleted != null)
                        {
                            ExportProgressCompletedArgs ec = new ExportProgressCompletedArgs(true);
                            ExportProgressCompleted.Invoke(this, ec);
                        }
                    }

                    Thread.Sleep(1000);
                }
                else
                {
                    System.Threading.Thread.Sleep(100);
                }
            }
        }
        /// <summary>
        /// 将数据表信息转换成文本数据
        /// </summary>
        /// <param name="splitChar">数据列之间的分割字符</param>
        /// <returns></returns>
        public string get_text(string splitChar = "\t") // 将数据表信息转换成文本数据
        {
            StringBuilder sb = new StringBuilder();

            Action<DataTable> objex = delegate(DataTable dt)
            {
                if (dt == null)
                    return;

                foreach (DataColumn dc in dt.Columns)
                {
                    sb.AppendFormat("{0}{1}", dc.ColumnName, splitChar);
                }
                sb.AppendLine();
                foreach (DataRow dr in dt.Rows)
                {
                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (Convert.IsDBNull(dr[dc.ColumnName]))
                        {
                            sb.Append(splitChar);
                        }
                        else
                        {
                            sb.AppendFormat("{0}{1}", dr[dc.ColumnName].ToString(), splitChar);
                        }
                    }
                    sb.AppendLine();
                }
            };
            if (_isdataset)
            {

                for (int i = 0; i < _dataset.Tables.Count ; i++)
                {
                    sb.AppendLine(string. Format("{0} owner flow data.", _dataset.Tables[i].TableName ));                    
                    objex(_dataset.Tables[i]);
                    sb.AppendLine ();
                }
            }
            else
            {
                objex(_data);
            }
            return sb.ToString();
        }
        /// <summary>
        /// 导出数据并保存至指定名称的文本文件中
        /// </summary>
        /// <param name="filename">保存的文本文件名</param>
        /// <param name="spc">数据列分割字符</param>
        /// <returns></returns>
        public bool export_txt(string filename, string spc) // 导出数据并保存至指定名称的文本文件中
        {
            string tmp = get_text(spc);

            try
            {
                using (StreamWriter sw = new StreamWriter(filename, false, Encoding.UTF8))
                {
                    sw.WriteLine(tmp);
                }
                return true;
            }
            catch (Exception ex)
            {
                raise_ExportState(new ExportArgs(string.Format("数据导出失败，{0}", ex.Message), -1));
            }
            return false;
        }
        /// <summary>
        /// 将数据表信息导出至XML文件
        /// </summary>
        /// <param name="filename">存储数据的xml文件名</param>
        /// <returns></returns>
        public bool export_xml(string filename) // 将数据表信息导出至XML文件
        {
            try
            {

                sub_on = true;
                //_data.WriteXmlSchema (@"d:\d.xml",true );
                //_data.WriteXml(@"d:\dd.xml",XmlWriteMode.WriteSchema ,true);
                using (StreamWriter sw = new StreamWriter(filename, false, Encoding.UTF8))
                {
                    sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");

                    Action<DataTable> objds = delegate(DataTable _dt)
                    {
                        _index = 0;
                        _count = _dt.Rows.Count;
                        sw.WriteLine("<Table>");
                        sw.WriteLine("<Name>{0}</Name>", _dt.TableName);
                        foreach (DataRow dr in _dt.Rows)
                        {
                            sw.WriteLine("<row>");
                            foreach (DataColumn dc in _dt.Columns)
                            {
                                if (Convert.IsDBNull(dr[dc.ColumnName]))
                                {
                                    sw.WriteLine("<{0}></{1}>", dc.ColumnName, dc.ColumnName);
                                }
                                else
                                {
                                    sw.WriteLine("<{0}>{1}</{2}>", dc.ColumnName, replaceCharbyXML(dr[dc.ColumnName].ToString()), dc.ColumnName);
                                }
                            }
                            _index += 1;
                            sw.WriteLine("</row>");
                        }
                        sw.WriteLine("</Table>");
                    };

                    if (_isdataset)
                    {
                        _isBatchExport = true;
                        _stepcount = (uint)_dataset.Tables.Count;
                        _stepindex = 1;
                        sw.WriteLine("<DataSet>");
                        for (int i = 0; i < _stepcount; i++)
                        {
                            objds(_dataset.Tables[i]);
                            _stepindex = (uint)i + 1;
                        }
                        sw.WriteLine("</DataSet>");
                    }
                    else
                    {
                        objds(_data);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                raise_ExportState(new ExportArgs(string.Format("数据导出失败，{0}", ex.Message), -1));
            }
            return false;
        }
        //public void ExportToClipboard()
        //{
        //    System.Windows.Forms.Clipboard.SetText(get_text());
        //}
        /// <summary>
        /// 替换xml文件中需要转义的字符
        /// </summary>
        private string replaceCharbyXML(string sour)
        {
            return sour.Replace("<", "&lt;").Replace(">", "&gt;").Replace("&", "&amp;").Replace("'", "&apos;").Replace("\"", "&quot;");
        }
        public void export_Excel(string filename)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(ve_Excel), filename);
        }
        /// <summary>
        /// 将数据输出至Excel
        /// </summary>
        private void ve_Excel(object data)// 将数据输出至Excel
        {
            string filename = data as string;
            if (string.IsNullOrEmpty(filename))
            {
                return;
            }

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            ExcelClass ecls = new ExcelClass(filename);
            int ex_count = 0;
            
            try
            {
                using (OleDbConnection ocon = new OleDbConnection(ecls.LinkString))
                {
                    ocon.Open();
                    using (OleDbCommand olcmd = ocon.CreateCommand())
                    {
                        olcmd.CommandType = CommandType.Text;

                        sub_on = true;
                        //导出单数据表操作的委托
                        Action<DataTable> objds = delegate(DataTable dt)
                        {
                            _index = 0;
                           _count = dt.Rows.Count;
                            StringBuilder sb = new StringBuilder(), sb_col = new StringBuilder();
                            if (dt.TableName.Length > 30 || string.IsNullOrEmpty(dt.TableName))
                            {
                                dt.TableName = "tmp_datatable";
                            }
                            //_data.TableName = "Sheet1$";
                            sb.AppendFormat("create table [{0}] (", dt.TableName);
                            sb_col.AppendFormat("insert into [{0}] (", dt.TableName);

                            foreach (DataColumn dc in dt.Columns)
                            {
                                if (dc.DataType.ToString().Equals("System.Single"))
                                {
                                    sb.AppendFormat("[{0}] float,", dc.ColumnName);
                                }
                                else if (dc.DataType.ToString().Equals("System.Double"))
                                {
                                    sb.AppendFormat("[{0}] float,", dc.ColumnName);
                                }
                                else if (dc.DataType.ToString().Equals("System.Int32"))
                                {
                                    sb.AppendFormat("[{0}] int,", dc.ColumnName);
                                }
                                else if (dc.DataType.ToString().Equals("System.DateTime"))
                                {
                                    sb.AppendFormat("[{0}] date,", dc.ColumnName);
                                }
                                else
                                {
                                    sb.AppendFormat("[{0}] varchar,", dc.ColumnName);
                                }
                                sb_col.AppendFormat("[{0}],", dc.ColumnName);
                            }
                            sb.Remove(sb.Length - 1, 1);
                            sb.Append(")");
                            sb_col.Remove(sb_col.Length - 1, 1);
                            sb_col.Append(") values (");

                            olcmd.CommandText = sb.ToString();
                            ex_count = olcmd.ExecuteNonQuery();
                            foreach (DataRow dr in dt.Rows)
                            {
                                sb.Remove(0, sb.Length);
                                sb.Append(sb_col.ToString());
                                foreach (DataColumn dc in dt.Columns)
                                {
                                    if (Convert.IsDBNull(dr[dc]))
                                    {
                                        if (dc.DataType.ToString().Equals("System.Single") || dc.DataType.ToString().Equals("System.Double"))
                                        {
                                            sb.Append("'0.0',");
                                        }
                                        else if (dc.DataType.ToString().Equals("System.Int32"))
                                        {
                                            sb.Append("'0',");
                                        }
                                        else if (dc.DataType.ToString().Equals("System.DateTime"))
                                        {
                                            sb.Append("'1900-1-1',");
                                        }
                                        else
                                        {
                                            sb.Append("'',");
                                        }
                                    }
                                    else
                                    {
                                        sb.AppendFormat("'{0}',", dr[dc].ToString());
                                    }
                                }
                                sb.Remove(sb.Length - 1, 1);
                                sb.Append(")");
                                olcmd.CommandText = sb.ToString();
                                ex_count = olcmd.ExecuteNonQuery();
                                _index += 1;
                            }
                        };//委托结束
                        
                        //判断是否是多个数据表导出（数据集）
                        if (_isdataset)
                        {
                          _isBatchExport  = true;
                            _stepcount =(uint) _dataset.Tables.Count;
                            _stepindex = 1;
                            for (int i=0 ;i<_stepcount ;i++)    //遍历数据集
                            {
                                objds(_dataset.Tables[i]);  //执行数据表数据导出
                                _stepindex =(uint)i+ 1;
                            }
                        }
                        else       //不是数据集时数据导出            
                        {
                            objds(_data);
                        }
                        //olcmd.CommandText = string.Format("drop table {0}$", _data.TableName);
                        //olcmd.ExecuteNonQuery();
                    }

                }
                
            }
            catch 
            {
                sub_on = false;
            }
        }
        /// <summary>
        /// 导出类型为excel时，在保存对话框中filter中的选项
        /// </summary>
        string _filter_string = "";
        /// <summary>
        /// 数据文件类型
        /// </summary>
        public string FileFilter
        {
            get { return _filter_string; }
        }
        //public string GetExcelVersion()
        //{
        //    object ObjApp;
        //    object Cells1;

        //    //反射取得类型
        //    Type ObjExcel = Type.GetTypeFromProgID("Excel.Application");
        //    if (ObjExcel == null)
        //    {
        //        raise_ExportState(new ExportArgs("请先安装Office Excel !", -3));
        //        filter_string = "Excel 文件(*.xls)|*.xls";
        //        return "8.0";
        //    }

        //    ObjApp = Activator.CreateInstance(ObjExcel);
        //    //Version 
        //    //设置可见属性 
        //    //获取Workbook集 
        //    Cells1 = ObjApp.GetType().InvokeMember("Version", BindingFlags.GetProperty, Type.DefaultBinder, ObjApp, null);
        //    float v = float.Parse(Cells1.ToString());
        //    if (v > 8.0)
        //    {
        //        filter_string = "Excel 文件(*.xlsx)|*.xlsx";
        //    }
        //    else
        //    {
        //        filter_string = "Excel 文件(*.xls)|*.xls";
        //    }
        //    return Cells1.ToString();
        //}
        /// <summary>
        /// 将dataTable数据输出至指定文件或数据库
        /// </summary>
        public void Export()
        {
            _ExportOtherType(_exportlinkstring, _exporttype);
        }
        /// <summary>
        /// 将dataTable数据输出至指定文件或数据库
        /// </summary>
        public void ExportOtherType(string filename, ExportTypeEnum type)
        {
            _ExportOtherType(filename, type);
        }
        /// <summary>
        /// 将dataTable数据输出至指定文件或数据库
        /// </summary>
        private void _ExportOtherType(string filename, ExportTypeEnum type)
        {
            if (_data == null)
            {
                raise_ExportState(new ExportArgs("没有数据，无法执行数据导出。", -2));
                return;
            }
            switch (type)
            {
                case ExportTypeEnum.Text:
                    export_txt(filename, "\t");
                    break;
                case ExportTypeEnum.Xml:
                    export_xml(filename);
                    break;
                case ExportTypeEnum.officeXML:
                    //export_officeXML(filename);
                    break;
                case ExportTypeEnum.Excel:
                    ThreadPool.QueueUserWorkItem(new WaitCallback(ve_Excel), filename);
                    break;
                case ExportTypeEnum.CSV:
                    export_txt(filename, ",");
                    break;
                default:
                    //这儿可以添加通过反射载入的其他数据导入格式
                    //接口格式如下
                    //datatable,linkstring,dataschema -->return bool
                    raise_ExportState(new ExportArgs("未指定导出格式", -1));
                    break;
            }

        }
        /// <summary>
        /// 数据输出方向即是数据库还是文件
        /// </summary>
        public ExportDirectionEnum Direction
        {
            get
            {
                return _direction;
            }
            set
            {
                _direction = value;
            }
        }
        /// <summary>
        /// 数据数据文件名或数据库连接字符串
        /// </summary>
        public string ExportLinkString
        {
            get { return _exportlinkstring; }
            set { _exportlinkstring = value; }
        }
        /// <summary>
        /// 设置用于输出的数据集
        /// </summary>
        public void SetData(DataTable dt)// 设置用于输出的数据集
        {
            if (dt != null)
            {
                _data = dt;
            }
        }
        /// <summary>
        /// 数据集与输出设置之间的关联
        /// </summary>
        public DataSetting.DataSchema SchemaSetting
        {
            get
            {
                return _schemasetting;
            }
            set
            {
                _schemasetting = value;
            }
        }
        /// <summary>
        /// 备注信息
        /// </summary>
        public string Summary
        {
            get
            {
                return _summary;
            }
            set
            {
                _summary = value;
            }
        }
        /// <summary>
        /// 数据输出类型
        /// </summary>
        public ExportTypeEnum ExportType
        {
            get
            {
                return _exporttype;
            }
            set
            {
                _exporttype = value;
            }
        }
        /// <summary>
        /// 数据导出方向
        /// </summary>
        private ExportDirectionEnum _direction;
        /// <summary>
        /// 接口IDATA中的summary
        /// </summary>
        private string _summary;
        /// <summary>
        /// 导入导出是，针对数据的不同设置
        /// </summary>
        private DataSetting.DataSchema _schemasetting;
        /// <summary>
        /// 导出类型控制
        /// </summary>
        private ExportTypeEnum _exporttype;
        /// <summary>
        /// 导出数据连接字符串或文件名
        /// </summary>
        private string _exportlinkstring;
        /// <summary>
        /// 输出指定数据连接中指定表的数据
        /// </summary>
        /// <param name="con">数据库连接</param>
        /// <param name="tn">表名称</param>
        /// <param name="et">导出的数据格式</param>
        /// <param name="replace">替换目标数据信息</param>
        public void Export(IDbConnection con, string tn, ExportTypeEnum et, bool replace)
        {
            throw new NotImplementedException();
        }
        public void ExportQueryString(IDbConnection con, string tn, ExportTypeEnum et, bool replace)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 设置导出数据的数据集
        /// </summary>
        /// <param name="ds"></param>
        public void SetData(DataSet ds)// 设置导出数据的数据集
        {
            if (ds != null)
            {
                _dataset = ds;
            }
        }
        /// <summary>
        /// 设置数据导出模式，真，dataset模式，假，datatable模式，默认为假
        /// </summary>
        public bool IsDataSetModel
        {
            get
            {
                return _isdataset;
            }
            set
            {
                _isdataset = value;
            }
        }
    }
}
