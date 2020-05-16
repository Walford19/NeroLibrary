using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Nero
{
    using SFML.Graphics;
    using NativeTexture = SFML.Graphics.Texture;
    public class Texture
    {
        NativeTexture texture;
        LargeTexture largeTexture;
        internal TextureTypes type = TextureTypes.Normal;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="name"></param>
        public Texture(string filename, bool large = false)
        {
            if (!File.Exists(filename))
                throw new Exception($"Arquivo não encontrado!\n{filename}");

            if (large)
            {
                type = TextureTypes.Large;
                largeTexture = Loader.LoadLargeTexture(filename);
            }
            else
            {
                type = TextureTypes.Normal;
                texture = Loader.LoadNativeTexture(filename);
            }
        }

        internal NativeTexture GetTexture() => texture;
        internal LargeTexture GetLargeTexture() => largeTexture;

        /// <summary>
        /// Tamanho da textura
        /// </summary>
        public Vector2 size
        {
            get
            {
                if (type == TextureTypes.Normal)
                    return texture != null ? (Vector2)texture.Size : Vector2.Zero;
                else
                    return largeTexture != null ? (Vector2)largeTexture.Size : Vector2.Zero;
            }
        }

        /// <summary>
        /// Redimensionamento suavel
        /// </summary>
        public bool Smooth
        {
            get
            {
                if (type == TextureTypes.Normal)
                    return texture.Smooth;
                else
                    return largeTexture.Smooth;
            }

            set
            {
                if (type == TextureTypes.Normal)
                    texture.Smooth = value;
                else
                    largeTexture.Smooth = value;
            }
        }
    }
}
