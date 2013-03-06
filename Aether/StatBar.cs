using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Aether
{
    class StatBar : Sprite
    {
        public Rectangle barPart;
        public int frameCount;
        public Stat stat;

        public void LoadContent(ContentManager contentManager, string assetName, Vector2 position, Rectangle bar)
        {
            frameCount = spriteTexture.Height / bar.Height;
            this.position = position;
            barPart = bar;
            base.LoadContent(contentManager, assetName);
        }

        public void LoadContent(ContentManager contentManager, string assetName, Vector2 position, Rectangle bar, Stat stat)
        {
            base.LoadContent(contentManager, assetName);
            frameCount = spriteTexture.Height / bar.Height;
            this.position = position;
            barPart = bar;
            this.stat = stat;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            int currentBar = barPart.Width;
            Color tint = Color.White;

            for (int i = 0; i < frameCount; i++)
            {
                if(i==1)
                {
                    currentBar =(int)((stat.currentValue/(float)stat.maxValue)* (float)barPart.Width);
                    tint = new Color(1f,(stat.currentValue/(float)stat.maxValue),(stat.currentValue/(float)stat.maxValue),1f);
                }

                else
                {
                    currentBar = barPart.Width;
                    tint = Color.White;
                }

                spriteBatch.Draw(this.spriteTexture, this.position, new Rectangle(0,barPart.Height*i,currentBar,barPart.Height), tint);
            }
       }
    }
}
