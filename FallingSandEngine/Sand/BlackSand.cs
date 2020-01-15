using System;
using System.Collections.Generic;
using System.Text;

namespace FallingSandEngine.Sand
{
    public class BlackSand : Powder
    {
        public BlackSand()
        {
            Name = "Black Sand";
            Color = System.Drawing.Color.Black;
            Processable = true;
            Drawable = true;
            Density = 20;
        }
    }
}
