using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    class PathFinder
    {
        public IList<INode> BlemanFordSearch(MyAbstractGraph<INode, IEdge> i_Graph, INode i_StartNode, INode I_TargetNode)
        {
            List<INode> resPath = new List<INode>();
            if (!i_Graph.GetAllNodes().Contains(i_StartNode))
            {
                //Console.WriteLine("wanted starting node is not in the graph");
                return null;
            }

            Dictionary<INode, int> distances = new Dictionary<INode, int>();
            Dictionary<INode, INode> parents = new Dictionary<INode, INode>();
            IList<IEdge> edges = i_Graph.GetAllEdges();

            foreach (INode node in i_Graph.GetAllNodes())
            {
                distances.Add(node, int.MaxValue);
                parents.Add(node, null);
            }
            distances[i_StartNode] = 0;

            for (int i = 0; i < i_Graph.GetNumberOfNodes() - 1; i++)
            {
                foreach (IEdge edge in i_Graph.GetAllEdges())
                {
                    int newDistance = distances[edge.U] + edge.GetWeight();
                    if ((distances[edge.U] != int.MaxValue) && (newDistance < distances[edge.V]))
                    {
                        distances[edge.V] = newDistance;
                        parents[edge.V] = edge.U;
                    }
                }
            }

            foreach (IEdge edge in i_Graph.GetAllEdges())
            {
                if (distances[edge.U] + edge.GetWeight() < distances[edge.V])
                {
                    Console.WriteLine("found negative cycle!");
                    return null;
                }
            }

            INode currentNode = I_TargetNode;
            while (currentNode != null)
            {
                resPath.Insert(0, currentNode);
                currentNode = parents[currentNode];
            }

            
            return resPath;
        }


        public IList<INode> Dijkstra(MyAbstractGraph<INode, IEdge> i_Graph, INode i_StartNode, INode i_TargetNode)
        {
            bool isTargetFound = false;

            if(!i_Graph.GetAllNodes().Contains(i_StartNode))
            {
                Console.WriteLine("starting node is not in the graph");
                return null;
            }

            PriorityQueueCopied<INode> priorityQueue = new PriorityQueueCopied<INode>(true);
            Dictionary<INode, int> distances = new Dictionary<INode, int>();
            Dictionary<INode, INode> parents = new Dictionary<INode, INode>();

            foreach (INode node in i_Graph.GetAllNodes())
            {
                priorityQueue.Enqueue(int.MaxValue/3, node);
                distances[node] = int.MaxValue/3;
                parents[node] = null;
            }
            distances[i_StartNode] = 0;
            priorityQueue.UpdatePriority(i_StartNode, 0);
           
            while (priorityQueue.Count != 0)
            {
                INode minNode = priorityQueue.Dequeue();
                foreach (IEdge neighbor in i_Graph.GetNeighbors(minNode))
                {
                    if (priorityQueue.IsInQueue(neighbor.V))
                    {
                        int newDistance = distances[minNode] + neighbor.GetWeight();
                        if (newDistance < distances[neighbor.V])
                        {
                            {
                                distances[neighbor.V] = newDistance;
                                priorityQueue.UpdatePriority(neighbor.V, newDistance);
                                parents[neighbor.V] = minNode;
                            }
                        }

                    }
                }

            }
            //UnityEngine.Debug.Log("distance: " + distances[i_TargetNode]);
            if (distances[i_TargetNode] == int.MaxValue)
            {
                
                return null;
            }
                
            else
            {
                List<INode> resPath = new List<INode>();
                INode currentNode = i_TargetNode;
                while (currentNode != null)
                {
                    resPath.Insert(0, currentNode);
                    currentNode = parents[currentNode];
                }
                return resPath;
            }
           
        }

    }
}
