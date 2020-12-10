using Engine.Sound;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace Engine.Functionality
{
    /// <summary>
    /// This class exists only to make it easier to keep track of all the content that should be loaded in the spesific gamestates
    /// </summary>
    public abstract class AbstractContentLibrary
    {
        public Dictionary<string, Texture2D> Texture2dDictionary { get; set; } = new Dictionary<string, Texture2D>();
        public Dictionary<string, Animation.Animation> AnimationDictionary { get; set; } = new Dictionary<string, Animation.Animation>();
        public Dictionary<string, SoundFX> SoundEffectDictionary { get; set; } = new Dictionary<string, SoundFX>();
        public Dictionary<string, BackgroundSong> SongDictionary { get; set; } = new Dictionary<string, BackgroundSong>();
        public Dictionary<string, SpriteFont> SpriteFontDictionary { get; set; } = new Dictionary<string, SpriteFont>();


        public abstract void ClearAll();
        public abstract void ClearTextures();
        public abstract void ClearAnimations();
        public abstract void ClearSoundEffects();
        public abstract void ClearSongs();
        public abstract void ClearFonts();
        public abstract void AddSongs(Dictionary<string, BackgroundSong> songs);
        public abstract void AddTextures(Dictionary<string, Texture2D> textures);
        public abstract void AddAnimations(Dictionary<string, Animation.Animation> animations);
        public abstract void AddSoundEffects(Dictionary<string, SoundFX> soundEffects);
        public abstract void AddSpriteFonts(Dictionary<string, SpriteFont> spriteFonts);
    }

   
}
