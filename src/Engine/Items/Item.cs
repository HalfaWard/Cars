namespace Engine.Items
{
    using Engine.Objects;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Defines the <see cref="Item" />
    /// </summary>
    public abstract class Item
    {
        /// <summary>
        /// Defines the name
        /// </summary>
        public string name;

        /// <summary>
        /// Defines the texture
        /// </summary>
        public Texture2D texture;

        /// <summary>
        /// Defines the type
        /// </summary>
        public string type;

        /// <summary>
        /// Defines the value
        /// </summary>
        public int value;

        /// <summary>
        /// Defines the isStackable
        /// </summary>
        public bool isStackable;

        /// <summary>
        /// Defines the count
        /// </summary>
        public int count;

        /// <summary>
        /// Defines the description
        /// </summary>
        public string description;

        /// <summary>
        /// Initializes a new instance of the <see cref="Item"/> class.
        /// </summary>
        /// <param name="name">The name<see cref="string"/></param>
        /// <param name="texture">The texture<see cref="Texture2D"/></param>
        /// <param name="type">The type<see cref="string"/></param>
        /// <param name="isStackable">The isStackable<see cref="bool"/></param>
        /// <param name="count">The count<see cref="int"/></param>
        /// <param name="description">The description<see cref="string"/></param>
        /// <param name="value">The value<see cref="int"/></param>
        public Item(string name, Texture2D texture, string type, bool isStackable, int count, string description, int value)
        {
            this.name = name;
            this.texture = texture;
            this.type = type;
            this.isStackable = isStackable;
            this.count = count;
            this.description = description;
            this.value = value;
        }

        /// <summary>
        /// The OnPickUp
        /// </summary>
        /// <param name="stageObject">The stageObject<see cref="StageObject"/></param>
        public abstract void OnPickUp(StageObject stageObject);

        /// <summary>
        /// The OnDrop
        /// </summary>
        /// <param name="stageObject">The stageObject<see cref="StageObject"/></param>
        public abstract void OnDrop(StageObject stageObject);

        /// <summary>
        /// The Equals
        /// </summary>
        /// <param name="obj">The obj<see cref="object"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj is Item item)
            {
                if (item.name == name && item.type == type && item.value == value && item.isStackable == isStackable)
                {
                    return true;
                }
            }
            return base.Equals(obj);
        }

        /// <summary>
        /// The Clone
        /// </summary>
        /// <returns>The <see cref="object"/></returns>
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
