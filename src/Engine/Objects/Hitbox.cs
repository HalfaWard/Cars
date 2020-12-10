using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Objects
{
    public class Hitbox
    {
        public List<Rectangle> rectangles = new List<Rectangle>();
        public Vector2 center;

        public Hitbox()
        {

        }

        public Hitbox(List<Rectangle> rectangles, Vector2 center)
        {
            this.rectangles = rectangles;
            this.center = center;
        }
    }
}
