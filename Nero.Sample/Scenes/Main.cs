using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nero;

namespace Nero.Sample.Scenes
{
    using Control;
    class Main : SceneBase
    {
        Texture tex_sprite;

        SpriteAnimation spriteAnimation;

        Components.fTeste form_test;

        /// <summary>
        /// Construtor
        /// </summary>
        public Main()
        {
            form_test = new Components.fTeste(this);
            form_test.OnDraw += Form_test_OnDraw;

            tex_sprite = new Texture(Resource.Path + "sprite.res");
            spriteAnimation = new SpriteAnimation(tex_sprite);
            spriteAnimation.origin = new Vector2(16, 48);
            spriteAnimation.frame_timer = 250;

            spriteAnimation.Add("normal_down", new Rectangle(Vector2.Zero, new Vector2(32, 48)));
            spriteAnimation.Add("move_down", new Rectangle(new Vector2(32,0), new Vector2(32, 48)),
                new Rectangle(new Vector2(32 * 3, 0), new Vector2(32, 48)));
        }

        private void Form_test_OnDraw(Control sender, RenderTarget target)
        {
            var gp = sender.GlobalPosition();
            spriteAnimation.Position = gp + sender.Size / 2 + new Vector2(0,10);
            if (!Keyboard.IsKeyPressed(Keyboard.Key.Down))
                spriteAnimation.Play(target, "normal_down");
            else
                spriteAnimation.Play(target, "move_down");
        }
    }
}
