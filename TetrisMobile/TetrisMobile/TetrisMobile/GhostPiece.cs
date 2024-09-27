using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TetrisMobile
{
    class GhostPiece : ActivePiece //ghost piece uses same algorithm as hard drop to hover over blocks
    {
        public float opacity { get; set; } //default to %20

        public GhostPiece(TetrisPiece piece) : base(piece)
        {
            this.piece = piece;
            color = Color.WhiteSmoke;
            opacity = .2f;

            blockz = new List<Block>();
            foreach (Rectangle block in piece.blocks)
            {
                blockz.Add(new Block(new Vector2(block.X, block.Y)));
            }
        }

        public void MoveTo(Vector2 offset, ActivePiece ap)
        {
            Vector2 temp = offset;
            int[] t = new int[blockz.Count()]; //records difference in positions
            //Move(new Vector2(0, (offset.Y)));
            for(int i = 0; i < blockz.Count(); i++)
            {
                if (i == 0)
                    t[i] = (int)ap.blockz[i].position.Y;
                if (i != 0)
                    t[i] = t[0] - (int)ap.blockz[i].position.Y;

                temp.X = ap.blockz[i].position.X; 
                if(i == 0)
                    temp.Y = offset.Y;
                else temp.Y = offset.Y - t[i];
                blockz[i].position = temp;
            }     
        }
    }
}
