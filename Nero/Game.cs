using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero
{
    using SFML.Graphics;
    using SFML.System;
    using SFML.Window;

    public sealed class Game
    {        
        public static bool Running = false;
        public static Vector2 Size = new Vector2(1024, 600);
        public static Game instance;
        public static bool FPS_visible = true;
        public static int FPS = 0;
        public static float DeltaTime;
        public static Vector2 MousePosition;
        static SceneBase scene = null;
        static SceneBase nextscene = null;

        public string Title = "Nero Library Game";
        public bool VSync = false;
        public Action OnInitialize = delegate { };        
        public Action OnResources = delegate { };

        RenderWindow Window;        

        /// <summary>
        /// Roda o jogo
        /// </summary>
        public void Run()
        {
            instance = this;

            OnInitialize.Invoke();
            OnResources.Invoke();

            CreateWindow();
            HandleEvents();

            Running = true;
            GameLoop();
        }

        /// <summary>
        /// Loop do jogo
        /// </summary>
        void GameLoop()
        {
            int timer_delay = 0;
            int timer_fps = 0, countfps = 0;
            var clock = new Clock();

            while(Running)
            {
                if (Environment.TickCount > timer_delay)
                {
                    // Delta Time
                    DeltaTime = clock.Restart().AsSeconds();

                    // Atualiza a cena
                    scene?.Update();

                    // Dispara os eventos da janela
                    Window.DispatchEvents();

                    Window.Clear(Color.CornflowerBlue);

                    // Desenha a cena
                    scene?.Draw(Window, RenderStates.Default);
                    
                    if (FPS_visible)
                        Renderer.DrawText(Window, "FPS: " + FPS, 12, new Vector2(10, 10), Color.White, 1, new Color(0, 0, 0, 100));

                    Window.Display();

                    countfps++;
                    if (Environment.TickCount > timer_fps)
                    {
                        FPS = countfps;
                        countfps = 0;
                        timer_fps = Environment.TickCount + 1000;
                    }

                    timer_delay = Environment.TickCount + 1;
                }

            }
        }

        /// <summary>
        /// Cria a janela
        /// </summary>
        void CreateWindow()
        {
            var video = new VideoMode((uint)Size.x, (uint)Size.y);
            Window = new RenderWindow(video, Title, Styles.Close);
            Window.SetVerticalSyncEnabled(VSync);            
        }

        /// <summary>
        /// Evento da janela
        /// </summary>
        void HandleEvents()
        {
            Window.Closed += Window_Closed;
            Window.MouseButtonPressed += Window_MouseButtonPressed;
            Window.MouseButtonReleased += Window_MouseButtonReleased;
            Window.MouseMoved += Window_MouseMoved;
            Window.MouseWheelScrolled += Window_MouseWheelScrolled;
            Window.TextEntered += Window_TextEntered;
        }

        /// <summary>
        /// Carrega a font
        /// </summary>
        /// <param name="filename"></param>
        public static void LoadFont(string filename)
        {
            var f = new Font(filename);
            Renderer.gameFont = f;
            Renderer._text = new Text("", Renderer.gameFont);
        }

        private void Window_TextEntered(object sender, TextEventArgs e)
        {
            if (scene != null) scene.TextEntered(e);
        }

        private void Window_MouseWheelScrolled(object sender, MouseWheelScrollEventArgs e)
        {
            if (scene != null) scene.MouseScrolled(e);
        }

        private void Window_MouseMoved(object sender, MouseMoveEventArgs e)
        {
            MousePosition = new Vector2(e.X, e.Y);
            if (scene != null) scene.MouseMoved(MousePosition);
        }

        private void Window_MouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            var m = new MouseButtonEvent();
            m.Button = e.Button;
            m.X = e.X;
            m.Y = e.Y;
            if (scene != null) scene.MouseReleased(m);
        }

        private void Window_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            var m = new MouseButtonEvent();
            m.Button = e.Button;
            m.X = e.X;
            m.Y = e.Y;
            if (scene != null) scene.MousePressed(m);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Window.Close();
            Running = false;
        }

        public static void SetScene(SceneBase newScene)
        {
            if (scene != null && nextscene.FadeActive)
            {
                nextscene = newScene;
                scene?.FadeOn();
            }
            else
            {
                scene = newScene;
                scene.FadeOff();
            }
        }

        public static T GetScene<T>() where T : SceneBase
            => (T)scene;

        internal static void NextScene()
        {
            if (nextscene != null && nextscene != scene)
            {
                scene = nextscene;
                scene.FadeOff();
            }
        }
    }
}
