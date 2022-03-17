using System;
using System.Runtime.InteropServices;
using System.Linq;
using System.Collections.Generic;
using SFML.System;
using SFML.Graphics;
using System.Text;

namespace Sem6_KG_Lab2
{
    public partial class SoftBody : IUpdate, IDraw
    {
        public const float circleDrag = 1;
        public const float springForce = .05f;
        public const float springDistance = 50;
        public const float sqrspringDistance = springDistance * springDistance;
        public const float circleRadius = 20;
        public Vector2f position => throw new NotImplementedException();
        public static ColCircle[] all_circles;
        public List<ColCircle> circles = new List<ColCircle>();
        public float gravity = 0.05f;

        public SoftBody(Vector2f position, int radius_in_circles)
        {
            //create circle
            for (int x = -radius_in_circles; x <= radius_in_circles; x++)
                for (int y = -radius_in_circles; y <= radius_in_circles; y++)
                {
                    var point = new Vector2f(x, y);
                    if (Tools.Length(point) <= radius_in_circles)
                        circles.Add(new ColCircle(circleRadius, point * springDistance + position));
                }
            //create springs
            ColCircle[] GetNear(Vector2f pos, float radius)
            {
                List<ColCircle> near = new List<ColCircle>();
                for (int k = 0; k < circles.Count; k++)
                    if (Tools.Distance(circles.ElementAt(k).center, pos) <= radius && circles.ElementAt(k).center != pos)
                        near.Add(circles.ElementAt(k));
                return near.ToArray();
            }
            int create_springs = 0;
            foreach (var c in circles)
                foreach (var near in GetNear(c.center, springDistance * 2 * (float)Math.Sqrt(2)))
                {
                    var spring = near.springs.FirstOrDefault(x => x.a == near && x.b == c);
                    if (spring == null)
                    {
                        c.springs.Add(new Spring(c, near));
                        create_springs++;
                    }
                    else
                        c.springs.Add(spring);
                }
            Console.WriteLine($"Circles: {circles.Count}");
            Console.WriteLine($"Springs created: {create_springs}");
        }
        public void Update()
        {
            //add velosities
            foreach (var s in circles)
                s.velosity += new Vector2f(0, gravity);

            //update velosities
            foreach (var c in circles)
                c.Update();
            //update springs
            foreach (var c in circles)
                foreach (var s in c.springs)
                    s.Update();
            //collision
            foreach (var c in circles)
                c.CollisionWithBorders();
            //collision
            foreach (var a in circles)
                foreach (var b in all_circles)
                {
                    float distance = Tools.Distance(a.center, b.center);
                    float penetration = distance - a.radius - b.radius ;
                    if (penetration < 0 && distance != 0)
                    {
                        var dir_normalized_to_b = Tools.Normalize(b.center - a.center);
                        b.center = a.center + dir_normalized_to_b * (a.radius + b.radius);
                        a.velosity += -dir_normalized_to_b * -penetration / 2;
                        b.velosity += dir_normalized_to_b * -penetration / 2;
                    }
                }
            //collision
            foreach (var circle in circles)
                foreach (var rect in RectagleCollider.bodies)
                {
                    if (rect.Intersect(circle, out var d, out var delta))
                    {
                        var delta_dir = Tools.Normalize(delta);
                        if (rect.viscosity == 0)
                        {
                            circle.center += delta_dir * (circle.radius - Tools.Length(delta));
                        }

                        circle.velosity *= rect.viscosity;//вязкость
                        float volume = Math.Min((circle.center.Y + circle.radius - rect.center.Y + rect.height / 2) / (circle.radius * 2), 2);//глубина погружения
                        circle.velosity.Y += rect.density * -gravity * volume;//сила архимеда
                    }
                }
        }

        public void Draw(RenderWindow app)
        {
            foreach (var s in circles)
                s.Draw(app);
        }
    }
}
