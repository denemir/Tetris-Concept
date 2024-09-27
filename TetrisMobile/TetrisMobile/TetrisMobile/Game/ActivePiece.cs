using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TetrisMobile
{
    class ActivePiece : TetrisPiece
    {
        public List<Block> blockz { get; set; } //List of blocks (not to be confused with blocks, list of rectangles)
        public TetrisPiece piece;

        public ActivePiece(TetrisPiece piece)
        {
            this.piece = piece;
            color = piece.color;
            blockz = new List<Block>();
            foreach(Rectangle block in piece.blocks)
            {
                blockz.Add(new Block(new Vector2(block.X, block.Y)));
            }
        }

        public void Move(Vector2 offset)
        {
            foreach (Block block in blockz)
            {
                block.position += offset;
            }
        }


        public new bool intersecting(TetrisPiece t)
        {
            bool intersect = false;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (new Rectangle((int)blockz[i].position.X, (int)blockz[i].position.Y, 20, 20).Intersects(t.blocks[j]))
                        intersect = true;
                }

            }
            return intersect;
        }

        public bool intersecting(Rectangle r)
        {
            bool intersect = false;

            for (int i = 0; i < 4; i++)
            {
                    if (new Rectangle((int)blockz[i].position.X, (int)blockz[i].position.Y, 20, 20).Intersects(r))
                        intersect = true;

            }
            return intersect;
        }
    }
}
