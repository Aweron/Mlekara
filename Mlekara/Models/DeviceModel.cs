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

        public DeviceModel(int id, string name, bool active)
        {
            Id = id;
            Name = name;
            Active = active;
            probes = new List<ProbeModel>();
        }

    }
}
