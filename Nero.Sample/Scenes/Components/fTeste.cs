using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nero;

namespace Nero.Sample.Scenes.Components
{
    using Control;
    class fTeste : Form
    {
        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="bond"></param>
        public fTeste(Bond bond) : base(bond)
        {
            Size = new Vector2(300, 150);
            Title = "Teste control";
            Anchor = Anchors.Center;
        }
    }
}
