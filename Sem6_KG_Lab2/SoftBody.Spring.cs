using System;
using SFML.Graphics;
using SFML.System;

namespace Sem6_KG_Lab2
{
    public partial class SoftBody
    {
        public sealed class Spring : IUpdate, IDraw
        {
            public ColCircle a, b;
            VertexArray shape = new VertexArray(PrimitiveType.Lines, 2);
            readonly Color color = Color.White;
            float init_distance;
            public Spring(ColCircle a, ColCircle b)
            {
                this.a = a;
                this.b = b;
                init_distance = Tools.Distance(a.center, b.center);
            }

            public void Draw(RenderWindow app)
            {
                shape[0] = new Vertex(a.center, color);
                shape[1] = new Vertex(b.center, color);
                app.Draw(shape);
            }

            public void Update()
            {
                float distance = Tools.Distance(a.center, b.center);

                float delta = init_distance - distance;
                Vector2f dir_to_b = Tools.Normalize(b.center - a.center);
                Vector2f speed = springForce * delta * dir_to_b;
                b.center += speed;
                b.velosity += speed;
                a.center -= speed;
                a.velosity -= speed;
            }
        }
    }
}
