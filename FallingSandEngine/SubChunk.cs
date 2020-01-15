using System;
using System.Collections.Generic;
using System.Text;

using FallingSandEngine.Sand;

namespace FallingSandEngine
{
    public class SubChunk
    {
        public Chunk Chunk;
        public World World { get { return Chunk.World; } }
        public Cell[,] Cells;

        int subChunkX;
        public int SubChunkX { get { return subChunkX; } }
        int subChunkY;
        public int SubChunkY { get { return subChunkY; } }

        int subChunkXLocal;
        public int SubChunkXLocal { get { return subChunkXLocal; } }
        int subChunkYLocal;
        public int SubChunkYLocal { get { return subChunkYLocal; } }
        
        private int processableCells = 0;
        internal int ProcessableCells
        {
            get { return processableCells; }
            set
            {
                if (processableCells > 0)
                {
                    Chunk.SubChunkDeactivated(this);
                    processableCells = value;
                    if (processableCells > 0) { Chunk.SubChunkActivated(this); }
                    return;
                }
                else if (processableCells == 0)
                {
                    Chunk.SubChunkActivated(this);
                    processableCells = value;
                    if (processableCells == 0) { Chunk.SubChunkDeactivated(this); }
                    return;
                }
                throw new Exception();
            }
        }

        public bool Active
        {
            get { return ProcessableCells > 0; }
        }

        public SubChunk(Chunk grid, int subChunkX, int subChunkY, int subChunkXLocal, int subChunkYLocal)
        {
            Chunk = grid;
            this.subChunkXLocal = subChunkXLocal;
            this.subChunkYLocal = subChunkYLocal;
            this.subChunkX = subChunkX;
            this.subChunkY = subChunkY;

            Cells = new Cell[Chunk.World.CellsPerSubChunkWidth, Chunk.World.CellsPerSubChunkHeight];
            for(int x = 0; x < Chunk.World.CellsPerSubChunkWidth; x++)
            {
                for (int y = 0; y < Chunk.World.CellsPerSubChunkHeight; y++)
                {
                    Cell c = new Cell(this, (subChunkXLocal * World.CellsPerSubChunkWidth + Chunk.ChunkXLocal * World.CellsPerChunkWidth) + x, (subChunkYLocal * World.CellsPerSubChunkHeight + Chunk.ChunkYLocal * World.CellsPerChunkHeight) + y, x, y);
                    Cells[x, y] = c;
                }
            }
        }

        public void Process()
        {
            for (int cellX = 0; cellX < Chunk.World.CellsPerSubChunkWidth; cellX++)
            {
                for (int cellY = Chunk.World.CellsPerSubChunkHeight - 1; cellY >= 0; cellY--)
                {
                    Cell cell = GetCellAtPosition(cellX, cellY);
                    if (cell.Processable)
                    {
                        cell.Process();
                    }
                }
            }
        }
        
        public Cell GetCellAtPosition(int x, int y)
        {
            return Cells[x, y];
        }
        
        public Cell GetCellAtPositionIfValid(int x, int y)
        {
            if (x > -1 && y > -1 && x < World.CellsPerSubChunkWidth && y < World.CellsPerSubChunkHeight)
            {
                return Cells[x, y];
            }
            return null;
        }
    }
}
