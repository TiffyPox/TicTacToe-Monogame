using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TicTacToe.Screens
{
    public interface IScreen
    {
        Color BackgroundColor { get; }

        public Action RequestScreenClose { get; set; }
        public Action<IScreen> AddScreen { get; set; }

        /// <summary>
        /// Called when the screen is shown
        /// </summary>
        void OnShow();
        void Load(ContentManager content);
        void Initialize();
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
    }
}
