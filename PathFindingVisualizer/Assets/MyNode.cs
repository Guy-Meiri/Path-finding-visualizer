using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    class MyNode : INode
    {
        private int m_Id;
        private int m_Data;

        public MyNode(int m_Id, int m_Data)
        {
            this.m_Id = m_Id;
            this.m_Data = m_Data;
        }

        public override bool Equals(object obj)
        {
            var node = obj as MyNode;
            return node != null &&
                   m_Id == node.m_Id;
        }

        public override int GetHashCode()
        {
            return -1501183392 + m_Id.GetHashCode();
        }

        public int GetNodeId()
        {
            return m_Id;
        }
        public override string ToString()
        {
            return m_Id.ToString();
        }
    }
}
