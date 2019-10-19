using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    class PathFinder
    {

        public enum NodeStatus { Unvisited = 0, Visited = 1, Done = 2 };

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

            if (!i_Graph.GetAllNodes().Contains(i_StartNode))
            {
                Console.WriteLine("starting node is not in the graph");
                return null;
            }

            PriorityQueueCopied<INode> priorityQueue = new PriorityQueueCopied<INode>(true);
            Dictionary<INode, int> distances = new Dictionary<INode, int>();
            Dictionary<INode, INode> parents = new Dictionary<INode, INode>();

            foreach (INode node in i_Graph.GetAllNodes())
            {
                priorityQueue.Enqueue(int.MaxValue / 3, node);
                distances[node] = int.MaxValue / 3;
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


        //returns
        //1. list of tuples - (node, distance)
        //2. list of the result path
        public Tuple<IList<Tuple<INode, int>>, IList<INode>> DijkstraWithDistances(MyAbstractGraph<INode, IEdge> i_Graph, INode i_StartNode, INode i_TargetNode)
        {

            if (!i_Graph.GetAllNodes().Contains(i_StartNode))
            {
                Console.WriteLine("starting node is not in the graph");
                return null;
            }

            List<Tuple<INode, int>> distancesHistory = new List<Tuple<INode, int>>();
            PriorityQueueCopied<INode> priorityQueue = new PriorityQueueCopied<INode>(true);
            Dictionary<INode, int> distances = new Dictionary<INode, int>();
            Dictionary<INode, INode> parents = new Dictionary<INode, INode>();

            foreach (INode node in i_Graph.GetAllNodes())
            {
                priorityQueue.Enqueue(int.MaxValue / 3, node);
                distances[node] = int.MaxValue / 3;
                parents[node] = null;
            }
            distances[i_StartNode] = 0;
            distancesHistory.Add(new Tuple<INode, int>(i_StartNode, 0));
            priorityQueue.UpdatePriority(i_StartNode, 0);

            while (priorityQueue.Count != 0)
            {
                INode minNode = priorityQueue.Dequeue();
                if (minNode.Id == i_TargetNode.Id)
                {
                    break;
                }
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
                                if (neighbor.V.Id != i_TargetNode.Id)
                                {
                                    distancesHistory.Add(new Tuple<INode, int>(neighbor.V, newDistance));
                                }
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
                return Tuple.Create<IList<Tuple<INode, int>>, IList<INode>>(distancesHistory, resPath);
            }

        }

        public IList<Tuple<INode, NodeStatus>> DFS(MyAbstractGraph<INode, IEdge> i_Graph, INode i_StartNode)
        {
            Dictionary<INode, NodeStatus> visitStatus = new Dictionary<INode, NodeStatus>();
            List<Tuple<INode, NodeStatus>> resultVisitedOrder = new List<Tuple<INode, NodeStatus>>();

            foreach (INode node in i_Graph.GetAllNodes())
                visitStatus[node] = NodeStatus.Unvisited;

            foreach (INode node in i_Graph.GetAllNodes())
            {
                if (visitStatus[node] == NodeStatus.Unvisited && !node.IsObstacle)
                {
                    visit(i_Graph, node, visitStatus, resultVisitedOrder);
                }
            }

            return resultVisitedOrder;
        }

        private void visit(MyAbstractGraph<INode, IEdge> i_Graph, INode i_Root, Dictionary<INode, NodeStatus> i_VisitStatus, List<Tuple<INode, NodeStatus>> i_VisitedOrder)
        {
            i_VisitStatus[i_Root] = NodeStatus.Visited;
            i_VisitedOrder.Add(new Tuple<INode, NodeStatus>(i_Root, NodeStatus.Visited));

            foreach (IEdge edge in i_Graph.GetNeighbors(i_Root))
            {
                if (i_VisitStatus[edge.V] == NodeStatus.Unvisited && !edge.V.IsObstacle)
                {
                    visit(i_Graph, edge.V, i_VisitStatus, i_VisitedOrder);
                }
            }

            i_VisitStatus[i_Root] = NodeStatus.Done;
            i_VisitedOrder.Add(new Tuple<INode, NodeStatus>(i_Root, NodeStatus.Done));
        }
    }


}
