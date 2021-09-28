using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TicTacToe.Graphics
{
    public class Sprite
    {
        private Texture2D Texture { get; set; }

        private int X { get; set; }
        private int Y { get; set; }

        private int Width { get; set; }
        private int Height { get; set; }
        private int Scale { get; }
        public int RenderWidth => Width * Scale;
        public int RenderHeight => Height * Scale;

        public Sprite(Texture2D texture, int x, int y, int width, int height, int scale = 1)
        {
            Texture = texture;
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Scale = scale;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color)
        {
            spriteBatch.Draw(Texture, position, new Rectangle(X, Y, Width, Height), color, 0, new Vector2(Width / 2.0f, Height / 2.0f), Scale, SpriteEffects.None, 0);
        }
        
        public void Draw(SpriteBatch spriteBatch, Vector2 position, Vector2 origin, Color color)
        {
            spriteBatch.Draw(Texture, position, new Rectangle(X, Y, Width, Height), color, 0, origin, Scale, SpriteEffects.None, 0);
        }
    }
}
