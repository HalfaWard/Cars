namespace Engine.Functionality
{
    using Engine.Objects;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="ObjectHandler" />
    /// </summary>
    public class ObjectHandler
    {
        /// <summary>
        /// Defines the objectLists
        /// </summary>
        private Dictionary<string, List<StageObject>> objectLists = new Dictionary<string, List<StageObject>>();

        /// <summary>
        /// Defines the collisionHandlers
        /// </summary>
        private List<CollisionHandler> collisionHandlers = new List<CollisionHandler>();

        /// <summary>
        /// Defines the updates
        /// </summary>
        private List<string> updates = new List<string>();

        /// <summary>
        /// Defines the screenSquare
        /// </summary>
        private Rectangle screenSquare;

        /// <summary>
        /// Gets or sets the Camera. Used along with screen size to avoid unnecessary drawing.
        /// </summary>
        public Camera Camera { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectHandler"/> class.
        /// </summary>
        /// <param name="drawScreenWidth">The drawScreenWidth<see cref="int"/></param>
        /// <param name="drawScreenHeight">The drawScreenHeight<see cref="int"/></param>
        public ObjectHandler(int drawScreenWidth, int drawScreenHeight)
        {
            screenSquare = new Rectangle(0, 0, drawScreenWidth, drawScreenHeight);
        }

        /// <summary>
        /// Adds an object to a list
        /// </summary>
        /// <param name="listName">The listName<see cref="string"/></param>
        /// <param name="stageObject">The stageObject<see cref="StageObject"/></param>
        public void AddObjectToList(string listName, StageObject stageObject)
        {
            if (!objectLists.ContainsKey(listName))
            {
                objectLists.Add(listName, new List<StageObject>());
            }

            objectLists[listName].Add(stageObject);
        }

        /// <summary>
        /// Adds a list of objects to a list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listName">The listName<see cref="string"/></param>
        /// <param name="stageObject">The stageObject<see cref="IEnumerable{T}"/></param>
        public void AddObjectToList<T>(string listName, IEnumerable<T> stageObject) where T : StageObject
        {
            if (!objectLists.ContainsKey(listName))
            {
                objectLists.Add(listName, new List<StageObject>());
            }

            objectLists[listName].AddRange(stageObject);
        }

        /// <summary>
        /// Creates an empty list
        /// </summary>
        /// <param name="listName">The listName<see cref="string"/></param>
        public void AddEmptyObjectList(string listName)
        {
            if (!objectLists.ContainsKey(listName))
            {
                objectLists.Add(listName, new List<StageObject>());
            }
        }

        /// <summary>
        /// Removes an object from a spesific list
        /// </summary>
        /// <param name="listName">The listName<see cref="string"/></param>
        /// <param name="stageObject">The stageObject<see cref="StageObject"/></param>
        public void RemoveObjectFromList(string listName, StageObject stageObject)
        {
            if (objectLists.ContainsKey(listName))
            {
                objectLists[listName].Remove(stageObject);
            }
        }

        /// <summary>
        /// Removes a list of objects from a spesific list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listName">The listName<see cref="string"/></param>
        /// <param name="stageObject">The stageObject<see cref="IEnumerable{T}"/></param>
        public void RemoveObjectFromList<T>(string listName, IEnumerable<T> stageObject) where T : StageObject
        {
            if (objectLists.ContainsKey(listName))
            {
                foreach (T obj in stageObject)
                {
                    objectLists[listName].Remove(obj);
                }
            }
        }

        /// <summary>
        /// Gets the list with the parameter name, or null if it does not exist
        /// </summary>
        /// <param name="listName">The listName<see cref="string"/></param>
        /// <returns>The <see cref="List{StageObject}"/></returns>
        public List<StageObject> GetObjectList(string listName)
        {
            if (objectLists.ContainsKey(listName))
            {
                return objectLists[listName];
            }

            return null;
        }

        /// <summary>
        /// Clears a list of objects
        /// </summary>
        /// <param name="listName">The listName<see cref="string"/></param>
        public void ClearList(string listName)
        {
            if (objectLists.ContainsKey(listName))
            {
                objectLists[listName].Clear();
            }
        }

        /// <summary>
        /// Clears all lists of objects
        /// </summary>
        public void ClearAllLists()
        {
            objectLists.Clear();
        }

        /// <summary>
        /// Clears the handler object
        /// </summary>
        public void Clear()
        {
            objectLists.Clear();
            collisionHandlers.Clear();
            updates.Clear();
        }

        /// <summary>
        /// Adds a collision check between two object lists
        /// </summary>
        /// <param name="objectListName">The objectListName<see cref="string"/></param>
        /// <param name="targetListName">The targetListName<see cref="string"/></param>
        /// <param name="objectHitbox">The objectHitbox<see cref="string"/></param>
        /// <param name="targetHitbox">The targetHitbox<see cref="string"/></param>
        /// <param name="onlyIfMoving">The onlyIfMoving<see cref="bool"/></param>
        /// <param name="collisionChecker">The collisionChecker<see cref="Action{string, string, string, string}"/></param>
        public void AddCollisionHandler(string objectListName, string targetListName, string objectHitbox = "", string targetHitbox = "", bool onlyIfMoving = false, Func<StageObject, StageObject, bool> collisionChecker = null)
        {
            collisionHandlers.Add(new CollisionHandler(objectListName, objectHitbox, targetListName, targetHitbox, objectLists, onlyIfMoving, collisionChecker));
        }

        /// <summary>
        /// Removes a collision check
        /// </summary>
        /// <param name="objectListName">The objectListName<see cref="string"/></param>
        /// <param name="targetListName">The targetListName<see cref="string"/></param>
        public void RemoveCollisionHandler(string objectListName, string targetListName)
        {
            collisionHandlers.RemoveAll(handler => handler.Contains(objectListName, targetListName));
        }

        /// <summary>
        /// Iterates through collision checks
        /// </summary>
        public void CheckCollisions()
        {
            collisionHandlers.ForEach(handler =>
            {
                handler.CheckCollision();
            });
        }

        /// <summary>
        /// Adds a list to be updated
        /// </summary>
        /// <param name="listName">The listName<see cref="string"/></param>
        public void AddObjectToUpdate(string listName)
        {
            updates.Add(listName);
        }

        /// <summary>
        /// Adds several lists to be updated
        /// </summary>
        /// <param name="listName">The listName<see cref="IEnumerable{string}"/></param>
        public void AddObjectToUpdate(IEnumerable<string> listName)
        {
            updates.AddRange(listName);
        }

        /// <summary>
        /// Updates all lists set to be updated
        /// </summary>
        /// <param name="gameTime">The gameTime<see cref="GameTimeHolder"/></param>
        public void Update(GameTimeHolder gameTime)
        {
            updates.ForEach(u =>
            {
                if (objectLists.ContainsKey(u))
                {
                    List<StageObject> list = objectLists[u];
                    for (int i = 0; i < list.Count;)
                    {
                        if (!list[i].IsAlive)
                        {
                            list.RemoveAt(i);
                            continue;
                        }
                        list[i].Update(gameTime);

                        if (!list[i].IsAlive)
                        {
                            list.RemoveAt(i);
                            continue;
                        }
                        i++;
                    }
                }
            });
        }

        /// <summary>
        /// Draws all objects inside the screen
        /// </summary>
        /// <param name="spriteBatch">The spriteBatch<see cref="SpriteBatch"/></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (Camera != null)
            {
                screenSquare = new Rectangle((int)Camera.cameraPosition.X - (Camera.screenWidth / 2), (int)Camera.cameraPosition.Y - (Camera.screenHeight / 2), Camera.screenWidth, Camera.screenHeight);
            }

            foreach (string key in objectLists.Keys)
            {
                objectLists[key].ForEach(stageObject =>
                {
                    if (stageObject.Rectangle().Intersects(screenSquare))
                    {
                        stageObject.Draw(spriteBatch);
                    }
                });
            }
        }

        /// <summary>
        /// Defines the <see cref="CollisionHandler" />
        /// </summary>
        private class CollisionHandler
        {
            /// <summary>
            /// Defines the objectLists
            /// </summary>
            private Dictionary<string, List<StageObject>> objectLists;

            /// <summary>
            /// Defines the objectsToCheck
            /// </summary>
            private readonly string objectsToCheck;

            /// <summary>
            /// Defines the objectHitbox
            /// </summary>
            private readonly string objectHitbox;

            /// <summary>
            /// Defines the collisionTargets
            /// </summary>
            private readonly string collisionTargets;

            /// <summary>
            /// Defines the targetHitbox
            /// </summary>
            private readonly string targetHitbox;

            //Implement moving check
            /// <summary>
            /// Defines the onlyIfMoving
            /// </summary>
            internal bool onlyIfMoving;

            /// <summary>
            /// Defines the collisionChecker
            /// </summary>
            private readonly Func<StageObject, StageObject, bool> collisionChecker;

            /// <summary>
            /// Initializes a new instance of the <see cref="CollisionHandler"/> class.
            /// </summary>
            /// <param name="objects">The objects<see cref="string"/></param>
            /// <param name="hitbox">The hitbox<see cref="string"/></param>
            /// <param name="targets">The targets<see cref="string"/></param>
            /// <param name="targetHitbox">The targetHitbox<see cref="string"/></param>
            /// <param name="objectLists">The objectLists<see cref="Dictionary{string, List{StageObject}}"/></param>
            /// <param name="onlyIfMoving">The onlyIfMoving<see cref="bool"/></param>
            /// <param name="collisionChecker">The collisionChecker<see cref="Action{string, string, string, string}"/></param>
            public CollisionHandler(string objects, string hitbox, string targets, string targetHitbox, Dictionary<string, List<StageObject>> objectLists, bool onlyIfMoving, Func<StageObject, StageObject, bool> collisionChecker)
            {
                objectsToCheck = objects;
                objectHitbox = hitbox;
                collisionTargets = targets;
                this.targetHitbox = targetHitbox;
                this.onlyIfMoving = onlyIfMoving;
                this.collisionChecker = collisionChecker;
                this.objectLists = objectLists;
            }

            /// <summary>
            /// The CheckCollision
            /// </summary>
            public void CheckCollision()
            {
                
                if (!objectLists.ContainsKey(objectsToCheck) || !objectLists.ContainsKey(collisionTargets))
                {
                    return;
                }

                List<StageObject> o = objectLists[objectsToCheck];
                List<StageObject> t = objectLists[collisionTargets];
                for (int i = 0; i < o.Count;)
                {
                    if (!o[i].IsAlive)
                    {
                        o.RemoveAt(i);
                        continue;
                    }
                    for (int j = 0; j < t.Count;)
                    {
                        if (o[i] == t[j])
                        {
                            j++;
                            continue;
                        }
                        if (!t[j].IsAlive)
                        {
                            t.RemoveAt(j);
                            continue;
                        }

                        bool collided = false;
                        List<Rectangle> rectangles = null;
                        if (collisionChecker != null)
                        {
                            collided = collisionChecker(o[i], t[j]);
                        }
                        else
                        {
                            rectangles = o[i].IntersectRectangles(t[j], objectHitbox, targetHitbox);
                            collided = rectangles.Count > 0;
                        }
                        if (collided)
                        {
                            //TODO
                            o[i].OnCollison(t[j], rectangles, objectHitbox, targetHitbox);
                            o[i].isColliding = true;
                            t[j].isColliding = true;
                        }
                        else
                        {
                            if (o[i].isColliding)
                            {
                                //o[i].OnLeaveCollision();
                            }
                            o[i].isColliding = false;
                        }
                        if (!t[j].IsAlive)
                        {
                            t.RemoveAt(j);
                        }
                        else
                        {
                            j++;
                        }

                        if (!o[i].IsAlive)
                        {
                            break;
                        }
                    }
                    if (!o[i].IsAlive)
                    {
                        o.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }
                
            }

            /// <summary>
            /// The Contains
            /// </summary>
            /// <param name="listName">The listName<see cref="string"/></param>
            /// <returns>The <see cref="bool"/></returns>
            public bool Contains(string listName)
            {
                return objectsToCheck == listName || collisionTargets == listName;
            }

            /// <summary>
            /// The Contains
            /// </summary>
            /// <param name="objectsList">The objectsList<see cref="string"/></param>
            /// <param name="targetList">The targetList<see cref="string"/></param>
            /// <returns>The <see cref="bool"/></returns>
            public bool Contains(string objectsList, string targetList)
            {
                return objectsToCheck == objectsList && collisionTargets == targetList;
            }
        }
    }
}
