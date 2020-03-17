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

        int subChunkXIndex;
        public int SubChunkXIndex { get { return subChunkXIndex; } }
        int subChunkYIndex;
        public int SubChunkYIndex { get { return subChunkYIndex; } }
        
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

        /// <summary>
        /// Create a new subchunk
        /// </summary>
        /// <param name="grid">The chunk the subchunk belongs to</param>
        /// <param name="subChunkX">X world position of the subchunk</param>
        /// <param name="subChunkY">Y world position of the subchunk</param>
        /// <param name="subChunkXIndex">X index of the subchunk in its chunk</param>
        /// <param name="subChunkYIndex">Y index of the subchunk in its chunk</param>
        public SubChunk(Chunk grid, int subChunkX, int subChunkY, int subChunkXIndex, int subChunkYIndex)
        {
            Chunk = grid;
            this.subChunkXIndex = subChunkXIndex;
            this.subChunkYIndex = subChunkYIndex;
            this.subChunkX = subChunkX;
            this.subChunkY = subChunkY;

            Cells = new Cell[Chunk.World.CellsPerSubChunkWidth, Chunk.World.CellsPerSubChunkHeight];
            for(int x = 0; x < Chunk.World.CellsPerSubChunkWidth; x++)
            {
                for (int y = 0; y < Chunk.World.CellsPerSubChunkHeight; y++)
                {
                    Cell c = new Cell(this, (subChunkXIndex * World.CellsPerSubChunkWidth + Chunk.ChunkXIndex * World.CellsPerChunkWidth) + x, (subChunkYIndex * World.CellsPerSubChunkHeight + Chunk.ChunkYIndex * World.CellsPerChunkHeight) + y, x, y);
                    Cells[x, y] = c;
                }
            }
        }

        /// <summary>
        /// Process all active cells
        /// </summary>
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

        /// <summary>
        /// Get the cell at the given index, does no bounds checking, faster than GetCellAtPositionIfValid
        /// </summary>
        /// <param name="xIndex">X index of the cell</param>
        /// <param name="yIndex">Y index of the cell</param>
        /// <returns>The cell at the given indices</returns>
        public Cell GetCellAtPosition(int xIndex, int yIndex)
        {
            return Cells[xIndex, yIndex];
        }

        /// <summary>
        /// Get the cell at the given index if the given index is valid (slower than GetCellAtPosition)
        /// </summary>
        /// <param name="xIndex">X index of the cell</param>
        /// <param name="yIndex">Y index of the cell</param>
        /// <returns>The cell at the given indices</returns>
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
