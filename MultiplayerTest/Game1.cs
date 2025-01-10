using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace MultiplayerTest
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private SpriteFont font;

        public static string Text = "";

        private Texture2D playerTexture;
        public static Vector2 playerPosition = new Vector2(50, 20);
        public static Vector2 otherPlayerPosition;
        private float speed = 120;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
            Server.Start();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            font = Content.Load<SpriteFont>("File");
            playerTexture = Content.Load<Texture2D>("Male1");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState keyboardState = Keyboard.GetState();

            // TODO: Add your update logic here
            if (keyboardState.IsKeyDown(Keys.A)) playerPosition.X -= (float)gameTime.ElapsedGameTime.TotalSeconds * speed;
            if (keyboardState.IsKeyDown(Keys.D)) playerPosition.X += (float)gameTime.ElapsedGameTime.TotalSeconds * speed;
            if (keyboardState.IsKeyDown(Keys.W)) playerPosition.Y -= (float)gameTime.ElapsedGameTime.TotalSeconds * speed;
            if (keyboardState.IsKeyDown(Keys.S)) playerPosition.Y += (float)gameTime.ElapsedGameTime.TotalSeconds * speed;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _spriteBatch.DrawString(font, Text, new Vector2(30, 10), Color.White);

            // Client
            _spriteBatch.Draw(playerTexture, otherPlayerPosition, null, Color.White * .5f,
                0f, Vector2.Zero, 3, SpriteEffects.None, 0);

            // Server (This)
            _spriteBatch.Draw(playerTexture, playerPosition, null, Color.White,
                0f, Vector2.Zero, 3, SpriteEffects.None, 0);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
            Server.Stop();
        }
    }
}
