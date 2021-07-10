using System;
using System.Collections.Generic;

namespace aiv_graphs_b
{
    class Node
    {
        public int Value { get; private set; }

        public int X { get; private set; }
        public int Y { get; private set; }

        public List<Tuple<Node, int>> Neighbours { get; private set; }


        public Node(int value, int x = 0, int y = 0)
        {
            Value = value;
            X = x;
            Y = y;
            Neighbours = new List<Tuple<Node, int>>();
        }

        public void AddNeighbour(Node node, int cost=1)
        {
            Neighbours.Add(new Tuple<Node, int>(node, Value));
        }

        public bool RemoveNeighbour(Node node)
        {
            foreach (var adjacent in Neighbours)
            {
                if (adjacent.Item1 == node)
                {
                    Neighbours.Remove(adjacent);
                    return true;
                }
            }
            return false;
        }
    }
}
