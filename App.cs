using ArcadianEngine.Types;
using Raylib_cs;

namespace ArcadianEngine
{
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

        /// <summary>
        /// A Arcadian Engine project, may it be a app or a game.
        /// </summary>
        /// <param name="windowTitle"></param>
        /// <param name="windowWidth"></param>
        /// <param name="windowHeigh"></param>
        /// <param name="showDebugSufixInTitle"></param>
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

            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();

                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();
        }

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
