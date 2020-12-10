using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Engine.GameStates
{
    public interface IGameState
    {
        void OnCreate();
        void LoadContent(ContentManager content);
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
        void OnExit();
    }
}
