using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mlekara.Models
{
    public class DeviceModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public List<ProbeModel> probes;

        public DeviceModel()
        {
            probes = new List<ProbeModel>();
        }

        public DeviceModel(int i, string n, bool a)
        {
            Id = i;
            Name = n;
            Active = a;
            probes = new List<ProbeModel>();
        }

    }
}
