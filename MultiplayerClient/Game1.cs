using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace MultiplayerClient
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private SpriteFont font;
        public static string text = "";

        private Texture2D playerTexture;
        public static Vector2 playerPosition = new Vector2(5, 20);
        public static Vector2 otherPlayerPosition;
        private float speed = 120;

        private const float UPDATE_CLIENT_DELAY = 1f / 60f;
        private float timer = 0;

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

            Client.Connect();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            font = Content.Load<SpriteFont>("File");
            playerTexture = Content.Load<Texture2D>("Male1");
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();

            Client.Close();
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            // TODO: Add your update logic here
            //Client.Handle();

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

            _spriteBatch.DrawString(font, text, new Vector2(30, 10), Color.White);

            // Server
            _spriteBatch.Draw(playerTexture, otherPlayerPosition, null, Color.White * .5f,
                0f, Vector2.Zero, 3, SpriteEffects.None, 0);
            
            // Client (This)
            _spriteBatch.Draw(playerTexture, playerPosition, null, Color.White,
                0f, Vector2.Zero, 3, SpriteEffects.None, 0);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
