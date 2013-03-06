using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Aether
{
    class Sprite
    {

        //The current position of the Sprite
        public Vector2 position = new Vector2(0, 0);

        //The texture object used when drawing the sprite
        public Texture2D spriteTexture;

        //Load the texture for the sprite using the Content Pipeline

        public virtual void LoadContent(ContentManager contentManager, string assetName)
        {
            spriteTexture = contentManager.Load<Texture2D>(assetName);
        }


        //Draw the sprite to the screen
        public virtual void Draw(SpriteBatch theSpriteBatch)
        {
            theSpriteBatch.Draw(spriteTexture, position, Color.White);
        }

        public virtual void Update(GameTime theGameTime, Vector2 theSpeed, Vector2 theDirection)
        {
            position += theDirection * theSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
        }

    }
}
