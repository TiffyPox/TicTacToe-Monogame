using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TicTacToe.UI;

namespace TicTacToe.Screens
{
    public abstract class BaseScreen : IScreen
    {
        protected readonly List<Button> Buttons = new List<Button>();
        protected MouseState LastMouseState;
        protected MouseState CurrentMouseState;
        public Color BackgroundColor { get; protected set; }
        public Action RequestScreenClose { get; set; }
        public Action<IScreen> AddScreen { get; set; }

        public abstract void OnShow();

        public abstract void Load(ContentManager content);

        public virtual void Initialize()
        {
        }

        public void Update(GameTime gameTime)
        {
            CurrentMouseState = Mouse.GetState();
            var mouseRectangle = new Rectangle(CurrentMouseState.X, CurrentMouseState.Y, 1, 1);

            foreach (var button in Buttons)
            {
                // Moving
                if (CurrentMouseState.X != LastMouseState.X || CurrentMouseState.Y != LastMouseState.Y)
                {
                    button.OnCursorMove(mouseRectangle);
                }

                // Holding
                if (CurrentMouseState.LeftButton == ButtonState.Pressed)
                {
                    if (LastMouseState.LeftButton == ButtonState.Pressed)
                    {
                        button.OnCursorHeld(MouseButton.Left);
                    }
                }

                // Clicking
                if (LastMouseState.LeftButton == ButtonState.Pressed &&
                    CurrentMouseState.LeftButton == ButtonState.Released)
                {
                    button.OnCursorClick(MouseButton.Left);
                }

                button.Update(gameTime);
            }

            OnUpdate(gameTime);
            
            LastMouseState = CurrentMouseState;
        }

        protected abstract void OnUpdate(GameTime gameTime);

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            foreach (var button in Buttons)
            {
                button.Draw(spriteBatch);
            }
            
            spriteBatch.End();
        }
    }
}