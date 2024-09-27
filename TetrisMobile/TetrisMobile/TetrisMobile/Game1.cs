using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TetrisMobile
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        const int GRID_WIDTH = 10;
        const int GRID_HEIGHT = 20;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D blockTexture;
        List<TetrisPiece> tetrisPieces; //active tetris pieces on board

        KeyboardState kb;
        KeyboardState oldKB;

        float rate; //rate at which pieces fall
        int level; //determines score ratio and rate

        //int bottom; //bottom y coordinate
        //int left; //far left x coordinate
        //int right; //far right x coordinate

        int spawnX; //spawn coordinates for piece
        int spawnY;

        int curDelay;
        int maxDelay; //frame delay
        int hDelay; //hold delay

        //settings
        bool isColor; //is color on
        bool isGhostPiece; //is ghost piece on

        ActivePiece activePiece;
        GhostPiece ghostPiece;

        TetrisPiece linePiece;
        TetrisPiece squarePiece;
        TetrisPiece tPiece;
        TetrisPiece sPiece;
        TetrisPiece zPiece;
        TetrisPiece lPiece;
        TetrisPiece jPiece;

        Block[,] grid = new Block[GRID_WIDTH, GRID_HEIGHT];

        Random randy;

        TetrisPiece holdPiece; //piece that player is storing

        enum GameState
        {
            main,
            play,
            highscore,
            pause
        }

        GameState gs;


        //int currentScore;
        //int currentNumOfPieces; //current number of pieces on field

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            gs = GameState.play;
            isColor = true;
            isGhostPiece = true;
            rate = 1f;
            randy = new Random();

            //movement delays
            curDelay = 0;
            maxDelay = 8;
            hDelay = 2;

            //spawns
            spawnX = 60;
            spawnY = -40;

            //Creating the pieces

            //Line Piece
            List<Rectangle> lineBlocks = new List<Rectangle>();
            lineBlocks.Add(new Rectangle(spawnX + 0, spawnY + 0, 20, 20));
            lineBlocks.Add(new Rectangle(spawnX + 20, spawnY + 0, 20, 20));
            lineBlocks.Add(new Rectangle(spawnX + 40, spawnY + 0, 20, 20));
            lineBlocks.Add(new Rectangle(spawnX + 60, spawnY + 0, 20, 20));
            linePiece = new TetrisPiece(lineBlocks, Color.Cyan/*new Color(randy.Next(1, 255), randy.Next(1, 255), randy.Next(1, 255))*/);

            //Square Piece
            List<Rectangle> squareBlocks = new List<Rectangle>();
            squareBlocks.Add(new Rectangle(spawnX + 0, spawnY + 0, 20, 20));
            squareBlocks.Add(new Rectangle(spawnX + 20, spawnY + 0, 20, 20));
            squareBlocks.Add(new Rectangle(spawnX + 0, spawnY + 20, 20, 20));
            squareBlocks.Add(new Rectangle(spawnX + 20, spawnY + 20, 20, 20));
            squarePiece = new TetrisPiece(squareBlocks, Color.Yellow/*new Color(randy.Next(1, 255), randy.Next(1, 255), randy.Next(1, 255))*/);

            //T Piece
            List<Rectangle> tBlocks = new List<Rectangle>();
            tBlocks.Add(new Rectangle(spawnX + 0, spawnY + 0, 20, 20));
            tBlocks.Add(new Rectangle(spawnX + 20, spawnY + 0, 20, 20));
            tBlocks.Add(new Rectangle(spawnX + 40, spawnY + 0, 20, 20));
            tBlocks.Add(new Rectangle(spawnX + 20, spawnY + 20, 20, 20));
            tPiece = new TetrisPiece(tBlocks, Color.Purple/*new Color(randy.Next(1, 255), randy.Next(1, 255), randy.Next(1, 255))*/);

            //S Piece
            List<Rectangle> sBlocks = new List<Rectangle>();
            sBlocks.Add(new Rectangle(spawnX + 0, spawnY + 20, 20, 20));
            sBlocks.Add(new Rectangle(spawnX + 20, spawnY + 20, 20, 20));
            sBlocks.Add(new Rectangle(spawnX + 20, spawnY + 0, 20, 20));
            sBlocks.Add(new Rectangle(spawnX + 40, spawnY + 0, 20, 20));
            sPiece = new TetrisPiece(sBlocks, Color.Green/*new Color(randy.Next(1, 255), randy.Next(1, 255), randy.Next(1, 255))*/);

            //Z Piece
            List<Rectangle> zBlocks = new List<Rectangle>();
            zBlocks.Add(new Rectangle(spawnX + 0, spawnY + 0, 20, 20));
            zBlocks.Add(new Rectangle(spawnX + 20, spawnY + 0, 20, 20));
            zBlocks.Add(new Rectangle(spawnX + 20, spawnY + 20, 20, 20));
            zBlocks.Add(new Rectangle(spawnX + 40, spawnY + 20, 20, 20));
            zPiece = new TetrisPiece(zBlocks, Color.Red/*new Color(randy.Next(1, 255), randy.Next(1, 255), randy.Next(1, 255))*/);

            //L Piece
            List<Rectangle> lBlocks = new List<Rectangle>();
            lBlocks.Add(new Rectangle(spawnX + 0, spawnY + 0, 20, 20));
            lBlocks.Add(new Rectangle(spawnX + 20, spawnY + 0, 20, 20));
            lBlocks.Add(new Rectangle(spawnX + 40, spawnY + 0, 20, 20));
            lBlocks.Add(new Rectangle(spawnX + 40, spawnY + 20, 20, 20));
            lPiece = new TetrisPiece(lBlocks, Color.Orange/*new Color(randy.Next(1, 255), randy.Next(1, 255), randy.Next(1, 255))*/);

            //J Piece
            List<Rectangle> jBlocks = new List<Rectangle>();
            jBlocks.Add(new Rectangle(spawnX + 0, spawnY + 0, 20, 20));
            jBlocks.Add(new Rectangle(spawnX + 0, spawnY + 20, 20, 20));
            jBlocks.Add(new Rectangle(spawnX + 20, spawnY + 0, 20, 20));
            jBlocks.Add(new Rectangle(spawnX + 40, spawnY + 0, 20, 20));
            jPiece = new TetrisPiece(jBlocks, Color.Blue/*new Color(randy.Next(1, 255), randy.Next(1, 255), randy.Next(1, 255))*/);


            tetrisPieces = new List<TetrisPiece>();

            //*TEST SPACE*//
            //tetrisPieces.Add(squarePiece);
            activePiece = new ActivePiece(generateRandomPiece());
            ghostPiece = new GhostPiece(activePiece.piece);



            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            blockTexture = this.Content.Load<Texture2D>("Block");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            kb = Keyboard.GetState();
            // TODO: Add your update logic here
            if (curDelay != 0)
                curDelay--;

            //Collision
            if (checkCollision(activePiece, tetrisPieces, new Rectangle(0, 480, 1000, 10)))
            {
                tetrisPieces.Add(new TetrisPiece(activePiece));
                activePiece = new ActivePiece(generateRandomPiece());
            }
            else activePiece.Move(new Vector2(0, rate));

            if (kb.IsKeyDown(Keys.W) && oldKB.IsKeyUp(Keys.W))
            {
                hardDrop(activePiece, tetrisPieces);
            }

            //foreach(TetrisPiece piece in tetrisPieces)
            //{
            //        randy = new Random();
            //        piece.color = new Color(randy.Next(1, 255), randy.Next(1, 255), randy.Next(1, 255));
            //}

            //Movement
            switch (kb.IsKeyDown(Keys.A))
            {
                case true:
                    if (curDelay == 0 && !checkLeftSide(activePiece, tetrisPieces) && oldKB.IsKeyUp(Keys.W))
                    {
                        activePiece.Move(new Vector2(-20, 0));
                        if (oldKB.IsKeyUp(Keys.A))
                            curDelay = maxDelay;
                        else curDelay = hDelay;
                    }
                    break;

            }


            switch (kb.IsKeyDown(Keys.D))
            {
                case true:
                    if (curDelay == 0 && !checkRightSide(activePiece, tetrisPieces) && oldKB.IsKeyUp(Keys.W))
                    {
                        activePiece.Move(new Vector2(20, 0));
                        if (oldKB.IsKeyUp(Keys.D))
                            curDelay = maxDelay;
                        else curDelay = hDelay;
                    }

                    break;
            }

            //Softdrop
            switch (kb.IsKeyDown(Keys.S))
            {
                case true:
                    activePiece.Move(new Vector2(0, 4)); //*DO NOT CHANGE VALUE FROM 4, UPPING THE VALUE WILL CAUSE IMPROPER ROUNDING AND BREAK THE COLLISION!*          
                    break;
            }

            oldKB = kb;
            ghost(activePiece, ghostPiece, tetrisPieces);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();

            //Case Gamestate play
            switch (isColor)
            {
                case true:
                    for (int i = 0; i < tetrisPieces.Count(); i++)
                    {
                        for (int j = 0; j < 4; j++)
                            spriteBatch.Draw(blockTexture, tetrisPieces.ElementAt(i).blocks.ElementAt(j), tetrisPieces.ElementAt(i).color);
                    }
                    for (int j = 0; j < 4; j++)
                        spriteBatch.Draw(blockTexture, new Rectangle((int)activePiece.blockz[j].position.X, (int)activePiece.blockz[j].position.Y, 20, 20), activePiece.color);
                    break;

                case false:
                    for (int i = 0; i < tetrisPieces.Count(); i++)
                    {
                        for (int j = 0; j < 4; j++)
                            spriteBatch.Draw(blockTexture, tetrisPieces.ElementAt(i).blocks.ElementAt(j), Color.White);
                    }
                    for (int j = 0; j < 4; j++)
                        spriteBatch.Draw(blockTexture, new Rectangle((int)activePiece.blockz[j].position.X, (int)activePiece.blockz[j].position.Y, 20, 20), Color.White);
                    break;
            }
            switch (isGhostPiece)
            {
                case true:
                    for (int j = 0; j < 4; j++)
                        spriteBatch.Draw(blockTexture, new Rectangle((int)ghostPiece.blockz[j].position.X, (int)ghostPiece.blockz[j].position.Y - 40, 20, 20), ghostPiece.color * ghostPiece.opacity);
                    break;
            }



            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        //Piece generation
        private TetrisPiece generateRandomPiece()
        {
            randy = new Random();
            TetrisPiece temp = new TetrisPiece();
            switch (randy.Next(0, 7))
            {
                case 0:
                    temp = new TetrisPiece(linePiece);
                    break;
                case 1:
                    temp = new TetrisPiece(squarePiece);
                    break;
                case 2:
                    temp = new TetrisPiece(sPiece);
                    break;
                case 3:
                    temp = new TetrisPiece(zPiece);
                    break;
                case 4:
                    temp = new TetrisPiece(tPiece);
                    break;
                case 5:
                    temp = new TetrisPiece(lPiece);
                    break;
                case 6:
                    temp = new TetrisPiece(jPiece);
                    break;
            }

            return temp;

        }




        //Collision detection
        private bool checkCollision(ActivePiece ap, List<TetrisPiece> pieces, Rectangle bottom)
        {
            bool colliding = false;

            foreach (TetrisPiece piece in pieces)
            {
                if (ap.intersecting(piece))
                    colliding = true;
            }



            foreach (Block block in activePiece.blockz)
            {
                if (new Rectangle((int)block.position.X, (int)block.position.Y, 20, 20).Intersects(bottom))
                {
                    colliding = true;
                }
            }

            return colliding;
        }

        private bool checkCollision(ActivePiece ap, List<TetrisPiece> pieces) //without bottom
        {
            bool colliding = false;

            foreach (TetrisPiece piece in pieces)
            {
                if (ap.intersecting(piece))
                    colliding = true;
            }

            return colliding;
        }

        private bool checkLeftSide(ActivePiece ap, List<TetrisPiece> pieces)
        {
            bool colliding = false;
            List<Rectangle> left = new List<Rectangle>();
            foreach (Block block in ap.blockz)
            {
                left.Add(new Rectangle((int)block.position.X - 20, (int)block.position.Y, 20, 20));
            }
            foreach (TetrisPiece piece in pieces)
            {
                foreach (Rectangle block in piece.blocks)
                {
                    foreach (Rectangle blk in left)
                        if (blk.Intersects(block))
                            colliding = true;
                }

            }
            return colliding;
        }

        private bool checkRightSide(ActivePiece ap, List<TetrisPiece> pieces)
        {
            bool colliding = false;
            List<Rectangle> left = new List<Rectangle>();
            foreach (Block block in ap.blockz)
            {
                left.Add(new Rectangle((int)block.position.X + 20, (int)block.position.Y, 20, 20));
            }
            foreach (TetrisPiece piece in pieces)
            {
                foreach (Rectangle block in piece.blocks)
                {
                    foreach (Rectangle blk in left)
                        if (blk.Intersects(block))
                            colliding = true;
                }

            }
            return colliding;
        }

        private bool checkBottomSide(ActivePiece ap, List<TetrisPiece> pieces) //used for recursive method for hard dropping
        {
            bool colliding = false;
            List<Rectangle> left = new List<Rectangle>();
            foreach (Block block in ap.blockz)
            {
                left.Add(new Rectangle((int)block.position.X + 20, (int)block.position.Y, 20, 20));
            }
            foreach (TetrisPiece piece in pieces)
            {
                foreach (Rectangle block in piece.blocks)
                {
                    foreach (Rectangle blk in left)
                        if (blk.Intersects(block))
                            colliding = true;
                }

            }
            return colliding;
        }





        //Hard drop
        private void hardDrop(ActivePiece ap, List<TetrisPiece> pieces)
        {
            int[] dist = new int[8];
            int temp;

            int k = 0;
            foreach (Block block in ap.blockz)
            {
                dist[k] = 460 - (int)block.position.Y; //REPLACE 460 WITH (FieldHeight - 20)
                k++;
            }

            foreach (TetrisPiece piece in pieces) //Check for each piece in the pieces list whether or not they share an x coordinate, if so then check the distance and compare with other blocks.
            {
                int i = 0;
                foreach (Rectangle r in piece.blocks)
                {
                    foreach (Block block in ap.blockz)
                    {
                        if (r.X == block.position.X && r.Y > block.position.Y)
                        {
                            dist[i] = r.Y - ((int)block.position.Y + 20); //+20 to account for block height
                            i++;
                        }
                    }
                }


            }

            temp = dist[0];
            for (int j = 0; j < 8; j++)
            {
                if (temp > dist[j] && dist[j] != 0)
                    temp = dist[j];
            }
            activePiece.Move(new Vector2(0, temp));

            foreach(Block block in activePiece.blockz)
            {
                if(checkCollision(activePiece, tetrisPieces))
                {
                    activePiece.Move(new Vector2(0, -20));
                }
            }
        }

        private void ghost(ActivePiece ap, GhostPiece gp, List<TetrisPiece> pieces)
        {
            int[] dist = new int[8];
            int temp;
            int temp2;

            int k = 0;
            foreach (Block block in ap.blockz)
            {
                dist[k] = 480; //REPLACE 460 WITH (FieldHeight - 20)
                k++;
            }

            foreach (TetrisPiece piece in pieces) //Check for each piece in the pieces list whether or not they share an x coordinate, if so then check the distance and compare with other blocks.
            {
                int i = 0;
                foreach (Rectangle r in piece.blocks)
                {
                    foreach (Block block in ap.blockz)
                    {
                        if (r.X == block.position.X && r.Y > block.position.Y)
                        {
                            dist[i] = r.Y + 20/* + (r.Y - (int)block.position.Y + 20*//* - ((int)block.position.Y + 20)*/; //+20 to account for block height
                            i++;
                        }
                    }
                }
            }

            //determine shortest distance
            temp = dist[0];
            temp2 = dist[0];
            for (int j = 0; j < 8; j++)
            {
                if (temp > dist[j] && dist[j] != 0)
                    temp = dist[j];
                if (temp2 < dist[j] && dist[j] != 0)
                    temp2 = dist[j];
            }

            if ((temp2 - temp) < 20)
                gp.MoveTo(new Vector2(0, temp2), ap);
            else if ((temp2 - temp) > 20)
                gp.MoveTo(new Vector2(0, temp), ap);
            else gp.MoveTo(new Vector2(0, temp), ap);
        }

        private int roundOff(int num) //rounds off to nearest 10
        {
            return ((int)Math.Round(num / 10.0)) * 10;
        }

        private bool IsBlockInPathOfFallingPiece(TetrisPiece piece)
        {
            foreach (Block block in piece.blockz)
            {
                // Get the position of the block in the game grid
                int gridX = (int)block.position.X / 20;
                int gridY = (int)block.position.Y / 20;

                // Check if there is already a block in this position
                if (grid[gridX, gridY] != null)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
