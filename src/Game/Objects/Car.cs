using System;
using System.Collections.Generic;
using Engine.Functionality;
using Engine.Interface;
using Engine.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game
{
    public class Car : Unit, IMoving
    {
        public Car(Texture2D texture, Dictionary<string, Hitbox> hitbox, Vector2 posistion, Vector2 direction, bool isPlayer = true, 
            string name = "F1Car") : base(texture, hitbox, posistion, direction, name)
        {
            IsPlayer = isPlayer;
            scale = new Vector2(0.1f, 0.1f);
            origin = new Vector2(CenterOrigin().X - CenterOrigin().X * 0.8f, CenterOrigin().Y);
        }

        public override void OnCollison(StageObject collisionTarget, List<Rectangle> intersections, string hitboxName,
            string targetHitboxName)
        {
            if(targetHitboxName == "outside")
                filter = Color.Red;
            if (targetHitboxName == "inside")
                filter = Color.White;
        }

        public override void Update(GameTimeHolder gameTime)
        {
            Move();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Texture != null)
            {
                spriteBatch.Draw(Texture, Position, null, filter, Rotation, origin, scale, SpriteEffects.None, 0);
            }
            else throw new Exception("Texture can not be null");
        }

        private bool IsPlayer;
        private bool Accelerating;
        private bool Braking;
        private readonly float _acceleration = 0.2f;
        private readonly float _brakeForce = 0.3f;
        private readonly float _friction = 0.98f;
        private readonly float _maxSpeed = 15;
        private Vector2 maxSpeed;
        private Vector2 currentSpeed;

        public Vector2 CurrentSpeed
        {
            get => currentSpeed;
            set => currentSpeed = value;
        }
        public float Speed { get; set; }
        public List<Vector2> CurrentPath { get; set; }
        public float LengthDriven { get; set; }

        public void Move()
        {
            if (IsPlayer) CheckInput();
            if (Accelerating && Speed < _maxSpeed)
                Speed += _acceleration;

            if (Braking)
            {
                Speed -= _brakeForce;
                if (Speed < 0) Speed = 0;
            }

            if (!Accelerating && !Braking)
            {
                Speed *= _friction;
                if (Speed < 0.1f) Speed = 0;
            }
            CurrentSpeed = Geometry.AngleToVector(-Rotation, Speed);
            LengthDriven += CurrentSpeed.Length();
            Position += CurrentSpeed;
        }

        private void CheckInput()
        {
            Accelerating = Game1.KeyBuffer.CheckKeybindPressOrHold("up");
            Braking = Game1.KeyBuffer.CheckKeybindPressOrHold("down");
            if (Game1.KeyBuffer.CheckKeybindPressOrHold("left")) TurnLeft();
            if (Game1.KeyBuffer.CheckKeybindPressOrHold("right")) TurnRight();
        }

        public void ToggleAccelerate() => Accelerating = !Accelerating;
        public void ToggleBrake() => Braking = !Braking;

        public void TurnLeft()
        {
            if(Speed == 0) return;
            Rotation -= MathHelper.ToRadians(2);
            if (Rotation > Math.PI * 2) Rotation = 0;
        }

        public void TurnRight()
        {
            if(Speed == 0) return;
            Rotation += MathHelper.ToRadians(2);
            if (Rotation > Math.PI * 2) Rotation = 0;
        }

        #region Inherited values we do not need
        public override void Update(GameTime gameTime) { } // Leave empty
        public float LengthOfPathMoved { get; set; }
        public bool IsMoving { get; set; }
        public Vector2 Knockback { get; set; }
        public float KnockbackFriction { get; set; }
        #endregion
    }
}