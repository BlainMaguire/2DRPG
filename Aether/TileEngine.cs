using System;
using System.Text;
using System.Collections.Generic;

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
    class TileEngine : DrawableGameComponent
    {
        SpriteBatch spriteBatch;

        int upBitset = 1;
        int downBitset = 2;
        int lefttBitset = 4;
        int rightBitset = 8;

        float timer = 0f;
        float interval = 1000f / 2f;

        /*  _ _ _
         * |5 1 9|
         * |4 0 8|
         * |6 2 10|
         *  - - -
         */

        int tileOffsetX = 0; //Player's offset in pixels to the current tile they are on.
        int tileOffsetY = 0;

        int screenOffsetX = 0; // offset of the screen, in tiles. this is how we know we're currently moving up by one tile, etc.
        int screenOffsetY = 0;

        int screenMapWidth = 11; // you'd think this would be 10*10 (240 is the screen width and 10 *24pixel tiles is 240), but remember we have half tiles you can't fully see around the edges (and quarter tiles at the corners), so we need to draw one extra either side and offset all of the tiles.
        int screenMapHeight = 11;

        int screenX = 2; // starting x and y in tiles. add to map class
        int screenY = 2;

        Player Player;
        private int currentMap = 0;
        private IList<Map> maps;

        private KeyboardState newKeyboardState;
        private GamePadState newGamePadState;

        private int step = 2;

        public TileEngine(Game game, Player Player, IList<Map> maps, KeyboardState keyboardState, GamePadState gamePadState)
            : base(game)
        {
            this.Player = Player;
            this.maps = maps;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public void LoadMaps(List<Map> maps)
        {
            this.maps = maps;

        }

        public void AddMap(Map map)
        {
            this.maps.Add(map);

        }

        public void RemoveMap(Map map)
        {
            this.maps.Remove(map);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer >= interval)
            {

                newKeyboardState = Keyboard.GetState();
                newGamePadState = GamePad.GetState(PlayerIndex.One);

                int yTilePositionOnMap = ((int)Player.screenPosition.Y) + screenY + 1;
                int xTilePositionOnMap = ((int)Player.screenPosition.X) + screenX + 1;

                if (Player.currentState != Player.State.Walking && (tileOffsetX == 0 && tileOffsetY == 0))
                {


                    if (maps[currentMap].layers[0].passMap[maps[currentMap].layers[0].tileMap[xTilePositionOnMap, yTilePositionOnMap]] == 16)
                    {
                        screenX = 5;
                        screenY = 5;
                        currentMap = 1;
                    }

                    else if ((newKeyboardState.IsKeyDown(Keys.Up) || newGamePadState.DPad.Up == ButtonState.Pressed)) //(newKeyboardState.IsKeyUp(Keys.Down)) && (newKeyboardState.IsKeyUp(Keys.Left)) && (newKeyboardState.IsKeyUp(Keys.Right)) && (okbs.IsKeyDown(Keys.Up))) || (newGamePadState.DPad.Up == ButtonState.Pressed && newGamePadState.DPad.Down == ButtonState.Released && newGamePadState.DPad.Left == ButtonState.Released && newGamePadState.DPad.Right == ButtonState.Released ))//&& ogps.DPad.Up == ButtonState.Pressed))
                    {


                        if ((yTilePositionOnMap - 1 >= 0) && (canWalk(xTilePositionOnMap, yTilePositionOnMap, Keys.Up)))
                        {


                            screenY--;
                            Player.currentState = Player.State.Walking;


                        }

                        Player.currentDirection = Player.Direction.Up;

                    }

                    else if ((newKeyboardState.IsKeyDown(Keys.Down) || newGamePadState.DPad.Down == ButtonState.Pressed))//(newKeyboardState.IsKeyUp(Keys.Up)) && (newKeyboardState.IsKeyUp(Keys.Left)) && (newKeyboardState.IsKeyUp(Keys.Right)) && (okbs.IsKeyDown(Keys.Down))) || (newGamePadState.DPad.Down == ButtonState.Pressed && newGamePadState.DPad.Up == ButtonState.Released && newGamePadState.DPad.Left == ButtonState.Released && newGamePadState.DPad.Right == ButtonState.Released ))//&& ogps.DPad.Down == ButtonState.Pressed))
                    {

                        if ((yTilePositionOnMap + 1 < maps[currentMap].layers[0].tileMap.GetLength(1)) && (canWalk(xTilePositionOnMap, yTilePositionOnMap, Keys.Down)))
                        {

                            screenY++;
                            Player.currentState = Player.State.Walking;
                        }

                        Player.currentDirection = Player.Direction.Down;

                    }

                    else if ((newKeyboardState.IsKeyDown(Keys.Left) || newGamePadState.DPad.Left == ButtonState.Pressed))//(newKeyboardState.IsKeyUp(Keys.Down)) && (newKeyboardState.IsKeyUp(Keys.Up)) && (newKeyboardState.IsKeyUp(Keys.Right)) && (okbs.IsKeyDown(Keys.Left))) || (newGamePadState.DPad.Left == ButtonState.Pressed && newGamePadState.DPad.Down == ButtonState.Released && newGamePadState.DPad.Up == ButtonState.Released && newGamePadState.DPad.Right == ButtonState.Released ))//&& ogps.DPad.Left == ButtonState.Pressed))
                    {

                        if ((xTilePositionOnMap - 1 >= 0) && canWalk(xTilePositionOnMap, yTilePositionOnMap, Keys.Left))
                        {


                            screenX--;
                            Player.currentState = Player.State.Walking;

                        }

                        Player.currentDirection = Player.Direction.Left;

                    }

                    else if ((newKeyboardState.IsKeyDown(Keys.Right) || newGamePadState.DPad.Right == ButtonState.Pressed))//(newKeyboardState.IsKeyUp(Keys.Down)) && (newKeyboardState.IsKeyUp(Keys.Left)) && (newKeyboardState.IsKeyUp(Keys.Up)) && (okbs.IsKeyDown(Keys.Right))) || (newGamePadState.DPad.Right == ButtonState.Pressed && newGamePadState.DPad.Down == ButtonState.Released && newGamePadState.DPad.Left == ButtonState.Released && newGamePadState.DPad.Up == ButtonState.Released )) //&& ogps.DPad.Right == ButtonState.Pressed)))
                    {

                        if ((xTilePositionOnMap + 1 < maps[currentMap].layers[0].tileMap.GetLength(0)) && (canWalk(xTilePositionOnMap, yTilePositionOnMap, Keys.Right)))
                        {
                            screenX++;
                            Player.currentState = Player.State.Walking;

                        }

                        Player.currentDirection = Player.Direction.Right;

                    }

                    else
                    {
                        Player.currentState = Player.State.Standing;
                    }

                }

                Player.mapPosition = new Vector2(screenX, screenY);
                timer = 0f;
            }
            Player.Update(gameTime, new Vector2(), new Vector2());
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {

            if (Player.currentState == Player.State.Standing)
                {
                    
                }

                //if we're walking in a direction, adjust the Player's offset a little in the current direction they are facing.

                if (Player.currentDirection == Player.Direction.Down && Player.currentState == Player.State.Walking)
                {
                    screenOffsetY = 1;
                    tileOffsetY += this.step; //using a variable instead of a value means we can change the speed dynamically, this varibable should be in the sprite class actually.
                }

                if (Player.currentDirection == Player.Direction.Up && Player.currentState == Player.State.Walking)
                {
                    screenOffsetY = -1;
                    tileOffsetY -= this.step;
                }

                if (Player.currentDirection == Player.Direction.Left && Player.currentState == Player.State.Walking)
                {
                    screenOffsetX = -1;
                    tileOffsetX -= this.step;
                }

                if (Player.currentDirection == Player.Direction.Right && Player.currentState == Player.State.Walking)
                {
                    screenOffsetX = 1;
                    tileOffsetX += this.step;
                }


                spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Texture, SaveStateMode.None);

                int currentPosition = 0;
                Rectangle tileAtPosition = new Rectangle(0, 0, maps[currentMap].tileSize, maps[currentMap].tileSize);


                for (int k = 0; k < maps[currentMap].layers.Count; k++)
                {

                    for (int j = -1; j < screenMapHeight + 1; j++)
                    {

                        for (int i = -1; i < screenMapWidth + 1; i++) //absolute value for the offset is because we want to draw an extra tile if the offset is -1. subtracting one would draw one less tile.
                        {

                            //foreach map layer:

                            // if a tile we're drawing to the screen is within the bounds of the map tile array, then draw it. else use the default tile. default tile varies from map to map, and should be stored in the map class. it could be water for an island or plain black for interiors.
                            currentPosition = ((i + screenX >= 0) && (j + screenY >= 0) && (i + screenX < maps[currentMap].layers[k].tileMap.GetLength(0)) && (j + screenY < maps[currentMap].layers[k].tileMap.GetLength(1))) ? maps[currentMap].layers[k].tileMap[i + screenX, j + screenY] : maps[currentMap].defaultTile;

                            tileAtPosition.X = (currentPosition % maps[currentMap].layers[k].tilesetX) * maps[currentMap].tileSize;
                            tileAtPosition.Y = (currentPosition / (maps[currentMap].layers[k].tilesetX) * maps[currentMap].tileSize);

                            if (!(maps[currentMap].layers[k].isAlphaLayer && currentPosition == 0) && maps[currentMap].layers[k].isEnabled)
                            {
                                spriteBatch.Draw(maps[currentMap].layers[k].mapTiles, new Vector2((float)((i + screenOffsetX) * maps[currentMap].tileSize) - 1 - maps[currentMap].tileSize / 2 - tileOffsetX, (float)((j + screenOffsetY) * maps[currentMap].tileSize) - 1 - maps[currentMap].tileSize / 2 - tileOffsetY), tileAtPosition, Color.White);
                            }

                        }
                    }
                }

                spriteBatch.End();

                //finally, if we're aligned to the grid then we are now standing and able to listen to another directional keypress.    
                if (tileOffsetX >= maps[currentMap].tileSize || tileOffsetY >= maps[currentMap].tileSize || tileOffsetX <= -1 * maps[currentMap].tileSize || tileOffsetY <= -1 * maps[currentMap].tileSize)
                {
                    Player.currentState = Player.State.Standing;
                    tileOffsetX = 0;
                    tileOffsetY = 0;
                    screenOffsetX = 0;
                    screenOffsetY = 0;
                }

                base.Draw(gameTime);

            }


        public bool canWalk(int x, int y, Keys direction)
        {
            int offsetX = x;
            int offsetY = y;

            int bitSetAtPosition = 0;
            int bitSetAhead = 0;

            if (direction == Keys.Up)
            {
                bitSetAtPosition = upBitset;
                bitSetAhead = downBitset;
                offsetY--; 
            }

            else if (direction == Keys.Down)
            {
                bitSetAtPosition = downBitset;
                bitSetAhead = upBitset;
                offsetY++;
            }

            else if (direction == Keys.Left)
            {
                bitSetAtPosition = lefttBitset;
                bitSetAhead = rightBitset;
                offsetX--;
            }

            else if (direction == Keys.Right)
            {
                bitSetAtPosition = rightBitset;
                bitSetAhead = lefttBitset;
                offsetX++;
            }


            for(int i = 0; i < maps[currentMap].layers.Count; i++) {

                if (((maps[currentMap].layers[i].passMap[maps[currentMap].layers[i].tileMap[x, y]] & bitSetAtPosition) == bitSetAtPosition) || ((maps[currentMap].layers[i].passMap[maps[currentMap].layers[i].tileMap[offsetX, offsetY]] & bitSetAhead) == bitSetAhead))
                {
                    return false;
                }

            }

            return true;
        }


        }
    
}
