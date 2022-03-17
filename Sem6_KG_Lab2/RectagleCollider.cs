using SFML.System;
using System.Collections.Generic;
using System;
using SFML.Graphics;

namespace Sem6_KG_Lab2
{
    public class RectagleCollider : IDraw
    {
        public static List<RectagleCollider> bodies = new List<RectagleCollider>();
        public Vector2f center;
        public float width;
        public float height;
        public Color color = Color.Blue;
        public float angle = 0;
        public float viscosity = 0;
        public float density = 1;
        public RectagleCollider()
        {
            bodies.Add(this);
        }
        public Vector2f PointOnRectangle(Vector2f vec)
        {
            float Clamp(float value, float min, float max) => Math.Min(Math.Max(value, min), max);

            vec.X = Clamp(vec.X, center.X - width / 2, center.X + width / 2);
            vec.Y = Clamp(vec.Y, center.Y - height / 2, center.Y + height / 2);

            return vec;
        }
        VertexArray shape = new VertexArray(PrimitiveType.Quads, 4);
        public void Draw(RenderWindow app)
        {
            shape[0] = new Vertex(new Vector2f(center.X - width / 2, center.Y + height / 2), color);
            shape[1] = new Vertex(new Vector2f(center.X + width / 2, center.Y + height / 2), color);
            shape[2] = new Vertex(new Vector2f(center.X + width / 2, center.Y - height / 2), color);
            shape[3] = new Vertex(new Vector2f(center.X - width / 2, center.Y - height / 2), color);

            app.Draw(shape);
        }
        public bool Intersect(SoftBody.ColCircle circle, out float distance, out Vector2f delta)
        {
            float DeltaX = circle.center.X - Math.Max(center.X - width / 2, Math.Min(circle.center.X, center.X + width / 2));
            float DeltaY = circle.center.Y - Math.Max(center.Y - height / 2, Math.Min(circle.center.Y, center.Y + height / 2));
            distance = (float)Math.Sqrt(DeltaX * DeltaX + DeltaY * DeltaY);
            delta = new Vector2f(DeltaX, DeltaY);
            return distance <= circle.radius;
        }
    }
}
