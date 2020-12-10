using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Objects
{
    public abstract class Unit : StageObject
    //TODO: find better name than "unit"
    {
        public string name;

        public Unit(Texture2D texture, Dictionary<string, Hitbox> hitbox, Vector2 posistion, Vector2 direction, string name) 
            : base(texture, hitbox, posistion, direction)
        {
            this.name = name;
        }
    }
}
