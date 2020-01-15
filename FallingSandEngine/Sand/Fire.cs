using System;
using System.Collections.Generic;
using System.Text;

namespace FallingSandEngine.Sand
{
    public class Fire : BaseSand
    {
        int life = 10;
        public Fire()
        {
            Name = "Fire";
            Density = -1;
            Processable = true;
            Drawable = true;
            Color = World.FireColor;
        }

        public override void Process()
        {
            life -= 1;
            if(life <= 0)
            {
                Cell.BaseSand = new Air();
                this.Cell = null;
                return;
            }

            int i = World.RNG.Next(0, 8);

            if(Cell.HasNeighborOfType(typeof(Oil)))
            {
                Cell[] cells = Cell.Adjacency.Cells;
                List<Cell> oilCells = new List<Cell>();

                foreach (Cell cell in cells)
                {
                    if (cell.BaseSand is Oil)
                    {
                        oilCells.Add(cell);
                    }
                }
                int oilIndex = World.RNG.Next(0, oilCells.Count);
                oilCells[oilIndex].BaseSand = new Fire();

                return;
            }

            if (Cell.HasNeighborOfType(typeof(Water))) { life = 0; return; }

            if (i == 0 && Cell.Adjacency.TL.BaseSand is Wood) { Fire f = new Fire(); Cell.Adjacency.TL.BaseSand = f; }
            else if (i == 1 && Cell.Adjacency.TM.BaseSand is Wood) { Fire f = new Fire(); Cell.Adjacency.TM.BaseSand = f; }
            else if (i == 2 && Cell.Adjacency.TR.BaseSand is Wood) { Fire f = new Fire(); Cell.Adjacency.TR.BaseSand = f; }
            else if (i == 3 && Cell.Adjacency.ML.BaseSand is Wood) { Fire f = new Fire(); Cell.Adjacency.ML.BaseSand = f; }
            else if (i == 4 && Cell.Adjacency.MR.BaseSand is Wood) { Fire f = new Fire(); Cell.Adjacency.MR.BaseSand = f; }
            else if (i == 5 && Cell.Adjacency.BL.BaseSand is Wood) { Fire f = new Fire(); Cell.Adjacency.BL.BaseSand = f; }
            else if (i == 6 && Cell.Adjacency.BM.BaseSand is Wood) { Fire f = new Fire(); Cell.Adjacency.BM.BaseSand = f; }
            else if (i == 7 && Cell.Adjacency.BR.BaseSand is Wood) { Fire f = new Fire(); Cell.Adjacency.BR.BaseSand = f; }

            if (Cell.Adjacency.TM.BaseSand.Density < Density)
            {
                Cell goal = Cell.Adjacency.TM;
                Cell.SwapCells(Cell, goal);
            }
            else if (Cell.Adjacency.TL.BaseSand.Density < Density)
            {
                Cell goal = Cell.Adjacency.TL;
                Cell.SwapCells(Cell, goal);
            }
            else if (Cell.Adjacency.TR.BaseSand.Density < Density)
            {
                Cell goal = Cell.Adjacency.TR;
                Cell.SwapCells(Cell, goal);
            }
            LastProcessedFrame = Cell.World.ProcessFrame;
        }
    }
}
