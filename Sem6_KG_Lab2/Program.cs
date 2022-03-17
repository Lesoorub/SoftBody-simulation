using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;
using SFML.System;
using static Sem6_KG_Lab2.Application;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using SFML.Graphics;

namespace Sem6_KG_Lab2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            OnUpdate += Update;
            Start();
            OnClickDown += OnClick;
            Application.Start();
        }

        private static void OnClick(MouseButtonEventArgs args)
        {
            if (args.Button == Mouse.Button.Left)
                foreach (var body in bodies)
                    foreach (var c in body.circles)
                        c.velosity += new Vector2f(args.X - c.center.X, args.Y - c.center.Y) / 100;
            if (args.Button == Mouse.Button.Right)
                foreach (var body in bodies)
                    foreach (var c in body.circles)
                        c.velosity -= new Vector2f(args.X - c.center.X, args.Y - c.center.Y) / 100;
            if (args.Button == Mouse.Button.Middle)
            {
                var b = new SoftBody(new Vector2f(args.X, args.Y), 0);
                bodies.Add(b);
                SoftBody.all_circles = SoftBody.all_circles.Concat(b.circles).ToArray();
            }
        }

        static List<SoftBody> bodies = new List<SoftBody>();
        static Stopwatch sw = new Stopwatch();
        private static void Start()
        {
            Random rnd = new Random();
            bodies.Add(new SoftBody(new Vector2f((int)width / 2, (int)height / 2), 1));
            //bodies.Add(new SoftBody(new Vector2f(rnd.Next(0, (int)width), rnd.Next(0, (int)height)), 1));

            var water = new RectagleCollider()
            {
                center = new Vector2f((int)width / 2, height - 100),
                width = width,
                height = 200,
                color = new Color(0, 0, 255, 128),
                viscosity = 0.95f,
                density = 2
            };
            var honey = new RectagleCollider()
            {
                center = new Vector2f(50, height / 2),
                width = 100,
                height = 200,
                color = new Color(0xff, 0xdc, 0, 128),
                viscosity = 0.5f,
                density = 0
            };
            var box = new RectagleCollider()
            {
                center = new Vector2f(width - 200, height / 2),
                width = 200,
                height = 10,
                color = new Color(0xff, 0xdc, 0xc4, 128),
                viscosity = 0
            };

            SoftBody.all_circles = bodies.SelectMany(x => x.circles).ToArray();
        }
        private static void Update()
        {
            sw.Restart();
            foreach (var body in bodies)
            {
                body.Update();
                body.Draw(app);
            }
            foreach (var rect in RectagleCollider.bodies)
            {
                rect.Draw(app);
            }
            Console.WriteLine("Update: " + sw.ElapsedTicks / (float)TimeSpan.TicksPerMillisecond + " ms");
        }
    }
}
