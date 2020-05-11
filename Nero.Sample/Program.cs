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
            Game.SetScene(new Scenes.Main());
            game.Run();            
        }

        static void Initialize()
        {
            Game.FPS_visible = true;
        }
    }
}
