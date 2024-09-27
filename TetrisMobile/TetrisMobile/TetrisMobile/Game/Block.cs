using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TetrisMobile
{
    class Block
    {
        public Vector2 position { get; set; }
        public Color color { get; set; }
        //public int yOffset { get; set;  }

        public Block(Vector2 position) //defaults to white
        {
            this.position = position;
            color = Color.White;
        }

        public Block(Vector2 position, Color color)
        {
            this.position = position;
            this.color = color;
        }
    }
}
