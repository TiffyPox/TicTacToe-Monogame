using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TicTacToe.Entities;
using TicTacToe.Graphics;

namespace TicTacToe.UI
{
    public class Button : IGameEntity
    {
        private bool _held;
        private bool _hovering;

        private readonly Sprite _sprite;
        private readonly Sprite _heldSprite;
        private readonly SpriteFont _font;
        
        private readonly Vector2 _position;

        public Action OnClick { get; set; }

        public int DrawOrder => 0;

        public Button(Sprite sprite, SpriteFont font, Vector2 position, Sprite heldSprite)
        {
            _sprite = sprite;
            _position = position;
            _font = font;
            _heldSprite = heldSprite;
        }

        public void OnCursorMove(Rectangle mouseBounds) => _hovering = GetBounds().Contains(mouseBounds);

        public void OnCursorClick(MouseButton button)
        {
            if (!_hovering)
            {
                return;
            }
            
            OnClick?.Invoke();
        }
        
        public void OnCursorHeld(MouseButton button)
        {
            if (_hovering)
            {
                _held = true;
            }
        }

        public void OnCursorRelease(MouseButton button)
        {
            _held = false;
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var color = _hovering ? Color.White * 0.7f : Color.White;
            
            if (_held)
            {
                _heldSprite.Draw(spriteBatch, _position, color);
                _held = false;
            }
            else
            {
                _sprite.Draw(spriteBatch, _position, color);
            }
        }

        private Rectangle GetBounds() => new Rectangle((int)(_position.X - _sprite.RenderWidth / 2.0f), (int)(_position.Y - _sprite.RenderHeight / 2.0f), _sprite.RenderWidth, _sprite.RenderHeight);
    }
}
