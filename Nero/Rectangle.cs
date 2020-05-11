using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero
{
    using SFML.Graphics;
    public struct Rectangle : IEquatable<Rectangle>
    {
        public float x;
        public float y;
        public float width;
        public float height;

        /// <summary>
        /// Posição
        /// </summary>
        public Vector2 position
        {
            get => new Vector2(x, y);
            set
            {
                x = value.x;
                y = value.y;
            }
        }

        /// <summary>
        /// Tamanho
        /// </summary>
        public Vector2 size
        {
            get => new Vector2(width, height);
            set
            {
                width = value.x;
                height = value.y;
            }
        }

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Rectangle(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="position"></param>
        /// <param name="size"></param>
        public Rectangle(Vector2 position, Vector2 size) : this(position.x, position.y, size.x, size.y)
        { }

        #region Operators
        public static Rectangle operator +(Rectangle v1, Rectangle v2)
            => new Rectangle(v1.position + v2.position, v1.size + v2.size);

        public static Rectangle operator -(Rectangle v1, Rectangle v2)
            => new Rectangle(v1.position - v2.position, v1.size - v2.size);

        public static bool operator ==(Rectangle v1, Rectangle v2)
            => v1.Equals(v2);

        public static bool operator !=(Rectangle v1, Rectangle v2)
            => !v1.Equals(v2);

        public static implicit operator IntRect(Rectangle v)
            => new IntRect(v.position, v.size);

        public static explicit operator Rectangle(IntRect v)
            => new Rectangle(v.Left, v.Top, v.Width, v.Height);

        public static implicit operator FloatRect(Rectangle v)
            => new FloatRect(v.position, v.size);

        public static explicit operator Rectangle(FloatRect v)
            => new Rectangle(v.Left, v.Top, v.Width, v.Height);

        #endregion

        public bool Equals(Rectangle other)
            => position.Equals(other.position) && size.Equals(other.size);

        public override bool Equals(object obj)
            => obj is Rectangle && Equals((Rectangle)obj);

        public override int GetHashCode()
            => position.GetHashCode() + size.GetHashCode();        
    }
}
