using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

using FallingSandEngine;
using FallingSandEngine.Sand;
namespace FallingSandGUI
{
    public partial class Form1 : Form
    {//.17,.18
        Random rng = new Random();
        Stopwatch FPSTimer = new Stopwatch();
        double miliSecondsPerFrame = 0;
        Type SandToPlace = typeof(Air);

        List<BaseSand> SandTypes = new List<BaseSand>()
        {
            new Air(),
            new Rock(),
            new Sand(),
            new BlackSand(),
            new Fire(),
            new Wood(),
            new Water(),
            new Lava(),
            new Oil()
        };

        Graphics backBufferGraphics;
        Graphics graphics;
        Thread drawThread;
        World world = new World(8, 3, 5, 5, 4, 4);//new World(8, 3, 100, 100, 20, 20, 5, 5);

        Pen ChunkPen = new Pen(Brushes.Black);
        Pen SubChunkPen = new Pen(Brushes.Green);
        Pen UnprocessablePen = new Pen(Brushes.Red);

        Bitmap ImageBuffer;

        int colorVariance = 10;
        float xResolution = 4;
        float yResolution = 4;

        public Form1()
        {
            InitializeComponent();

            for(int x = 0; x < world.TotalWorldWidthCells; x++)
            {
                Rock cell = new Rock();
                cell.Color = RandomColorOffset(cell.Color, rng, colorVariance, colorVariance, colorVariance);
                world.GetCellAtPosition(x, 0).BaseSand = cell;

                cell = new Rock();
                cell.Color = RandomColorOffset(cell.Color, rng, colorVariance, colorVariance, colorVariance);
                world.GetCellAtPosition(x, world.TotalWorldHeightCells-1).BaseSand = cell;

                for(int xx = 0; xx <world.TotalWorldWidthCells-2; xx++)
                {
                    for (int y = 1; y < 5; y++)
                    {
                        BaseSand sCell = world.RNG.Next(0, 2) == 0 ? (BaseSand)new Sand() : new BlackSand();
                        sCell.Color = RandomColorOffset(sCell.Color, rng, colorVariance, colorVariance, colorVariance);
                        world.GetCellAtPosition(xx, y).BaseSand = sCell;
                    }
                }
            }

            for (int y = 0; y < world.TotalWorldHeightCells; y++)
            {
                Rock cell = new Rock();
                cell.Color = RandomColorOffset(cell.Color, rng, colorVariance, colorVariance, colorVariance);
                world.GetCellAtPosition(0, y).BaseSand = cell;

                cell = new Rock();
                cell.Color = RandomColorOffset(cell.Color, rng, colorVariance, colorVariance, colorVariance);
                world.GetCellAtPosition(world.TotalWorldWidthCells - 1, y).BaseSand = cell;
            }

            ImageBuffer = new Bitmap((int)(world.TotalWorldWidthCells * xResolution), (int)(world.TotalWorldHeightCells * yResolution));
            backBufferGraphics = Graphics.FromImage(ImageBuffer);
            graphics = drawArea.CreateGraphics();

            drawArea.MouseClick += DrawArea_MouseClick;
            
            for(int i = 0; i < SandTypes.Count; i++)
            {
                BaseSand type = SandTypes[i];
                Button b = new Button();
                b.Name = "bs:" + i;
                b.Text = type.Name;
                buttonLayout.Controls.Add(b);
                b.Click += B_Click;
            }

            ActiveRegion ar = new ActiveRegion(world, 0, 0, 8, 3);
            world.ActiveRegions.Add(ar);

            drawThread = new Thread(Draw);
            drawThread.Start();
        }

        private void B_Click(object sender, EventArgs e)
        {
            int i = int.Parse(((Button)sender).Name.Split(':')[1]);
            SandToPlace = SandTypes[i].GetType();
        }

        private void DrawArea_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                float x = ((float)e.Location.X / xResolution);
                float y = ((float)e.Location.Y / yResolution);
                Cell c = world.GetCellAtPosition((int)x, (int)y);
                BaseSand sand = (BaseSand)Activator.CreateInstance(SandToPlace);
                sand.Color = RandomColorOffset(sand.Color, c.World.RNG, colorVariance, colorVariance, colorVariance);
                c.BaseSand = sand;
            }
        }

        public void Draw()
        {
            while (true)
            {
                FPSTimer.Start();
                backBufferGraphics.Clear(Color.White);
                
                foreach(KeyValuePair<string, Chunk> chunk_kvp in world.Chunks)
                {
                    Chunk chunk = chunk_kvp.Value;
                    //if (!chunk.Active) { continue; }
                    foreach (SubChunk subchunk in chunk.SubChunks)
                    {
                        //if(!subchunk.Active) { continue; }
                        foreach (Cell cell in subchunk.Cells)
                        {
                            if (cell.BaseSand.Drawable)
                            {
                                SolidBrush sb = new SolidBrush(cell.BaseSand.Color);
                                backBufferGraphics.FillRectangle(sb, cell.CellX*xResolution, cell.CellY*yResolution, xResolution, yResolution);
                            }
                        }
                        //backBufferGraphics.DrawRectangle(SubChunkPen, subchunk.SubChunkX*xResolution, subchunk.SubChunkY * yResolution, world.CellsPerSubChunkWidth*xResolution, world.CellsPerSubChunkHeight*yResolution);
                    }
                    //backBufferGraphics.DrawRectangle(ChunkPen, chunk.ChunkX*xResolution, chunk.ChunkY*yResolution, world.CellsPerChunkWidth * xResolution, world.CellsPerChunkHeight * yResolution);
                }
                backBufferGraphics.DrawString(world.TimeForLastFrame.ToString(), DefaultFont, Brushes.Black, Point.Empty);
                backBufferGraphics.DrawString((1d / (world.TimeForLastFrame / 1000d)).ToString(), DefaultFont, Brushes.Black, new Point(0, 10));
                
                backBufferGraphics.DrawString(miliSecondsPerFrame.ToString(), DefaultFont, Brushes.Black, new Point(0, 30));
                backBufferGraphics.DrawString((1d / (miliSecondsPerFrame / 1000d)).ToString(), DefaultFont, Brushes.Black, new Point(0, 40));
                
                graphics.DrawImage(ImageBuffer, Point.Empty);
                world.Process();

                miliSecondsPerFrame = FPSTimer.Elapsed.TotalMilliseconds;
                FPSTimer.Reset();
                Thread.Sleep(100);
            }
        }

        public static Color RandomColorOffset(Color originalColor, Random rng, int redVariance, int greenVariance, int blueVariance)
        {
            int rC = Math.Max(0, Math.Min(rng.Next(-redVariance, redVariance) + originalColor.R, 255));
            int gC = Math.Max(0, Math.Min(rng.Next(-greenVariance, greenVariance) + originalColor.G, 255));
            int bC = Math.Max(0, Math.Min(rng.Next(-blueVariance, blueVariance) + originalColor.B, 255));

            return Color.FromArgb(rC, gC, bC);
        }
    }
}
