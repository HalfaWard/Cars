using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Save
{
    [Serializable]
    public abstract class Savefile<P> where P : SavePreview
    {
        public int Index { get; set; }

        public abstract P GeneratePreview();
    }
}
