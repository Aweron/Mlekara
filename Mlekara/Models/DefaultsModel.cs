using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mlekara.Models
{
    public class DefaultsModel
    {
        public int Id { get; set; }
        public int StackSize { get; set; }
        public int GraphMin { get; set; }
        public int GraphMax { get; set; }

        public DefaultsModel()
        {
        }

        public DefaultsModel(int stackSize, int graphMin, int graphMax)
        {
            Id = 1;
            StackSize = stackSize;
            GraphMin = graphMin;
            GraphMax = graphMax;
        }
    }
}
