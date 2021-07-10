using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace aiv_graphs_b
{
    class Agent
    {
        private Sprite sprite;
        public int x { get { return Convert.ToInt32(sprite.position.X); } }
        public int y { get { return Convert.ToInt32(sprite.position.Y); } }
        private List<Node> path;
        private Node target;
        private float speed;
        private Vector4 color;

        public Agent(int x, int y, Vector4 color, float speed = 3f)
        {
            sprite = new Sprite(1, 1);
            sprite.position = new Vector2(x, y);
            target = null;

            this.speed = speed;
            this.color = color;
        }

        public void SetColor(float r, float g, float b)
        {
            sprite.SetMultiplyTint(r, g, b, 1);

        }

        public void SetPath(List<Node> path)
        {
            this.path = path;
            if (target == null || target == path[0])
            {
                target = path[0];
                path.RemoveAt(0);
            }
        }

        public void Draw()
        {
            sprite.DrawSolidColor(color);
        }

        public void Update(float deltaTime)
        {
            if (target == null)
            {
                return;
            }

            Vector2 destination = new Vector2(target.X, target.Y);
            float dist = (destination - sprite.position).Length;
            if (dist <= 0.02f)
            {
                if (path.Count > 0)
                {
                    target = path[0];
                    path.RemoveAt(0);
                }
                else
                {
                    target = null;
                }
            }
            else
            {
                Vector2 direction = (destination - sprite.position).Normalized();
                sprite.position += direction * deltaTime * speed;
            }
        }
    }
}
