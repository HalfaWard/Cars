using Microsoft.Xna.Framework.Media;

namespace Engine.Sound
{
    public class BackgroundSong
    {
        public string name;
        public bool loopable;

        public Song Song { get; private set; }

        public BackgroundSong(Song song, string name, bool loopable = false)
        {
            this.name = name;
            this.loopable = loopable;
            Song = song;
        }
    }
}