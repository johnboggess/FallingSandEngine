using System;
using System.Collections.Generic;
using System.Text;

namespace FallingSandEngine.Sand
{
    public class Oil : Fluid
    {
        public Oil()
        {
            Density = 0;
            Processable = true;
            Drawable = true;
            Color = World.OilColor;
            Name = "Oil";
        }
        public override void Process()
        {
            base.Process();
        }
    }
}
