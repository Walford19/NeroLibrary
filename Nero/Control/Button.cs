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
    public class Button : Control
    {
        #region Properties
        /// <summary>
        /// Texto no botão
        /// </summary>
        public string Text = "";

        /// <summary>
        /// Cor do Botão
        /// </summary>
        public Color FillColor = new Color(52, 75, 75);

        /// <summary>
        /// Cor do Botão - Hover
        /// </summary>
        public Color FillColor_hover = new Color(72, 95, 95);

        /// <summary>
        /// Cor do Botão - Press
        /// </summary>
        public Color FillColor_press = new Color(32, 55, 55);

        /// <summary>
        /// Espessura da borda
        /// </summary>
        public float OutlineThickness = 1f;

        /// <summary>
        /// Cor da borda
        /// </summary>
        public Color OutlineColor = new Color(40, 40, 40);

        /// <summary>
        /// Cor da borda - Hover
        /// </summary>
        public Color OutlineColor_hover = new Color(60, 60, 60);

        /// <summary>
        /// Cor da borda - Press
        /// </summary>
        public Color OutlineColor_press = new Color(30, 30, 30);

        /// <summary>
        /// Transparência da borda
        /// </summary>
        public byte Border_Opacity = 255;

        /// <summary>
        /// Escala da borda
        /// </summary>
        public float Border_Scale = 0.1f;

        Vertex[] grandients;
        bool _debugHover = false;
        bool _debugPress = false;
        #endregion

        #region Methods
        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="bond"></param>
        public Button(Bond bond) : base(bond)
        {
            grandients = new Vertex[4];
        }

        /// <summary>
        /// Desenha o botão
        /// </summary>
        /// <param name="target"></param>
        /// <param name="states"></param>
        public override void Draw(RenderTarget target, RenderStates states)
        {
            var gp = GlobalPosition();

            if (_debugHover && !Hover()) _debugHover = false;

            // Fundo
            DrawShadowRectangle(target, gp - new Vector2(OutlineThickness, OutlineThickness), Size +
                new Vector2(OutlineThickness, OutlineThickness) * 2, Border_Opacity, Border_Scale);
            if (_debugHover)
                if (_debugPress)
                    DrawRectangle(target, gp, Size, FillColor_press, OutlineThickness, OutlineColor_press);
                else
                    DrawRectangle(target, gp, Size, FillColor_hover, OutlineThickness, OutlineColor_hover);
            else
                DrawRectangle(target, gp, Size, FillColor, OutlineThickness, OutlineColor);


            // Gradient
            grandients[0] = new Vertex(gp + new Vector2(0, Size.y), new Color(0, 0, 0, 100));
            grandients[1] = new Vertex(gp + new Vector2(0, 0), new Color(0, 0, 0, 10));
            grandients[2] = new Vertex(gp + new Vector2(Size.x, 0), new Color(0, 0, 0, 10));
            grandients[3] = new Vertex(gp + new Vector2(Size.x, Size.y), new Color(0, 0, 0, 100));
            target.Draw(grandients, PrimitiveType.Quads);

            // Texto do botão
            DrawText(target, Text, 11, gp + new Vector2((Size.x - GetTextWidth(Text, 11)) / 2, (Size.y / 2) - 7), Color.White);

            base.Draw(target, states);
        }

        /// <summary>
        /// Movimento do mouse
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public override bool MouseMoved(Vector2 e)
        {
            var result = base.MouseMoved(e);
            _debugHover = false;
            if (Hover())
                _debugHover = true;

            return result;
        }

        /// <summary>
        /// Mouse pressionado
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public override bool MousePressed(MouseButtonEvent e)
        {
            var result = base.MousePressed(e);
            if (_debugHover)
                _debugPress = true;
            return result;
        }

        /// <summary>
        /// Solta o clique
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public override bool MouseReleased(MouseButtonEvent e)
        {
            var result = base.MouseReleased(e);
            _debugPress = false;
            return result;
        }
        #endregion

    }
}
