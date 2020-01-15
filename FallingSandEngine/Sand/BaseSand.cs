using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace FallingSandEngine.Sand
{
    public abstract class BaseSand
    {
        public Cell Cell;
        public World World { get { return Cell.World; } }
        public bool Drawable = false;
        public bool Processable = false;
        public Color Color = Color.Black;
        public ulong LastProcessedFrame = ulong.MaxValue;
        public string Name;

        public float Gravity = .5f;
        public float velocityX = 0;
        public float velocityY = 0;
        public int Density = 1;

        public virtual void Process()
        {
            throw new Exception();
        }
    }
}
