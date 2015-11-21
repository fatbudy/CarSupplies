using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace OutPutData.Log
{
    public class TextLog:ILog ,IDisposable 
    {
        /// <summary>
        /// 文件名称
        /// </summary>
        private string _filename;

        private StreamWriter _stream_writer=null;

        /// <summary>
        /// 文件名称
        /// </summary>
        public string Filename
        {
            get
            {
                return _filename;
            }
            set
            {
                if (!string.IsNullOrEmpty(_filename))
                {
                    _filename = value;
                    if (_stream_writer != null)
                    {
                        _stream_writer.Flush();
                        _stream_writer.Dispose();
                    }
                    _stream_writer = new StreamWriter(_filename, true, Encoding.UTF8);
                }
                
            }
        }
        public void WriteText(string strValue)
        {
            try
            {
                _stream_writer.WriteLine("[{0}] ",DateTime.Now , strValue);
            }
            catch
            {
                //
            }
        }

        public void Dispose()
        {
            if (_stream_writer != null)
            {
                _stream_writer.Flush();
                _stream_writer.Dispose();
            }
            GC.SuppressFinalize(this);
        }
    }
}
