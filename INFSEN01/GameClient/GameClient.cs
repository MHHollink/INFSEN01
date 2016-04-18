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
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
