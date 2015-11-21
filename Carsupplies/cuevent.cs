using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms ;

namespace Carsupplies
{
    public delegate void ClosedEventHandler(object sender,ClosedArgs e);
    public class ClosedArgs:EventArgs 
    {
        public DialogResult DialogResult { get; internal set; }
        public bool Closing { get; internal set; }
        public string Data { get; internal set; }
    }
}
