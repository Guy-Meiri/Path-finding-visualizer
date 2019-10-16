using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    public class Program
    {
        public static void Main(String[] args)
        {

            //PriorityQueueCopied<int> queue = new PriorityQueueCopied<int>();
            //Random rnd = new Random();
            ////enqueue
            //for (int i = 0; i < 10; i++)
            //{
            //    queue.Enqueue(rnd.Next(3), rnd.Next(3));
            //}
            ////dequeue
            //while (queue.Count > 0)
            //{
            //    Console.WriteLine(queue.Dequeue());
            //}
            //Console.ReadLine();

            MyDirectedGraph graph = new MyDirectedGraph();
            MyNode v1 = new MyNode(1, 11);
            MyNode v2 = new MyNode(2, 12);
            MyNode v3 = new MyNode(3, 13);
            MyEdge e1 = new MyEdge(v1, v2, 1);
            MyEdge e2 = new MyEdge(v2, v3, 1);
            MyEdge e3 = new MyEdge(v2, v1 , 1);

            Console.WriteLine("e2 u hash: " + e2.U.GetNodeId());
            Console.WriteLine("e3 u hash: " + e3.U.GetNodeId());
            graph.AddEdge(e1);
            graph.AddEdge(e2);
            graph.AddEdge(e3);
            

            IList<INode> allNodes  = graph.GetAllNodes();
            IList<IEdge> neighbors = graph.GetNeighbors(v1);
            PathFinder pathFinder = new PathFinder();
            IList<INode> res = pathFinder.BlemanFordSearch(graph, v1, v3);
            IList<INode> resDijkstra = pathFinder.Dijkstra(graph, v1, v3);



            int x = 1;
            int y = x + 1;

        }
    }
}
