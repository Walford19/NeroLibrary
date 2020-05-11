using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero
{
    public static class Extensions
    {
        /// <summary>
        /// Corrige o valor flutuante
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static float Floor(this float obj)
            => (float)Math.Floor(obj);

        /// <summary>
        /// Valor numérico ou não
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsNumeric(this string s)
        {
            float output;
            return float.TryParse(s, out output);
        }

        /// <summary>
        /// String to Int
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int ToInt32(this string s)
            => int.Parse(s);
    }
}
