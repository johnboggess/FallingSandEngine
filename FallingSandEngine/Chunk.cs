using System;
using System.Collections.Generic;
namespace FallingSandEngine
{
    public class Chunk
    {
        public SubChunk[,] SubChunks;
        internal World World;

        internal int ProcessableSubChunks = 0;

        int chunkX;
        public int ChunkX{ get { return chunkX; } }
        int chunkY;
        public int ChunkY { get { return chunkY; } }

        int chunkXLocal;
        public int ChunkXLocal { get { return chunkXLocal; } }
        int chunkYLocal;
        public int ChunkYLocal { get { return chunkYLocal; } }

        private int[] activeSubChunksInColumn;

        public bool Active
        {
            get { return ProcessableSubChunks > 0; }
        }

        public Chunk(World world, int chunkX, int chunkY, int chunkXLocal, int chunkYLocal)
        {
            World = world;
            SubChunks = new SubChunk[world.SubChunksPerChunkWidth, world.SubChunksPerChunkHeight];
            this.chunkXLocal = chunkXLocal;
            this.chunkYLocal = chunkYLocal;

            this.chunkX = chunkX;
            this.chunkY = chunkY;

            activeSubChunksInColumn = new int[World.SubChunksPerChunkWidth];

            for(int x = 0; x < world.SubChunksPerChunkWidth; x++)
            {
                activeSubChunksInColumn[x] = 0;
                for (int y = 0; y < world.SubChunksPerChunkHeight; y++)
                {
                    SubChunk c = new SubChunk(this, x * world.CellsPerSubChunkWidth + ChunkXLocal*world.CellsPerChunkWidth, y * world.CellsPerSubChunkHeight + ChunkYLocal * world.CellsPerChunkHeight, x, y);
                    SubChunks[x,y] = c;
                }
            }
        }

        public void Process()
        {
            for (int subchunkX = 0; subchunkX < World.SubChunksPerChunkWidth; subchunkX++)
            {
                int activeSubChunksFound = 0;
                if (activeSubChunksInColumn[subchunkX] == 0) { continue; }
                int activeSubChunks = activeSubChunksInColumn[subchunkX];

                for (int subchunkY = World.SubChunksPerChunkHeight - 1; subchunkY >= 0; subchunkY--)
                {
                    SubChunk subchunk = GetSubChunkAtPosition(subchunkX, subchunkY);
                    if (!subchunk.Active) { continue; }
                    subchunk.Process();

                    activeSubChunksFound += 1;
                    if (activeSubChunksFound >= activeSubChunks) { break; }
                }
            }
        }

        public SubChunk GetSubChunkAtPosition(int x, int y)
        {
            return SubChunks[x, y];
        }

        public SubChunk GetSubChunkAtPositionIfValid(int x, int y)
        {
            if (x > -1 && y > -1 && x < World.SubChunksPerChunkWidth && y < World.SubChunksPerChunkHeight)
            {
                return SubChunks[x, y];
            }
            return null;
        }

        internal void SubChunkDeactivated(SubChunk subChunk)
        {
            ProcessableSubChunks -= 1;
            activeSubChunksInColumn[subChunk.SubChunkXLocal] -= 1;
        }

        internal void SubChunkActivated(SubChunk subChunk)
        {
            ProcessableSubChunks += 1;
            activeSubChunksInColumn[subChunk.SubChunkXLocal] += 1;
        }
    }
}
