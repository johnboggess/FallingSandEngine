using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
namespace FallingSandEngine.Sand
{
    public class Sand : Powder
    {
        public Sand()
        {
            Name = "Sand";
            Color = World.SandColor;
            Processable = true;
            Drawable = true;
            Density = 10;
        }

        public override void Process()
        {
            base.Process();
        }
    }
}
