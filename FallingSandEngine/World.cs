using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Drawing;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("FallingSandEngineTest")]
namespace FallingSandEngine
{
    public class World
    {
        #region SandColors
        public static Color SandColor = Color.FromArgb(242, 201, 145);
        public static Color BlackSandColor = Color.Black;
        public static Color WaterColor = Color.LightBlue;
        public static Color FireColor = Color.Red;
        public static Color LavaColor = Color.OrangeRed;
        public static Color RockColor = Color.LightGray;
        public static Color WoodColor = Color.FromArgb(111, 77, 52);
        public static Color OilColor = Color.FromArgb(93, 94, 52);
        #endregion

        public Random RNG = new Random();
        public Stopwatch Stopwatch = new Stopwatch();
        public double TimeForLastFrame = 0;

        public Dictionary<string, Chunk> Chunks = new Dictionary<string, Chunk>();
        public List<ActiveRegion> ActiveRegions = new List<ActiveRegion>();

        internal ulong ProcessFrame = 0;
        
        internal int totalWorldWidthChunk;
        public int TotalWorldWidthChunk { get { return totalWorldWidthChunk; } }
        internal int totalWorldHeightChunk;
        public int TotalWorldHeightChunk { get { return totalWorldHeightChunk; } }

        private int subChunksPerChunkWidth;
        public int SubChunksPerChunkWidth { get { return subChunksPerChunkWidth; } }
        private int subChunksPerChunkHeight;
        public int SubChunksPerChunkHeight { get { return subChunksPerChunkHeight; } }

        private int cellsPerSubChunkWidth;
        public int CellsPerSubChunkWidth { get { return cellsPerSubChunkWidth; } }
        private int cellsPerSubChunkHeight;
        public int CellsPerSubChunkHeight { get { return cellsPerSubChunkHeight; } }

        private int cellsPerChunkWidth;
        public int CellsPerChunkWidth { get { return cellsPerChunkWidth; } }
        private int cellsPerChunkHeight;
        public int CellsPerChunkHeight { get { return cellsPerChunkHeight; } }

        private int totalWorldWidthCells;
        public int TotalWorldWidthCells { get { return totalWorldWidthCells; } }
        private int totalWorldHeightCells;
        public int TotalWorldHeightCells { get { return totalWorldHeightCells; } }




        public World(int chunksInRow, int chunksInColumn, int subChunksInRow, int subChunksInColumn, int cellsInRow, int cellsInColumn)
        {
            totalWorldWidthChunk = chunksInRow;
            totalWorldHeightChunk = chunksInColumn;

            subChunksPerChunkWidth = subChunksInRow;
            subChunksPerChunkHeight = subChunksInColumn;

            cellsPerSubChunkWidth = cellsInRow;
            cellsPerSubChunkHeight = cellsInColumn;

            cellsPerChunkWidth = SubChunksPerChunkWidth * cellsPerSubChunkWidth;
            cellsPerChunkHeight = SubChunksPerChunkHeight * cellsPerSubChunkHeight;

            totalWorldWidthCells = totalWorldWidthChunk * SubChunksPerChunkWidth * cellsPerSubChunkWidth;
            totalWorldHeightCells = totalWorldHeightChunk * SubChunksPerChunkHeight * cellsPerSubChunkHeight;

            for (int x = 0; x < chunksInRow; x++)
            {
                for (int y = 0; y < chunksInColumn; y++)
                {
                    Chunk c = CreateChunkAtWorldPosition(x*CellsPerChunkWidth, y*CellsPerChunkHeight);
                }
            }

            for(int x = 0; x < TotalWorldWidthCells; x++)
            {
                for(int y = 0; y < TotalWorldHeightCells; y++)
                {
                    Cell cell = GetCellAtPosition(x, y);
                    List<Cell> cells = cell.GetAdjacentCells();

                    Adjacency adjacency = new Adjacency();
                    adjacency.Cells[0] = cells[0];
                    adjacency.Cells[1] = cells[1];
                    adjacency.Cells[2] = cells[2];

                    adjacency.Cells[3] = cells[3];
                    adjacency.Cells[4] = cell;
                    adjacency.Cells[5] = cells[4];

                    adjacency.Cells[6] = cells[5];
                    adjacency.Cells[7] = cells[6];
                    adjacency.Cells[8] = cells[7];
                    cell.Adjacency = adjacency;
                }
            }
        }

        public void Process()
        {
            Stopwatch.Start();

            foreach(ActiveRegion activeRegion in ActiveRegions)
            {
                activeRegion.ProcessRect();
            }

            ProcessFrame += 1;
            Stopwatch.Stop();
            TimeForLastFrame = Stopwatch.Elapsed.TotalMilliseconds;
            Stopwatch.Reset();
        }

        public Chunk CreateChunkAtWorldPosition(int x, int y)
        {
            Chunk chunk = new Chunk(this, x, y, x / CellsPerChunkWidth, y / CellsPerChunkHeight);
            string key = gridPositionToGridKey(x/CellsPerChunkWidth, y/CellsPerChunkHeight);
            Chunks.Add(key, chunk);
            return chunk;
        }

        public Chunk GetChunkAtLocalPosition(int x, int y)
        {
            string key = gridPositionToGridKey(x,y);
            return Chunks[key];
        }

        public Chunk GetChunkAtPosition(int x, int y)
        {
            string key = gridPositionToGridKey(CellCoordToChunkCoordX(x), CellCoordToChunkCoordY(y));
            return Chunks[key];
        }

        public SubChunk GetSubChunkAtPosition(int x, int y)
        {
            return GetSubChunkAtPosition(CellCoordToChunkCoordX(x), CellCoordToChunkCoordY(y), CellCoordToSubChunkCoordX(x), CellCoordToSubChunkCoordY(y));
        }

        public SubChunk GetSubChunkAtPosition(int chunkX, int chunkY, int subChunkX, int subChunkY)
        {
            return GetChunkAtLocalPosition(chunkX, chunkY).GetSubChunkAtPosition(subChunkX, subChunkY);
        }

        public Cell GetCellAtPosition(int x, int y)
        {
            SubChunk subChunk = GetSubChunkAtPosition(x, y);
            return subChunk.GetCellAtPosition(x % cellsPerSubChunkWidth, y % cellsPerSubChunkHeight);
        }

        public Cell GetCellAtPosition(int chunkX, int chunkY, int subChunkX, int subChunkY, int cellX, int cellY)
        {
            return GetChunkAtPosition(chunkX, chunkY).GetSubChunkAtPosition(subChunkX, subChunkY).GetCellAtPosition(cellX, cellY);
        }


        public Chunk GetChunkAtLocalPositionIfValid(int x, int y)
        {
            string key = gridPositionToGridKey(x, y);
            if (Chunks.ContainsKey(key))
            {
                return Chunks[key];
            }
            return null;
        }

        public Chunk GetChunkAtPositionIfValid(int x, int y)
        {
            string key = gridPositionToGridKey(CellCoordToChunkCoordX(x), CellCoordToChunkCoordY(y));
            if (Chunks.ContainsKey(key))
            {
                return Chunks[key];
            }
            return null;
        }

        public SubChunk GetSubChunkAtPositionIfValid(int x, int y)
        {
            return GetSubChunkAtPositionIfValid(CellCoordToChunkCoordX(x), CellCoordToChunkCoordY(y), CellCoordToSubChunkCoordX(x), CellCoordToSubChunkCoordY(y));
        }

        public SubChunk GetSubChunkAtPositionIfValid(int chunkX, int chunkY, int subChunkX, int subChunkY)
        {
            Chunk chunk = GetChunkAtLocalPositionIfValid(chunkX, chunkY);
            if (chunk != null)
            {
                return chunk.GetSubChunkAtPosition(subChunkX, subChunkY);
            }
            return null;
        }

        public Cell GetCellAtPositionIfValid(int x, int y)
        {
            SubChunk subchunk = GetSubChunkAtPositionIfValid(x, y);
            if (subchunk != null)
            {
                return subchunk.GetCellAtPosition(x % cellsPerSubChunkWidth, y % cellsPerSubChunkHeight);
            }
            return null;
        }

        public Cell GetCellAtPositionIfValid(int chunkX, int chunkY, int subChunkX, int subChunkY, int cellX, int cellY)
        {
            Chunk chunk = GetChunkAtPositionIfValid(chunkX, chunkY);
            if (chunk != null)
            {
                return chunk.GetSubChunkAtPosition(subChunkX, subChunkY).GetCellAtPosition(cellX, cellY);
            }
            return null;
        }

        internal static string gridPositionToGridKey(int x, int y)
        {
            return string.Concat(x,",", y);
        }

        internal int CellCoordToChunkCoordX(int x)
        {
            return (int)Math.Floor((float)x / (float)CellsPerChunkWidth);
        }

        internal int CellCoordToChunkCoordY(int y)
        {
            return (int)Math.Floor((float)y / (float)CellsPerChunkHeight);
        }

        internal int CellCoordToSubChunkCoordX(int x)
        {
            return (int)Math.Floor((float)x / (float)CellsPerSubChunkWidth) % SubChunksPerChunkWidth;
        }

        internal int CellCoordToSubChunkCoordY(int y)
        {
            return (int)Math.Floor((float)y / (float)CellsPerSubChunkHeight) % SubChunksPerChunkHeight;
        }
    }
}
