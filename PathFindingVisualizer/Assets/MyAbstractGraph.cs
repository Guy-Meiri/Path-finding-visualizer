using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Assets
{
    public abstract class MyAbstractGraph<T, K>
    {
        public abstract IList<K> GetAllEdges();
        public abstract IList<T> GetAllNodes();
        public abstract void AddEdge(K i_Edge);
        public abstract IList<K> GetNeighbors(T Node);
        public abstract int GetNumberOfNodes();
        public abstract void UpdateWeight(K i_Edge, int i_Weight);
    }
}

