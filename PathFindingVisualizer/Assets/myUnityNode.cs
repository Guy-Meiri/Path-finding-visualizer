using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    class MyUnityNode: INode
    {
        private int m_Id;
        private int m_Data;
        private Vector3 m_Position;
        private GameObject m_CellPrefab;
        private bool m_IsObstacle;

        public MyUnityNode(int i_Id, int i_Data, Vector3 i_Position, GameObject i_CellPrefab)
        {
            this.Position = i_Position;
            this.Id = i_Id;
            this.m_Data = i_Data;
            this.CellPrefab = i_CellPrefab;
        }

        public Vector3 Position { get => m_Position; set => m_Position = value; }
        public GameObject CellPrefab { get => m_CellPrefab; set => m_CellPrefab = value; }
        public int Id { get => m_Id; set => m_Id = value; }
        public bool IsObstacle { get => m_IsObstacle; set => m_IsObstacle = value; }

        public override bool Equals(object obj)
        {
            var node = obj as MyUnityNode;
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
