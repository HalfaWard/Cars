namespace Engine.Animation
{
    using Engine.Functionality;
    using Engine.Objects;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="AnimationManager" />
    /// </summary>
    public class AnimationManager
    {
        /// <summary>
        /// The private animation object
        /// </summary>
        private Animation _animation;

        /// <summary>
        /// Defines the _timer
        /// </summary>
        private float _timer;

        /// <summary>
        /// Defines the alpha
        /// </summary>
        public float alpha = 100;

        /// <summary>
        /// Defines the obj
        /// </summary>
        public StageObject obj;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationManager"/> class.
        /// </summary>
        /// <param name="animation">The animation<see cref="Animation"/></param>
        /// <param name="obj">The obj<see cref="StageObject"/></param>
        public AnimationManager(Animation animation, StageObject obj)
        {
            Play(animation);
            this.obj = obj;
        }

        /// <summary>
        /// The main draw funtion for animations
        /// This checks if the sprite file is allowed to rotate and then rotates it
        /// It main function is to loop through the given spritesheet based on its width and framecount 
        /// </summary>
        /// <param name="sprite">The sprite<see cref="SpriteBatch"/></param>
        public void Draw(SpriteBatch sprite)
        {
            Color color = new Color(obj.filter, alpha / 100);
            if (_animation.RotationAnimation)
            {
                sprite.Draw(_animation.Texture, obj.Position,
                    new Rectangle(0, 0, _animation.FrameWidth, _animation.FrameHeight),
                    color, _animation.RotationDirection * _animation.CurrentFrame / _animation.FrameCount, obj.origin, obj.scale, SpriteEffects.None, 0);
            }
            else
            {
                sprite.Draw(_animation.Texture, obj.Position,
                    new Rectangle(_animation.CurrentFrame * _animation.FrameWidth, 0, _animation.FrameWidth, _animation.FrameHeight),
                    color, 0, obj.origin, obj.scale, SpriteEffects.None, 0);
            }
        }

        /// <summary>
        /// This starts the animation and loops through the frames of the spritesheet
        /// </summary>
        /// <param name="animation">The animation<see cref="Animation"/></param>
        public void Play(Animation animation)
        {
            if (_animation != null && _animation.Equals(animation))
            {
                return;
            }

            _animation = animation;
            _animation.CurrentFrame = 0;
            _timer = 0;
        }

        /// <summary>
        /// An inner method only for stopping the current animation timer and freezing the animation in place
        /// </summary>
        private void Stop()
        {
            _timer = 0f;
        }

        /// <summary>
        /// The update function checks the timer of the spritesheet and updates the position of the frames.
        /// </summary>
        /// <remarks>
        /// It should be mentioned that this funciton does not support the GameTimeHolder class, wich is an extention of the GameTime class
        /// </remarks>
        /// <param name="gameTime">The gameTime<see cref="GameTime"/></param>
        public void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_timer > _animation.FrameSpeed)
            {
                _timer = 0f;
                _animation.CurrentFrame++;

                if (_animation.CurrentFrame >= _animation.FrameCount)
                {
                    _animation.CurrentFrame = 0;
                }
            }
        }

        /// <summary>
        /// Stop any frame at a specified frame
        /// </summary>
        /// <remarks>
        /// After the use of this function if you want the animation to start you have to use the "Play" function
        /// </remarks>
        /// <param name="spesificFrame">The frame where you want it to stop/pause</param>
        public void StopAtFrame(int spesificFrame)
        {
            _animation.CurrentFrame = spesificFrame;
            Stop();
        }

        /// <summary>
        /// The update function checks the timer of the spritesheet and updates the position of the frames.
        /// </summary>
        /// <remarks>
        /// Should be mentioned that this would be the most suited update function for comercial use, given its 
        /// </remarks>
        /// <param name="gameTime">The gameTime<see cref="GameTimeHolder"/></param>
        public void Update(GameTimeHolder gameTime)
        {
            _timer += (float)gameTime.GameTime.ElapsedGameTime.TotalSeconds;
            if (_timer > _animation.FrameSpeed)
            {
                _timer = 0f;
                _animation.CurrentFrame++;

                if (_animation.CurrentFrame >= _animation.FrameCount)
                {
                    _animation.CurrentFrame = 0;
                }
            }
        }

        /// <summary>
        /// Gets the animation hitbox of the object created
        /// </summary>
        /// <returns>The <see cref="Dictionary{string, Hitbox}"/></returns>
        public Dictionary<string, Hitbox> GetCurrentHitbox()
        {
            return _animation.GetCurrentHitbox();
        }
    }
}
