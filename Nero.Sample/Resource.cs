using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nero;
namespace Nero.Sample
{
    static class Resource
    {
        /// <summary>
        /// Inicializa os recursos
        /// </summary>
        public static void Initialize()
        {
            Game.LoadFont("res/consola.ttf");
        }
    }
}
