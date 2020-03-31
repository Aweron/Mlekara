using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mlekara.Models
{
    public class GraphDataModel
    {
        public ProbeModel Probe { get; set; }
        public string Date { get; set; }
        public int StartHour { get; set; }
        public int HourCount { get; set; }
    }
}
