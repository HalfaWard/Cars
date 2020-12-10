using Engine.Functionality;
using Engine.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Game.Track
{
    public class TurnEdge : StageObject
    {
        public TurnEdge(Vector2 center, int radius, int degreesFrom, int degreesTo) : base(GetCircleTexture(radius*2, degreesFrom, degreesTo), null, center, new Vector2())
        {
            origin = CenterOrigin();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Texture != null)
            {
                spriteBatch.Draw(Texture, Position, null, filter, Rotation, origin, scale, SpriteEffects.None, 0);
            }
        }

        public override void OnCollison(StageObject collisionTarget, List<Rectangle> intersections, string hitboxName, string targetHitboxName)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Update(GameTimeHolder gameTime)
        {
            
        }

        private static Texture2D GetCircleTexture(int diam, int degreesFrom, int degreesTo)
        {
            var texture = new Texture2D(Game1.Instance.GraphicsDevice, diam, diam);
            var colorData = new Color[diam * diam];

            float radius = diam / 2f;

            for (var x = 0; x < diam; x++)
            {
                for (var y = 0; y < diam; y++)
                {
                    var index = x * diam + y;
                    var pos = new Vector2(x - radius, y - radius);
                    var angle = Geometry.RadiansToDegrees(Geometry.AngleOfVector(pos));
                    if (pos.Length() == radius && ((angle >= degreesFrom && angle <= degreesTo) || (degreesTo < degreesFrom && (angle >= degreesFrom || angle <= degreesTo )))) // Don't ask
                    {
                        colorData[index] = Color.White;
                    }
                    else
                    {
                        colorData[index] = Color.Transparent;
                    }
                }
            }

            texture.SetData(colorData);
            return texture;
        }
    }
}
