namespace Engine.Sound
{
    using Microsoft.Xna.Framework.Media;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// This class hold both the Dictionary for BackGroundSongs and SoundFX
    /// its main task is to be used in the different gamestates to hold on to 
    /// its music and soundeffects
    /// 
    /// <see cref="SoundManager" />
    /// </summary>
    public class SoundManager : ISoundManager
    {
        /// <summary>
        /// Defines the songs
        /// </summary>
        public Dictionary<string, BackgroundSong> songs;

        /// <summary>
        /// Defines the soundFX
        /// </summary>
        public Dictionary<string, SoundFX> soundFX;

        /// <summary>
        /// Defines the songNames
        /// </summary>
        public List<string> songNames = new List<string>();

        /// <summary>
        /// Defines the SoundFXVolume
        /// </summary>
        internal static Dictionary<string, float> SoundFXVolume = new Dictionary<string, float>();

        /// <summary>
        /// Defines the currentSong
        /// </summary>
        private string currentSong;

        /// <summary>
        /// Defines the isPlaying
        /// </summary>
        private bool isPlaying;

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundManager"/> class.
        /// </summary>
        public SoundManager()
        {
            songs = new Dictionary<string, BackgroundSong>();
            soundFX = new Dictionary<string, SoundFX>();
        }

        /// <summary>
        /// This method doubles as a playNext method since
        /// only one song can be played at anygiven time
        /// </summary>
        /// <param name="songName">For use in the dictionary to find the song</param>
        public void PlayBackgroundMusic(string songName)
        {
            isPlaying = true;
            if (songs.ContainsKey(songName))
            {
                BackgroundSong song = songs[songName];
                currentSong = song.name;
                MediaPlayer.Stop();
                MediaPlayer.IsRepeating = song.loopable;
                MediaPlayer.Play(song.Song);
            }
        }

        /// <summary>
        /// The nameOfSong
        /// </summary>
        /// <returns>The <see cref="string"/></returns>
        public string NameOfSong()
        {
            return currentSong;
        }

        /// <summary>
        /// Adds a song to the dictionary with given name
        /// </summary>
        /// <param name="song">TThe BackgroundSong object<see cref="BackgroundSong"/></param>
        public void AddBackgroundSong(BackgroundSong song)
        {
            if (!songs.ContainsKey(song.name))
            {
                songs.Add(song.name, song);
            }
            else
            {
                songs[song.name] = song;
                songNames.Add(song.name);
            }
        }

        /// <summary>
        /// The AddBackgroundSong
        /// </summary>
        /// <param name="listOfSongs">The listOfSongs<see cref="List{BackgroundSong}"/></param>
        public void AddBackgroundSong(List<BackgroundSong> listOfSongs)
        {
            foreach (BackgroundSong song in listOfSongs)
            {
                AddBackgroundSong(song);
                songNames.Add(song.name);
            }
        }

        /// <summary>
        /// The PauseAndResumeBackgroundMusic
        /// </summary>
        public void PauseAndResumeBackgroundMusic()
        {
            (isPlaying ? (Action)PauseBackgroundMusic : ResumeBackgroundMusic)();
        }

        /// <summary>
        /// The PauseBackgroundMusic
        /// </summary>
        public void PauseBackgroundMusic()
        {
            isPlaying = false;
            MediaPlayer.Pause();
        }

        /// <summary>
        /// The StopBackgroundMusic
        /// </summary>
        public void StopBackgroundMusic()
        {
            MediaPlayer.Stop();
        }

        /// <summary>
        /// The ResumeBackgroundMusic
        /// </summary>
        public void ResumeBackgroundMusic()
        {
            isPlaying = true;
            MediaPlayer.Resume();
        }

        /// <summary>
        /// Mutes and unmutes the background music
        /// depentding on the need
        /// </summary>
        public void MuteBackgroundMusic()
        {
            MediaPlayer.IsMuted = !MediaPlayer.IsMuted;
        }

        /// <summary>
        /// Removes a specified song from the dictionary
        /// </summary>
        /// <param name="songName">The songName<see cref="string"/></param>
        public void RemoveBackgroundSong(string songName)
        {
            if (songs.ContainsKey(songName))
            {
                songs.Remove(songName);
            }
        }

        /// <summary>
        /// The UpdateBackgroundMusicVolume
        /// </summary>
        /// <param name="volume">The volume<see cref="float"/></param>
        public static void UpdateBackgroundMusicVolume(float volume)
        {
            MediaPlayer.Volume = volume;
        }

        /// <summary>
        /// The PlayNextTrack
        /// </summary>
        public void PlayNextTrack()
        {
            int currentSongPlaying = songNames.IndexOf(currentSong);
            if (currentSongPlaying >= songNames.Count - 1)
            {
                currentSongPlaying = 0;
            }
            else
            {
                currentSongPlaying++;
            }

            currentSong = songNames[currentSongPlaying];
            PlayBackgroundMusic(currentSong);
        }

        /// <summary>
        /// Adds a soundeffect to the dictionary
        /// </summary>
        /// <param name="sound">The SoundFX object<see cref="SoundFX"/></param>
        public void AddSoundEffects(SoundFX sound)
        {
            if (!soundFX.ContainsKey(sound.name))
            {
                soundFX.Add(sound.name, sound);
            }
            else
            {
                soundFX[sound.name] = sound;
            }
        }

        /// <summary>
        /// The AddSoundEffects
        /// </summary>
        /// <param name="soundEffects">The soundEffects<see cref="List{SoundFX}"/></param>
        public void AddSoundEffects(List<SoundFX> soundEffects)
        {
            foreach (SoundFX sound in soundEffects)
            {
                AddSoundEffects(sound);
            }
        }

        /// <summary>
        /// Stoppes a spesific sound effect
        /// </summary>
        /// <param name="effectInstanceName">The effectInstanceName<see cref="string"/></param>
        public void StopSoundEffect(string effectInstanceName)
        {
            if (soundFX.ContainsKey(effectInstanceName))
            {
                soundFX[effectInstanceName].StopSoundEffectInstance();
            }
        }

        /// <summary>
        /// Pauses all soundeffects
        /// </summary>
        public void PauseAllSoundEffects()
        {
            foreach (SoundFX sound in soundFX.Values)
            {
                sound.PauseSoundEffect();
            }
        }

        /// <summary>
        /// The ResumeSoundEffect
        /// </summary>
        /// <param name="effectName">The effectName<see cref="string"/></param>
        public void ResumeSoundEffect(string effectName)
        {
            if (soundFX.ContainsKey(effectName))
            {
                soundFX[effectName].PauseSoundEffect();
            }
        }

        /// <summary>
        /// The PauseSoundEffect
        /// </summary>
        /// <param name="effectInstanceName">The effectInstanceName<see cref="string"/></param>
        public void PauseSoundEffect(string effectInstanceName)
        {
            if (soundFX.ContainsKey(effectInstanceName))
            {
                soundFX[effectInstanceName].PauseSoundEffect();
            }
        }

        /// <summary>
        /// Removes a specefied soundeffect from the dictionary
        /// </summary>
        /// <param name="effectInstanceName">The effectInstanceName<see cref="string"/></param>
        public void RemoveSoundEffect(string effectInstanceName)
        {
            if (soundFX.ContainsKey(effectInstanceName))
            {
                soundFX.Remove(effectInstanceName);
            }
        }

        /// <summary>
        /// Plays a soundeffect that is specified with the name string
        /// </summary>
        /// <param name="soundName">The soundName<see cref="string"/></param>
        public void PlaySoundEffect(string soundName)
        {
            if (soundFX.ContainsKey(soundName))
            {
                soundFX[soundName].PlaySoundEffect();
            }
        }

        /// <summary>
        /// Reduces the volume of soundeffects
        /// </summary>
        public void ToggleMuteSoundEffects()
        {
            if (SoundFX.isMuted == SoundFX.Muted.True)
            {
                SoundFX.isMuted = SoundFX.Muted.False;
            }
            else
            {
                SoundFX.isMuted = SoundFX.Muted.True;
            }

            foreach (SoundFX sound in soundFX.Values)
            {
                if (sound.IsInstance)
                {
                    sound.EffectInstance.Volume = sound.Volume * (int)SoundFX.isMuted;
                }
            }
        }

        /// <summary>
        /// Stops all instances of soundeffects
        /// </summary>
        public void StopAllSoundEffects()
        {
            foreach (SoundFX sound in soundFX.Values)
            {
                sound.StopSoundEffectInstance();
            }
        }

        /// <summary>
        /// The ResumeAllSoundEffects
        /// </summary>
        private void ResumeAllSoundEffects()
        {
            foreach (SoundFX sound in soundFX.Values)
            {
                sound.ResumeSoundEffect();
            }
        }

        /// <summary>
        /// The UpdateSoundEffectVolume
        /// </summary>
        public void UpdateSoundEffectVolume()
        {
            foreach (SoundFX sound in soundFX.Values)
            {
                sound.Volume = SoundFXVolume.ContainsKey(sound.volumeType) ? SoundFXVolume[sound.volumeType] : 0.3f;
                if (sound.IsInstance)
                {
                    sound.EffectInstance.Volume = sound.Volume;
                }
            }
        }

        /// <summary>
        /// The UpdateSoundEffectVolume
        /// </summary>
        /// <param name="volumeType">The volumeType<see cref="string"/></param>
        /// <param name="volume">The volume<see cref="float"/></param>
        public static void UpdateSoundEffectVolume(string volumeType, float volume)
        {
            if (!SoundFXVolume.ContainsKey(volumeType))
            {
                SoundFXVolume.Add(volumeType, volume);
            }
            else
            {
                SoundFXVolume[volumeType] = volume;
            }
        }

        /// <summary>
        /// Stops all sounds
        /// </summary>
        public void StopAllSounds()
        {
            StopBackgroundMusic();
            StopAllSoundEffects();
        }

        /// <summary>
        /// Pause All sounds
        /// </summary>
        public void PauseAllSound()
        {
            PauseBackgroundMusic();
            PauseAllSoundEffects();
        }

        /// <summary>
        /// Resumes both background music and all instances of soundeffect
        /// </summary>
        public void ResmueAllSound()
        {
            ResumeAllSoundEffects();
            ResumeBackgroundMusic();
        }
    }
}
