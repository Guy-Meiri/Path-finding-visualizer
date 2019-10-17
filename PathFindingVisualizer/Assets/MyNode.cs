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
            this.Id = m_Id;
            this.m_Data = m_Data;
        }

        public int Id { get => m_Id; set => m_Id = value; }

        public override bool Equals(object obj)
        {
            var node = obj as MyNode;
            return node != null &&
                   Id == node.Id;
        }

        public override int GetHashCode()
        {
            return -1501183392 + Id.GetHashCode();
        }

        public int GetNodeId()
        {
            return Id;
        }
        public override string ToString()
        {
            return Id.ToString();
        }
    }
}
