using System;
using System.Collections.Generic;
using System.Text;

namespace FallingSandEngine.Sand
{
    public class Wood : BaseSand
    {
        public Wood()
        {
            Name = "Wood";
            Density = int.MaxValue;
            Drawable = true;
            Color = World.WoodColor;
        }
    }
}
