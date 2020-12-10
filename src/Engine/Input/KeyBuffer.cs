namespace Engine.Input
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="KeyBuffer" />
    /// </summary>
    public class KeyBuffer
    {
        /// <summary>
        /// Defines the keyboardBinds
        /// </summary>
        internal Keybind keyboardBinds = new Keybind();

        /// <summary>
        /// Defines the keyboardCombinations
        /// </summary>
        internal Keybind keyboardCombinations = new Keybind();

        /// <summary>
        /// Defines the controllerBinds
        /// </summary>
        internal Keybind controllerBinds = new Keybind();

        /// <summary>
        /// Defines the controllerCombinations
        /// </summary>
        internal Keybind controllerCombinations = new Keybind();

        /// <summary>
        /// Defines the keyBuffer
        /// </summary>
        internal List<KeyRegistration> keyBuffer = new List<KeyRegistration>();

        /// <summary>
        /// Defines the lastKeyRegistration
        /// </summary>
        internal KeyRegistration lastKeyRegistration;

        /// <summary>
        /// Defines the useController
        /// </summary>
        public bool useController;

        /// <summary>
        /// Defines the millis
        /// </summary>
        private float millis;

        /// <summary>
        /// Defines the lastKeyboardState
        /// </summary>
        private KeyboardState lastKeyboardState;

        /// <summary>
        /// Defines the bindsInUse
        /// </summary>
        private Dictionary<string, List<int>> bindsInUse;

        /// <summary>
        /// Defines the combinationBindsInUse
        /// </summary>
        private Dictionary<string, List<int>> combinationBindsInUse;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyBuffer"/> class.
        /// </summary>
        /// <param name="useController">The useController<see cref="bool"/></param>
        /// <param name="millis">The millis<see cref="int"/></param>
        public KeyBuffer(bool useController, int millis)
        {
            this.useController = useController;
            this.millis = millis;
            bindsInUse = useController ? controllerBinds.binds : keyboardBinds.binds;
            combinationBindsInUse = useController ? controllerCombinations.binds : keyboardCombinations.binds;
        }

        /// <summary>
        /// Sets the buffer time
        /// </summary>
        /// <param name="millis">The millis<see cref="float"/></param>
        public void SetKeybufferMS(float millis)
        {
            this.millis = millis;
        }

        /// <summary>
        /// Registers a new set of keyboard input
        /// </summary>
        /// <param name="keyboardState">The keyboardState<see cref="KeyboardState"/></param>
        /// <param name="time">The time<see cref="GameTime"/></param>
        public void RegisterKeyPressed(KeyboardState keyboardState, GameTime time)
        {
            Keys[] inputs = keyboardState.GetPressedKeys();
            List<int> action = Array.ConvertAll(inputs, value => (int)value).ToList();
            if (!keyboardState.Equals(lastKeyboardState) || keyBuffer.Count == 0)
            {
                keyBuffer.Add(new KeyRegistration(action, time.TotalGameTime.TotalSeconds));
                lastKeyRegistration = keyBuffer[keyBuffer.Count - 1];
            }
            else
            {
                lastKeyRegistration.timeUpdated = time.TotalGameTime.TotalSeconds;
            }

            lastKeyboardState = keyboardState;
            while (time.TotalGameTime.TotalSeconds - keyBuffer[0].timeUpdated > millis / 1000)
            {
                keyBuffer.RemoveAt(0);
            }
        }

        //In-game checks

        //You must insure there to be input before you can call to check it
        /// <summary>
        /// Checks for the press of an action
        /// </summary>
        /// <param name="action">The action<see cref="string"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool CheckKeybindPress(string action)
        {
            if (keyBuffer.Count <= 1 || lastKeyRegistration.keys.Count == 0)
            {
                return false;
            }

            return bindsInUse.ContainsKey(action) &&
                   bindsInUse[action].Any(bind => keyBuffer.ElementAt(keyBuffer.Count - 1).keys.Contains(bind)) &&
                   bindsInUse[action].All(bind => !keyBuffer.ElementAt(keyBuffer.Count - 2).keys.Contains(bind)) &&
                   lastKeyRegistration.timeCreated.Equals(lastKeyRegistration.timeUpdated);
        }

        /// <summary>
        /// Checks for a press or hold on an action
        /// </summary>
        /// <param name="action">The action<see cref="string"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool CheckKeybindPressOrHold(string action)
        {
            if (lastKeyRegistration.keys.Count == 0)
            {
                return false;
            }

            return bindsInUse.ContainsKey(action) &&
                   bindsInUse[action].Any(bind => keyBuffer.ElementAt(keyBuffer.Count - 1).keys.Contains(bind));
        }

        /// <summary>
        /// Checks for an action that has been pressed or held for a certain amount of time
        /// </summary>
        /// <param name="action">The action<see cref="string"/></param>
        /// <param name="holdTimeMillis">The holdTimeMillis<see cref="float"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool CheckKeybindPressAndHold(string action, float holdTimeMillis)
        {
            if (lastKeyRegistration.keys.Count == 0)
            {
                return false;
            }

            return CheckKeybindPress(action) || CheckKeybindHold(action, holdTimeMillis);
        }

        /// <summary>
        /// Checks if an action is being held
        /// </summary>
        /// <param name="action">The action<see cref="string"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool CheckKeybindHold(string action)
        {
            if (keyBuffer != null && lastKeyRegistration.keys.Count == 0)
            {
                return false;
            }

            return bindsInUse.ContainsKey(action) &&
                   bindsInUse[action].Any(bind => keyBuffer.ElementAt(keyBuffer.Count - 1).keys.Contains(bind)) &&
                   !lastKeyRegistration.timeCreated.Equals(lastKeyRegistration.timeUpdated);
        }

        /// <summary>
        /// Checks if an action has been held for over a certain amount of time
        /// </summary>
        /// <param name="action">The action<see cref="string"/></param>
        /// <param name="minimumHoldTimeMillis">The minimumHoldTimeMillis<see cref="float"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool CheckKeybindHold(string action, float minimumHoldTimeMillis)
        {
            if (keyBuffer != null && lastKeyRegistration.keys.Count == 0)
            {
                return false;
            }

            return bindsInUse.ContainsKey(action) &&
                   bindsInUse[action].Any(bind => keyBuffer.ElementAt(keyBuffer.Count - 1).keys.Contains(bind)) &&
                   !lastKeyRegistration.timeCreated.Equals(lastKeyRegistration.timeUpdated) &&
                   lastKeyRegistration.timeUpdated - lastKeyRegistration.timeCreated >= minimumHoldTimeMillis / 1000;
        }

        /// <summary>
        /// Checks for the release of an action
        /// </summary>
        /// <param name="action">The action<see cref="string"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool CheckKeybindRelease(string action)
        {
            if (keyBuffer.Count <= 1)
            {
                return false;
            }

            return bindsInUse.ContainsKey(action) &&
                   bindsInUse[action].All(bind => !keyBuffer.ElementAt(keyBuffer.Count - 1).keys.Contains(bind)) &&
                   bindsInUse[action].Any(bind => keyBuffer.ElementAt(keyBuffer.Count - 2).keys.Contains(bind)) &&
                   lastKeyRegistration.timeCreated.Equals(lastKeyRegistration.timeUpdated);
        }

        /// <summary>
        /// Checks for the press of a combination action
        /// </summary>
        /// <param name="action">The action<see cref="string"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool CheckKeyCombinationPress(string action)
        {
            if (keyBuffer.Count <= 1 || lastKeyRegistration.keys.Count == 0)
            {
                return false;
            }

            return combinationBindsInUse.ContainsKey(action) &&
                   combinationBindsInUse[action].All(bind => keyBuffer.ElementAt(keyBuffer.Count - 1).keys.Contains(bind)) &&
                   lastKeyRegistration.timeCreated.Equals(lastKeyRegistration.timeUpdated);
        }

        /// <summary>
        /// Checks if a combination action is being held
        /// </summary>
        /// <param name="action">The action<see cref="string"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool CheckKeyCombinationHold(string action)
        {
            if (keyBuffer != null && lastKeyRegistration.keys.Count == 0)
            {
                return false;
            }

            return combinationBindsInUse.ContainsKey(action) &&
                   combinationBindsInUse[action].All(bind => keyBuffer.ElementAt(keyBuffer.Count - 1).keys.Contains(bind)) &&
                   !lastKeyRegistration.timeCreated.Equals(lastKeyRegistration.timeUpdated);
        }

        /// <summary>
        /// Checks if a combination action is being pressed or held
        /// </summary>
        /// <param name="action">The action<see cref="string"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool CheckKeyCombinationPressOrHold(string action)
        {
            if (lastKeyRegistration.keys.Count == 0)
            {
                return false;
            }

            return combinationBindsInUse.ContainsKey(action) &&
                   combinationBindsInUse[action].All(bind => keyBuffer.ElementAt(keyBuffer.Count - 1).keys.Contains(bind));
        }

        /// <summary>
        /// Checks the release of a combination action
        /// </summary>
        /// <param name="action">The action<see cref="string"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool CheckKeyCombinationRelease(string action)
        {
            if (keyBuffer.Count <= 1)
            {
                return false;
            }

            return combinationBindsInUse.ContainsKey(action) &&
                   combinationBindsInUse[action].Any(bind => !keyBuffer.ElementAt(keyBuffer.Count - 1).keys.Contains(bind)) &&
                   combinationBindsInUse[action].All(bind => keyBuffer.ElementAt(keyBuffer.Count - 2).keys.Contains(bind)) &&
                   lastKeyRegistration.timeCreated.Equals(lastKeyRegistration.timeUpdated);
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        public void ResetKeybinds()
        {
        }

        /// <summary>
        /// Sets the keybinds from the file in the given path
        /// </summary>
        /// <param name="filePath">The filePath<see cref="string"/></param>
        /// <param name="type">The type<see cref="keybindType"/></param>
        public void SetKeybinds(string filePath, keybindType type)
        {
            Keybind bind = bindings(type);
            bind.SetKeybind(filePath);
        }

        /// <summary>
        /// Adds an action to a key
        /// </summary>
        /// <param name="action">The action<see cref="string"/></param>
        /// <param name="key">The key<see cref="string"/></param>
        /// <param name="type">The type<see cref="keybindType"/></param>
        public void AddKeybind(string action, string key, keybindType type)
        {
            Keybind bind = bindings(type);
            bind.AddKeybind(action, key);
        }

        /// <summary>
        /// Adds an action to several keys or an action to a combination of keys
        /// </summary>
        /// <param name="action">The action<see cref="string"/></param>
        /// <param name="key">The key<see cref="int[]"/></param>
        /// <param name="type">The type<see cref="keybindType"/></param>
        public void AddKeybind(string action, int[] key, keybindType type)
        {
            Keybind bind = bindings(type);
            bind.AddKeybind(action, key);
        }

        /// <summary>
        /// Adds an action to several keys or an action to a combination of keys
        /// </summary>
        /// <param name="action">The action<see cref="string"/></param>
        /// <param name="key">The key<see cref="List{int}"/></param>
        /// <param name="type">The type<see cref="keybindType"/></param>
        public void AddKeybind(string action, List<int> key, keybindType type)
        {
            Keybind bind = bindings(type);
            bind.AddKeybind(action, key);
        }

        //TODO: make it able to remove combinations
        /// <summary>
        /// Removes a keybind
        /// </summary>
        /// <param name="action">The action<see cref="string"/></param>
        /// <param name="key">The key<see cref="string"/></param>
        /// <param name="type">The type<see cref="keybindType"/></param>
        public void RemoveKeybind(string action, string key, keybindType type)
        {
            Keybind bind = bindings(type);
            bind.RemoveKeybind(action, key);
        }

        /// <summary>
        /// Removes several keybinds
        /// </summary>
        /// <param name="action">The action<see cref="string"/></param>
        /// <param name="key">The key<see cref="int[]"/></param>
        /// <param name="type">The type<see cref="keybindType"/></param>
        public void RemoveKeybind(string action, int[] key, keybindType type)
        {
            Keybind bind = bindings(type);
            bind.RemoveKeybind(action, key);
        }

        /// <summary>
        /// Removes several keybinds
        /// </summary>
        /// <param name="action">The action<see cref="string"/></param>
        /// <param name="key">The key<see cref="List{int}"/></param>
        /// <param name="type">The type<see cref="keybindType"/></param>
        public void RemoveKeybind(string action, List<int> key, keybindType type)
        {
            Keybind bind = bindings(type);
            bind.RemoveKeybind(action, key);
        }

        /// <summary>
        /// The bindings
        /// </summary>
        /// <param name="type">The type<see cref="keybindType"/></param>
        /// <returns>The <see cref="Keybind"/></returns>
        private Keybind bindings(keybindType type)
        {
            switch (type)
            {
                case keybindType.KeyboardBind:
                    return keyboardBinds;
                case keybindType.KeyboardCombination:
                    return keyboardCombinations;
                case keybindType.ControllerBind:
                    return controllerBinds;
                case keybindType.ControllerCombination:
                    return controllerCombinations;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, "Must use keybindType enum");
            }
        }
    }

    /// <summary>
    /// Defines the keybindType
    /// </summary>
    public enum keybindType
    { /// <summary>
      /// Defines the KeyboardBind
      /// </summary>
        KeyboardBind,
        /// <summary>
        /// Defines the KeyboardCombination
        /// </summary>
        KeyboardCombination,
        /// <summary>
        /// Defines the ControllerBind
        /// </summary>
        ControllerBind,
        /// <summary>
        /// Defines the ControllerCombination
        /// </summary>
        ControllerCombination
    }
}
