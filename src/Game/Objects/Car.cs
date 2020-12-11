using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Engine.Functionality;
using Engine.Interface;
using Engine.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game
{
    public class Car : Unit, IMoving

    {
        public Car(Texture2D texture, Dictionary<string, Hitbox> hitbox, Vector2 posistion, Vector2 direction,
            string name = "F1Car") : base(texture, hitbox, posistion, direction, name)
        {
            scale = new Vector2(0.1f, 0.1f);
            origin = CenterOrigin();
        }

        public override void OnCollison(StageObject collisionTarget, List<Rectangle> intersections, string hitboxName,
            string targetHitboxName)
        {
            if(targetHitboxName == "outside")
                filter = Color.Red;
            if (targetHitboxName == "inside")
                filter = Color.White;
        }

        public override void Update(GameTime gameTime)
        {
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

        private readonly float _friction = 0.7f;
        public bool IsMoving { get; set; }
        private bool Accelerate;
        private bool Brake;
        private float Acceleration = 0.1f;
        private float BreakeForce = 0.2f;
        private Vector2 maxSpeed;
        private Vector2 currentSpeed;

        public Vector2 CurrentSpeed
        {
            get => currentSpeed;
            set => currentSpeed = value;
        }

        public float Speed { get; set; } = 0f;
        public List<Vector2> CurrentPath { get; set; }
        public float LengthOfPathMoved { get; set; }

        public void Move()
        {
            Accelerate = false;
            Brake = false;
            if (Game1.KeyBuffer.CheckKeybindPressOrHold("up")) Accelerate = true;
            if (Game1.KeyBuffer.CheckKeybindPressOrHold("down")) Brake = true;
            if (Game1.KeyBuffer.CheckKeybindPressOrHold("left")) TurnLeft();
            if (Game1.KeyBuffer.CheckKeybindPressOrHold("right")) TurnRight();
            if (Accelerate && Speed < 10)
                Speed += Acceleration;

            if (Brake)
            {
                Speed -= BreakeForce;
                if (Speed < 0) Speed = 0;
            }

            CurrentSpeed = Geometry.AngleToVector(-Rotation, Speed);
            Position += CurrentSpeed;
        }

        public Vector2 Knockback { get; set; }
        public float KnockbackFriction { get; set; }

        private void TurnLeft()
        {
            if(Speed == 0) return;
            Rotation -= MathHelper.ToRadians(5);
            if (Rotation > Math.PI * 2) Rotation = 0;
        }

        private void TurnRight()
        {
            if(Speed == 0) return;
            Rotation += MathHelper.ToRadians(5);
            if (Rotation > Math.PI * 2) Rotation = 0;
        }
    }
}