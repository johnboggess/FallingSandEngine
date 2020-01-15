using System;
using System.Collections.Generic;
using System.Text;

using FallingSandEngine.Sand;

namespace FallingSandEngine
{
    public class Cell
    {
        public SubChunk SubChunk;
        public World World { get { return SubChunk.Chunk.World; } }
        public Adjacency Adjacency;


        BaseSand baseSand;
        public BaseSand BaseSand
        {
            get { return baseSand; }
            set
            {
                if(baseSand.Processable) { SubChunk.ProcessableCells -= 1; }
                if(value.Processable) { SubChunk.ProcessableCells += 1; }

                baseSand = value;
                baseSand.Cell = this;
            }
        }

        int cellX;
        public int CellX { get { return cellX; } }
        int cellY;
        public int CellY { get { return cellY; } }

        int cellXLocal;
        public int CellXLocal { get { return cellXLocal; } }
        int cellYLocal;
        public int CellYLocal { get { return cellYLocal; } }
        
        public bool Processable
        {
            get { return baseSand.Processable && BaseSand.LastProcessedFrame != World.ProcessFrame; }
        }

        public Cell(SubChunk subChunk, int cellX, int cellY, int cellXLocal, int cellYLocal)
        {
            SubChunk = subChunk;
            this.cellX = cellX;
            this.cellY = cellY;

            this.cellXLocal = cellXLocal;
            this.cellYLocal = cellYLocal;

            baseSand = new Sand.Air();
            BaseSand = baseSand;
        }

        public void Process()
        {
            BaseSand.Process();
        }

        internal List<Cell> GetAdjacentCells()
        {
            List<Cell> result = new List<Cell>();
            World w = SubChunk.Chunk.World;

            Cell c = w.GetCellAtPositionIfValid(cellX - 1, CellY - 1);
            result.Add(c);
            c = w.GetCellAtPositionIfValid(cellX, CellY - 1);
            result.Add(c);
            c = w.GetCellAtPositionIfValid(cellX + 1, CellY - 1);
            result.Add(c);

            c = w.GetCellAtPositionIfValid(cellX - 1, CellY);
            result.Add(c);
            c = w.GetCellAtPositionIfValid(cellX + 1, CellY);
            result.Add(c);
            
            c = w.GetCellAtPositionIfValid(cellX - 1, CellY + 1);
            result.Add(c);
            c = w.GetCellAtPositionIfValid(cellX, CellY + 1);
            result.Add(c);
            c = w.GetCellAtPositionIfValid(cellX + 1, CellY + 1);
            result.Add(c);

            return result;

        }
        
        public bool HasNeighborOfType(Type type)
        {
            return Adjacency.TL.BaseSand.GetType() == type || Adjacency.TM.BaseSand.GetType() == type || Adjacency.TR.BaseSand.GetType() == type
                || Adjacency.ML.BaseSand.GetType() == type || Adjacency.MR.BaseSand.GetType() == type
                || Adjacency.BL.BaseSand.GetType() == type || Adjacency.BM.BaseSand.GetType() == type || Adjacency.MR.BaseSand.GetType() == type;
        }

        public static void SwapCells(Cell origin, Cell dest)
        {
            BaseSand originSand = origin.baseSand;
            origin.BaseSand = dest.BaseSand;
            dest.BaseSand = originSand;
        }
    }
}
