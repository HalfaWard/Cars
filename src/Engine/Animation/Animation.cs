namespace Engine.Animation
{
    using Engine.Objects;
    using Engine.Functionality;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Defines the <see cref="Animation" />
    /// </summary>
    public class Animation
    {
        /// <summary>
        /// The list of hitbox elements in the animation
        /// </summary>
        public List<Dictionary<string, Hitbox>> animationHitbox;

        /// <summary>
        /// The current frame
        /// </summary>
        public int CurrentFrame { get; set; }

        /// <summary>
        /// The frame count
        /// </summary>
        public int FrameCount { get; set; }

        /// <summary>
        /// The height of the frame
        /// </summary>
        public int FrameHeight => Texture.Height;

        /// <summary>
        /// Gets the frame with of the animation based on the texture size and frame count
        /// If the animation is a rotatable it will only return the textures width
        /// </summary>
        public int FrameWidth
        {
            get
            {
                if (RotationAnimation)
                {
                    return Texture.Width;
                }

                return Texture.Width / FrameCount;
            }
        }

        /// <summary>
        /// Sets the frames speed, the higher it gets the faster it skips through the spritesheet
        /// </summary>
        public double FrameSpeed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsLooping
        /// </summary>
        public bool IsLooping { get; set; }

        /// <summary>
        /// Defines the Texture
        /// </summary>
        public Texture2D Texture;

        /// <summary>
        /// Gets or sets a value indicating whether RotationAnimation
        /// </summary>
        public bool RotationAnimation { get; set; }

        /// <summary>
        /// Gets or sets the RotationDirection
        /// </summary>
        public float RotationDirection { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Animation"/> class.
        /// </summary>
        /// <param name="texture">The texture<see cref="Texture2D"/></param>
        /// <param name="frameCount">The amount of frames in the spritesheet<see cref="AnimationManager"/></param>
        /// <param name="frameSpeed">The wanted frame speed for this animation<see cref="AnimationManager"/></param>
        /// <param name="hitboxFile">Location of the hitbox string <see cref="ObjectHandler"/></param>
        /// <param name="rotationAnimation">Sets if the animation is rotatable</param>
        /// <param name="clockwiseRotation">Sets the rotation to clockwise, standard is counter colckwise</param>
        public Animation(Texture2D texture, int frameCount, float frameSpeed, string hitboxFile, bool rotationAnimation = false, bool clockwiseRotation = true) :
            this(texture, frameCount, frameSpeed, rotationAnimation, clockwiseRotation)
        {
            LoadHitbox(hitboxFile);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Animation"/> class.
        /// </summary>
        /// <param name="texture">The texture<see cref="Texture2D"/></param>
        /// <param name="frameCount">The amount of frames in the spritesheet<see cref="AnimationManager"/></param>
        /// <param name="frameSpeed">The wanted frame speed for this animation<see cref="AnimationManager"/></param>
        /// <param name="rotationAnimation">Sets if the animation is rotatable</param>
        /// <param name="clockwiseRotation">Sets the rotation to clockwise, standard is counter colckwise</param>
        public Animation(Texture2D texture, int frameCount, float frameSpeed, bool rotationAnimation = false, bool clockwiseRotation = true)
        {
            Texture = texture;
            FrameCount = frameCount;
            FrameSpeed = frameSpeed;
            IsLooping = true;
            RotationAnimation = rotationAnimation;
            RotationDirection = clockwiseRotation ? -1 : 1;
        }

        /// <summary>
        /// This is a set gamefile hitbox that should be created.
        /// </summary>
        private static readonly string FilePath = "../../../../GameFiles/HitboxFiles/AnimationHitbox/";// for debug

        //private static string FilePath = "./Content/HitboxFiles/AnimationHitbox/"; // for release
        /// <summary>
        /// Goes through the hitbox txt file and sets the animation hitbox.
        /// </summary>
        /// <param name="filePath">The filePath<see cref="string"/></param>
        private void LoadHitbox(string filePath)
        {
            animationHitbox = new List<Dictionary<string, Hitbox>>();
            string[] input = File.ReadAllLines(FilePath + filePath);
            Dictionary<string, Hitbox> hitboxes = new Dictionary<string, Hitbox>();
            int frames = 0;
            for (int i = 0; i < input.Length; i++)
            {
                string line = input[i];
                if (line.Length <= 0)
                {
                    continue;
                }

                if (line[0] == '#')
                {
                    if (hitboxes.Count > 0)
                    {
                        for (int j = 0; j < frames; j++)
                        {
                            animationHitbox.Add(hitboxes);
                        }
                    }

                    string[] frameInput = line.Split(' ');
                    frames = int.Parse(frameInput[1]);
                    hitboxes = new Dictionary<string, Hitbox>();
                    continue;
                }
                string[] data = line.Split(';');
                Hitbox hitbox = new Hitbox();
                for (int j = 1; j < data.Length - 1; j++)
                {
                    int[] square = Array.ConvertAll(data[j].Split(','), int.Parse);
                    hitbox.rectangles.Add(new Rectangle(square[0], square[1], square[2], square[3]));
                }
                int[] centerCoords = Array.ConvertAll(data[data.Length - 1].Split(','), int.Parse);
                hitbox.center = new Vector2(centerCoords[0], centerCoords[1]);
                hitboxes.Add(data[0], hitbox);
                if (i == input.Length - 1)
                {
                    for (int j = 0; j < frames; j++)
                    {
                        animationHitbox.Add(hitboxes);
                    }
                }
            }
            if (animationHitbox.Count != FrameCount)
            {
                throw new InvalidDataException("Animation hitbox does not have hitbox count equal to the amount of frames in animation");
            }
        }

        /// <summary>
        /// The GetCurrentHitbox
        /// </summary>
        /// <returns>The <see cref="Dictionary{string, Hitbox}"/></returns>
        public Dictionary<string, Hitbox> GetCurrentHitbox()
        {
            return animationHitbox?[CurrentFrame];
        }

        /// <summary>
        /// Creats an exact copy of the created member, but as a new object and a new referance in the internal memory
        /// </summary>
        /// <returns>The <see cref="object"/></returns>
        public object Clone()
        {
            return MemberwiseClone();
        }

        /// <summary>
        /// The Equals
        /// </summary>
        /// <param name="obj">The obj<see cref="object"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public override bool Equals(object obj)
        {
            if (obj is Animation animation)
            {
                if (animation.Texture.Name == Texture.Name)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// The GetHashCode
        /// </summary>
        /// <returns>The <see cref="int"/></returns>
        public override int GetHashCode()
        {
            return Texture.GetHashCode();
        }
    }
}
