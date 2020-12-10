using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Objects
{
    public abstract class Tile : StageObject
    {
        public bool IsTraversable { get; set; }
        public Tile(Texture2D texture, Dictionary<string, Hitbox> hitbox, Vector2 direction, bool isTraversable) : base(texture, hitbox, new Vector2(), direction)
        {
            IsTraversable = isTraversable;
        }
    }
}
