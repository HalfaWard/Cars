using Microsoft.Xna.Framework.Content;

namespace Engine.Sound
{
    internal interface ISoundManager
    {
        void AddBackgroundSong(BackgroundSong song);
        void AddSoundEffects(SoundFX sound);
        void MuteBackgroundMusic();
        void ToggleMuteSoundEffects();
        void PauseAllSound();
        void PauseAllSoundEffects();
        void PauseBackgroundMusic();
        void PauseSoundEffect(string effectInstanceName);
        void PlayBackgroundMusic(string songName);
        void PlaySoundEffect(string soundName);
        void RemoveBackgroundSong(string songName);
        void RemoveSoundEffect(string effectInstanceName);
        void ResumeBackgroundMusic();
        void ResumeSoundEffect(string effectName);
        void ResmueAllSound();
        void StopAllSoundEffects();
        void StopAllSounds();
        void StopBackgroundMusic();
        void StopSoundEffect(string effectInstanceName);
    }
}