namespace Engine.GameStates
{
    using Engine.Functionality;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="GameStateManager" />
    /// </summary>
    public class GameStateManager
    {
        /// <summary>
        /// Defines the _gameStateStack
        /// </summary>
        private Stack<GameState> _gameStateStack = new Stack<GameState>();

        /// <summary>
        /// Defines the _content
        /// </summary>
        private ContentManager _content;

        /// <summary>
        /// Defines the _spriteBatch
        /// </summary>
        private SpriteBatch _spriteBatch;

        /// <summary>
        /// Defines the _instance
        /// </summary>
        private static GameStateManager _instance;

        /// <summary>
        /// Defines the _staticSpriteBatch
        /// </summary>
        private SpriteBatch _staticSpriteBatch;

        /// <summary>
        /// Sets the content manager for the state manager
        /// </summary>
        /// <param name="content">The content<see cref="ContentManager"/></param>
        public void SetContentManager(ContentManager content)
        {
            _content = content;
        }

        /// <summary>
        /// Sets the spritebatch to use for drawing
        /// </summary>
        /// <param name="spriteBatch">The spriteBatch<see cref="SpriteBatch"/></param>
        public void SetSpriteBatch(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
        }

        /// <summary>
        /// Sets the spritebatch to use for static drawing
        /// </summary>
        /// <param name="spriteBatch">The spriteBatch<see cref="SpriteBatch"/></param>
        public void SetStaticSpriteBatch(SpriteBatch spriteBatch)
        {
            _staticSpriteBatch = spriteBatch;
        }

        /// <summary>
        /// Sets a spritebatch for drawing non-statically and statically
        /// </summary>
        /// <param name="spriteBatch">The spriteBatch<see cref="SpriteBatch"/></param>
        /// <param name="staticSpriteBatch">The staticSpriteBatch<see cref="SpriteBatch"/></param>
        public void SetSpriteBatches(SpriteBatch spriteBatch, SpriteBatch staticSpriteBatch)
        {
            _spriteBatch = spriteBatch;
            _staticSpriteBatch = staticSpriteBatch;
        }

        /// <summary>
        /// Gets or creates an instance of a state manager
        /// </summary>
        public static GameStateManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameStateManager();
                }
                return _instance;
            }
        }

        /// <summary>
        /// Adds a state to the stack and loads its content
        /// </summary>
        /// <param name="state">The state<see cref="GameState"/></param>
        public void AddState(GameState state)
        {
            _gameStateStack.Push(state);
            _gameStateStack.Peek().LoadContent(_content);
        }

        /// <summary>
        /// Removes the top state from the stack and runs onExit on the state
        /// </summary>
        public void RemoveTopState()
        {
            if (_gameStateStack.Count > 0)
            {
                _gameStateStack.Peek().OnExit();
                _gameStateStack.Pop();
            }
        }

        /// <summary>
        /// Removes all states from the stack and runs onExit on them
        /// </summary>
        public void ClearAllStates()
        {
            while (_gameStateStack.Count > 0)
            {
                _gameStateStack.Peek().OnExit();
                _gameStateStack.Pop();
            }
        }

        /// <summary>
        /// Updates the top state
        /// </summary>
        /// <param name="gameTime">The gameTime<see cref="GameTime"/></param>
        public void Update(GameTime gameTime)
        {
            if (_gameStateStack.Count > 0)
            {
                _gameStateStack.Peek().Update(gameTime);
            }
        }

        /// <summary>
        /// Updates the top sate
        /// </summary>
        /// <param name="gameTime">The gameTime<see cref="GameTimeHolder"/></param>
        public void Update(GameTimeHolder gameTime)
        {
            gameTime.Update();
            if (_gameStateStack.Count > 0)
            {
                _gameStateStack.Peek().Update(gameTime);
            }
        }

        /// <summary>
        /// Draws the non-static content of the state
        /// </summary>
        public void Draw()
        {
            if (_gameStateStack.Count > 0)
            {
                _gameStateStack.Peek().Draw(_spriteBatch);
            }
        }

        /// <summary>
        /// Draws the static content of the state
        /// </summary>
        public void DrawStatic()
        {
            if (_gameStateStack.Count > 0)
            {
                _gameStateStack.Peek().DrawStatic(_staticSpriteBatch);
            }
        }

        /// <summary>
        /// Clears all states, runs onExit on them, and adds the state to the stack
        /// </summary>
        /// <param name="state">The state<see cref="GameState"/></param>
        public void ChangeState(GameState state)
        {
            ClearAllStates();
            AddState(state);
        }

        /// <summary>
        /// Returns the top state in the stack, or null
        /// </summary>
        /// <returns>The <see cref="GameState"/></returns>
        public GameState Peek()
        {
            if (_gameStateStack.Count == 0)
            {
                return null;
            }

            return _gameStateStack.Peek();
        }
    }
}
