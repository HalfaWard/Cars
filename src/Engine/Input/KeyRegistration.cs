using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Engine.Input
{
    internal class KeyRegistration
    {
        public double timeCreated;
        public double timeUpdated;
        public List<int> keys;
        

        public KeyRegistration(List<int> keys, double time)
        {
            this.keys = keys;
            timeCreated = time;
            timeUpdated = time;
        }
    }
}