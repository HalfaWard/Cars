namespace Engine.Functionality
{
    using System;
    using System.IO;
    using System.Xml.Serialization;

    /// <summary>
    /// Defines the <see cref="Settings" />
    /// </summary>
    [Serializable]
    public class Settings
    {
        /// <summary>
        /// Defines the serializer
        /// </summary>
        private static XmlSerializer serializer = new XmlSerializer(typeof(Settings));

        /// <summary>
        /// Defines the _gamma
        /// </summary>
        private float _gamma;

        /// <summary>
        /// Defines the _masterVolume
        /// </summary>
        private Volume _masterVolume;

        /// <summary>
        /// Defines the _isFullScreen
        /// </summary>
        private bool _isFullScreen;

        /// <summary>
        /// Defines the _isBorderless
        /// </summary>
        private bool _isBorderless;

        /// <summary>
        /// Defines the defaultSettings
        /// </summary>
        private DefaultSettings defaultSettings;

        /// <summary>
        /// Defines the SettingsChanged
        /// </summary>
        public event EventHandler<SettingsChangeArgs> SettingsChanged;

        /// <summary>
        /// Defines the screenResolution
        /// </summary>
        public ScreenResolution screenResolution;

        /// <summary>
        /// Gets or sets the Gamma
        /// </summary>
        public float Gamma
        {
            get => _gamma;
            set
            {
                if (value < 0)
                {
                    value = 0;
                }

                if (value > 1)
                {
                    value = 1;
                }

                _gamma = value;
                OnSettingsChanged(new SettingsChangeArgs("Gamma", _gamma, false));
            }
        }

        /// <summary>
        /// Gets or sets the MasterVolume
        /// </summary>
        public float MasterVolume
        {
            get => _masterVolume.Value;
            set
            {
                _masterVolume.Value = value;
                OnSettingsChanged(new SettingsChangeArgs("MasterVolume", _masterVolume.Value, false));
            }
        }

        /// <summary>
        /// Defines the Volumes
        /// </summary>
        public SerializableDictionary<string, Volume> Volumes = new SerializableDictionary<string, Volume>();

        /// <summary>
        /// Defines the OtherSettings
        /// </summary>
        public SerializableDictionary<string, object> OtherSettings = new SerializableDictionary<string, object>();

        /// <summary>
        /// Gets or sets a value indicating whether IsFullScreen
        /// </summary>
        public bool IsFullScreen
        {
            get => _isFullScreen;
            set
            {
                _isFullScreen = value;
                OnSettingsChanged(new SettingsChangeArgs("IsFullScreen", value, false));
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether IsBorderless
        /// </summary>
        public bool IsBorderless
        {
            get => _isBorderless;
            set
            {
                _isBorderless = value;
                OnSettingsChanged(new SettingsChangeArgs("IsBorderless", value, false));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Settings"/> class.
        /// </summary>
        public Settings()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Settings"/> class.
        /// </summary>
        /// <param name="defaultSettings">The defaultSettings<see cref="DefaultSettings"/></param>
        public Settings(DefaultSettings defaultSettings)
        {
            SetDefaultSettings(defaultSettings);
            ResetSettings();
        }

        /// <summary>
        /// The SetScreenResolution
        /// </summary>
        /// <param name="width">The width<see cref="int"/></param>
        /// <param name="height">The height<see cref="int"/></param>
        public void SetScreenResolution(int width, int height)
        {
            screenResolution = new ScreenResolution(width, height);
            OnSettingsChanged(new SettingsChangeArgs("ScreenResolution", screenResolution, false));
        }

        /// <summary>
        /// The GetScreenResolution
        /// </summary>
        /// <returns>The <see cref="ScreenResolution"/></returns>
        public ScreenResolution GetScreenResolution()
        {
            return screenResolution;
        }

        /// <summary>
        /// Resets all settings to default
        /// </summary>
        public void ResetSettings()
        {
            ResetScreen();
            ResetVolume();
            ResetOtherSettings();
        }

        /// <summary>
        /// Resets screen size, gamma, fullscreen and borderless to default
        /// </summary>
        public void ResetScreen()
        {
            SetScreenResolution(defaultSettings.ScreenResolution.Width, defaultSettings.ScreenResolution.Height);
            IsFullScreen = defaultSettings.IsFullScreen;
            IsBorderless = defaultSettings.IsBorderless;
            Gamma = defaultSettings.Gamma;
        }

        /// <summary>
        /// Resets all volumes
        /// </summary>
        public void ResetVolume()
        {
            MasterVolume = defaultSettings.MasterVolume;
            ResetVolumes();
        }

        /// <summary>
        /// Resets user defined settings
        /// </summary>
        public void ResetOtherSettings()
        {
            if (OtherSettings != null)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, object> keyPair in OtherSettings)
                {
                    if (!defaultSettings.OtherSettings.ContainsKey(keyPair.Key))
                    {
                        OnSettingsChanged(new SettingsChangeArgs(keyPair.Key, keyPair.Value, true));
                    }
                }
            }

            foreach (System.Collections.Generic.KeyValuePair<string, object> keyPair in defaultSettings.OtherSettings)
            {
                SetSetting(keyPair.Key, keyPair.Value);
            }
        }

        /// <summary>
        /// Resets all volumes except master volume
        /// </summary>
        private void ResetVolumes()
        {
            if (Volumes != null)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, Volume> keyPair in Volumes)
                {
                    if (!defaultSettings.Volumes.ContainsKey(keyPair.Key))
                    {
                        OnSettingsChanged(new SettingsChangeArgs("Volume-" + keyPair.Key, keyPair.Value.Value, true));
                    }
                }
            }

            foreach (System.Collections.Generic.KeyValuePair<string, Volume> keyPair in defaultSettings.Volumes)
            {
                SetVolume(keyPair.Key, keyPair.Value.Value);
            }
        }

        /// <summary>
        /// Sets the value of a volume, between 0 and 1
        /// </summary>
        /// <param name="name">The name<see cref="string"/></param>
        /// <param name="volume">The volume<see cref="float"/></param>
        public void SetVolume(string name, float volume)
        {
            if (!Volumes.ContainsKey(name))
            {
                Volumes.Add(name, new Volume(volume));
            }
            else
            {
                Volumes[name] = new Volume(volume);
            }

            OnSettingsChanged(new SettingsChangeArgs("Volume-" + name, Volumes[name].Value, false));
        }

        /// <summary>
        /// Gets the value of a volume
        /// </summary>
        /// <param name="name">The name<see cref="string"/></param>
        /// <returns>The <see cref="float"/></returns>
        public float GetVolume(string name)
        {
            if (Volumes.ContainsKey(name))
            {
                return Volumes[name].Value;
            }

            return 0;
        }

        /// <summary>
        /// Removes a volume
        /// </summary>
        /// <param name="name">The name<see cref="string"/></param>
        public void RemoveVolume(string name)
        {
            if (Volumes.ContainsKey(name))
            {
                OnSettingsChanged(new SettingsChangeArgs("Volume-" + name, Volumes[name], true));
                Volumes.Remove(name);
            }
        }

        /// <summary>
        /// Adds a user defined setting
        /// </summary>
        /// <param name="name">The name<see cref="string"/></param>
        /// <param name="setting">The setting<see cref="object"/></param>
        public void SetSetting(string name, object setting)
        {
            if (!OtherSettings.ContainsKey(name))
            {
                OtherSettings.Add(name, setting);
            }
            else
            {
                OtherSettings[name] = setting;
            }

            OnSettingsChanged(new SettingsChangeArgs(name, OtherSettings[name], false));
        }

        /// <summary>
        /// Gets a user defined setting
        /// </summary>
        /// <param name="name">The name<see cref="string"/></param>
        /// <returns>The <see cref="object"/></returns>
        public object GetSetting(string name)
        {
            if (OtherSettings.ContainsKey(name))
            {
                return OtherSettings[name];
            }

            return null;
        }

        /// <summary>
        /// Removes a user defined setting
        /// </summary>
        /// <param name="name">The name<see cref="string"/></param>
        public void RemoveSetting(string name)
        {
            if (OtherSettings.ContainsKey(name))
            {
                OnSettingsChanged(new SettingsChangeArgs(name, OtherSettings[name], true));
                OtherSettings.Remove(name);
            }
        }

        /// <summary>
        /// Sets the default settings
        /// </summary>
        /// <param name="defaultSettings">The defaultSettings<see cref="DefaultSettings"/></param>
        public void SetDefaultSettings(DefaultSettings defaultSettings)
        {
            this.defaultSettings = defaultSettings;
        }

        /// <summary>
        /// The OnSettingsChanged
        /// </summary>
        /// <param name="e">The e<see cref="SettingsChangeArgs"/></param>
        private void OnSettingsChanged(SettingsChangeArgs e)
        {
            SettingsChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Saves the settings in xml to the path
        /// </summary>
        /// <param name="path">The path<see cref="string"/></param>
        public void Save(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Create))
            {
                SerializableDictionary<string, object>.tempValueSerializer = new XmlSerializer(typeof(object), defaultSettings.Types);
                serializer.Serialize(stream, this);
            }
        }

        /// <summary>
        /// Loads settings from xml from a path. If it does not exist, it uses the default settings
        /// </summary>
        /// <param name="path">The path<see cref="string"/></param>
        /// <param name="defaultSettings">The defaultSettings<see cref="DefaultSettings"/></param>
        /// <returns>The <see cref="Settings"/></returns>
        public static Settings Load(string path, DefaultSettings defaultSettings)
        {
            Settings settings;
            try
            {
                using (Stream stream = File.Open(path, FileMode.Open))
                {
                    SerializableDictionary<string, object>.tempValueSerializer = new XmlSerializer(typeof(object), defaultSettings.Types);
                    settings = (Settings)serializer.Deserialize(stream);
                }
                settings.SetDefaultSettings(defaultSettings);
            }
            catch
            {
                settings = new Settings(defaultSettings);
            }
            return settings;
        }

        /// <summary>
        /// Defines the <see cref="ScreenResolution" />
        /// </summary>
        [Serializable]
        public struct ScreenResolution
        {
            /// <summary>
            /// Gets or sets the Width
            /// </summary>
            public int Width { get; set; }

            /// <summary>
            /// Gets or sets the Height
            /// </summary>
            public int Height { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref=""/> class.
            /// </summary>
            /// <param name="width">The width<see cref="int"/></param>
            /// <param name="heigth">The heigth<see cref="int"/></param>
            public ScreenResolution(int width, int heigth)
            {
                Width = width;
                Height = heigth;
            }
        }
    }

    /// <summary>
    /// Defines the <see cref="SettingsChangeArgs" />
    /// </summary>
    public class SettingsChangeArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the SettingName
        /// </summary>
        public string SettingName { get; set; }

        /// <summary>
        /// Gets or sets the SettingValue
        /// </summary>
        public object SettingValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether SettingRemoved
        /// </summary>
        public bool SettingRemoved { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsChangeArgs"/> class.
        /// </summary>
        /// <param name="settingName">The settingName<see cref="string"/></param>
        /// <param name="settingValue">The settingValue<see cref="object"/></param>
        /// <param name="settingRemoved">The settingRemoved<see cref="bool"/></param>
        public SettingsChangeArgs(string settingName, object settingValue, bool settingRemoved)
        {
            SettingName = settingName;
            SettingValue = settingValue;
            SettingRemoved = settingRemoved;
        }
    }

    /// <summary>
    /// Defines the <see cref="Volume" />
    /// </summary>
    public struct Volume
    {
        /// <summary>
        /// Defines the _volume
        /// </summary>
        private float _volume;

        /// <summary>
        /// Gets or sets the Value
        /// </summary>
        public float Value
        {
            get => _volume;
            set => _volume = CheckVolumeValue(value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref=""/> class.
        /// </summary>
        /// <param name="value">The value<see cref="float"/></param>
        public Volume(float value)
        {
            if (value < 0)
            {
                value = 0;
            }

            if (value > 1)
            {
                value = 1;
            }

            _volume = value;
        }

        /// <summary>
        /// The CheckVolumeValue
        /// </summary>
        /// <param name="value">The value<see cref="float"/></param>
        /// <returns>The <see cref="float"/></returns>
        private float CheckVolumeValue(float value)
        {
            if (value < 0)
            {
                return 0;
            }

            if (value > 1)
            {
                return 1;
            }

            return value;
        }
    }
}
