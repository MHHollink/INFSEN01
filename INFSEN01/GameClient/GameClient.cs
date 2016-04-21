using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameClient
{
    public class GameClient : Game
    {
        private Gamestate.Gamestate gamestate;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpriteFont spriteFont;

        public GameClient()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 1024;
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            spriteFont = Content.Load<SpriteFont>("Courier New");

            // Create a new GameLogix. which can is used to update, handle events and draw
            gamestate = Gamestate.init();

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            // Dispose of all the graphics
            spriteBatch.Dispose();
            graphics.Dispose();
        }
    
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            gamestate = GameLogic.update(
                    Keyboard.GetState(), 
                    Mouse.GetState(),
                    (float) gameTime.ElapsedGameTime.TotalSeconds,
                    gamestate
            );

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSeaGreen);
            
            spriteBatch.Begin();
            foreach (var drawable in GameLogic.draw(gamestate))
            {
                Texture2D sprite = Content.Load<Texture2D>(drawable.Image);

                spriteBatch.Draw(
                    sprite, 
                    new Vector2(
                        drawable.Position.X,
                        drawable.Position.Y
                    ),
                    null,
                    Color.White, 
                    (float) drawable.Rotation + (float)(Math.PI / 2.0f), 
                    new Vector2(sprite.Width / 2.0f, sprite.Height / 2.0f),
                    1.0f, 
                    SpriteEffects.None,
                    0
                );
            }

            spriteBatch.DrawString(spriteFont, "Score : "+gamestate.Score,new Vector2(5,10),Color.Black);
            spriteBatch.DrawString(spriteFont, "Kills : " + gamestate.Kills, new Vector2(5,25),Color.Black);
            spriteBatch.DrawString(spriteFont, "Accuracy : " + gamestate.Accuracy.ToString("N3"), new Vector2(5,40),Color.Black);
            spriteBatch.DrawString(spriteFont, "Highscore : " + gamestate.Highscore, new Vector2(graphics.PreferredBackBufferWidth - 150 ,10),Color.Black);

            if (!gamestate.Alive)
            {
                string[] strings = new[] {"You have an appointment with death!", "press [enter] to try again!"};
                for(int i = 0; i < strings.Length; i++)
                {
                    Vector2 sc = spriteFont.MeasureString(strings[i]);
                    spriteBatch.DrawString(
                        spriteFont,
                        strings[i],
                        new Vector2(graphics.PreferredBackBufferWidth / 1.25f, graphics.PreferredBackBufferHeight / 2.0f + 36*i),
                        Color.Red,
                        0.0f,
                        sc,
                        1.75f,
                        SpriteEffects.None,
                        1.0f
                    );
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
