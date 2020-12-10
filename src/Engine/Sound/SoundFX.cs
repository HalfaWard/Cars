namespace Engine.Sound
{
    using Microsoft.Xna.Framework.Audio;

    /// <remarks>
    /// The best practis of this class is to always use the 
    /// instance of an effect since it is easier 
    /// to use after the initial firing 
    /// </remarks>
    /// <summary>
    /// Defines the <see cref="SoundFX" />
    /// </summary>
    public class SoundFX
    {
        /// <summary>
        /// Defines the _soundFX
        /// </summary>
        private SoundEffect _soundFX;

        /// <summary>
        /// Defines the type
        /// </summary>
        public string type;

        /// <summary>
        /// Defines the pitch
        /// </summary>
        public float pitch;

        /// <summary>
        /// Defines the pan
        /// </summary>
        public float pan;

        /// <summary>
        /// Gets a value indicating whether IsInstance
        /// </summary>
        public bool IsInstance { get; private set; }

        /// <summary>
        /// Defines the name
        /// </summary>
        public string name;

        /// <summary>
        /// Defines the volumeType
        /// </summary>
        public string volumeType;

        /// <summary>
        /// Gets the EffectInstance
        /// </summary>
        public SoundEffectInstance EffectInstance { get; private set; }

        /// <summary>
        /// Gets or sets the Volume
        /// </summary>
        public float Volume { get; set; }

        /// <summary>
        /// Defines the Muted
        /// </summary>
        public enum Muted
        { /// <summary>
          /// Defines the True
          /// </summary>
            True = 0,
            /// <summary>
            /// Defines the False
            /// </summary>
            False = 1
        }

        /// <summary>
        /// Defines the isMuted
        /// </summary>
        internal static Muted isMuted = Muted.False;

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundFX"/> class.
        /// </summary>
        /// <param name="soundFX">The soundFX<see cref="SoundEffect"/></param>
        /// <param name="name">The name<see cref="string"/></param>
        /// <param name="isInstance">The isInstance<see cref="bool"/></param>
        /// <param name="volumeType">The volumeType<see cref="string"/></param>
        public SoundFX(SoundEffect soundFX, string name, bool isInstance, string volumeType = "") : this(soundFX, isInstance, volumeType)
        {
            this.name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundFX"/> class.
        /// </summary>
        /// <param name="soundFX">The soundFX<see cref="SoundEffect"/></param>
        /// <param name="isInstance">The isInstance<see cref="bool"/></param>
        /// <param name="volumeType">The volumeType<see cref="string"/></param>
        public SoundFX(SoundEffect soundFX, bool isInstance, string volumeType = "")
        {
            this.volumeType = volumeType;
            if (volumeType.Length > 0 && SoundManager.SoundFXVolume.ContainsKey(volumeType))
            {
                Volume = SoundManager.SoundFXVolume[volumeType];
            }
            else
            {
                Volume = 0.3f;
            }

            pitch = 0.0f;
            pan = 0.1f;
            IsInstance = isInstance;
            _soundFX = soundFX;
            if (isInstance)
            {
                SetSoundEffectInstance(soundFX);
            }
        }

        /// <summary>
        /// The SetSoundEffectInstance
        /// </summary>
        /// <param name="soundFX">The soundFX<see cref="SoundEffect"/></param>
        private void SetSoundEffectInstance(SoundEffect soundFX)
        {
            EffectInstance = soundFX.CreateInstance();
            EffectInstance.Volume = Volume;
            EffectInstance.Pitch = pitch;
            EffectInstance.Pan = pan;
        }

        /// <summary>
        /// For playing a spesific Instance of a sound, 
        /// that can only be used one at a time
        /// </summary>
        private void PlayInstance()
        {
            EffectInstance.Stop();
            EffectInstance.Play();
        }

        /// <summary>
        /// The PauseSoundEffect
        /// </summary>
        public void PauseSoundEffect()
        {
            if (IsInstance)
            {
                EffectInstance.Pause();
            }
        }

        /// <summary>
        /// The ResumeSoundEffect
        /// </summary>
        public void ResumeSoundEffect()
        {
            EffectInstance.Resume();
        }

        /// <summary>
        /// The PlaySoundEffect
        /// </summary>
        public void PlaySoundEffect()
        {
            if (IsInstance)
            {
                PlayInstance();
            }
            else
            {
                PlaySound();
            }
        }

        /// <remarks>
        /// Only used if the need for the same sound is fired multiple times
        /// Be wary this method is not stoppable after used
        /// </remarks>
        /// <summary>
        /// The PlaySound
        /// </summary>
        private void PlaySound()
        {
            float volume = SoundManager.SoundFXVolume.ContainsKey(volumeType) ? SoundManager.SoundFXVolume[volumeType] : 0.3f;
            _soundFX.Play(volume * (int)isMuted, pitch, pan);
        }

        /// <summary>
        /// The StopSoundEffectInstance
        /// </summary>
        internal void StopSoundEffectInstance()
        {
            if (IsInstance)
            {
                EffectInstance.Stop();
            }
        }
    }
}
