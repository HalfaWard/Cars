namespace Engine.Objects
{
    using Engine.Animation;
    using Engine.Interface;
    using Microsoft.Xna.Framework;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="AbstractProjectile" />
    /// </summary>
    public abstract class AbstractProjectile : StageObject, IProjectile, IAnimate, IMoving
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractProjectile"/> class.
        /// </summary>
        /// <param name="hitbox">The hitbox<see cref="Dictionary{string, Hitbox}"/></param>
        protected AbstractProjectile(Dictionary<string, Hitbox> hitbox) : base(hitbox)
        {
        }

        /// <summary>
        /// Gets or sets the Speed
        /// </summary>
        public float Speed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsMoving
        /// </summary>
        public virtual bool IsMoving { get; set; }

        /// <summary>
        /// Gets or sets the NameOfSoundOfEffect
        /// </summary>
        public string NameOfSoundOfEffect { get; set; }

        /// <summary>
        /// Gets or sets the AnimationManager
        /// </summary>
        public AnimationManager AnimationManager { get; set; }

        /// <summary>
        /// Gets or sets the CurrentSpeed
        /// </summary>
        public Vector2 CurrentSpeed { get; set; }

        /// <summary>
        /// Gets or sets the CurrentPath
        /// </summary>
        public List<Vector2> CurrentPath { get; set; }

        /// <summary>
        /// Gets or sets the LengthOfPathMoved
        /// </summary>
        public float LengthOfPathMoved { get; set; }

        /// <summary>
        /// Gets or sets the Animations
        /// </summary>
        public Dictionary<string, Animation> Animations { get; set; }

        /// <summary>
        /// Gets or sets the Knockback
        /// </summary>
        public Vector2 Knockback { get; set; }

        /// <summary>
        /// Gets or sets the KnockbackFriction
        /// </summary>
        public float KnockbackFriction { get; set; }

        /// <summary>
        /// The CountDown
        /// </summary>
        /// <param name="time">The time<see cref="GameTime"/></param>
        public abstract void CountDown(GameTime time);

        /// <summary>
        /// The Move
        /// </summary>
        public abstract void Move();
    }
}
