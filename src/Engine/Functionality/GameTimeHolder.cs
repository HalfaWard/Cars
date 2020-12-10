namespace Engine.Functionality
{
    using Microsoft.Xna.Framework;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="GameTimeHolder" />
    /// </summary>
    public class GameTimeHolder
    {
        /// <summary>
        /// creates an instance of the GameTimeHolder object, for easier use in differenct classes
        /// </summary>
        public static GameTimeHolder instance;

        /// <summary>
        /// The list of every timer with the given name
        /// </summary>
        private Dictionary<string, Timer> TimerDictionary = new Dictionary<string, Timer>();

        /// <summary>
        /// Defines the GameTime
        /// </summary>
        public GameTime GameTime;

        /// <summary>
        /// Defines the timersToAdd
        /// </summary>
        private List<Timer> timersToAdd = new List<Timer>();

        /// <summary>
        /// Prevents a default instance of the <see cref="GameTimeHolder"/> class from being created.
        /// </summary>
        /// <param name="gameTime">The gameTime<see cref="GameTime"/></param>
        private GameTimeHolder(GameTime gameTime)
        {
            GameTime = gameTime;
        }

        /// <summary>
        /// The Initializes the game time object
        /// </summary>
        /// <param name="gameTime">The gameTime<see cref="GameTime"/></param>
        /// <returns>The <see cref="GameTimeHolder"/></returns>
        public static GameTimeHolder Initialize(GameTime gameTime)
        {
            if (instance == null)
            {
                instance = new GameTimeHolder(gameTime);
            }

            return instance;
        }

        /// <summary>
        /// Loops through the list of timers
        /// <Remarks>
        /// One thing to remember that this method checks all the Timer objects fields, and sees if they are 
        /// </Remarks>
        /// </summary>
        public void Update()
        {
            timersToAdd.ForEach(timer => TimerDictionary.Add(timer.name, timer));
            timersToAdd.Clear();
            List<string> timersToRemove = new List<string>();
            foreach (KeyValuePair<string, Timer> pair in TimerDictionary)
            {
                Timer v = pair.Value;
                if (v.isCountingDown)
                {
                    v.time -= GameTime.ElapsedGameTime.Milliseconds;
                    CheckInterval(v);
                }
                if (v.time <= 0 && v.isCountingDown)
                {
                    v.isCountingDown = false;
                    v.onEnd?.Invoke(null, null);
                    if (v.removeOnEnd)
                    {
                        timersToRemove.Add(pair.Key);
                    }

                    if (v.restart)
                    {
                        v.isCountingDown = true;
                        v.time = v.maxTime;
                        v.onStart?.Invoke(null, null);
                    }
                }
            }
            timersToRemove.ForEach(timer => TimerDictionary.Remove(timer));
        }

        /// <summary>
        /// Adds a timer to the timer list.
        /// </summary>
        /// <param name="timer">The timer<see cref="Timer"/></param>
        public void AddTimer(Timer timer)
        {
            CheckForExistingTimer(timer);
        }

        /// <summary>
        /// Adds a list of timers to the timer list.
        /// </summary>
        /// <param name="timers">The timers<see cref="List{Timer}"/></param>
        public void AddTimer(List<Timer> timers)
        {
            foreach (Timer timer in timers)
            {
                CheckForExistingTimer(timer);
            }
        }

        /// <summary>
        /// Checks existing timers and invokes onstart Eventhandler <see cref="EventHandler"/>
        /// </summary>
        /// <param name="timer">The timer<see cref="Timer"/></param>
        private void CheckForExistingTimer(Timer timer)
        {
            if (TimerDictionary.ContainsKey(timer.name))
            {
                Timer time = TimerDictionary[timer.name];

                if (timer.isReplenishable)
                {
                    if (timer.isStackable)
                    {
                        time.time += timer.maxTime;

                    }
                    if (!timer.isStackable)
                    {
                        time.time = timer.maxTime;
                    }

                    time.lastIntervalTime = time.time;
                    time.isCountingDown = true;
                    timer.onStart?.Invoke(null, null);
                }
            }
            else
            {
                timer.onStart?.Invoke(null, null);
                if (!timersToAdd.Exists(t => timer.name == t.name))
                {
                    timersToAdd.Add(timer);
                }
            }
        }

        /// <summary>
        /// Gets a timer with the given name
        /// </summary>
        /// <param name="nameOfTimer">The nameOfTimer<see cref="string"/></param>
        /// <returns>The <see cref="Timer"/></returns>
        public Timer GetTimer(string nameOfTimer)
        {
            if (TimerDictionary.ContainsKey(nameOfTimer))
            {
                return TimerDictionary[nameOfTimer];
            }

            return null;
        }

        /// <summary>
        /// Gets the current time of the timer object with the given name
        /// </summary>
        /// <param name="nameOfTimer">The nameOfTimer<see cref="string"/></param>
        /// <returns>The <see cref="float"/></returns>
        public float GetCurrentTime(string nameOfTimer)
        {
            return (float)Math.Round(TimerDictionary[nameOfTimer].time, 2);
        }

        /// <summary>
        /// Checks and invokes intervals of the designated timer object
        /// </summary>
        /// <param name="timer">The timer<see cref="Timer"/></param>
        private void CheckInterval(Timer timer)
        {
            if (timer.interval <= 0)
            {
                return;
            }

            int count = (int)((timer.lastIntervalTime - timer.time) / timer.interval);
            timer.lastIntervalTime -= timer.interval * count;
            if (count > 0)
            {
                timer.onInterval?.Invoke(null, new OnIntervalArgs(count));
            }
        }

        /// <summary>
        /// Removes a timer with the given name
        /// </summary>
        /// <param name="nameOfTimer">The nameOfTimer<see cref="string"/></param>
        public void CancelTimer(string nameOfTimer)
        {
            if (TimerDictionary.ContainsKey(nameOfTimer))
            {
                TimerDictionary.Remove(nameOfTimer);
            }
        }

        /// <summary>
        /// Clears the timer dictionary
        /// </summary>
        public void Clear()
        {
            TimerDictionary.Clear();
            timersToAdd.Clear();
        }

        /// <summary>
        /// Defines the <see cref="Timer" />
        /// </summary>
        public class Timer
        {
            /// <summary>
            /// Defines the onStart
            /// </summary>
            public EventHandler onStart;

            /// <summary>
            /// Defines the onEnd
            /// </summary>
            public EventHandler onEnd;

            /// <summary>
            /// Defines the onInterval
            /// </summary>
            public EventHandler<OnIntervalArgs> onInterval;

            /// <summary>
            /// Defines the name
            /// </summary>
            public string name;

            /// <summary>
            /// Defines the time
            /// </summary>
            public float time;

            /// <summary>
            /// Defines the isCountingDown
            /// </summary>
            public bool isCountingDown = true;

            /// <summary>
            /// Defines the maxTime
            /// </summary>
            public float maxTime;

            /// <summary>
            /// Defines the isStackable
            /// </summary>
            public bool isStackable;

            /// <summary>
            /// Defines the isReplenishable
            /// </summary>
            public bool isReplenishable;

            /// <summary>
            /// Defines the interval
            /// </summary>
            public float interval = 100;

            /// <summary>
            /// Defines the lastIntervalTime
            /// </summary>
            public float lastIntervalTime;

            /// <summary>
            /// Defines the removeOnEnd
            /// </summary>
            public bool removeOnEnd;

            /// <summary>
            /// Defines the restart
            /// </summary>
            public bool restart;

            /// <summary>
            /// Initializes a new instance of the <see cref="Timer"/> class.
            /// </summary>
            /// <param name="name">The name<see cref="string"/></param>
            /// <param name="maxTime">The maxTime<see cref="float"/></param>
            /// <param name="interval">The interval<see cref="float"/></param>
            /// <param name="isStackable">The isStackable<see cref="bool"/></param>
            /// <param name="isReplenishable">The isReplenishable<see cref="bool"/></param>
            /// <param name="onStart">The onStart<see cref="EventHandler"/></param>
            /// <param name="onEnd">The onEnd<see cref="EventHandler"/></param>
            /// <param name="removeOnEnd">The removeOnEnd<see cref="bool"/></param>
            /// <param name="restart">The restart<see cref="bool"/></param>
            /// <param name="onInterval">The onInterval<see cref="EventHandler{OnIntervalArgs}"/></param>
            public Timer(string name, float maxTime, float interval = 0, bool isStackable = false, bool isReplenishable = true, EventHandler onStart = null, EventHandler onEnd = null,
                 bool removeOnEnd = false, bool restart = false, EventHandler<OnIntervalArgs> onInterval = null)
            {
                this.onInterval = onInterval;
                this.name = name;
                time = maxTime;
                this.interval = interval;
                this.isStackable = isStackable;
                this.isReplenishable = isReplenishable;
                this.maxTime = maxTime;
                lastIntervalTime = time;
                this.onStart = onStart;
                this.onEnd = onEnd;
                this.removeOnEnd = removeOnEnd;
                this.restart = restart;
            }
        }
    }

    /// <summary>
    /// Defines the <see cref="OnIntervalArgs" />
    /// </summary>
    public class OnIntervalArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the Count
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OnIntervalArgs"/> class.
        /// </summary>
        /// <param name="count">The count<see cref="int"/></param>
        public OnIntervalArgs(int count)
        {
            Count = count;
        }
    }
}
