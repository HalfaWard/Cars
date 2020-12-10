namespace Engine.Input
{
    using Microsoft.Xna.Framework.Input;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="Keybind" />
    /// </summary>
    internal class Keybind
    {
        /// <summary>
        /// Defines the binds
        /// </summary>
        public Dictionary<string, List<int>> binds;

        /// <summary>
        /// Initializes a new instance of the <see cref="Keybind"/> class.
        /// </summary>
        public Keybind()
        {
            binds = new Dictionary<string, List<int>>();
        }

        /// <summary>
        /// The SetKeybind
        /// </summary>
        /// <param name="filePath">The filePath<see cref="string"/></param>
        public void SetKeybind(string filePath)
        {
            string[] input = File.ReadAllLines(filePath);
            foreach (string keybinding in input)
            {
                if (keybinding.Length <= 0)
                {
                    continue;
                }

                string[] actionAndKey = keybinding.Split(';');
                AddKeybind(actionAndKey[0], actionAndKey[1]);
            }
        }

        /// <summary>
        /// The AddKeybind
        /// </summary>
        /// <param name="action">The action<see cref="string"/></param>
        /// <param name="key">The key<see cref="List{int}"/></param>
        public void AddKeybind(string action, List<int> key)
        {
            //TODO: Append key if action exists
            if (binds.ContainsKey(action))
            {
                foreach (int var in key)
                {
                    binds[action].Add(var);
                }
            }
            else
            {
                binds.Add(action, key);
            }
        }

        /// <summary>
        /// The AddKeybind
        /// </summary>
        /// <param name="action">The action<see cref="string"/></param>
        /// <param name="key">The key<see cref="int[]"/></param>
        public void AddKeybind(string action, int[] key)
        {
            AddKeybind(action, key.ToList());
        }

        /// <summary>
        /// The AddKeybind
        /// </summary>
        /// <param name="action">The action<see cref="string"/></param>
        /// <param name="key">The key<see cref="string"/></param>
        public void AddKeybind(string action, string key)
        {
            AddKeybind(action, GetIntValues(key));
        }

        /// <summary>
        /// The RemoveKeybind
        /// </summary>
        /// <param name="action">The action<see cref="string"/></param>
        /// <param name="key">The key<see cref="string"/></param>
        public void RemoveKeybind(string action, string key)
        {
            RemoveKeybind(action, GetIntValues(key));
        }

        /// <summary>
        /// The RemoveKeybind
        /// </summary>
        /// <param name="action">The action<see cref="string"/></param>
        /// <param name="key">The key<see cref="int[]"/></param>
        public void RemoveKeybind(string action, int[] key)
        {
            RemoveKeybind(action, key.ToList());
        }

        /// <summary>
        /// The RemoveKeybind
        /// </summary>
        /// <param name="action">The action<see cref="string"/></param>
        /// <param name="key">The key<see cref="List{int}"/></param>
        public void RemoveKeybind(string action, List<int> key)
        {
            foreach (int var in key)
            {
                binds[action].Remove(var);
            }
        }

        /// <summary>
        /// The GetIntValues
        /// </summary>
        /// <param name="key">The key<see cref="string"/></param>
        /// <returns>The <see cref="List{int}"/></returns>
        private List<int> GetIntValues(string key)
        {
            string[] keys = key.Split(',');
            bool isInts = false;
            foreach (string input in keys)
            {
                isInts = int.TryParse(input, out _);
            }
            if (isInts)
            {
                int[] keyInts = Array.ConvertAll(keys, int.Parse);
                return keyInts.ToList();
            }
            else
            {
                int[] keyInts = new int[keys.Length];
                for (int i = 0; i < keys.Length; i++)
                {
                    if (!Enum.TryParse(keys[i], out Keys keysEnum))
                    {
                        continue;
                    }

                    int value = (int)keysEnum;
                    keyInts[i] = value;
                }
                return keyInts.ToList();
            }
        }
    }
}
