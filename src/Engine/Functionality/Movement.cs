namespace Engine.Functionality
{
    using Engine.Interface;
    using Engine.Objects;
    using Microsoft.Xna.Framework;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="Movement" />
    /// </summary>
    public class Movement
    {
        /// <summary>
        /// Defines the Algorithm
        /// </summary>
        public enum Algorithm
        { /// <summary>
          /// Defines the A_star
          /// </summary>
            A_star,
            /// <summary>
            /// Defines the Dijkstra
            /// </summary>
            Dijkstra
        }

        /// <summary>
        /// Returns the vector between and stageobject and a vector2 position
        /// </summary>
        /// <param name="obj">The obj<see cref="StageObject"/></param>
        /// <param name="position">The position<see cref="Vector2"/></param>
        /// <returns>The <see cref="Vector2"/></returns>
        public static Vector2 ToPosition(StageObject obj, Vector2 position)
        {
            return position - obj.Position;
        }

        /// <summary>
        /// Returns vector from object to object
        /// </summary>
        /// <param name="from">The from object<see cref="StageObject"/></param>
        /// <param name="to">The to object<see cref="StageObject"/></param>
        /// <returns>The <see cref="Vector2"/></returns>
        public static Vector2 ToObject(StageObject from, StageObject to)
        {
            return to.Position - from.Position;
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="alg">The alg<see cref="Algorithm"/></param>
        /// <returns>The <see cref="List{Vector2}"/></returns>
        public static List<Vector2> FindPath(Algorithm alg)
        {
            return null;
        }

        /// <summary>
        /// Makes object move around a given center
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="moveObject">The moveObject<see cref="T"/></param>
        /// <param name="clockwise">The clockwise<see cref="bool"/></param>
        /// <param name="center">The center<see cref="Vector2"/></param>
        public static void CircleMovement<T>(T moveObject, bool clockwise, Vector2 center) where T : StageObject, IMoving
        {
            Vector2 direction = -ToPosition(moveObject, center);
            double radians = Geometry.AngleOfVector(direction);
            float radius = (moveObject.Position - center).Length();
            float radialMovement = moveObject.Speed / radius;
            if (clockwise)
            {
                radians -= radialMovement;
            }
            else
            {
                radians += radialMovement;
            }

            if (radians > 2 * Math.PI)
            {
                radians %= 2 * Math.PI;
            }
            else
            {
                while (radians < 0)
                {
                    radians += 2 * Math.PI;
                }
            }

            Vector2 fromCenter = Geometry.AngleToVector(radians, radius);
            Vector2 newPosition = center + fromCenter;
            moveObject.direction = Geometry.UnitVector(newPosition - moveObject.Position);
            moveObject.Position = newPosition;
        }

        /// <summary>
        /// Defines the CollisionType
        /// </summary>
        public enum CollisionType
        { /// <summary>
          /// Defines the Rectangle
          /// </summary>
            Rectangle,
            /// <summary>
            /// Defines the Detailed
            /// </summary>
            Detailed
        }

        /// <summary>
        /// Pushes two objects away from each other
        /// </summary>
        /// <param name="moveObject">The moveObject<see cref="StageObject"/></param>
        /// <param name="collisionObject">The collisionObject<see cref="StageObject"/></param>
        /// <param name="intersections">The intersections<see cref="List{Rectangle}"/></param>
        /// <param name="weigth">The weigth<see cref="float"/></param>
        /// <param name="moveHitbox">The moveHitbox<see cref="string"/></param>
        /// <param name="collisionHitbox">The collisionHitbox<see cref="string"/></param>
        /// <param name="type">The type<see cref="CollisionType"/></param>
        public static void CollisionSeparation(StageObject moveObject, StageObject collisionObject, List<Rectangle> intersections, float weigth, string moveHitbox = "", string collisionHitbox = "",
            CollisionType type = CollisionType.Detailed)
        {
            Vector2 moveCenter;
            Vector2 collisionCenter;
            if (moveHitbox == "")
            {
                moveCenter = moveObject.CenterPosition();
            }
            else
            {
                moveCenter = moveObject.Hitbox(moveHitbox).center;
            }

            if (collisionHitbox == "")
            {
                collisionCenter = collisionObject.CenterPosition();
            }
            else
            {
                collisionCenter = collisionObject.Hitbox(collisionHitbox).center;
            }

            float maxX = 0;
            float maxY = 0;

            if (type == CollisionType.Detailed)
            {

                intersections.ForEach(r =>
                {
                    if (r.Width < r.Height)
                    {
                        maxX = Math.Max(maxX, r.Width);
                    }
                    else
                    {
                        maxY = Math.Max(maxY, r.Height);
                    }
                });
                maxX *= weigth;
                maxY *= weigth;

                Vector2 movePosition = new Vector2();
                if (moveCenter.X > collisionCenter.X)
                {
                    movePosition.X += maxX;
                }
                else
                {
                    movePosition.X -= maxX;
                }

                if (moveCenter.Y > collisionCenter.Y)
                {
                    movePosition.Y += maxY;
                }
                else
                {
                    movePosition.Y -= maxY;
                }

                moveObject.Position += movePosition;
            }
            else if (type == CollisionType.Rectangle)
            {
                intersections.ForEach(r =>
                {
                    maxX = Math.Max(maxX, r.Width);
                    maxY = Math.Max(maxY, r.Height);
                });
                maxX *= weigth;
                maxY *= weigth;

                Geometry.Direction direction = Geometry.MajorityDirectionVerticalOrHorizontal(moveCenter - collisionCenter);
                Vector2 movePosition = new Vector2();
                switch (direction)
                {
                    case Geometry.Direction.East:
                        movePosition.X = maxX;
                        break;
                    case Geometry.Direction.West:
                        movePosition.X = -maxX;
                        break;
                    case Geometry.Direction.North:
                        movePosition.Y = -maxY;
                        break;
                    case Geometry.Direction.South:
                        movePosition.Y = maxY;
                        break;
                }
                moveObject.Position += movePosition;
            }
        }

        /// <summary>
        /// Defines the FollowPathType
        /// </summary>
        public enum FollowPathType
        { /// <summary>
          /// Defines the None
          /// </summary>
            None,
            /// <summary>
            /// Defines the Loop
            /// </summary>
            Loop,
            /// <summary>
            /// Defines the Patrol
            /// </summary>
            Patrol
        };

        /// <summary>
        /// Makes a stage object follow its defined path
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="moveObject">The moveObject<see cref="T"/></param>
        /// <param name="type">The type<see cref="FollowPathType"/></param>
        public static void FollowPath<T>(T moveObject, FollowPathType type) where T : StageObject, IMoving
        {
            float startPosition = moveObject.LengthOfPathMoved;
            List<Vector2> path = moveObject.CurrentPath;
            int vectorToMove = -1;
            for (int i = 0; i < path.Count; i++)
            {
                if (path[i].Length() <= startPosition)
                {
                    startPosition -= path[i].Length();
                }
                else
                {
                    vectorToMove = i;
                    break;
                }
            }
            if (vectorToMove == -1)
            {
                return;
            }

            float restOfVector = path[vectorToMove].Length() - startPosition;
            float leftToMove = moveObject.Speed;
            moveObject.LengthOfPathMoved += leftToMove;
            while (leftToMove >= restOfVector)
            {
                moveObject.Position += Geometry.UnitVector(path[vectorToMove]) * restOfVector;
                leftToMove -= restOfVector;
                if (++vectorToMove >= path.Count)
                {
                    if (type == FollowPathType.None)
                    {
                        return;
                    }
                    else
                    {
                        vectorToMove = 0;
                        moveObject.LengthOfPathMoved = leftToMove;
                        if (type == FollowPathType.Patrol)
                        {
                            ReversePath(moveObject.CurrentPath);
                        }

                        restOfVector = path[vectorToMove].Length();
                    }
                }
            }
            Vector2 objectMovement = Geometry.UnitVector(path[vectorToMove]) * leftToMove;
            moveObject.direction = Geometry.UnitVector(objectMovement);
            moveObject.Position += objectMovement;
        }

        /// <summary>
        /// Returns lenght of path
        /// </summary>
        /// <param name="path">The path<see cref="List{Vector2}"/></param>
        /// <returns>The <see cref="float"/></returns>
        public static float LengthOfPath(List<Vector2> path)
        {
            float length = 0;
            path.ForEach(p => length += p.Length());
            return length;
        }

        /// <summary>
        /// Reverses the given path
        /// </summary>
        /// <param name="path">The path<see cref="List{Vector2}"/></param>
        public static void ReversePath(List<Vector2> path)
        {
            path.Reverse();
            for (int i = 0; i < path.Count; i++)
            {
                path[i] *= -1;
            }
        }

        /// <summary>
        /// Calculates where object has been, and where it is going
        /// </summary>
        /// <returns>The <see cref="Vector2"/></returns>
        public static Vector2 FuturePositionCalc()
        {
            //var t = Geometry.DistanceBetweenObjects(this, player, Geometry.DistanceCheckType.Center) / ShotSpeed;
            //var futurePos = player.position + player.CurrentSpeed * t;
            //var vectorToFuturePlayer = Movement.ToPosition(this, futurePos);
            // UNDER CONSTRUCTION!
            return new Vector2();
        }
    }
}
