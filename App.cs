using ArcadianEngine.Types;
using Raylib_cs;

namespace ArcadianEngine
{
    /// <summary>
    /// A Arcadian Engine project, may it be a app or a game.
    /// </summary>
    public class App
    {
        /// <summary>
        /// Structure for storing app settings and metadata like the window title.
        /// </summary>
        struct AppData
        {
            public string Title { get; set; }
            public Vector2i WindowSize { get; set; }
            bool ShowDebugPrefix { get; set; }
        }

        /// <summary>
        /// The Main app settings and metadata.
        /// </summary>
        AppData _data;

        public App(string windowTitle = "Arcadian Engine App", int windowWidth = 960, int windowHeigh = 540, bool showDebugSufixInTitle = true)
        {
            _data = new()
            {
                Title = windowTitle,
                WindowSize = new Vector2i(windowWidth, windowHeigh)
            };
        }

        /// <summary>
        /// Initialize the main window and start the game loop.
        /// </summary>
        public void Run()
        {
#if DEBUG
            Raylib.InitWindow(_data.WindowSize.x, _data.WindowSize.y, $"{_data.Title} - [Debug]");
#else
            Raylib.InitWindow(_data.WindowSize.x, _data.WindowSize.y, $"{_data.Title}");
#endif
            Raylib.SetTargetFPS(60);

            this.OnAppStart();

            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();

                this.OnAppUpdate(Raylib.GetFrameTime());
                this.OnAppDraw();

                Raylib.EndDrawing();
            }

            this.OnAppStop();

            Raylib.CloseWindow();
        }

        public virtual void OnAppStart() { }

        public virtual void OnAppUpdate(float deltaTime) { }

        public virtual void OnAppDraw() { }

        public virtual void OnAppStop() { }

        public void SetWindowTitle(string title)
        {
            _data.Title = title;
            Raylib.SetWindowTitle(_data.Title);
        }

        public void ResizeWindow(Vector2i size)
        {
            _data.WindowSize = size;
            Raylib.SetWindowSize(_data.WindowSize.x, _data.WindowSize.y);
        }
    }
}
