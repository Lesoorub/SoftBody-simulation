using SFML.Graphics;
using SFML.Window;

namespace Sem6_KG_Lab2
{
    public static class Application
    {
        public static RenderWindow app;
        public static uint width = 1920;
        public static uint height = 1080;
        public static void Start()
        {
            app = new RenderWindow(new VideoMode(width, height), "Лабораторная #2", Styles.None);
            app.Position = new SFML.System.Vector2i(0, 0);
            app.SetFramerateLimit(60);
            app.SetVerticalSyncEnabled(true);
            app.Closed += (e, s) => app.Close();
            app.KeyPressed += App_KeyPressed;
            app.MouseButtonPressed += (a, b) =>
            {
                OnClickDown?.Invoke(b);
            };
            app.MouseButtonReleased += (a, b) =>
            {
                OnClickUp?.Invoke(b);
            };
            while (app.IsOpen)
            {
                app.DispatchEvents();
                app.Clear();
                OnUpdate?.Invoke();
                app.Display();
            }
        }

        private static void App_KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Escape)
                app.Close();
        }

        public delegate void ClickArgs(MouseButtonEventArgs args);
        public static event ClickArgs OnClickDown;
        public static event ClickArgs OnClickUp;

        public delegate void UpdateArgs();
        public static event UpdateArgs OnUpdate;
    }
}
