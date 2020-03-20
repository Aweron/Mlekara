using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mlekara.Models
{
    public class PortSettingsModel
    {
        public int Id { get; set; }
        public string Port { get; set; }
        public int BaudRate { get; set; }
        public int DataBits { get; set; }
        public string StopBits { get; set; }
        public string ParityBits { get; set; }

        public PortSettingsModel()
        {
        }

        public PortSettingsModel(string port, int baudRate, int dataBits, string stopBits, string parityBits)
        {
            Id = 1;
            Port = port;
            BaudRate = baudRate;
            DataBits = dataBits;
            StopBits = stopBits;
            ParityBits = parityBits;
        }
    }
}
