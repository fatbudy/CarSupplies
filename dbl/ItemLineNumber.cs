using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;

namespace dbl
{
    sealed  class ItemLineNumber
    {
        private static string get_mainboardSerial()
        {
            ManagementClass mc = new ManagementClass("WIN32_BaseBoard");
            ManagementObjectCollection moc = mc.GetInstances();
            string SerialNumber =moc.GetHashCode ().ToString ("X2");
            foreach (ManagementObject mo in moc)
            {
                SerialNumber = mo["SerialNumber"].ToString().GetHashCode ().ToString ("X2");
                break;
            }
            return SerialNumber;
        }
        private static string _mbsnumer = get_mainboardSerial();
        public static string MainBoardSerialNumber
        {
            get
            {
                return _mbsnumer;
            }
        }
        public static string GetSerialNumber()
        {
            return string.Format("'{0}:{1:X2}'", _mbsnumer, DateTime.Now.Ticks).GetHashCode ().ToString ("00000000");
        }
    }
}
