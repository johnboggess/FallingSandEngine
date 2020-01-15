using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
namespace FallingSandEngine
{
    public class ActiveRegion
    {
        public Rectangle Rectangle;
        World world;
        
        public ActiveRegion(World world, int x, int y, int widthInChunks, int heightInChunks)
        {
            this.world = world;
            Rectangle = new Rectangle(x, y, widthInChunks, heightInChunks);
        }

        public void ProcessRect()
        {
            for (int chunkX = 0; chunkX < Rectangle.Width; chunkX++)
            {
                for (int chunkY = Rectangle.Height - 1; chunkY >= 0; chunkY--)
                {
                    Chunk chunk = world.GetChunkAtLocalPosition(chunkX + Rectangle.X, chunkY + Rectangle.Y);
                    if (!chunk.Active) { continue; }
                    chunk.Process();
                }
            }
        }
    }
}
