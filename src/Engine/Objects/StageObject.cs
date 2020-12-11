namespace Engine.Objects
{
    using Engine.Functionality;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="StageObject" />
    /// </summary>
    public abstract class StageObject
    {
        /// <summary>
        /// Defines the texture
        /// </summary>
        private Texture2D texture;

        /// <summary>
        /// Gets or sets the Texture
        /// </summary>
        public Texture2D Texture
        {
            get => texture;
            set
            {
                texture = value;
                if (texture != null)
                {
                    height = texture.Height;
                    width = texture.Width;
                }
            }
        }

        /// <summary>
        /// Defines the hitbox
        /// </summary>
        protected Dictionary<string, Hitbox> hitbox;

        /// <summary>
        /// Gets or sets the position
        /// </summary>
        private Vector2 position { get; set; }

        /// <summary>
        /// Gets or sets the Position
        /// </summary>
        public virtual Vector2 Position
        {
            get => position;
            set => position = value;
        }

        /// <summary>
        /// Defines the direction
        /// </summary>
        public Vector2 direction;

        /// <summary>
        /// Defines the origin
        /// </summary>
        public Vector2 origin;

        /// <summary>
        /// Defines the width
        /// </summary>
        public int width = 0;

        /// <summary>
        /// Defines the height
        /// </summary>
        public int height = 0;

        /// <summary>
        /// Defines the scale
        /// </summary>
        public Vector2 scale = new Vector2(1, 1);

        /// <summary>
        /// Defines the filter
        /// </summary>
        public Color filter = Color.White;

        /// <summary>
        /// Defines the makeHitboxStatic
        /// </summary>
        public static bool makeHitboxStatic = false;

        /// <summary>
        /// Defines the isColliding
        /// </summary>
        public bool isColliding = false;

        /// <summary>
        /// Gets or sets the Rotation
        /// </summary>
        public virtual float Rotation { get; set; } = 0;

        /// <summary>
        /// Gets or sets a value indicating whether IsAlive
        /// </summary>
        public bool IsAlive { get; set; } = true;

        /// <summary>
        /// Gets or sets the Width
        /// </summary>
        public int Width { get => (int)(width * scale.X); set => width = value; }

        /// <summary>
        /// Gets or sets the Height
        /// </summary>
        public int Height { get => (int)(height * scale.Y); set => height = value; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StageObject"/> class.
        /// </summary>
        /// <param name="texture">The texture<see cref="Texture2D"/></param>
        /// <param name="hitbox">The hitbox<see cref="Dictionary{string, Hitbox}"/></param>
        /// <param name="position">The position<see cref="Vector2"/></param>
        /// <param name="direction">The direction<see cref="Vector2"/></param>
        /// <param name="origin">The origin<see cref="Vector2"/></param>
        protected StageObject(Texture2D texture, Dictionary<string, Hitbox> hitbox, Vector2 position, Vector2 direction, Vector2 origin = new Vector2())
        {
            Texture = texture;
            this.hitbox = hitbox;
            this.position = position;
            this.direction = direction;
            this.origin = origin;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StageObject"/> class.
        /// </summary>
        /// <param name="texture">The texture<see cref="Texture2D"/></param>
        /// <param name="hitbox">The hitbox<see cref="Dictionary{string, Hitbox}"/></param>
        protected StageObject(Texture2D texture, Dictionary<string, Hitbox> hitbox)
        {
            Texture = texture;
            this.hitbox = hitbox;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StageObject"/> class.
        /// </summary>
        /// <param name="hitbox">The hitbox<see cref="Dictionary{string, Hitbox}"/></param>
        protected StageObject(Dictionary<string, Hitbox> hitbox)
        {
            this.hitbox = hitbox;
        }

        /// <summary>
        /// Calculates the scaled rectangle of the object
        /// </summary>
        /// <returns>The <see cref="Rectangle"/></returns>
        public Rectangle Rectangle()
        {

            return new Rectangle((int)(position.X - (origin.X * scale.X)), (int)(position.Y - origin.Y * scale.Y), Width, Height);
        }

        /// <summary>
        /// Calculates the scaled center position of the object
        /// </summary>
        /// <returns>The <see cref="Vector2"/></returns>
        public Vector2 CenterPosition()
        {
            return position - (origin * scale) + (new Vector2(Width, Height) / 2);
        }

        /// <summary>
        /// Finds the unscaled center origin of the object
        /// </summary>
        /// <returns>The <see cref="Vector2"/></returns>
        public Vector2 CenterOrigin()
        {
            return new Vector2(width, height) / 2;
        }

        /// <summary>
        /// Returns all hitboxes
        /// </summary>
        /// <returns>The <see cref="List{Hitbox}"/></returns>
        public List<Hitbox> CombinedHitbox()
        {
            List<Hitbox> combined = new List<Hitbox>();
            foreach (string key in hitbox.Keys)
            {
                combined.Add(Hitbox(key));
            }
            return combined;
        }

        /// <summary>
        /// Returns all hitboxes based on the names
        /// </summary>
        /// <param name="hitboxNames">The hitboxNames<see cref="string[]"/></param>
        /// <returns>The <see cref="List{Hitbox}"/></returns>
        public List<Hitbox> CombinedHitbox(params string[] hitboxNames)
        {
            List<Hitbox> combined = new List<Hitbox>();
            hitboxNames.ToList().ForEach(name =>
            {
                if (hitbox.ContainsKey(name))
                {
                    combined.Add(Hitbox(name));
                }
            });
            return combined;
        }

        /// <summary>
        /// Returns a hitbox
        /// </summary>
        /// <param name="hitboxName">The hitboxName<see cref="string"/></param>
        /// <returns>The <see cref="Hitbox"/></returns>
        public Hitbox Hitbox(string hitboxName)
        {
            List<Rectangle> rectangles = new List<Rectangle>();
            if (!hitbox.ContainsKey(hitboxName))
            {
                return new Hitbox(rectangles, new Vector2());
            }

            hitbox[hitboxName].rectangles.ForEach(h =>
            {
                rectangles.Add(new Rectangle((int)(position.X + (h.Left - origin.X) * scale.X), (int)(position.Y + (h.Top - origin.Y) * scale.Y), (int)(h.Width * scale.X), (int)(h.Height * scale.Y)));
            });
            return new Hitbox(rectangles, position + (hitbox[hitboxName].center - origin) * scale);
        }

        /// <summary>
        /// Checks for intersections of this and another object
        /// </summary>
        /// <param name="target">The target<see cref="StageObject"/></param>
        /// <param name="hitboxName">The hitboxName<see cref="string"/></param>
        /// <param name="targetHitboxName">The targetHitboxName<see cref="string"/></param>
        /// <returns>The <see cref="List{Rectangle}"/></returns>
        public List<Rectangle> IntersectRectangles(StageObject target, string hitboxName = "", string targetHitboxName = "")
        {
            List<Rectangle> ownHitbox = new List<Rectangle>();
            List<Rectangle> targetHitbox = new List<Rectangle>();
            if (hitboxName == "")
            {
                CombinedHitbox().ForEach(h =>
                {
                    ownHitbox.AddRange(h.rectangles);
                });
            }
            else
            {
                ownHitbox = Hitbox(hitboxName).rectangles;
            }

            if (targetHitboxName == "")
            {
                target.CombinedHitbox().ForEach(h =>
                {
                    targetHitbox.AddRange(h.rectangles);
                });
            }
            else
            {
                targetHitbox = target.Hitbox(targetHitboxName).rectangles;
            }

            List<Rectangle> rectangles = new List<Rectangle>();
            Rectangle rect = Rectangle();
            Rectangle targetRect = target.Rectangle();
            if (Rectangle().Intersects(target.Rectangle()))
            {
                for (int a = 0; a < ownHitbox.Count; a++)
                {
                    Rectangle r1 = ownHitbox[a];
                    for (int b = 0; b < targetHitbox.Count; b++)
                    {
                        Rectangle r2 = targetHitbox[b];
                        if (r1.Intersects(r2))
                        {
                            rectangles.Add(Geometry.IntersectingRectangle(r1, r2));
                        }
                    }
                }
            }
            return rectangles;
        }

        public bool IntersectRectangles(StageObject target, Func<StageObject, StageObject, bool> collisionChecker)
        {
            if (Rectangle().Intersects(target.Rectangle()))
            {
                return collisionChecker(this, target);
            }
            return false;
        }

        /// <summary>
        /// Returns a dictionary with a hitbox named "default", based on the texture size
        /// </summary>
        /// <param name="texture">The texture<see cref="Texture2D"/></param>
        /// <returns>The <see cref="Dictionary{string, Hitbox}"/></returns>
        public static Dictionary<string, Hitbox> LoadDefaultHitbox(Texture2D texture)
        {
            return new Dictionary<string, Hitbox>()
            {
                {"default", new Hitbox(new List<Rectangle>()
                    {
                        new Rectangle(0,0,texture.Width, texture.Height)
                    },
                    new Vector2(texture.Width, texture.Height)*0.5f)
                }
            };
        }

        /// <summary>
        /// Defines the FilePath
        /// </summary>
        private static readonly string FilePath = "../../../../GameFiles/HitboxFiles/";// for debug

        //private static string FilePath = "./Content/HitboxFiles/"; // for release
        /// <summary>
        /// Loads hitbox from a file (to be removed)
        /// </summary>
        /// <param name="filePath">The filePath<see cref="string"/></param>
        /// <returns>The <see cref="Dictionary{string, Hitbox}"/></returns>
        public static Dictionary<string, Hitbox> LoadHitbox(string filePath)
        {
            string[] input = File.ReadAllLines(FilePath + filePath);
            Dictionary<string, Hitbox> hitboxes = new Dictionary<string, Hitbox>();
            foreach (string line in input)
            {
                if (line.Length <= 0)
                {
                    continue;
                }

                string[] data = line.Split(';');
                Hitbox hitbox = new Hitbox();
                for (int i = 1; i < data.Length - 1; i++)
                {
                    int[] square = Array.ConvertAll(data[i].Split(','), int.Parse);
                    hitbox.rectangles.Add(new Rectangle(square[0], square[1], square[2], square[3]));
                }
                int[] centerCoords = Array.ConvertAll(data[data.Length - 1].Split(','), int.Parse);
                hitbox.center = new Vector2(centerCoords[0], centerCoords[1]);
                hitboxes.Add(data[0], hitbox);

            }
            return hitboxes;
        }

        /// <summary>
        /// Run when two objects collides
        /// </summary>
        /// <param name="collisionTarget">The collisionTarget<see cref="StageObject"/></param>
        /// <param name="intersections">The intersections<see cref="List{Rectangle}"/></param>
        /// <param name="hitboxName">The hitboxName<see cref="string"/></param>
        /// <param name="targetHitboxName">The targetHitboxName<see cref="string"/></param>
        public abstract void OnCollison(StageObject collisionTarget, List<Rectangle> intersections, string hitboxName, string targetHitboxName);

        /// <summary>
        /// Updates the object
        /// </summary>
        /// <param name="gameTime">The gameTime<see cref="GameTime"/></param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// Updates the object
        /// </summary>
        /// <param name="gameTime">The gameTime<see cref="GameTimeHolder"/></param>
        public abstract void Update(GameTimeHolder gameTime);

        /// <summary>
        /// Draws the object
        /// </summary>
        /// <param name="spriteBatch">The spriteBatch<see cref="SpriteBatch"/></param>
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
