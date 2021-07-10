using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace aiv_graphs_b
{
    static class GraphUtils
    {
        public static Dictionary<Node, Node> DeepFirstSearch(Node u)
        {
            Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
            Stack<Node> frontier = new Stack<Node>();
            frontier.Push(u);
            cameFrom.Add(u, u);

            while (frontier.Count > 0)
            {
                Node v = frontier.Peek();
                bool adjacentFound = false;
                foreach (Tuple<Node, int> edge in v.Neighbours)
                {
                    Node w = edge.Item1;
                    if (!cameFrom.ContainsKey(w))
                    {
                        cameFrom.Add(w, v);
                        frontier.Push(w);
                        adjacentFound = true;
                        break;
                    }
                }
                if (!adjacentFound)
                {
                    frontier.Pop();
                }
            }
            return cameFrom;
        }

        public static Tuple<Dictionary<Node, Node>, Dictionary<Node, int>> BreadthFirstSearch(Node u)
        {
            Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
            Dictionary<Node, int> distance = new Dictionary<Node, int>();

            Queue<Node> frontier = new Queue<Node>();
            frontier.Enqueue(u);
            cameFrom.Add(u, u);
            distance.Add(u, 0);

            while (frontier.Count > 0)
            {
                Node v = frontier.Dequeue();
                foreach (Tuple<Node, int> edge in v.Neighbours)
                {
                    Node w = edge.Item1;
                    if (!cameFrom.ContainsKey(w))
                    {
                        cameFrom.Add(w, v);
                        distance.Add(w, distance[v] + 1);
                        frontier.Enqueue(w);
                    }
                }
            }
            return new Tuple<Dictionary<Node, Node>,
                             Dictionary<Node, int>>(cameFrom, distance);
        }

        public static Tuple<Dictionary<Node, Node>, Dictionary<Node, int>> Dijkstra(Node u)
        {
            Dictionary<Node, int> Dist = new Dictionary<Node, int>();
            Dictionary<Node, Node> P = new Dictionary<Node, Node>();

            Dist[u] = 0;
            P[u] = u;
            PriorityQueue H = new PriorityQueue();
            H.Enqueue(u, 0);

            while (!H.IsEmpty)
            {
                Node v = H.Dequeue();
                foreach (Tuple<Node, int> edge in v.Neighbours)
                {
                    Node w = edge.Item1;
                    int cost = edge.Item2;
                    if (!Dist.ContainsKey(w) || Dist[w] > Dist[v] + cost)
                    {
                        Dist[w] = Dist[v] + cost;
                        P[w] = v;
                        H.Enqueue(w, Dist[w]);
                    }
                }
            }
            return new Tuple<Dictionary<Node, Node>,
                             Dictionary<Node, int>>(P, Dist);
        }

        private static int Heuristic(Node start, Node end)
        {
            return Math.Abs(start.X - end.X) + Math.Abs(start.Y - end.Y);
        }

        public static Dictionary<Node, Node> AStar(Node start, Node end)
        {
            Dictionary<Node, int> costSoFar = new Dictionary<Node, int>();
            Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();

            costSoFar[start] = 0;
            cameFrom[start] = start;
            PriorityQueue frontier = new PriorityQueue();
            frontier.Enqueue(start, Heuristic(start, end));

            while (!frontier.IsEmpty)
            {
                Node v = frontier.Dequeue();
                //Console.WriteLine("Cost:" + v.Value);
                if (v == end)
                {
                    return cameFrom;
                }

                foreach (Tuple<Node, int> edge in v.Neighbours)
                {
                    Node w = edge.Item1;
                    int cost = edge.Item2;
                    if (!costSoFar.ContainsKey(w) || costSoFar[w] > costSoFar[v] + cost)
                    {
                        costSoFar[w] = costSoFar[v] + cost;
                        cameFrom[w] = v;
                        frontier.Enqueue(w, costSoFar[w] + Heuristic(w, end));
                    }
                }
            }
            return cameFrom;
        }

        //public static List<Node> GetPathBetweenNodes(Dictionary<Node, Node> cameFrom, Node start, Node end)
        //{

        //}

        public static Node LoadGraphFromFile(string filename)
        {
            Dictionary<int, Node> nodeGraph = new Dictionary<int, Node>();
            Dictionary<int, string> graph = new Dictionary<int, string>();

            using (StreamReader reader = new StreamReader(filename))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] splittedLine = line.Split('\t');
                    int value = int.Parse(splittedLine[0]);
                    nodeGraph[value] = new Node(value);
                    graph[value] = splittedLine[1];
                }
            }
            Node returnNode = null;
            foreach (var node in nodeGraph)
            {
                string[] neighbours = graph[node.Key].Split(',');
                for (int i = 0; i < neighbours.Length; i++)
                {
                    node.Value.AddNeighbour(
                        nodeGraph[
                            int.Parse(neighbours[i])
                            ]);
                }
                if (returnNode == null)
                {
                    returnNode = node.Value;
                }
            }
            return returnNode;
        }
    }
}
