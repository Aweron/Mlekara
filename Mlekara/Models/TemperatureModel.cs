using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mlekara.Models
{
    public class MeasurementModel
    {
        public int Id { get; set; }
        public int ProbeId { get; set; }
        public float Value { get; set; }
        public string Date { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
    }
}
