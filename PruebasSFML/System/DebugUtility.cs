using System;
using System.Threading;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using PruebasSFML.System;

namespace PruebasSFML.Game
{
    public static class DebugUtility
    {
        public const string FontPath = @"D:\Users\Adri\Documents\Visual Studio Projects\PruebasSFML\PruebasSFML\bin\Debug\fonts\Roboto-Regular.ttf";
        public const uint FontSize = 14;
        public static Color FontColor = Color.Black;
        public static Font Font;

        public static void LoadContent()
        {
            Font = new Font(FontPath);
        }

        public static void DrawPerformanceData(GameLoop gameLoop)
        {
            if (Font != null)
            {
                string timeElapsed = gameLoop.GameTime.TotalTimeElapsed.ToString("0.000");
                string deltaTime = gameLoop.GameTime.DeltaTime.ToString("0.000");
                string fps = (1f / gameLoop.GameTime.DeltaTime).ToString("0.000");

                Text timeElapsed_lbl = new Text(timeElapsed, Font, FontSize);
                timeElapsed_lbl.Position = new Vector2f(4, 8);
                timeElapsed_lbl.FillColor = FontColor;

                Text deltaTime_lbl = new Text(deltaTime, Font, FontSize);
                deltaTime_lbl.Position = new Vector2f(4, 28);
                deltaTime_lbl.FillColor = FontColor;

                Text fps_lbl = new Text(fps, Font, FontSize);
                fps_lbl.Position = new Vector2f(4, 48);
                fps_lbl.FillColor = FontColor;

                gameLoop.Window.Draw(timeElapsed_lbl);
                gameLoop.Window.Draw(deltaTime_lbl);
                gameLoop.Window.Draw(fps_lbl);
            }
        }

    }
}
