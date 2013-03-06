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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Aether
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        BattleSystem bs;
        StaticGraphics sg;
        TileEngine ta;
        PlayerManager pm;

        SpriteFont courierNew;

        Song titleSong;
        Song worldSong;
        Song battleSong;

        Texture2D[] tilePalette = new Texture2D[8]; // bad idea.

        Texture2D rose;

        KeyboardState okbs; //not used.
        KeyboardState newKeyboardState;

        GamePadState ogps;
        GamePadState newGamePadState; //zune maps to 360 controls, so the d-pad is directions, one of the anlogue thumbsticks is the touchpad and so on.

        int tileSize = 24; // I seriously doubt we'll ever change the tile size dynamically so that's why it's a constant.

        Map testMap;
        Map testMap2;

        const int defaultTile = 0; //water in this case. shouldn't be hardcoded. add to map class.

        int mapWidth = 36; //hard coding, more evil. should be stored in map class, and read in from a file.
        int mapHeight = 36;

        int screenX = 2; // starting x and y in tiles. add to map class
        int screenY = 2;

        const int screenMapWidth = 11; // you'd think this would be 10*10 (240 is the screen width and 10 *24pixel tiles is 240), but remember we have half tiles you can't fully see around the edges (and quarter tiles at the corners), so we need to draw one extra either side and offset all of the tiles.
        const int screenMapHeight = 11;

        bool titleScreen = true;
        bool battleSystem = false;
        bool mainGame = false;

        bool progress = false;
        bool revert = false;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 240;
            graphics.PreferredBackBufferHeight = 320;

            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Zune.
            
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //screensize for zune, hitting ctrl+alt+del in vista and then canceling resizes the default screen size to a zune size. (although your monitor will still make it look bigger)
            titleSong = Content.Load<Song>("titlescreen");
            worldSong = Content.Load<Song>("world1");
            battleSong = Content.Load<Song>("battle");

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(titleSong);

            TargetElapsedTime = TimeSpan.FromSeconds(1 / 30.0);

            testMap = new Map();
            testMap.GetMapDataFromFile("map.txt");
            testMap.GetTilesetData(Content.Load<Texture2D>("overworldtileset"));

            testMap2 = new Map();
            testMap2.GetMapDataFromFile("map2.txt");
            testMap2.GetTilesetData(Content.Load<Texture2D>("overworldtileset"));
            
            IList<Map> testMaps = new List<Map>();
            testMaps.Add(testMap);
            testMaps.Add(testMap2);


            //testMap.WriteFile("test1.txt");

            pm = new PlayerManager(this);
            bs = new BattleSystem(this, pm.player);
            sg = new StaticGraphics(this, pm.player);
            ta = new TileEngine(this, pm.player, testMaps, newKeyboardState, newGamePadState);

            // for randomly filling a map with tiles, I used the following code to fill a large map with varying tiles to check performance.
            
            /*Random r = new Random();

            for(int i = 0; i < mymap.layer[i].tileMap.GetUpperBound(0)+1; i++)
            {

                for (int j = 0; j < mymap.layer[i].tileMap.GetUpperBound(1)+1; j++)
                {
                    mymap.layer[i].tileMap[i, j] = r.Next(tilePalette.GetLength(0));
                }

            }*/

            //mymap.layer[i].tileMap[2, 2] = 1;

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
            courierNew = Content.Load<SpriteFont>("CourierNew");
            rose = Content.Load<Texture2D>("roof");

            // create our hero and position him in the centre of the screen.
            pm.player.walkaround.LoadContent(this.Content, "character");
            // -1 because of the whole zero based thing, half a tile(-12) because we have our grid offset by a half a tile to the top and to the left (to align the hero to the grid as a screen size of 10 tiles means the centre is between two tiles)
            pm.player.walkaround.position = new Vector2((float)((((screenMapWidth + screenX) / 2) - 1) * tileSize - 1 - tileSize / 2), (float)((((screenMapHeight + screenY) / 2) - 1) * tileSize - 1 - tileSize / 2));

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            okbs = newKeyboardState; // old keyboard state isn't actually used but may be used in future.
            newKeyboardState = Keyboard.GetState();

            ogps = newGamePadState;
            newGamePadState = GamePad.GetState(PlayerIndex.One);

            progress = GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed || GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed || newKeyboardState.IsKeyDown(Keys.Enter) || newKeyboardState.IsKeyDown(Keys.Space);
            revert = GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || newKeyboardState.IsKeyDown(Keys.Back) || newKeyboardState.IsKeyDown(Keys.Escape);

            int yPositionOnMap = ((int)pm.player.screenPosition.Y) + screenY+1;
            int xPositionOnMap = ((int)pm.player.screenPosition.X) + screenX+1;

            //I know I should use a screen manager, this is just a hack for the demo.

            if (progress == true && mainGame == true)
            {
                mainGame = false;
             
                if (!base.Components.Contains(bs))
                {
                    base.Components.Add(bs);
                }
                if (base.Components.Contains(sg))
                {
                    base.Components.Remove(sg);
                }

                if (base.Components.Contains(ta))
                {
                    base.Components.Remove(ta);
                }

                MediaPlayer.Stop();
                MediaPlayer.Play(battleSong);
                battleSystem = true;
            }

            if (progress == true && titleScreen == true)
            {
                    titleScreen = false;

                    if (!base.Components.Contains(ta))
                    {
                        base.Components.Add(ta);
                    }

                    if (!base.Components.Contains(sg))
                    {
                        base.Components.Add(sg);
                    }
                    MediaPlayer.Stop();
                    MediaPlayer.Play(worldSong);
                    
                    mainGame = true;
             }

            if (revert == true && battleSystem == true)
            {
                base.Components.Remove(bs);
                mainGame = true;
                base.Components.Add(ta);
                base.Components.Add(sg);
                battleSystem = false;
                MediaPlayer.Stop();
                MediaPlayer.Play(worldSong);
            }

            if (revert == true && titleScreen == true)
            {
                this.Exit();
            }

            if (progress && battleSystem == true)
            {
                if (bs.currentState == BattleSystem.BattleState.Done)
                {
                    base.Components.Remove(bs);
                    mainGame = true;
                    base.Components.Add(ta);
                    base.Components.Add(sg);
                    battleSystem = false;
                    MediaPlayer.Stop();
                    MediaPlayer.Play(worldSong);
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);

            if (titleScreen)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(this.Content.Load<Texture2D>("AetherSplashScreen"), new Vector2(0, 0), Color.White);
                //spriteBatch.Draw(this.Content.Load<Texture2D>("HeroForeground"), new Vector2(0, 240), Color.White);
                spriteBatch.DrawString(courierNew,"0.1",new Vector2(179,287), Color.BlueViolet);
                spriteBatch.DrawString(courierNew, "0.1", new Vector2(180, 288), Color.Black);
                spriteBatch.Draw(this.Content.Load<Texture2D>("Rose"), new Vector2(52, 2), Color.White);
                spriteBatch.End();
            }

            else
            {
                /*spriteBatch.Begin();

                spriteBatch.End();
                */
            }

                base.Draw(gameTime);

        }

    }
}
