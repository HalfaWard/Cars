namespace Engine.Functionality
{
    using Engine.Objects;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Defines the <see cref="Camera" />
    /// </summary>
    public class Camera
    {
        /// <summary>
        /// Defines the cameraPosition
        /// </summary>
        internal Vector2 cameraPosition;

        /// <summary>
        /// Defines the screenWidth
        /// </summary>
        internal int screenWidth;

        /// <summary>
        /// Defines the screenHeight
        /// </summary>
        internal int screenHeight;

        /// <summary>
        /// Gets the Transform
        /// </summary>
        public Matrix Transform { get; private set; }

        /// <summary>
        /// Makes the screen follow a spesific StageObject
        /// </summary>
        /// <param name="stageObject">The stageObject<see cref="StageObject"/></param>
        /// <param name="screenWidth">The screenWidth<see cref="int"/></param>
        /// <param name="screenHeight">The screenHeight<see cref="int"/></param>
        public void Follow(StageObject stageObject, int screenWidth, int screenHeight)
        {
            cameraPosition = stageObject.Position;
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            Matrix position = Matrix.CreateTranslation(
                -stageObject.Position.X,
                -stageObject.Position.Y,
                0);

            Matrix offset = Matrix.CreateTranslation(
                screenWidth / 2,
                screenHeight / 2,
                0);
            Transform = position * offset;
        }

        /// <summary>
        /// Sets the camera to a spesific Vector2 position 
        /// </summary>
        /// <param name="cameraPosition">The cameraPosition<see cref="Vector2"/></param>
        /// <param name="screenWidth">The screenWidth<see cref="int"/></param>
        /// <param name="screenHeight">The screenHeight<see cref="int"/></param>
        public void Follow(Vector2 cameraPosition, int screenWidth, int screenHeight)
        {
            this.cameraPosition = cameraPosition;
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            Matrix position = Matrix.CreateTranslation(
                -cameraPosition.X,
                -cameraPosition.Y,
                0);

            Matrix offset = Matrix.CreateTranslation(
                screenWidth / 2,
                screenHeight / 2,
                0);
            Transform = position * offset;
        }
    }
}
