using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Engine.Functionality;

namespace Engine.GameStates
{
    public abstract class GameState : IGameState
    {
        public int stateNumber;   
        public Matrix Transform { get; set; }
        public bool HasTransform { get; set; } = false;
        public GameState()
        {
            OnCreate();
        }

        public abstract void OnCreate();
        public abstract void LoadContent(ContentManager content);
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void DrawStatic(SpriteBatch staticSpriteBatch);
        public abstract void Update(GameTime gameTime);
        public abstract void Update(GameTimeHolder gameTime);
        public abstract void OnExit();
    }
}
