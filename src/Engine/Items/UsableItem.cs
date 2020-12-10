namespace Engine.Items
{
    using Engine.Objects;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Defines the <see cref="UsableItem" />
    /// </summary>
    public abstract class UsableItem : Item
    {
        /// <summary>
        /// Defines the maxTimes
        /// </summary>
        public int maxTimes;

        /// <summary>
        /// Defines the usedTimes
        /// </summary>
        public int usedTimes;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsableItem"/> class.
        /// </summary>
        /// <param name="name">The name<see cref="string"/></param>
        /// <param name="texture">The texture<see cref="Texture2D"/></param>
        /// <param name="type">The type<see cref="string"/></param>
        /// <param name="isStackable">The isStackable<see cref="bool"/></param>
        /// <param name="count">The count<see cref="int"/></param>
        /// <param name="description">The description<see cref="string"/></param>
        /// <param name="value">The value<see cref="int"/></param>
        /// <param name="maxTimes">The maxTimes<see cref="int"/></param>
        /// <param name="usedTimes">The usedTimes<see cref="int"/></param>
        public UsableItem(string name, Texture2D texture, string type, bool isStackable, int count, string description, int value, int maxTimes, int usedTimes)
            : base(name, texture, type, isStackable, count, description, value)
        {
            this.maxTimes = maxTimes;
            this.usedTimes = usedTimes;
        }

        /// <summary>
        /// The OnUse
        /// </summary>
        /// <param name="stageObject">The stageObject<see cref="StageObject"/></param>
        public abstract void OnUse(StageObject stageObject);
    }
}
