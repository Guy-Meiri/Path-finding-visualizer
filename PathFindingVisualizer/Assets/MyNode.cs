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
        private bool m_IsObstacle;

        public MyNode(int i_Id, int i_Data, bool i_IsObtacle = false)
        {
            this.Id = i_Id;
            this.m_Data = i_Data;
            m_IsObstacle = i_IsObtacle;
        }

        public int Id { get => m_Id; set => m_Id = value; }
        public bool IsObstacle { get => m_IsObstacle; set => m_IsObstacle = value; }
        

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
