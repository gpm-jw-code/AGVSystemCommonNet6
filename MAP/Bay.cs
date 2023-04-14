using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGVSytemCommonNet6.MAP
{
    public class Bay
    {
        public List<string> Points { get; set; }
        public string InPoint { get; set; }
        public string OutPoint { get; set; }
    }
}
