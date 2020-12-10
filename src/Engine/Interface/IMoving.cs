using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Interface
{
    public interface IMoving
    {
        bool IsMoving { get; set; }
        Vector2 CurrentSpeed { get; set; }
        float Speed { get; set; }
        List<Vector2> CurrentPath { get; set; }
        float LengthOfPathMoved { get; set; }
        void Move();
        Vector2 Knockback{get; set;}
        float KnockbackFriction { get; set; }
    }
}
