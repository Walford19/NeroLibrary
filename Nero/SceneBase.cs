﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero
{
    using Control;
    using SFML.Window;
    using static Renderer;
    public abstract class SceneBase : Bond
    {
        #region Properties
        protected string alert_message = "";
        protected int alert_timer = 0;

        public bool FadeActive = true;

        int fade_opacity = 0;
        byte fade_state = 0;
        #endregion

        #region Methods
        /// <summary>
        /// Construtor
        /// </summary>
        public SceneBase() : base()
        {
            Size = (Vector2)Game.Size;
        }

        /// <summary>
        /// Gera os componentes
        /// </summary>
        protected void GenerateComponents()
        {
            try
            {
                string _namespace = GetType().Namespace + ".Components";
                var list_componentes = GetType().Assembly.GetTypes()
                    .Where(i => i.IsClass && i.Namespace == _namespace && i.BaseType.BaseType != null &&
                    (i.BaseType.BaseType.Name.Contains("Control") || i.BaseType.BaseType.Name.Contains("Bond"))).ToList();

                if (list_componentes.Count > 0)
                    foreach (var i in list_componentes)
                        Activator.CreateInstance(i, this);
            }
            catch { }
        }

        /// <summary>
        /// Desenha a tela
        /// </summary>
        /// <param name="target"></param>
        /// <param name="states"></param>
        public override void Draw(RenderTarget target, RenderStates states)
        {
            base.Draw(target, states);

            if (isAlert)
                Draw_Alert(target);

            if (isFade)
                Draw_Fade(target);
        }

        /// <summary>
        /// Atualiza a tela
        /// </summary>
        public virtual void Update()
        {
            if (isAlert && Environment.TickCount > alert_timer)
            {
                alert_message = "";
                alert_timer = 0;
            }

            if (fade_state > 0)
            {
                if (fade_state == 1 && fade_opacity < 255)
                {
                    fade_opacity += (int)(150 * Game.DeltaTime);
                    if (fade_opacity >= 255)
                    {
                        fade_opacity = 255;
                        fade_state = 0;
                        Game.NextScene();
                    }
                }
                else if (fade_state == 2 && fade_opacity > 0)
                {
                    fade_opacity -= (int)(150 * Game.DeltaTime);
                    if (fade_opacity <= 0)
                    {
                        fade_state = 0;
                        fade_opacity = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Entrada de texto
        /// </summary>
        /// <param name="e"></param>
        public virtual void TextEntered(TextEventArgs e)
        {
            if (isFade) return;

            if (isAlert)
                return;

            if (TextBox.Focus != null && !TextBox.Focus.Blocked)
            {
                var tbox = TextBox.Focus;
                int m = char.Parse(e.Unicode);
                if (m >= 32 && m <= 255)
                {
                    if (TextBox.Focus.MaxLength > 0 && TextBox.Focus.Text.Length >= TextBox.Focus.MaxLength)
                        return;

                    if (TextBox.Focus.isNumeric)
                    {
                        if (e.Unicode.IsNumeric())
                        {
                            tbox.Text = tbox.Text.Insert(TextBox.Focus.Character_CurrentIndex, e.Unicode);
                            tbox.Character_CurrentIndex++;
                        }
                    }
                    else
                    {
                        tbox.Text = tbox.Text.Insert(TextBox.Focus.Character_CurrentIndex, e.Unicode);
                        tbox.Character_CurrentIndex++;
                    }

                }
                else
                {

                    if (e.Unicode == "\r")
                        TextBox.Focus.EnterPress();

                    // Backspace
                    if (e.Unicode == "\b")
                    {
                        if (tbox.Character_CurrentIndex > 0)
                        {
                            tbox.Text = tbox.Text.Remove(tbox.Character_CurrentIndex - 1, 1);
                            tbox.Character_CurrentIndex--;
                        }
                        else if (tbox.Multiple_Lines && tbox.Lines.Count > 1)
                        {
                            string lineText = tbox.Text;
                            TextBox.Focus.Lines.RemoveAt(TextBox.Focus.Line_SelectIndex);
                            if (TextBox.Focus.Line_SelectIndex > 0)
                            {
                                TextBox.Focus.Line_SelectIndex--;
                                tbox.Character_CurrentIndex = tbox.Text.Length;
                                tbox.Text += lineText;
                            }
                        }
                    }

                    // Tab
                    if (e.Unicode == "\t")
                        TextBox.Focus.TabPress();
                }
            }
        }

        /// <summary>
        /// Desenha o alerta
        /// </summary>
        /// <param name="target"></param>
        protected virtual void Draw_Alert(RenderTarget target)
        {
            // Fundo
            DrawRectangle(target, new Vector2(), Size, new Color(0, 0, 0, 210));

            // Texto
            DrawText(target, alert_message, 12, new Vector2((Size.x - GetTextWidth(alert_message)) / 2, Size.y / 2 - 7), Color.White);
        }

        /// <summary>
        /// Desenha o fade
        /// </summary>
        /// <param name="target"></param>
        void Draw_Fade(RenderTarget target)
        {
            DrawRectangle(target, new Vector2(), Size, new Color(0, 0, 0, (byte)fade_opacity));
        }

        /// <summary>
        /// Alerta
        /// </summary>
        /// <param name="message"></param>
        public void Alert(string message)
        {
            alert_message = message;
            alert_timer = Environment.TickCount + 3000;
        }

        /// <summary>
        /// Checa se tem alerta
        /// </summary>
        protected bool isAlert
            => alert_message.Length > 0;

        /// <summary>
        /// Mouse pressionado
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public override bool MousePressed(MouseButtonEvent e)
        {
            if (isFade) return false;
            if (isAlert)
                return false;

            return base.MousePressed(e);
        }

        /// <summary>
        /// Movimento do mouse
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public override bool MouseMoved(Vector2 e)
        {
            if (isFade) return false;
            if (isAlert)
                return false;
            return base.MouseMoved(e);
        }

        /// <summary>
        /// Solta o clique
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public override bool MouseReleased(MouseButtonEvent e)
        {
            if (isFade) return false;
            if (isAlert)
            {
                alert_message = "";
                alert_timer = 0;
                return false;
            }

            var result = base.MouseReleased(e);
            if (!result)
            {
                TextBox.Focus = null;
            }

            return result;
        }

        public virtual void KeyPressed(KeyEventArgs e)
        {
            if (isFade) return;
            var tBox = TextBox.Focus;
            if (tBox != null)
            {
                if (e.Code == Keyboard.Key.Left)
                {
                    if (tBox.Character_CurrentIndex > 0)
                        tBox.Character_CurrentIndex--;
                }

                if (e.Code == Keyboard.Key.Right)
                {
                    if (tBox.Character_CurrentIndex < tBox.Text.Length)
                        tBox.Character_CurrentIndex++;
                }

                if (e.Code == Keyboard.Key.Up)
                {
                    if (tBox.Multiple_Lines && tBox.Line_SelectIndex > 0)
                    {
                        tBox.Line_SelectIndex--;
                        if (tBox.Character_CurrentIndex > tBox.Text.Length) tBox.Character_CurrentIndex = tBox.Text.Length + 1;
                    }
                }

                if (e.Code == Keyboard.Key.Down)
                {
                    if (tBox.Multiple_Lines && tBox.Line_SelectIndex < tBox.Lines.Count - 1)
                    {
                        tBox.Line_SelectIndex++;
                        if (tBox.Character_CurrentIndex > tBox.Text.Length) tBox.Character_CurrentIndex = tBox.Text.Length;
                    }
                }

                if (e.Code == Keyboard.Key.End)
                {
                    tBox.Character_CurrentIndex = tBox.Text.Length;
                }

                if (e.Code == Keyboard.Key.Home)
                {
                    tBox.Character_CurrentIndex = 0;
                }

                if (e.Code == Keyboard.Key.Delete)
                {
                    if (tBox.Character_CurrentIndex < tBox.Text.Length)
                        tBox.Text = tBox.Text.Remove(tBox.Character_CurrentIndex, 1);
                    else
                    {
                        if (tBox.Multiple_Lines && tBox.Line_SelectIndex < tBox.Lines.Count - 1)
                        {
                            tBox.Text += tBox.Lines[tBox.Line_SelectIndex + 1];
                            tBox.Lines.RemoveAt(tBox.Line_SelectIndex + 1);
                        }
                    }
                }

            }

        }

        public virtual void KeyReleased(KeyEventArgs e)
        { }

        protected bool isFade => fade_state > 0;

        internal void FadeOn()
        {
            if (!FadeActive) return;
            fade_state = 1;
            fade_opacity = 0;
        }

        internal void FadeOff()
        {
            if (!FadeActive) return;
            fade_state = 2;
            fade_opacity = 255;
        }

        #endregion
    }
}
