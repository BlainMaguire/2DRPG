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

/*

This is unfinished. I began porting over to PyGame.
Several placeholder bits and pieces in here, primarily to demonstrate components I wrote.

*/
    class BattleSystem : DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        SpriteFont courierNew;
        private KeyboardState newKeyboardState;
        private GamePadState newGamePadState;
        Texture2D bg;
        Texture2D hero;
        Texture2D battleBG;
        Texture2D spider;

        Random randGen = new Random();

        Player player;

        StatBar HPBar;
        StatBar EnemyHP;

        Vector2 playerPosition = new Vector2(179, 229);
        int yOffset = 2;
        bool hasMovedUp = false;

        IList<Enemy> enemies = new List<Enemy>();

        public enum BattleState { Waiting, Animating, Over, Done };
        public BattleState currentState = BattleState.Waiting;

        public BattleSystem(Game game, Player player) : base(game)
        {
            HPBar = new StatBar();
            EnemyHP = new StatBar();
            this.player = player;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            courierNew = this.Game.Content.Load<SpriteFont>("CourierNew");
            hero = this.Game.Content.Load<Texture2D>("herobattle");
            bg = this.Game.Content.Load<Texture2D>("textbox1");
            battleBG = this.Game.Content.Load<Texture2D>("battlebg");
            spider = this.Game.Content.Load<Texture2D>("spider");
            

            HPBar.LoadContent(this.Game.Content, "HPBar", new Vector2(130f, 229f), new Rectangle(0, 0, 108, 14), new Stat(60,60));
            EnemyHP.LoadContent(this.Game.Content, "HPBar", new Vector2(66f, 120f), new Rectangle(0, 0, 108, 14), new Stat(45,45));

            enemies.Add(new Enemy("spider", spider, EnemyHP, new Vector2(90, 139), 25));

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            newKeyboardState = Keyboard.GetState();
            newGamePadState = GamePad.GetState(PlayerIndex.One);

            bool progress = GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed || GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed || newKeyboardState.IsKeyDown(Keys.Enter) || newKeyboardState.IsKeyDown(Keys.Space);
            bool revert = GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || newKeyboardState.IsKeyDown(Keys.Back) || newKeyboardState.IsKeyDown(Keys.Escape);

            if (currentState == BattleState.Waiting && progress)
            {
                currentState = BattleState.Animating;
            }

            else if (currentState == BattleState.Animating)
            {

                if (hasMovedUp == false)
                {

                    playerPosition.Y = playerPosition.Y + yOffset;
                    yOffset += 2;

                    if (yOffset <= 20)
                    {
                        hasMovedUp = true;
                    }

                }

                else
                {

                    playerPosition.Y = playerPosition.Y - yOffset;
                    yOffset -= 2;

                    if (yOffset <= 0)
                    {
                        HPBar.stat.currentValue = player.HP.currentValue - 8;
                        enemies[randGen.Next(0, enemies.Count - 1)].HP.stat.currentValue = EnemyHP.stat.currentValue - 8;
                        currentState = BattleState.Waiting;
                        hasMovedUp = false;
                    }

                }

            }

            else if (currentState == BattleState.Over)
            {
                int totalExp = 0;

                foreach (Enemy e in enemies)
                {
                    totalExp += e.expPoints;
                }

                enemies.Clear();
                enemies.Add(new Enemy("spider", spider, EnemyHP, new Vector2(90, 139), 25));

                if (HPBar.stat.currentValue > 0)
                {
                    player.currentExp = totalExp;
                    if (player.currentExp <= player.requiredExp)
                    {
                        player.currentLevel++;
                        player.requiredExp = player.requiredExp * 2 + 10;
                        player.HP.maxValue += 10;
                        player.HP.currentValue = player.HP.maxValue;
                    }

                    currentState = BattleState.Done;

                }

                else
                {
                    this.Game.Exit();
                }

            }

                if (HPBar.stat.currentValue <= 0)
                {
                    currentState = BattleState.Over;
                }


                if (enemies.Count <= 0)
                {
                    currentState = BattleState.Over;
                }

                else
                {
                    for (int i = 0; i < enemies.Count; i++)
                    {
                        if (enemies[i].HP.stat.currentValue <= 0)
                        {
                            enemies.Remove(enemies[i]);
                        }
                    }
                }

          


        }


        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(battleBG, new Rectangle(0, 0, 240, 320), Color.White);
            spriteBatch.Draw(hero, this.playerPosition, Color.White);
            
            foreach(Enemy e in enemies)
            {
            spriteBatch.Draw(e.battleGraphic,e.position, Color.White);
            e.HP.Draw(spriteBatch);
            }

            spriteBatch.Draw(bg, new Rectangle(0, 239, 120, 80), Color.White);
            spriteBatch.DrawString(courierNew, "Attack\nDefend\nItem", new Vector2(24, 249), Color.Black);
            EnemyHP.Draw(this.spriteBatch);
            HPBar.Draw(this.spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }

    }
}
