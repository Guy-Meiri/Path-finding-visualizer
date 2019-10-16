using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    public class MyDirectedGraph : MyAbstractGraph<INode, IEdge>
    {
        private Dictionary<INode, List<IEdge>> m_Graph;

        public MyDirectedGraph()
        {
            this.m_Graph = new Dictionary<INode, List<IEdge>>();
        }

        public INode GetNodeById(int i_Id)
        {
            foreach(INode node in GetAllNodes())
            {
                if(node.GetNodeId() == i_Id)
                {
                    return node;
                }
            }

            return null;
        }

        public override void AddEdge(IEdge i_Edge)
        {
            //Console.WriteLine(i_Edge.U.ToString() + ",new EdgeHash: " + i_Edge.U.GetHashCode());
            foreach (INode node in m_Graph.Keys)
            {
                //Console.WriteLine(node.ToString() + ", in  HashCode: " + node.GetHashCode());
            }

            if (!m_Graph.ContainsKey(i_Edge.V))
            {
                m_Graph[i_Edge.V] = new List<IEdge>();
            }
            if (!m_Graph.ContainsKey(i_Edge.U))
            {
                List<IEdge> newEdges = new List<IEdge>();
                newEdges.Add(i_Edge);

                m_Graph[i_Edge.U] =  newEdges;
            }
          
            else // U is already in the graph! just add the new edge to its list
            {
                //UnityEngine.Debug.Log("in AddEdge" + )
                List<IEdge> edges;
                m_Graph.TryGetValue(i_Edge.U, out edges);

                if (edges == null)
                {
                    edges = new List<IEdge>();
                }

                edges.Add(i_Edge);
                m_Graph[i_Edge.U] = edges;
            }
        }

        public void AddNode(INode i_NewNode)
        {
            if (!m_Graph.ContainsKey(i_NewNode))
            {
                m_Graph[i_NewNode] = new List<IEdge>();
            }
        }

        public override IList<IEdge> GetAllEdges()
        {
            List<IEdge> tempList = new List<IEdge>();
            List<IEdge> edges = new List<IEdge>();
            foreach (INode node in m_Graph.Keys)
            {
                if (m_Graph.TryGetValue(node, out tempList))
                {
                    edges.AddRange(tempList);
                    //edges = edges.Union(tempList).ToList<IEdge>();
                }
            }

            return edges;
        }

        public override IList<INode> GetAllNodes()
        {
            return m_Graph.Keys.ToList<INode>();
        }

        public override IList<IEdge> GetNeighbors(INode i_Node)
        {
            if (m_Graph.ContainsKey(i_Node))
            {
                List<IEdge> neighbors;
                if (m_Graph.TryGetValue(i_Node, out neighbors))
                    return neighbors.ToList<IEdge>();
            }
            return null;
        }

        public override int GetNumberOfNodes()
        {
            return m_Graph.Keys.Count;
        }

        public override void UpdateWeight(IEdge i_Edge, int i_Weight)
        {
            List<IEdge> neighbors;
            if (m_Graph.TryGetValue(i_Edge.U, out neighbors))
            {
               foreach(IEdge edge in neighbors.ToList())
                {
                    if (edge == i_Edge)
                    {
                        neighbors.Remove(i_Edge);
                        neighbors.Add(new MyEdge(i_Edge.U, i_Edge.V, i_Weight));
                    }
                }
            }
        }

    }
}
