using System;
using System.Collections.Generic;
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
        private bool followCar;

        private Texture2D trackTexture;

        private int[] outerX = {0300, 1700, 1825, 1900, 1900, 1825, 1700, 0300, 0175, 0100, 0100, 0175};
        private int[] outerY = {0100, 0100, 0175, 0300, 0900, 1025, 1100, 1100, 1025, 0900, 0300, 0175};
        
        private int[] innerX = {0500, 1500, 1625, 1700, 1700, 1625, 1500, 0500, 0425, 0300, 0300, 0375};
        private int[] innerY = {0300, 0300, 0375, 0500, 0900, 0975, 1100, 1100, 0975, 0900, 0500, 0375};
        
        private Point[] innerTrack;
        private Point[] outerTrack; 

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
                new Vector2(200, 200),
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
            if (innerX.Length != innerY.Length) throw new Exception();
            if (outerX.Length != outerY.Length) throw new Exception();
            innerTrack = innerX.Zip(innerY, (x, y) => new Point(x, y)).ToArray();
            outerTrack = outerX.Zip(outerY, (x, y) => new Point(x, y)).ToArray();
            
            var texture = new Texture2D(Game1.Instance.GraphicsDevice, Game1.Instance.Graphics.PreferredBackBufferWidth,
                Game1.Instance.Graphics.PreferredBackBufferHeight);
            var colorArray = new Color[texture.Height * texture.Width];
            for (int x = 0; x < texture.Width; x++)
            {
                for (int y = 0; y < texture.Height; y++)
                {
                    var index = y * texture.Width + x;
                    if (x == 200 && y == 300)
                    {
                        var a = 1;
                    }
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
            if (Game1.KeyBuffer.CheckKeybindPress("space"))
                followCar = !followCar;

            if (followCar)
            {
                _camera.Follow(_car, Game1.Instance.Graphics.PreferredBackBufferWidth, Game1.Instance.Graphics.PreferredBackBufferHeight);
                Transform = _camera.Transform;
                HasTransform = true;
            }
            else
                HasTransform = false;

            objectHandler.Update(gameTime);
            objectHandler.CheckCollisions();
            var carPolygon = Geometry.GetRotatedRectangle(_car.Position, _car.Rectangle(), _car.Rotation);
            var carState = "inside";
            foreach(var point in carPolygon)
            {
                if(!Geometry.CheckIfPointIsInside(point, outerTrack) || Geometry.CheckIfPointIsInside(point, innerTrack))
                {
                    carState = "outside";
                    break;
                }
            }
            _car.OnCollison(null, null, null, carState);
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