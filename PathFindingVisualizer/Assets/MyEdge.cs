using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    class MyEdge : IEdge
    {
        private INode m_U;
        private INode m_V;
        private int m_Weight;

        public MyEdge(INode i_U, INode i_V, int m_Weight = 1)
        {
            this.U = i_U;
            this.V = i_V;
            this.Weight = m_Weight;
        }

        public INode U { get => m_U; set => m_U = value; }
        public INode V { get => m_V; set => m_V = value; }
        public int Weight { get => m_Weight; set => m_Weight = value; }

        public override bool Equals(object obj)
        {
            var edge = obj as MyEdge;
            return edge != null &&
                   EqualityComparer<INode>.Default.Equals(V, edge.V) &&
                   Weight == edge.Weight;
        }

        public override int GetHashCode()
        {
            var hashCode = 1031288857;
            hashCode = hashCode * -1521134295 + EqualityComparer<INode>.Default.GetHashCode(V);
            hashCode = hashCode * -1521134295 + Weight.GetHashCode();
            return hashCode;
        }

        public int GetWeight()
        {
            return Weight;
        }
    }
}
