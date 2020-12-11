using System.Linq;
using Engine.Functionality;
using Engine.GameStates;
using Engine.Overlay;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game.Track
{
    public class TrackState : GameState
    {
        public static ObjectHandler objectHandler;
        public static OverlayManager OverlayManager { get; } = new OverlayManager();

        private Car _car;
        private Camera _camera;

        private Texture2D trackTexture;
        Point[] innerTrack =
            {new Point(200, 200), new Point(800, 200), new Point(800, 600), new Point(200, 600)};

        Point[] outerTrack =
            {new Point(50, 50), new Point(1000, 50), new Point(1000, 750), new Point(50, 750)};

        public override void OnCreate()
        {
            objectHandler = new ObjectHandler(Game1.Instance.Graphics.PreferredBackBufferWidth,
                Game1.Instance.Graphics.PreferredBackBufferHeight);
            _camera = new Camera();
            //Essential stuff like camera
        }

        public override void LoadContent(ContentManager content)
        {
            // Load AbstractContentLibrary, create objects like car and stage
            var carTexture = content.Load<Texture2D>("F1Car");
            _car = new Car(carTexture,
                null,
                new Vector2(100, 100),
                new Vector2(0, 0)
            );
            objectHandler.AddObjectToList("car", _car);

            trackTexture = PaintTrack();

            var updates = new[] {"car"};
            objectHandler.AddObjectToUpdate(updates);

        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(trackTexture, new Vector2(0,0), null, Color.White, 0, new Vector2(0,0), 1, SpriteEffects.None, 0);
            objectHandler.Draw(spriteBatch);
            OverlayManager.DrawOverlays(spriteBatch);
        }

        private Texture2D PaintTrack()
        {
            var texture = new Texture2D(Game1.Instance.GraphicsDevice, Game1.Instance.Graphics.PreferredBackBufferWidth,
                Game1.Instance.Graphics.PreferredBackBufferHeight);
            var colorArray = new Color[texture.Height * texture.Width];
            for (int x = 0; x < texture.Width; x++)
            {
                for (int y = 0; y < texture.Height; y++)
                {
                    var index = y * texture.Width + x;
                    if (Geometry.CheckIfPointIsInside(new Point(x, y), outerTrack) &&
                        !Geometry.CheckIfPointIsInside(new Point(x, y), innerTrack))
                    {
                        colorArray[index] = Color.LightGray;
                    }
                    else
                    {
                        colorArray[index] = Color.Transparent;
                    }
                }
            }

            texture.SetData(colorArray);
            return texture;
        }

        public override void DrawStatic(SpriteBatch staticSpriteBatch)
        {
            OverlayManager.DrawStaticOverlays(staticSpriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            // We use the one below
        }

        public override void Update(GameTimeHolder gameTime)
        {
            // Update objects, camera, transform
            objectHandler.Update(gameTime);
            objectHandler.CheckCollisions();
        }

        public override void OnExit()
        {
            // Clear loaded content
            OverlayManager.Clear();
            objectHandler.Clear();
            GameTimeHolder.Instance.Clear();
        }
    }
}