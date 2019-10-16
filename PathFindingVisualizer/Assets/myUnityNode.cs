﻿using System;
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

        public MyUnityNode(int i_Id, int i_Data, Vector3 i_Position, GameObject i_CellPrefab)
        {
            this.Position = i_Position;
            this.m_Id = i_Id;
            this.m_Data = i_Data;
            this.CellPrefab = i_CellPrefab;
        }

        public Vector3 Position { get => m_Position; set => m_Position = value; }
        public GameObject CellPrefab { get => m_CellPrefab; set => m_CellPrefab = value; }

        public override bool Equals(object obj)
        {
            var node = obj as MyUnityNode;
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
