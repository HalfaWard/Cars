namespace Engine.Functionality
{
    using Engine.Objects;
    using Microsoft.Xna.Framework;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="Geometry" />
    /// </summary>
    public class Geometry
    {
        /// <summary>
        /// Defines the Shapes
        /// </summary>
        public enum Shapes { }

        /// <summary>
        /// Defines the DistanceCheckType
        /// </summary>
        public enum DistanceCheckType
        { /// <summary>
          /// Defines the Rectangle
          /// </summary>
            Rectangle,
            /// <summary>
            /// Defines the Position
            /// </summary>
            Position,
            /// <summary>
            /// Defines the Center
            /// </summary>
            Center,
            /// <summary>
            /// Defines the Hitbox
            /// </summary>
            Hitbox
        }

        /// <summary>
        /// Returns the distance between two stage objects, based on the given DistanceCheckType
        /// </summary>
        /// <param name="o1">The first stage object<see cref="StageObject"/></param>
        /// <param name="o2">The second stage object<see cref="StageObject"/></param>
        /// <param name="type">The type<see cref="DistanceCheckType"/></param>
        /// <param name="o1Hitbox">Hitbox of first stage object
        /// <param name="o2Hitbox">Hitbox of the second stage object
        /// <returns>The <see cref="float"/></returns>
        public static float DistanceBetweenObjects(StageObject o1, StageObject o2, DistanceCheckType type, string o1Hitbox = "", string o2Hitbox = "")
        {
            return VectorBetweenObjects(o1, o2, type, o1Hitbox, o2Hitbox).Length();
        }

        /// <summary>
        /// Returns the Vector2 between two stage objects, based on the given DistanceCheckType
        /// </summary>
        /// <param name="from">The object to start from<see cref="StageObject"/></param>
        /// <param name="to">The object to go to<see cref="StageObject"/></param>
        /// <param name="type">The type<see cref="DistanceCheckType"/></param>
        /// <param name="fromHitbox">From hitbox<see cref="string"/></param>
        /// <param name="toHitbox">To hitbox<see cref="string"/></param>
        /// <returns>The <see cref="Vector2"/></returns>
        public static Vector2 VectorBetweenObjects(StageObject from, StageObject to, DistanceCheckType type, string fromHitbox = "", string toHitbox = "")
        {
            if (type == DistanceCheckType.Position)
            {
                return to.Position - from.Position;
            }

            if (type == DistanceCheckType.Center)
            {
                return to.CenterPosition() - from.CenterPosition();
            }

            if (type == DistanceCheckType.Hitbox)
            {
                Vector2 minLength = Vector2.Zero;

                List<Rectangle> hitboxFrom = new List<Rectangle>();
                if (fromHitbox == "")
                {
                    from.CombinedHitbox().ForEach(h =>
                    {
                        hitboxFrom.AddRange(h.rectangles);
                    });
                }
                else
                {
                    hitboxFrom = from.Hitbox(fromHitbox).rectangles;
                }

                List<Rectangle> hitboxTo = new List<Rectangle>();
                if (toHitbox == "")
                {
                    to.CombinedHitbox().ForEach(h =>
                    {
                        hitboxTo.AddRange(h.rectangles);
                    });
                }
                else
                {
                    hitboxTo = to.Hitbox(toHitbox).rectangles;
                }

                foreach (Rectangle r1 in hitboxFrom)
                {
                    foreach (Rectangle r2 in hitboxTo)
                    {
                        Vector2 distance = VectorBetweenRectangles(r1, r2);
                        if (distance.Length() == 0)
                        {
                            return distance;
                        }
                        else if (minLength.Length() == 0)
                        {
                            minLength = distance;
                        }
                        else
                        {
                            Vector2 min = ShortestVector(minLength, distance);
                            minLength = min;
                        }
                    }
                }
                return minLength;
            }
            else
            {
                return VectorBetweenRectangles(from.Rectangle(), to.Rectangle());
            }
        }

        /// <summary>
        /// Finds the shortest of two vectors
        /// </summary>
        /// <param name="v1">First vector<see cref="Vector2"/></param>
        /// <param name="v2">Second vector<see cref="Vector2"/></param>
        /// <returns>The <see cref="Vector2"/></returns>
        public static Vector2 ShortestVector(Vector2 v1, Vector2 v2)
        {
            if (v1.Length() < v2.Length())
            {
                return v1;
            }
            else
            {
                return v2;
            }
        }

        /// <summary>
        /// Finds the distance between two rectangles
        /// </summary>
        /// <param name="from">First rectangle<see cref="Rectangle"/></param>
        /// <param name="to">Second rectangle<see cref="Rectangle"/></param>
        /// <returns>The <see cref="float"/></returns>
        public static float DistanceBetweenRectangles(Rectangle from, Rectangle to)
        {
            return VectorBetweenRectangles(from, to).Length();
        }

        /// <summary>
        /// Finds the vector from a rectangle to another
        /// </summary>
        /// <param name="from">Rectangle to start from<see cref="Rectangle"/></param>
        /// <param name="to">Rectangle to go to<see cref="Rectangle"/></param>
        /// <returns>The <see cref="Vector2"/></returns>
        public static Vector2 VectorBetweenRectangles(Rectangle from, Rectangle to)
        {
            if (from.Intersects(to))
            {
                return Vector2.Zero;
            }

            Vector2 distance = new Vector2();
            if (from.Right < to.Left)
            {
                distance.X = to.Left - from.Right;
            }
            else if (to.Right < from.Left)
            {
                distance.X = to.Right - from.Left;
            }

            if (from.Bottom < to.Top)
            {
                distance.Y = to.Top - from.Bottom;
            }
            else if (to.Bottom < from.Top)
            {
                distance.Y = to.Bottom - from.Top;
            }

            return distance;
        }

        /// <summary>
        /// Checks a list of objects in a given radius around a given center, and returns list of stages objects inside it
        /// </summary>
        /// <param name="radius">The radius<see cref="float"/></param>
        /// <param name="center">The center<see cref="Vector2"/></param>
        /// <param name="objectsToSearch">The objects To Search<see cref="List{StageObject}"/></param>
        /// <param name="targetHitboxName">The target Hitbox Name<see cref="string"/></param>
        /// <returns>The <see cref="List{StageObject}"/></returns>
        public static List<StageObject> ObjectsInRadius(float radius, Vector2 center, List<StageObject> objectsToSearch, string targetHitboxName = "")
        {
            List<StageObject> objectsInRange = new List<StageObject>();
            objectsToSearch.ForEach(o =>
            {
                List<Hitbox> boxes = new List<Hitbox>();
                if (targetHitboxName == "")
                {
                    boxes = o.CombinedHitbox();
                }
                else
                {
                    boxes.Add(o.Hitbox(targetHitboxName));
                }

                boxes.ForEach(h =>
                {
                    h.rectangles.ForEach(r =>
                    {
                        float nearestX = center.X;
                        float nearestY = center.Y;

                        if (center.X < r.Left)
                        {
                            nearestX = r.Left;
                        }
                        else if (center.X > r.Right)
                        {
                            nearestX = r.Right;
                        }

                        if (center.Y < r.Top)
                        {
                            nearestY = r.Top;
                        }
                        else if (center.Y > r.Bottom)
                        {
                            nearestY = r.Bottom;
                        }

                        Vector2 nearestPoint = new Vector2(nearestX, nearestY);
                        if (Vector2.Distance(center, nearestPoint) <= radius)
                        {
                            objectsInRange.Add(o);
                        }
                    });
                });
            });
            return objectsInRange;
        }

        /// <summary>
        /// checks for the overlapping rectangle of two rectangles
        /// </summary>
        /// <param name="a">Rectangle a<see cref="Rectangle"/></param>
        /// <param name="b">Rectangle b<see cref="Rectangle"/></param>
        /// <returns>The <see cref="Rectangle"/></returns>
        public static Rectangle IntersectingRectangle(Rectangle a, Rectangle b)
        {
            if (!a.Intersects(b))
            {
                throw new ArgumentException("Rectangles do not intersect");
            }

            int left = Math.Max(a.Left, b.Left);
            int top = Math.Max(a.Top, b.Top);
            int width = Math.Min(a.Right, b.Right) - left;
            int height = Math.Min(a.Bottom, b.Bottom) - top;

            return new Rectangle(left, top, width, height);
        }

        /// <summary>
        /// Returns the radian angle
        /// </summary>
        /// <param name="from">Object to start from<see cref="StageObject"/></param>
        /// <param name="to">Object to go to<see cref="StageObject"/></param>
        /// <returns>The <see cref="double"/></returns>
        public static double AngleBetweenObjects(StageObject from, StageObject to)
        {
            Vector2 direction = Movement.ToObject(from, to);
            return AngleOfVector(direction);
        }

        /// <summary>
        /// Retruns the angle of the given vector in radian
        /// </summary>
        /// <param name="vector">The vector<see cref="Vector2"/></param>
        /// <returns>The <see cref="double"/></returns>
        public static double AngleOfVector(Vector2 vector)
        {
            double radians = Math.Acos(vector.X / vector.Length());
            if (vector.Y > 0)
            {
                radians = 2 * Math.PI - radians;
            }

            return radians;
        }

        public static Point[] GetRotatedRectangle(Vector2 rotationPoint, Rectangle rect, float angleRad)
        {
            var p1 = new Vector2(rect.Left, rect.Top);
            var p2 = new Vector2(rect.Left, rect.Bottom);
            var p3 = new Vector2(rect.Right, rect.Top);
            var p4 = new Vector2(rect.Right, rect.Bottom);

            var newP1 = rotationPoint + AngleToVector(AngleOfVector(p1 - rotationPoint) + angleRad, (p1 - rotationPoint).Length());
            var newP2 = rotationPoint + AngleToVector(AngleOfVector(p2 - rotationPoint) + angleRad, (p2 - rotationPoint).Length());
            var newP3 = rotationPoint + AngleToVector(AngleOfVector(p3 - rotationPoint) + angleRad, (p3 - rotationPoint).Length());
            var newP4 = rotationPoint + AngleToVector(AngleOfVector(p4 - rotationPoint) + angleRad, (p4 - rotationPoint).Length());

            return new Point[]
            {
                newP1.ToPoint(),
                newP2.ToPoint(),
                newP3.ToPoint(),
                newP4.ToPoint()
            };
        }

        /// <summary>
        /// Converts a radian angle to a vector
        /// </summary>
        /// <param name="radians">The radians<see cref="double"/></param>
        /// <param name="length">The length<see cref="float"/></param>
        /// <returns>The <see cref="Vector2"/></returns>
        public static Vector2 AngleToVector(double radians, float length)
        {
            float x = (float)Math.Cos(radians);
            float y = -(float)Math.Sin(radians);
            return new Vector2(x, y) * length;
        }

        /// <summary>
        /// Returns list of objects that has interescted the given hitbox and its boundaries
        /// </summary>
        /// <param name="objects">List of objects<see cref="List{StageObject}"/></param>
        /// <param name="hitbox">List internal hitboxes<see cref="List{Rectangle}"/></param>
        /// <param name="hitboxBounds">The hitboxBounds<see cref="Rectangle"/></param>
        /// <param name="targetHitboxName">The target Hitbox Name<see cref="string"/></param>
        /// <returns>The <see cref="List{StageObject}"/></returns>
        public static List<StageObject> ObjectsInHitbox(List<StageObject> objects, List<Rectangle> hitbox, Rectangle hitboxBounds, string targetHitboxName = "")
        {
            List<StageObject> objectsInHitbox = new List<StageObject>();
            objects.ForEach(o =>
            {
                if (hitboxBounds.Intersects(o.Rectangle()))
                {
                    List<Rectangle> targetHitbox = new List<Rectangle>();
                    if (targetHitboxName == "")
                    {
                        o.CombinedHitbox().ForEach(h =>
                        {
                            targetHitbox.AddRange(h.rectangles);
                        });
                    }
                    else
                    {
                        targetHitbox = o.Hitbox(targetHitboxName).rectangles;
                    }

                    for (int a = 0; a < hitbox.Count; a++)
                    {
                        Rectangle r1 = new Rectangle(hitboxBounds.Left + hitbox[a].Left, hitboxBounds.Top + hitbox[a].Top, hitbox[a].Width, hitbox[a].Height);
                        for (int b = 0; b < targetHitbox.Count; b++)
                        {
                            if (r1.Intersects(targetHitbox[b]))
                            {
                                objectsInHitbox.Add(o);
                            }
                        }
                    }
                }
            });
            return objectsInHitbox;
        }

        /// <summary>
        /// Converts degrees to radain
        /// </summary>
        /// <param name="degrees">The degrees<see cref="int"/></param>
        /// <returns>The <see cref="double"/></returns>
        public static double DegreesToRadians(int degrees)
        {
            return degrees * Math.PI / 180;
        }

        /// <summary>
        /// Converts radian to degrees
        /// </summary>
        /// <param name="radians">The radians<see cref="double"/></param>
        /// <returns>The <see cref="int"/></returns>
        public static int RadiansToDegrees(double radians)
        {
            return (int)Math.Round(radians * 180 / Math.PI, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// converts a vector to a unitvector
        /// </summary>
        /// <param name="vector">The vector<see cref="Vector2"/></param>
        /// <returns>The <see cref="Vector2"/></returns>
        public static Vector2 UnitVector(Vector2 vector)
        {
            if (vector.Length() != 0)
            {
                return vector / vector.Length();
            }

            return vector;
        }

        /// <summary>
        /// Defines the Direction
        /// </summary>
        public enum Direction
        { /// <summary>
          /// Defines the North
          /// </summary>
            North = 1,
            /// <summary>
            /// Defines the NorthEast
            /// </summary>
            NorthEast = 2,
            /// <summary>
            /// Defines the East
            /// </summary>
            East = 3,
            /// <summary>
            /// Defines the SouthEast
            /// </summary>
            SouthEast = 4,
            /// <summary>
            /// Defines the South
            /// </summary>
            South = -1,
            /// <summary>
            /// Defines the SouthWest
            /// </summary>
            SouthWest = -2,
            /// <summary>
            /// Defines the West
            /// </summary>
            West = -3,
            /// <summary>
            /// Defines the NorthWest
            /// </summary>
            NorthWest = -4
        }

        /// <summary>
        /// Checks the majority of direction based on horizontal and vertical lengths
        /// </summary>
        /// <param name="vector">The vector<see cref="Vector2"/></param>
        /// <returns>The <see cref="Direction"/></returns>
        public static Direction MajorityDirectionVerticalOrHorizontal(Vector2 vector)
        {
            if (vector.X > 0 && vector.X > Math.Abs(vector.Y))
            {
                return Direction.East;
            }
            else if (vector.X < 0 && Math.Abs(vector.X) > Math.Abs(vector.Y))
            {
                return Direction.West;
            }
            else if (vector.Y < 0)
            {
                return Direction.North;
            }
            else
            {
                return Direction.South;
            }
        }

        /// <summary>
        /// Returns the majority direction, based on a compass
        /// </summary>
        /// <param name="vector">The vector<see cref="Vector2"/></param>
        /// <returns>The <see cref="Direction"/></returns>
        public static Direction MajorityDirection(Vector2 vector)
        {
            int degree = RadiansToDegrees(AngleOfVector(vector));
            if (degree > 337.5 || degree < 22.5)
            {
                return Direction.East;
            }
            else if (degree > 22.5 && degree < 67.5)
            {
                return Direction.NorthEast;
            }
            else if (degree > 67.5 && degree < 112.5)
            {
                return Direction.North;
            }
            else if (degree > 112.5 && degree < 157.5)
            {
                return Direction.NorthWest;
            }
            else if (degree > 157.5 && degree < 202.5)
            {
                return Direction.West;
            }
            else if (degree > 202.5 && degree < 247.5)
            {
                return Direction.SouthWest;
            }
            else if (degree > 247.5 && degree < 292.5)
            {
                return Direction.South;
            }
            else
            {
                return Direction.SouthEast;
            }
        }
    }
}