using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace aiv_graphs_b
{
    class Map
    {
        private int height;
        private int width;

        private int[] cells;
        private Node[] nodes;

        private Sprite sprite;

        public Map(int width, int height, int[] cells)
        {
            this.width = width;
            this.height = height;
            this.cells = cells;

            nodes = new Node[cells.Length];

            sprite = new Sprite(1, 1);

            for (int i = 0; i < cells.Length; i++)
            {
                if (cells[i] <= 0)
                {
                    continue;
                }

                int x = i % width;
                int y = i / height;

                nodes[i] = new Node(cells[i], x, y);
            }

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int index = y * width + x;
                    if (nodes[index] == null)
                    {
                        continue;
                    }
                    CheckNeighbour(nodes[index], x - 1, y);
                    CheckNeighbour(nodes[index], x + 1, y);
                    CheckNeighbour(nodes[index], x, y - 1);
                    CheckNeighbour(nodes[index], x, y + 1);
                }
            }
        }

        public List<Node> GetPath(int startX, int startY, int endX, int endY)
        {
            if (startX < 0 || startX > width)
            {
                return null;
            }
            if (endX < 0 || endX > width)
            {
                return null;
            }
            if (startY < 0 || startY > height)
            {
                return null;
            }
            if (endY < 0 || endY > height)
            {
                return null;
            }

            int startIndex = startY * width + startX;
            int endIndex = endY * width + endX;

            Node startNode = nodes[startIndex];
            Node endNode = nodes[endIndex];

            if (startNode == null || endNode == null)
            {
                return null;
            }

            Dictionary<Node, Node> cameFrom = GraphUtils.AStar(startNode, endNode);
            List<Node> path = new List<Node>();
            Node currNode = endNode;
            while (cameFrom[currNode] != currNode)
            {
                path.Add(currNode);
                currNode = cameFrom[currNode];
            }

            path.Reverse();
            return path;
        }

        public void Draw()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    sprite.position = new Vector2(x, y);
                    int index = y * width + x;
                    if (nodes[index] == null)
                    {
                        sprite.DrawSolidColor(0, 0, 0);
                    }
                    else
                    {
                        sprite.DrawSolidColor(1f, 1f, 1f);
                    }
                }
            }
        }

        public void RemoveNode(int index)
        {
            cells[index] = 0;
            foreach (var adj in nodes[index].Neighbours)
            {
                adj.Item1.RemoveNeighbour(nodes[index]);
            }
            nodes[index] = null;
        }

        private void CheckNeighbour(Node currNode, int x, int y)
        {
            if (x < 0 || x >= width)
            {
                return;
            }

            if (y < 0 || y >= height)
            {
                return;
            }

            int index = y * width + x;

            if (nodes[index] != null)
            {
                currNode.AddNeighbour(nodes[index]);
            }
        }
    }
}
