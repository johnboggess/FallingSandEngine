using System;
using System.Collections.Generic;
using System.Text;

namespace FallingSandEngine.Sand
{
    public class Lava : Fluid
    {
        int count = 0;
        public Lava()
        {
            Density = 2;
            Processable = true;
            Drawable = true;
            Color = World.LavaColor;
            Name = "Lava";
        }

        public override void Process()
        {
            if(count < 2)
            {
                count++;
                return;
            }
            count = 0;

            Cell[] cells = Cell.Adjacency.Cells;
            
            if (Cell.HasNeighborOfType(typeof(Oil)))
            {
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

            if (Cell.HasNeighborOfType(typeof(Water)))
            {
                List<Cell> waterCells = new List<Cell>();
                
                foreach (Cell cell in cells)
                {
                    if (cell.BaseSand is Water)
                    {
                        waterCells.Add(cell);
                    }
                }
                int waterIndex = World.RNG.Next(0, waterCells.Count);
                waterCells[waterIndex].BaseSand = new Air();
                Cell.BaseSand = new Rock();

                return;
            }
            
            base.Process();
        }
    }
}
