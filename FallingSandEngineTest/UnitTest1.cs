using FallingSandEngine;
using System;
using NUnit.Framework;

namespace Tests
{
    public class Tests
    {
        int ChunksX = 3;
        int ChunksY = 6;

        int SubChunksX = 2;
        int SubChunksY = 5;

        int CellsX = 1;
        int CellsY = 4;
        World world;
        [SetUp]
        public void Setup()
        {
            world = new World(ChunksX, ChunksY, SubChunksX, SubChunksY, CellsX, CellsY);
        }

        [Test]
        public void CoordinateCoversions()
        {
            int cellIndexX = 0;
            int cellIndexY = 0;
            for (int chX = 0; chX < ChunksX; chX++)
            {
                for (int chY = 0; chY < ChunksY; chY++)
                {
                    Chunk chunk = world.GetChunkAtPosition(cellIndexX, cellIndexY);
                    Assert.True(chunk.ChunkXIndex == chX);
                    Assert.True(chunk.ChunkYIndex == chY);

                    Assert.True(chunk.ChunkX == chX * world.CellsPerChunkWidth);
                    Assert.True(chunk.ChunkY == chY * world.CellsPerChunkHeight);
                    for (int schX = 0; schX < SubChunksX; schX++)
                    {
                        for (int schY = 0; schY < SubChunksY; schY++)
                        {
                            SubChunk subchunk = world.GetSubChunkAtPosition(cellIndexX, cellIndexY);
                            Assert.True(subchunk.SubChunkXIndex == schX);
                            Assert.True(subchunk.SubChunkYIndex == schY);

                            Assert.True(subchunk.SubChunkX == chX * world.CellsPerChunkWidth + schX * world.CellsPerSubChunkWidth);
                            Assert.True(subchunk.SubChunkY == chY * world.CellsPerChunkHeight + schY * world.CellsPerSubChunkHeight);
                            for (int cX = 0; cX < CellsX; cX++)
                            {
                                for (int cY = 0; cY < CellsY; cY++)
                                {
                                    Cell cell = world.GetCellAtPosition(cellIndexX, cellIndexY);
                                    Assert.True(cell.CellXLocal == cX);
                                    Assert.True(cell.CellYLocal == cY);
                                    Assert.True(cell.CellX == cellIndexX);
                                    Assert.True(cell.CellY == cellIndexY);
                                    cellIndexY++;
                                }
                                cellIndexY -= CellsY;
                                cellIndexX++;
                            }
                            cellIndexX -= CellsX;
                            cellIndexY += CellsY;
                        }
                        cellIndexX += CellsX;
                        cellIndexY -= CellsY*SubChunksY;
                    }
                    cellIndexX -= CellsX * SubChunksX;
                    cellIndexY += CellsY * SubChunksY;
                }
                cellIndexX += CellsX * SubChunksX;
                cellIndexY = 0;
            }

            for (int x = 0; x < world.TotalWorldWidthCells; x++)
            {
                for (int y = 0; y < world.TotalWorldHeightCells; y++)
                {
                    Cell cell = world.GetCellAtPosition(x, y);

                    if (cell.Adjacency.TL != null)
                    {
                        Assert.True(cell.Adjacency.TL.CellX == x - 1);
                        Assert.True(cell.Adjacency.TL.CellY == y - 1);
                    }
                    if (cell.Adjacency.TM != null)
                    {
                        Assert.True(cell.Adjacency.TM.CellX == x);
                        Assert.True(cell.Adjacency.TM.CellY == y - 1);
                    }
                    if (cell.Adjacency.TR != null)
                    {
                        Assert.True(cell.Adjacency.TR.CellX == x + 1);
                        Assert.True(cell.Adjacency.TR.CellY == y - 1);
                    }

                    if (cell.Adjacency.ML != null)
                    {
                        Assert.True(cell.Adjacency.ML.CellX == x - 1);
                        Assert.True(cell.Adjacency.ML.CellY == y);
                    }
                    if (cell.Adjacency.MM != null)
                    {
                        Assert.True(cell.Adjacency.MM.CellX == x);
                        Assert.True(cell.Adjacency.MM.CellY == y);
                    }
                    if (cell.Adjacency.MR != null)
                    {
                        Assert.True(cell.Adjacency.MR.CellX == x + 1);
                        Assert.True(cell.Adjacency.MR.CellY == y);
                    }

                    if (cell.Adjacency.BL != null)
                    {
                        Assert.True(cell.Adjacency.BL.CellX == x - 1);
                        Assert.True(cell.Adjacency.BL.CellY == y + 1);
                    }
                    if (cell.Adjacency.BM != null)
                    {
                        Assert.True(cell.Adjacency.BM.CellX == x);
                        Assert.True(cell.Adjacency.BM.CellY == y + 1);
                    }
                    if (cell.Adjacency.BR != null)
                    {
                        Assert.True(cell.Adjacency.BR.CellX == x + 1);
                        Assert.True(cell.Adjacency.BR.CellY == y + 1);
                    }

                }
            }
        }
    }
}