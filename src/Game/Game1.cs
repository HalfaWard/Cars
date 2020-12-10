using Engine.Functionality;
using Engine.GameStates;
using Engine.Input;
using Game.Track;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public static Game1 Instance { get; private set; }

        public GameStateManager stateManager;
        public GraphicsDeviceManager Graphics { get; private set; }
        public SpriteBatch SpriteBatch { get; private set; }
        public SpriteBatch StaticSpriteBatch { get; private set; }
        public RenderTarget2D RenderTarget { get; set; }
        public GameTimeHolder GameTimeHolder { get; private set; }
        public static KeyBuffer KeyBuffer { get; private set; }
        public Matrix Transform { get; set; }
        public bool HasTransform { get; set; } = false;

        private Color Gamma = Color.White;

        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            KeyBuffer = new KeyBuffer(false, 10);
        }

        protected override void Initialize()
        {
            Instance = this;
            KeyBuffer.SetKeybinds(".\\Keybindings.txt", keybindType.KeyboardBind);
            stateManager = new GameStateManager();
            stateManager.SetContentManager(Content);
            UpdateScreenResolution();
            RenderTarget = new RenderTarget2D(GraphicsDevice, Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight);

            base.Initialize();
        }

        private void UpdateScreenResolution()
        {
            Graphics.PreferredBackBufferHeight = 1000;
            Graphics.PreferredBackBufferWidth = 1000;
            Graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            StaticSpriteBatch = new SpriteBatch(GraphicsDevice);
            stateManager.SetSpriteBatches(SpriteBatch, StaticSpriteBatch);
            stateManager.AddState(new TrackState()); //Create new state
        }

        protected override void Update(GameTime gameTime)
        {
            if (GameTimeHolder == null)
                GameTimeHolder = GameTimeHolder.Initialize(gameTime);
            else
                GameTimeHolder.GameTime = gameTime;

            var keyState = Keyboard.GetState();
            KeyBuffer.RegisterKeyPressed(keyState, gameTime);

            stateManager.Update(GameTimeHolder);

            Transform = stateManager.Peek().Transform;
            HasTransform = stateManager.Peek().HasTransform;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            GraphicsDevice.Clear(ClearOptions.Target, new Color(16, 30, 41), 1, 0);
            GraphicsDevice.SetRenderTarget(RenderTarget);

            if (HasTransform) SpriteBatch.Begin(transformMatrix: Transform, samplerState: SamplerState.PointClamp);
            else SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            StaticSpriteBatch.Begin(samplerState: SamplerState.PointClamp);

            stateManager.Draw();
            stateManager.DrawStatic();

            SpriteBatch.End();
            StaticSpriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            SpriteBatch.Begin();
            SpriteBatch.Draw(RenderTarget, new Rectangle(0, 0, Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight), Gamma);
            SpriteBatch.End();

            StaticSpriteBatch.Begin();
            StaticSpriteBatch.Draw(RenderTarget, new Rectangle(0, 0, Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight), Gamma);
            StaticSpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}