using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TicTacToe.Graphics;
using TicTacToe.UI;

namespace TicTacToe.Screens
{
    public class CreditsScreen : BaseScreen
    {
        private SpriteFont _font;
        private SpriteFont _credits;
        
        private const string Return = "Return To Menu";
        private const string Line1 = "Lead Programmer \n       @Tiffypox";
        private const string Line2 = "Adviser\n @Rixium";
        private const string Line3 = "Music";
        private const string Line4 = "Doctor DreamChip";
        private const string Line5 = "Sound Effects";  
        private const string Line6 = "SRJA_Gaming";

        public CreditsScreen(GraphicsDeviceManager graphicsDeviceManager, SoundSystem soundSystem)
        {
            BackgroundColor = new Color(25, 25, 112, 255);
            var credits = new Texture2D(graphicsDeviceManager.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            credits.SetData(new[] { Color.White });
        }
        
        public override void OnShow()
        {
        }

        public override void Load(ContentManager content)
        {
            var spriteSheet = content.Load<Texture2D>("Tic-Tac-Toe-SpriteSheet");
            content.Load<Texture2D>("Tic-Tac-Toe-Title");
            
            _font = content.Load<SpriteFont>("ReturnText");
            _credits = content.Load<SpriteFont>("CreditText");
            
            var returnButtonHeldSprite = new Sprite(spriteSheet, 128, 16, 128, 48);
            var returnButtonSprite = new Sprite(spriteSheet, 128, 80, 128, 48);
            var returnButtonPosition = new Vector2(TicTacToe.ScreenWidth - returnButtonSprite.RenderWidth + 50,
                TicTacToe.ScreenHeight - returnButtonSprite.RenderHeight + -400);
            var returnButton = new Button(returnButtonSprite, _font, returnButtonPosition, returnButtonHeldSprite);
            
            Buttons.Add(returnButton);
            
            returnButton.OnClick += () =>
            {
                RequestScreenClose?.Invoke();
            };
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            
            spriteBatch.DrawString(_font, Return,
                new Vector2(TicTacToe.ScreenWidth / 2.0f - -267, TicTacToe.ScreenHeight / 2.0f - 215), Color.OrangeRed);

            spriteBatch.DrawString(_credits, Line1,
                new Vector2(TicTacToe.ScreenWidth / 2.0f - 100, TicTacToe.ScreenHeight / 2.0f - 200), Color.Chocolate);
            spriteBatch.DrawString(_credits, Line2,
                new Vector2(TicTacToe.ScreenWidth / 2.0f - 35, TicTacToe.ScreenHeight / 2.0f - 90), Color.Chocolate);
            spriteBatch.DrawString(_credits, Line3,
                new Vector2(TicTacToe.ScreenWidth / 2.0f - 20, TicTacToe.ScreenHeight / 2.0f - -30), Color.Chocolate);
            spriteBatch.DrawString(_credits, Line4,
                new Vector2(TicTacToe.ScreenWidth / 2.0f - 100, TicTacToe.ScreenHeight / 2.0f + 60), Color.Chocolate);
            spriteBatch.DrawString(_credits, Line5,
                new Vector2(TicTacToe.ScreenWidth / 2.0f - 75, TicTacToe.ScreenHeight / 2.0f + 145), Color.Chocolate);
            spriteBatch.DrawString(_credits, Line6,
                new Vector2(TicTacToe.ScreenWidth / 2.0f - 60, TicTacToe.ScreenHeight / 2.0f + 175), Color.Chocolate);
            
            spriteBatch.End();
        }
        
        protected override void OnUpdate(GameTime gameTime)
        {
        }
    }
}