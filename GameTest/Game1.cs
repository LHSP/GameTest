#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using System.IO;
#endregion

namespace GameName1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        string[] texFilePaths;              //  Array of the textures' paths
        int[] charPos = new int[2];         //  Char position on the map (not the screen!)
        Texture2D[] tiles;                  //  Texture array of map tiles
        int tileSize = 42;                  //  Size of the texture tile to use (TODO: calc it based on window size)
        Texture2D hero;                     //  Hero texure



        int[,] level = {
                       { 0, 1, 2 },
                       { 0, 1, 2 },
                       { 0, 1, 2 },
                       { 0, 1, 2 },
                       { 0, 1, 2 },
                       { 0, 1, 2 },
                       { 0, 1, 2 },
                       { 0, 1, 2 },
                       { 0, 1, 2 },
                       { 0, 1, 2 },
                       { 0, 1, 2 },
                       { 0, 1, 2 }
                       //{ 2, 1, 2, 0, 0, 1, 2, 0, 1 }
                       }; // Level (TODO: Read from file)

        public Game1()
            : base()
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

            texFilePaths = Directory.GetFiles("Content/Tiles/", "*.png");
            tiles = new Texture2D[texFilePaths.Length];

            charPos[0] = Window.ClientBounds.Center.X - tileSize / 2;
            charPos[1] = Window.ClientBounds.Center.Y - tileSize / 2;

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

            hero = Content.Load<Texture2D>("Tiles/floor001.jpg");

            for (int i = 0; i < texFilePaths.Length; i++)
            {
                tiles[i] = Content.Load<Texture2D>(texFilePaths[i].Replace(Content.RootDirectory + "/", ""));
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        KeyboardState oldState;

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState newState = Keyboard.GetState();

            // TODO: Add your update logic here
            if (newState.IsKeyDown(Keys.W))
            {
                if (!oldState.IsKeyDown(Keys.W))
                    charPos[1] -= tileSize;
            }
            else if (newState.IsKeyDown(Keys.S))
            {
                if (!oldState.IsKeyDown(Keys.S))
                    charPos[1] += tileSize;
            }
            else if (newState.IsKeyDown(Keys.A))
            {
                if (!oldState.IsKeyDown(Keys.A))
                    charPos[0] -= tileSize;
            }
            else if (newState.IsKeyDown(Keys.D))
            {
                if (!oldState.IsKeyDown(Keys.D))
                    charPos[0] += tileSize;
            }

            oldState = newState;
                
            Console.WriteLine("X: " + charPos[0] + "; Y: " + charPos[1]);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            DrawVisibleMap();

            spriteBatch.Draw(hero, new Rectangle(charPos[0], charPos[1], tileSize, tileSize), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        #region CustomMethods

        protected void DrawVisibleMap()
        {
            int i = 0;
            int j = 0;
            while(i < 9)
            {
                int tileX = (charPos[0] / tileSize) - 4 + i;
                while(j < 9)
                {
                    int tileY = (charPos[1] / tileSize) - 4 + j;
                    if (tileX >= 0 && tileX < level.GetLength(0) && tileY >= 0 && tileY < level.GetLength(1))
                        spriteBatch.Draw(tiles[level[tileX, tileY]], new Rectangle(i * tileSize, j * tileSize, tileSize, tileSize), Color.White);
                    j++;
                }
                i++;
                j = 0;
            }
        }

        #endregion
    }
}
