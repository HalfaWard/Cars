namespace Engine.Overlay
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Defines the <see cref="Overlay" />
    /// </summary>
    public class Overlay
    {
        /// <summary>
        /// Gets the Name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets or sets the Position
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Gets or sets the DestinationRectangle
        /// </summary>
        public Rectangle DestinationRectangle { get; set; }

        /// <summary>
        /// Gets or sets the Origin
        /// </summary>
        public Vector2 Origin { get; set; }

        /// <summary>
        /// Gets or sets the Filter
        /// </summary>
        public Color Filter { get; set; } = Color.White;

        /// <summary>
        /// Gets or sets the Texture
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        /// Gets or sets the Font
        /// </summary>
        public SpriteFont Font { get; set; }

        /// <summary>
        /// Gets or sets the Text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the Opacity
        /// </summary>
        public int Opacity { get; set; }

        /// <summary>
        /// Gets or sets the Rotation
        /// </summary>
        public float Rotation { get; set; } = 0;

        /// <summary>
        /// Gets or sets the Scale
        /// </summary>
        public Vector2 Scale { get; set; } = Vector2.One;

        /// <summary>
        /// Gets or sets a value indicating whether IsActive
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Overlay"/> class.
        /// </summary>
        /// <param name="name">The name<see cref="string"/></param>
        /// <param name="destinationRectangle">The destinationRectangle<see cref="Rectangle"/></param>
        /// <param name="texture">The texture<see cref="Texture2D"/></param>
        /// <param name="origin">The origin<see cref="Vector2"/></param>
        /// <param name="opacity">The opacity<see cref="int"/></param>
        public Overlay(string name, Rectangle destinationRectangle, Texture2D texture, Vector2 origin = new Vector2(), int opacity = 100) : this(name, default(Vector2), origin, opacity)
        {
            DestinationRectangle = destinationRectangle;
            Texture = texture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Overlay"/> class.
        /// </summary>
        /// <param name="name">The name<see cref="string"/></param>
        /// <param name="position">The position<see cref="Vector2"/></param>
        /// <param name="texture">The texture<see cref="Texture2D"/></param>
        /// <param name="origin">The origin<see cref="Vector2"/></param>
        /// <param name="opacity">The opacity<see cref="int"/></param>
        public Overlay(string name, Vector2 position, Texture2D texture, Vector2 origin = new Vector2(), int opacity = 100) : this(name, position, origin, opacity)
        {
            Texture = texture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Overlay"/> class.
        /// </summary>
        /// <param name="name">The name<see cref="string"/></param>
        /// <param name="position">The position<see cref="Vector2"/></param>
        /// <param name="texture">The texture<see cref="Texture2D"/></param>
        /// <param name="color">The color<see cref="Color"/></param>
        /// <param name="origin">The origin<see cref="Vector2"/></param>
        /// <param name="opacity">The opacity<see cref="int"/></param>
        public Overlay(string name, Vector2 position, Texture2D texture, Color color, Vector2 origin = new Vector2(), int opacity = 100) : this(name, position, origin, opacity)
        {
            Color[] data = new Color[texture.Width * texture.Height];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = color;
            }

            texture.SetData(data);
            Texture = texture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Overlay"/> class.
        /// </summary>
        /// <param name="name">The name<see cref="string"/></param>
        /// <param name="position">The position<see cref="Vector2"/></param>
        /// <param name="font">The font<see cref="SpriteFont"/></param>
        /// <param name="text">The text<see cref="string"/></param>
        /// <param name="origin">The origin<see cref="Vector2"/></param>
        /// <param name="opacity">The opacity<see cref="int"/></param>
        public Overlay(string name, Vector2 position, SpriteFont font, string text, Vector2 origin = new Vector2(), int opacity = 100) : this(name, position, origin, opacity)
        {
            Font = font;
            Text = text;
        }

        //Base for all overlays
        /// <summary>
        /// Prevents a default instance of the <see cref="Overlay"/> class from being created.
        /// </summary>
        /// <param name="name">The name<see cref="string"/></param>
        /// <param name="position">The position<see cref="Vector2"/></param>
        /// <param name="origin">The origin<see cref="Vector2"/></param>
        /// <param name="opacity">The opacity<see cref="int"/></param>
        /// <param name="isActive">The isActive<see cref="bool"/></param>
        private Overlay(string name, Vector2 position, Vector2 origin, int opacity, bool isActive = true)
        {
            IsActive = isActive;
            Name = name;
            Position = position;
            Origin = origin;
            Opacity = opacity;
        }

        /// <summary>
        /// Draws the overlay
        /// </summary>
        /// <param name="spriteBatch">The spriteBatch<see cref="SpriteBatch"/></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            Color filter = new Color(Filter, (float)Opacity / 100);

            if (DestinationRectangle.Width > 0 && DestinationRectangle.Height > 0)
            {
                spriteBatch.Draw(Texture, DestinationRectangle, null, filter, Rotation, Origin, SpriteEffects.None, 0f);
            }
            else if (Texture != null)
            {
                spriteBatch.Draw(Texture, Position, null, filter, Rotation, Origin, Scale, SpriteEffects.None, 0f);
            }
            if (Font != null)
            {
                spriteBatch.DrawString(Font, Text, Position, filter, Rotation, Origin, Scale, SpriteEffects.None, 0f);
            }
        }
    }
}
