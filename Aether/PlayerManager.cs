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
    /// This is a game component that implements IUpdateable.
    /// </summary>
    class PlayerManager : DrawableGameComponent
    {
        public Player player;
        public SpriteBatch spriteBatch;

        public PlayerManager(Game game)
            : base(game)
        {
            player = new Player();
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
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            this.player.Update(gameTime, new Vector2(), new Vector2());
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            this.player.Draw(spriteBatch);
            base.Draw(gameTime);
        }
    }
}