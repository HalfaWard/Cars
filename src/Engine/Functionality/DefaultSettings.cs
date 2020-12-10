using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Engine.Functionality.Settings;

namespace Engine.Functionality
{
    public abstract class DefaultSettings
    {
        public abstract Type[] Types { get; set; }
        public abstract ScreenResolution ScreenResolution { get; set; }
        public abstract float Gamma { get; set; }
        public abstract float MasterVolume { get; set; }
        public abstract SerializableDictionary<string, Volume> Volumes { get; set; }
        public abstract SerializableDictionary<string, object> OtherSettings { get; set; }
        public abstract bool IsFullScreen { get; set; }
        public abstract bool IsBorderless { get; set; }
    }
}
