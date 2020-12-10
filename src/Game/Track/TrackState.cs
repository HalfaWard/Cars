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
        public override void OnCreate()
        {
            objectHandler = new ObjectHandler(Game1.Instance.Graphics.PreferredBackBufferWidth, Game1.Instance.Graphics.PreferredBackBufferHeight);
            //Essential stuff like camera
        }

        public override void LoadContent(ContentManager content)
        {
            // Load AbstractContentLibrary, create objects like car and stage
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            objectHandler.Draw(spriteBatch);
            OverlayManager.DrawOverlays(spriteBatch);
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
