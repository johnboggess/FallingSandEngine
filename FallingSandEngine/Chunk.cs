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

        int chunkXIndex;
        public int ChunkXIndex { get { return chunkXIndex; } }
        int chunkYIndex;
        public int ChunkYIndex { get { return chunkYIndex; } }

        private int[] activeSubChunksInColumn;
        
        /// <summary>
        /// Does the chunk have any active/processable cells
        /// </summary>
        public bool Active
        {
            get { return ProcessableSubChunks > 0; }
        }

        /// <summary>
        /// Create a new chunk
        /// </summary>
        /// <param name="world">World the chunk is apart of</param>
        /// <param name="chunkX">X position in world</param>
        /// <param name="chunkY">Y position in world</param>
        /// <param name="chunkXIndex">X index</param>
        /// <param name="chunkYIndex">Y index</param>
        public Chunk(World world, int chunkX, int chunkY, int chunkXIndex, int chunkYIndex)
        {
            World = world;
            SubChunks = new SubChunk[world.SubChunksPerChunkWidth, world.SubChunksPerChunkHeight];
            this.chunkXIndex = chunkXIndex;
            this.chunkYIndex = chunkYIndex;

            this.chunkX = chunkX;
            this.chunkY = chunkY;

            activeSubChunksInColumn = new int[World.SubChunksPerChunkWidth];

            for(int x = 0; x < world.SubChunksPerChunkWidth; x++)
            {
                activeSubChunksInColumn[x] = 0;
                for (int y = 0; y < world.SubChunksPerChunkHeight; y++)
                {
                    SubChunk c = new SubChunk(this, x * world.CellsPerSubChunkWidth + ChunkXIndex*world.CellsPerChunkWidth, y * world.CellsPerSubChunkHeight + ChunkYIndex * world.CellsPerChunkHeight, x, y);
                    SubChunks[x,y] = c;
                }
            }
        }

        /// <summary>
        /// Process any active subchunks
        /// </summary>
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

        /// <summary>
        /// Get the subchunk at the given index, does no bounds checking, faster than GetSubChunkAtPositionIfValid
        /// </summary>
        /// <param name="xIndex">X index of the subchunk</param>
        /// <param name="yIndex">Y index of the subchunk</param>
        /// <returns>The suchunk at the given indices</returns>
        public SubChunk GetSubChunkAtPosition(int xIndex, int yIndex)
        {
            return SubChunks[xIndex, yIndex];
        }

        /// <summary>
        /// Get the subchunk at the given index if the given index is valid (slower than GetSubChunkAtPosition)
        /// </summary>
        /// <param name="xIndex">X index of the subchunk</param>
        /// <param name="yIndex">Y index of the subchunk</param>
        /// <returns>The suchunk at the given indices</returns>
        public SubChunk GetSubChunkAtPositionIfValid(int xIndex, int yIndex)
        {
            if (xIndex > -1 && yIndex > -1 && xIndex < World.SubChunksPerChunkWidth && yIndex < World.SubChunksPerChunkHeight)
            {
                return SubChunks[xIndex, yIndex];
            }
            return null;
        }

        /// <summary>
        /// Deactivate the given subchunk
        /// </summary>
        /// <param name="subChunk">The subchunk to deactivate</param>
        internal void SubChunkDeactivated(SubChunk subChunk)
        {
            ProcessableSubChunks -= 1;
            activeSubChunksInColumn[subChunk.SubChunkXIndex] -= 1;
        }

        /// <summary>
        /// Activate the given subchunk
        /// </summary>
        /// <param name="subChunk">The subchunk to Activate</param>
        internal void SubChunkActivated(SubChunk subChunk)
        {
            ProcessableSubChunks += 1;
            activeSubChunksInColumn[subChunk.SubChunkXIndex] += 1;
        }
    }
}
