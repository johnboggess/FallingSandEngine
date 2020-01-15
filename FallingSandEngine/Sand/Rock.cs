using System;
using System.Collections.Generic;
using System.Text;

namespace FallingSandEngine.Sand
{
    public class Rock : BaseSand
    {
        public Rock()
        {
            Name = "Rock";
            Color = World.RockColor;
            Drawable = true;
            Density = int.MaxValue;
        }
    }
}
