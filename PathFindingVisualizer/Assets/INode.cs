using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    public interface INode
    {
        int GetNodeId();
        int Id { get; set; }
        bool IsObstacle { get; set; }
    }
}
