using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aiv_graphs_b
{
    class PriorityQueue
    {
        Dictionary<Node, int> items;

        public PriorityQueue()
        {
            items = new Dictionary<Node, int>();
        }

        public bool IsEmpty { get { return items.Count == 0; } }

        public void Enqueue(Node node, int priority)
        {
            items[node] = priority;
        }

        public Node Dequeue()
        {
            int minPriority = int.MaxValue;
            Node nodeToReturn = null;
            foreach (KeyValuePair<Node, int> item in items)
            {
                if (minPriority > item.Value)
                {
                    minPriority = item.Value;
                    nodeToReturn = item.Key;
                }
            }
            items.Remove(nodeToReturn);
            return nodeToReturn;
        }
    }
}
