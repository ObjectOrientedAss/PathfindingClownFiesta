using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace aiv_graphs_b
{
    class Program
    {
        static void Main(string[] args)
        {
            Node node1 = new Node(1);
            Node node2 = new Node(2);
            Node node3 = new Node(3);
            Node node4 = new Node(4);
            Node node5 = new Node(5);
            Node node6 = new Node(6);

            node1.AddNeighbour(node2);
            node1.AddNeighbour(node3);
            node1.AddNeighbour(node4);

            node2.AddNeighbour(node1);
            node2.AddNeighbour(node5);

            node3.AddNeighbour(node1);

            node4.AddNeighbour(node1);

            node5.AddNeighbour(node2);
            node5.AddNeighbour(node6);

            node6.AddNeighbour(node5);

            Node node = GraphUtils.LoadGraphFromFile("Assets/graph1.txt");
            Dictionary<Node, Node> dfsCameFrom = GraphUtils.DeepFirstSearch(node);

            foreach (KeyValuePair<Node, Node> currentNode in dfsCameFrom)
            {
                Console.WriteLine("Node {0} came from {1}", currentNode.Key.Value, currentNode.Value.Value);
            }

            Tuple<Dictionary<Node, Node>, Dictionary<Node, int>> bfsResult =
                GraphUtils.BreadthFirstSearch(node);
            Dictionary<Node, Node> bfsCameFrom = bfsResult.Item1;
            Dictionary<Node, int> bfsDistance = bfsResult.Item2;

            foreach (KeyValuePair<Node, Node> currentNode in bfsCameFrom)
            {
                Console.WriteLine(
                    "Node {0} came from {1} with distance {2}",
                    currentNode.Key.Value, currentNode.Value.Value, bfsDistance[currentNode.Key]);
            }

            Window window = new Window(800, 800, "A Star");
            window.SetDefaultOrthographicSize(10);
            int[] cells = new int[]
            {
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 1, 1, 1, 1, 1, 1, 1, 1, 0,
                0, 1, 1, 1, 0, 0, 0, 0, 1, 0,
                0, 1, 0, 1, 1, 0, 0, 0, 1, 0,
                0, 1, 0, 0, 1, 1, 0, 0, 1, 0,
                0, 1, 0, 0, 0, 1, 1, 0, 1, 0,
                0, 1, 0, 0, 0, 0, 1, 1, 1, 0,
                0, 1, 0, 0, 0, 0, 0, 0, 1, 0,
                0, 1, 1, 1, 1, 1, 1, 1, 1, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0
            };

            Map map = new Map(10, 10, cells);
            Agent agent = new Agent(1, 1, new Vector4(1f, 0f, 0f, 1f));
            Agent enemy = new Agent(5, 1, new Vector4(0f, 1f, 1f, 1f));
            enemy.SetColor(0f, 1f, 1f);

            bool clicked = false;

            while (window.IsOpened)
            {
                if (window.mouseLeft)
                {
                    if (!clicked)
                    {
                        clicked = true;
                        List<Node> path = map.GetPath(agent.x, agent.y, Convert.ToInt32(window.mouseX), Convert.ToInt32(window.mouseY));
                        if (path != null && path.Count > 0)
                        {
                            agent.SetPath(path);
                        }
                    }
                }
                else if (window.mouseRight)
                {
                    if (!clicked)
                    {
                        clicked = true;
                        
                    }
                }
                else
                {
                    clicked = false;
                }
                agent.Update(window.deltaTime);
                enemy.Update(window.deltaTime);

                List<Node> enemyPath = map.GetPath(enemy.x, enemy.y, agent.x, agent.y);
                if (enemyPath != null && enemyPath.Count > 1)
                {
                    enemyPath.RemoveAt(enemyPath.Count - 1);
                    enemy.SetPath(enemyPath);
                }

                map.Draw();
                agent.Draw();
                enemy.Draw();

                window.Update();
            }

        }
    }
}
