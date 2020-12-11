﻿using System.Collections.Generic;
using System.Linq;
using Engine.Functionality;
using Engine.GameStates;
using Engine.Objects;
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
        private StraightTrack _straightTrack;
        private Camera _camera;
        
        public override void OnCreate()
        {
            objectHandler = new ObjectHandler(Game1.Instance.Graphics.PreferredBackBufferWidth, Game1.Instance.Graphics.PreferredBackBufferHeight);
            _camera = new Camera();
            //Essential stuff like camera
        }

        public override void LoadContent(ContentManager content)
        {
            // Load AbstractContentLibrary, create objects like car and stage
            var carTexture = content.Load<Texture2D>("F1Car");
            _car = new Car(carTexture,
                new Dictionary<string, Hitbox>
                {
                    {"default", new Hitbox(new List<Rectangle> {new Rectangle(0, 0, 100, 000)}, Vector2.Zero)}
                },
                new Vector2(650, 380),
                new Vector2(0, 0)
            );
            _straightTrack = new StraightTrack(new Vector2(500, 500), 400, 200, 90);

            objectHandler.AddObjectToList("car", _car);
            objectHandler.AddObjectToList("straightTrack", _straightTrack);
            var updates = new []{"car", "straightTrack" };
            objectHandler.AddObjectToUpdate(updates);

            objectHandler.AddCollisionHandler("car", "straightTrack", "", "outside", false, (obj, tar) =>
            {
                var carPoints = Geometry.GetRotatedRectangle(obj.Position, obj.Rectangle(), obj.Rotation);
                return carPoints.Any(p => !((StraightTrack)tar).CheckIfPointIsInside(p));
            });
            objectHandler.AddCollisionHandler("car", "straightTrack", "", "inside", false, (obj, tar) =>
            {
                var carPoints = Geometry.GetRotatedRectangle(obj.Position, obj.Rectangle(), obj.Rotation);
                return carPoints.All(p => ((StraightTrack)tar).CheckIfPointIsInside(p));
            });
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