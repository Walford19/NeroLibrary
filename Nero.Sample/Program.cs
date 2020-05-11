using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nero;

namespace Nero.Sample
{
    class Program
    {
        [STAThread]
        public static void Main()
        {
            var game = new Game();
            game.Title = "Nero Library Game - Test";
            game.VSync = false;
            game.OnInitialize = Initialize;
            game.OnResources = Resource.Initialize;
            game.OnDraw = Draw;
            game.Run();            
        }

        static void Initialize()
        {
            Game.FPS_visible = true;
        }

        static void Draw(RenderTarget target)
        {
            Renderer.DrawText(target, "Porra", 24, new Vector2(20.5f, 100), Color.White);
        }
    }
}
