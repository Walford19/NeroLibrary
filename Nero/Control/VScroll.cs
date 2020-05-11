using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero.Control
{
    using SFML.Window;
    using SFML.Graphics;
    using static Renderer;
    using Color = Nero.Color;
    public class VScroll : Control
    {
        /// <summary>
        /// Máximo de valor
        /// </summary>
        public int Maximum = 100;

        /// <summary>
        /// Valor atual
        /// </summary>
        public int Value = 0;

        /// <summary>
        /// Cor de fundo
        /// </summary>
        public Color FillColor = new Color(10, 10, 10, 200);

        /// <summary>
        /// Espessura da borda
        /// </summary>
        public float OutlineThickness = 1;

        /// <summary>
        /// Cor da borda
        /// </summary>
        public Color OutlineColor = new Color(33, 42, 52);

        public SceneBase scene;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="bond"></param>
        public VScroll(Bond bond, SceneBase scene) : base(bond)
        {
            this.scene = scene;
        }

        /// <summary>
        /// Desenha a scroll
        /// </summary>
        /// <param name="target"></param>
        /// <param name="states"></param>
        public override void Draw(RenderTarget target, RenderStates states)
        {
            if (Value > Maximum) Value = Maximum;
            var gp = GlobalPosition();

            DrawRectangle(target, gp, Size, FillColor, OutlineThickness, OutlineColor);
            base.Draw(target, states);

            float percent = (1f) / (Maximum + 1f);
            float height = (Size.y - 2) * percent;
            float height_real = height < 4 ? 4 : height;
            float posY = 1 + height * Value - (height_real / 2);
            DrawRectangle(target, gp + new Vector2(1, posY < 1 ? 1 : (posY + height_real > Size.y - 1 ? Size.y - 1 - height_real : posY)),
                new Vector2(Size.x - 2, height_real), OutlineColor);
        }

        /// <summary>
        /// Scroll do Mouse
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public override bool MouseScrolled(MouseWheelScrollEventArgs e)
        {
            var result = base.MouseScrolled(e);
            if (Hover())
            {
                if (e.Wheel == Mouse.Wheel.VerticalWheel)
                {
                    Value -= (int)(e.Delta * (float)(Maximum / 100));
                    if (Value < 0) Value = 0;
                    if (Value > Maximum) Value = Maximum;
                    return true;
                }
            }

            return result;
        }

        /// <summary>
        /// Scroll do Mouse
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool MouseScrolled2(MouseWheelScrollEventArgs e)
        {
            var result = base.MouseScrolled(e);

            if (e.Wheel == Mouse.Wheel.VerticalWheel)
            {
                Value -= (int)(e.Delta * (float)Math.Max(5, (Maximum / 100)));
                if (Value < 0) Value = 0;
                if (Value > Maximum) Value = Maximum;
                return true;
            }

            return result;
        }

        bool _press = false;
        int _y = 0;
        public override bool MousePressed(MouseButtonEvent e)
        {
            var result = base.MousePressed(e);
            if (Hover())
            {
                var gp = GlobalPosition();
                float percent = (1f) / (Maximum + 1f);
                float height = (Size.y - 2) * percent;
                var pos = gp + new Vector2(1, 1 + height * Value);
                if (e.Button == Mouse.Button.Left && Maximum > 1)
                    if (e.X >= pos.x && e.X <= pos.x + Size.x - 2)
                        if (e.Y >= pos.y && e.Y <= pos.y + height)
                        {
                            _press = true;
                            _y = e.Y;
                            scene?.SetControlPriority(this);
                        }
            }
            return result;
        }

        public override bool MouseReleased(MouseButtonEvent e)
        {
            _press = false;
            scene?.SetControlPriority(null);
            return base.MouseReleased(e);
        }

        public override bool MouseMoved(Vector2 e)
        {
            var result = base.MouseMoved(e);

            if (Maximum > 1 && Mouse.IsButtonPressed(Mouse.Button.Left) && _press)
            {
                var calcy = e.y - (GlobalPosition().y + 1);
                if (calcy < 0) calcy = 0;

                float percent = calcy / (Size.y - 2);
                if (percent > 1f) percent = 1;

                Value = (int)(percent * Maximum);
                if (Value < 0) Value = 0;
                if (Value > Maximum) Value = Maximum;
            }
            return result;
        }
    }
}
