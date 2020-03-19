using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mlekara.Models
{
    public class ProbeModel
    {
        public int Id { get; set; }
        public int DeviceId { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public int Marker { get; set; }

        public ProbeModel(int id, int deviceId, string name, bool active, int min, int max, int marker)
        {
            Id = id;
            DeviceId = deviceId;
            Name = name;
            Active = active;
            Min = min;
            Max = max;
            Marker = marker;
        }
    }
}
