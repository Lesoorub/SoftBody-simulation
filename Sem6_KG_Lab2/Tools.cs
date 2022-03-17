using System;
using SFML.System;

namespace Sem6_KG_Lab2
{
    public static class Tools
    {
        public static float LengthSqr(Vector2f a) => (float)Math.Pow(a.X, 2) + (float)Math.Pow(a.Y, 2);
        public static float DistanceSqr(Vector2f a, Vector2f b) => LengthSqr(b - a);
        public static float Length(Vector2f a) => (float)Math.Sqrt(Math.Pow(a.X, 2) + Math.Pow(a.Y, 2));
        public static float Distance(Vector2f a, Vector2f b) => (float)Math.Sqrt(DistanceSqr(a, b));
        public static Vector2f Normalize(Vector2f a) => a / Length(a);
    }
}
