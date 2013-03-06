using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Aether
{
    class Player
    {
        public int step = 0;

        public Sprite walkaround;

        public int currentExp = 0;
        public int requiredExp = 45;
        public int currentLevel = 1;
        public Stat HP = new Stat(60, 60);

        float timer = 0f;
        float interval = 1000f / 30f;

        #if ZUNE

        //interval = 1000f / 15f;

        #endif
        
        //frames for animation hard coded for now
        public Rectangle currentFrame = new Rectangle(24, 24, 24, 24);

        public enum State { Standing, Walking };
        public enum Direction { Up=0, Down=24, Left=48, Right=72 };

        public State currentState = State.Standing;
        public Direction currentDirection = Direction.Down;

        public Vector2 screenPosition
        {
            get
            {
                return Vector2.Divide((Vector2.Subtract(this.walkaround.position, new Vector2(this.walkaround.position.X % 24, this.walkaround.position.Y % 24))),24f);
            }

            set
            {
                this.walkaround.position = value;
            }
        }

        public Vector2 mapPosition;

        public Player()
        {
            this.walkaround = new Sprite();
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            currentFrame.Y = (int)currentDirection;
            //spriteBatch.Begin();
            spriteBatch.Draw(this.walkaround.spriteTexture, this.walkaround.position, currentFrame, Color.White);
            //spriteBatch.End();
        }

        public void LoadContent(ContentManager contentManager, string assetName)
        {
            walkaround.spriteTexture = contentManager.Load<Texture2D>(assetName);
        }

        public void Update(GameTime gameTime, Vector2 speed, Vector2 direction)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if ((currentState == State.Walking ) && (timer >= interval))
            {
                if (step == 0)
                {
                    currentFrame.X = 24;
                    step = 1;
                }

                else if (step == 1)
                {
                    currentFrame.X = 48;
                    step = 2;
                }

                else if (step == 2)
                {
                    currentFrame.X = 24;
                    step = 3;
                }

                else if (step == 3)
                {
                    currentFrame.X = 0;
                    step = 0;
                }

                timer = 0f;
            
            }

            if (currentState == State.Standing)
            {
                currentFrame.X = 24;
                step = 0;
            }
        }
    }
}
