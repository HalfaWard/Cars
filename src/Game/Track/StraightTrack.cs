using Engine.Functionality;
using Engine.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Game.Track
{
    public class StraightTrack : StageObject
    {

        public StraightTrack(Vector2 start, int length, int width, int degreeAngle) : base(GetLineTexture(length, width), null, start, new Vector2(), new Vector2(width / 2, length))
        {
            Rotation = (float)Geometry.DegreesToRadians(degreeAngle);
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

        public bool CheckIfPointIsInside(Point p)
        {
            var polygon = Geometry.GetRotatedRectangle(Position, Rectangle(), Rotation);
            var n = polygon.Length;
            if (n < 3)
            {
                return false;
            }

            Point extreme = new Point(10000, p.Y);

            int count = 0, i = 0;
            do
            {
                int next = (i + 1) % n;

                if (DoIntersect(polygon[i],
                                polygon[next], p, extreme))
                {
                    if (Orientation(polygon[i], p, polygon[next]) == 0)
                    {
                        return OnSegment(polygon[i], p,
                                         polygon[next]);
                    }
                    count++;
                }
                i = next;
            } while (i != 0);

            return (count % 2 == 1);
        }

        static bool DoIntersect(Point p1, Point q1,
                            Point p2, Point q2)
        {
            int o1 = Orientation(p1, q1, p2);
            int o2 = Orientation(p1, q1, q2);
            int o3 = Orientation(p2, q2, p1);
            int o4 = Orientation(p2, q2, q1);

            if (o1 != o2 && o3 != o4)
            {
                return true;
            }

            if (o1 == 0 && OnSegment(p1, p2, q1))
            {
                return true;
            }

            if (o2 == 0 && OnSegment(p1, q2, q1))
            {
                return true;
            }

            if (o3 == 0 && OnSegment(p2, p1, q2))
            {
                return true;
            }

            if (o4 == 0 && OnSegment(p2, q1, q2))
            {
                return true;
            }

            return false;
        }

        static bool OnSegment(Point p, Point q, Point r)
        {
            if (q.X <= Math.Max(p.X, r.X) &&
                q.X >= Math.Min(p.X, r.X) &&
                q.Y <= Math.Max(p.Y, r.Y) &&
                q.Y >= Math.Min(p.Y, r.Y))
            {
                return true;
            }
            return false;
        }

        static int Orientation(Point p, Point q, Point r)
        {
            int val = (q.Y - p.Y) * (r.X - q.X) -
                      (q.X - p.X) * (r.Y - q.Y);

            if (val == 0)
            {
                return 0;
            }
            return (val > 0) ? 1 : 2;
        }

        private static Texture2D GetLineTexture(int length, int width)
        {
            var texture = new Texture2D(Game1.Instance.GraphicsDevice, width, length);
            Color[] colors = new Color[texture.Width * texture.Height];
            var borderWidth = 1;
            
            for (int x = 0; x < texture.Width; x++)
            {
                for (int y = 0; y < texture.Height; y++)
                {
                    bool colored = false;
                    for (int i = 0; i <= borderWidth; i++)
                    {
                        if (x == i || y == i || x == texture.Width - 1 - i || y == texture.Height - 1 - i)
                        {
                            colors[x + y * texture.Width] = Color.White;
                            colored = true;
                            break;
                        }
                    }

                    if (colored == false)
                        colors[x + y * texture.Width] = Color.Transparent;
                }
            }

            texture.SetData(colors);
            return texture;
        }
    }
}
