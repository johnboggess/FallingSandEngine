using System;
using System.Collections.Generic;
using System.Text;

namespace FallingSandEngine.Sand
{
    public abstract class Fluid : BaseSand
    {
        public override void Process()
        {
            if (Cell.Adjacency.BM.BaseSand.Density < Density)
            {
                Cell goal = Cell.Adjacency.BM;
                Cell.SwapCells(Cell, goal);
            }
            else if (Cell.Adjacency.BL.BaseSand.Density < Density)
            {
                Cell goal = Cell.Adjacency.BL;
                Cell.SwapCells(Cell, goal);
            }
            else if (Cell.Adjacency.BR.BaseSand.Density < Density)
            {
                Cell goal = Cell.Adjacency.BR;
                Cell.SwapCells(Cell, goal);
            }
            else
            {
                int i = World.RNG.Next(0, 2);
                if (i == 0 && Cell.Adjacency.ML.BaseSand.Density < Density) { Cell.SwapCells(Cell, Cell.Adjacency.ML); }
                if (i == 1 && Cell.Adjacency.MR.BaseSand.Density < Density) { Cell.SwapCells(Cell, Cell.Adjacency.MR); }
            }
            LastProcessedFrame = Cell.World.ProcessFrame;
        }
    }
}
