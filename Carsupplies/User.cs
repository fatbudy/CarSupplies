using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carsupplies
{
    public class UserArgs:EventArgs 
    {
        public string UID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Permission { get; set; }
        public string Levels { get; set; }
    }
}
