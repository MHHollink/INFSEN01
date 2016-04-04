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
            spriteBatch.Dispose();
            graphics.Dispose();
        }
    
        protected override void Update(GameTime gameTime)
        {
            if (
                GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || 
                Keyboard.GetState().IsKeyDown(Keys.Escape)
            )
                Exit();

            // TODO: Add your update logic here
            gamestate = GameLogic.update(
                    Keyboard.GetState(), Mouse.GetState(),
                    (float) gameTime.ElapsedGameTime.TotalSeconds,
                    gamestate
                );

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            spriteBatch.Begin();
            foreach (var drawable in GameLogic.draw(gamestate))
            {
                spriteBatch.Draw(Content.Load<Texture2D>(drawable.Image),
                  drawable.Position, Color.White);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
