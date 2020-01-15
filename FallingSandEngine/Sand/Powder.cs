using System;
using System.Collections.Generic;
using System.Text;

namespace FallingSandEngine.Sand
{
    public abstract class Powder : BaseSand
    {
        public override void Process()
        {

            if(Cell.Adjacency.BM.BaseSand.Density < Density)
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

            /*Cell BM = Cell.World.GetCellAtPosition(Cell.CellX, Cell.CellY + 1);
            if (BM.BaseSand.Density < Density)
            {
                Cell goal = Cell.Adjacency.BM;
                Cell.SwapCells(Cell, goal);
                LastProcessedFrame = Cell.World.ProcessFrame;
                return;
            }

            Cell BL = Cell.World.GetCellAtPosition(Cell.CellX - 1, Cell.CellY + 1);
            if (BL.BaseSand.Density < Density)
            {
                Cell goal = Cell.Adjacency.BL;
                Cell.SwapCells(Cell, goal);
                LastProcessedFrame = Cell.World.ProcessFrame;
                return;
            }

            Cell BR = Cell.World.GetCellAtPosition(Cell.CellX + 1, Cell.CellY + 1);
            if (BR.BaseSand.Density < Density)
            {
                Cell goal = Cell.Adjacency.BR;
                Cell.SwapCells(Cell, goal);
                LastProcessedFrame = Cell.World.ProcessFrame;
                return;
            }*/
        }
    }
}
