using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    public interface IEdge
    {
        INode U { get; set; }
        INode V { get; set; }
        int GetWeight();
        int Weight { get; set; }
    }
}
