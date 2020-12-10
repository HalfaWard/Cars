using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Engine.Objects;

namespace Engine.Interface
{
    public interface IProjectile
    {
        void CountDown(GameTime time);
    }
}