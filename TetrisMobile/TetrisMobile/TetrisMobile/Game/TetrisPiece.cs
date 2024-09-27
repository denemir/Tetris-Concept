using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TetrisMobile
{
    class TetrisPiece
    {
        public List<Rectangle> blocks { get; set; } //blocks used for manipulation
        public List<Block> blockz { get; set; }
        public Color color { get; set; }

        public TetrisPiece()
        {
            color = Color.White;
        }

        public TetrisPiece(List<Rectangle> blocks, Color color)
        {
            this.blocks = blocks;
            this.color = color;
            blockz = new List<Block>();
            foreach (Rectangle block in blocks)
            {
                blockz.Add(new Block(new Vector2(block.X, block.Y)));
            }
        }


        public TetrisPiece(TetrisPiece piece)
        {
            blocks = piece.blocks;
            color = piece.color;
            for (int i = 0; i < blocks.Count(); i++)
            {
                blocks[i] = new Rectangle(roundOff(blocks[i].X), roundOff(blocks[i].Y), 20, 20);
            }
            blockz = new List<Block>();
            foreach (Rectangle block in piece.blocks)
            {
                blockz.Add(new Block(new Vector2(block.X, block.Y)));
            }
        }

        public TetrisPiece(ActivePiece piece)
        {
            blocks = new List<Rectangle>();
            blockz = piece.blockz;
            color = piece.color;
            foreach (Block block in piece.blockz)
            {
                blocks.Add(new Rectangle(roundOff((int)block.position.X), roundOff((int)block.position.Y), 20, 20));
            }
        }

        public bool intersecting(TetrisPiece t)
        {
            bool intersect = false;
            
            for(int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 4; j++)
                {
                    if (blocks[i].Intersects(t.blocks[j]))
                        if (blocks[i].Equals(t.blocks[j]))
                            intersect = false;
                        else intersect = true;
                }
                
            }
            return intersect;
        }

        protected int roundOff(int num) //rounds off to nearest 10
        {
            return ((int)Math.Round(num / 10.0)) * 10;
        }
    }
}
