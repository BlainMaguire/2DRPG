using System;
using System.Text;
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
    /// This is a game component that implements IUpdateable.
    /// </summary>
    class StaticGraphics : DrawableGameComponent
    {

        SpriteBatch spriteBatch;

        Texture2D background;

        Player player;
        StatBar HPBar;

        SpriteFont courierNew;

        public StaticGraphics(Game game, Player player) : base(game)
        {
            this.player = player;
            this.HPBar = new StatBar();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            courierNew = this.Game.Content.Load<SpriteFont>("CourierNew");
            background = this.Game.Content.Load<Texture2D>("TextBox1");
            HPBar.LoadContent(this.Game.Content, "HPBar", new Vector2(66f, 249f), new Rectangle(0, 0, 108, 14), new Stat(60, (60 - DateTime.Now.Second)));

            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            HPBar.stat.currentValue = DateTime.Now.Second;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {

            spriteBatch.Begin();
            
            //draw textbox at botton. it sucks, I know, but it's useful for displaying things. this is where stats and dialogue boxes will appear.
            spriteBatch.Draw(background, new Vector2(0, 240), Color.White);
            spriteBatch.DrawString(courierNew, "\n" + (((int)(this.player.mapPosition.X) + (int)(this.player.screenPosition.X))) + ", " + (((int)(this.player.mapPosition.Y)+ (int)(this.player.screenPosition.Y))) + " " + this.player.step + "\n Level: " + this.player.currentLevel, new Vector2(8, 249), Color.Black);
            player.Draw(this.spriteBatch);
            HPBar.Draw(this.spriteBatch);
            
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}