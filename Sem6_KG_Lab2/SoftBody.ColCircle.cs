using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace Sem6_KG_Lab2
{
    public partial class SoftBody
    {
        public sealed class ColCircle : IUpdate, IDraw
        {
            private float _radius = 10;
            readonly Color color = Color.Red;
            public float radius
            {
                get { return _radius; }
                set 
                { 
                    _radius = value;
                    InitShape();
                }
            }
            public Vector2f velosity;
            public Vector2f center;
            CircleShape shape;
            public List<Spring> springs = new List<Spring>();

            public ColCircle(float radius, Vector2f center)
            {
                this.radius = radius;
                this.center = center;
            }

            public void Draw(RenderWindow app)
            {
                shape.Position = center;
                app.Draw(shape);
                foreach (var s in springs)
                    s.Draw(app);
            }

            public void Update()
            {
                center += velosity;
            }

            public void CollisionWithBorders()
            {
                if (center.Y - radius < 0)
                {
                    center.Y = radius;
                    velosity.Y = 0;
                    velosity.X *= circleDrag;
                }
                if (center.Y + radius >= Application.height)
                {
                    center.Y = Application.height - radius;
                    velosity.Y = 0;
                    velosity.X *= circleDrag;
                }
                if (center.X - radius < 0)
                {
                    center.X = radius;
                    velosity.X = 0;
                    velosity.Y *= circleDrag;
                }
                if (center.X + radius >= Application.width)
                {
                    center.X = Application.width - radius;
                    velosity.X = 0;
                    velosity.Y *= circleDrag;
                }
            }

            private void InitShape()
            {
                shape?.Dispose();
                shape = new CircleShape(_radius, 8);
                shape.OutlineColor = color;
                shape.FillColor = Color.Black;
                shape.OutlineThickness = 1;
                shape.Origin = new Vector2f(radius, radius);
            }
        }
    }
}
