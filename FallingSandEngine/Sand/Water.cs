using System;
using System.Collections.Generic;
using System.Text;

namespace FallingSandEngine.Sand
{
    public class Water : Fluid
    {
        public Water()
        {
            Density = 1;
            Processable = true;
            Drawable = true;
            Color = World.WaterColor;
            Name = "Water";
        }

        public override void Process()
        {
            base.Process();
        }
    }
}
