using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TicTacToe.Graphics;
using TicTacToe.UI;


namespace TicTacToe.Screens
{
    public class SoundScreen : BaseScreen
    {
        private SpriteFont _font;
        private SpriteFont _musicText;
        private const string Music = "Music";
        private const string Return = "Return To Menu";
        private const string On = "On";
        private const string Off = "Off";
        
        private readonly GraphicsDeviceManager _graphicsDeviceManager;
        private readonly SoundSystem _soundSystem;

        public SoundScreen(GraphicsDeviceManager graphicsDeviceManager, SoundSystem soundSystem)
        {
            _graphicsDeviceManager = graphicsDeviceManager;
            _soundSystem = soundSystem;
            BackgroundColor = new Color(25, 25, 112, 255);
        }
        
        public override void OnShow()
        { 
        }

        public override void Load(ContentManager content)
        {
            var spriteSheet = content.Load<Texture2D>("Tic-Tac-Toe-SpriteSheet");
            _font = content.Load<SpriteFont>("ReturnText");
            _musicText = content.Load<SpriteFont>("CreditText");
            
            var returnButtonHeldSprite = new Sprite(spriteSheet, 128, 16, 128, 48);
            var returnButtonSprite = new Sprite(spriteSheet, 128, 80, 128, 48);
            var returnButtonPosition = new Vector2(TicTacToe.ScreenWidth - returnButtonSprite.RenderWidth + 50,
                TicTacToe.ScreenHeight - returnButtonSprite.RenderHeight + -400);
            var returnButton = new Button(returnButtonSprite, _font, returnButtonPosition, returnButtonHeldSprite);

            var onButtonHeldSprite = new Sprite(spriteSheet, 16, 48, 64, 48);
            var onButtonSprite = new Sprite(spriteSheet, 16, 101, 64, 48);
            var onButtonPosition = new Vector2(TicTacToe.ScreenWidth / 2.0f, TicTacToe.ScreenHeight / 2.0f);
            var onButton = new Button(onButtonSprite, _font, onButtonPosition, onButtonHeldSprite);

            var offButtonHeldSprite = new Sprite(spriteSheet, 16, 48, 64, 48);
            var offButtonSprite = new Sprite(spriteSheet, 16, 101, 64, 48);
            var offButtonPosition = new Vector2(TicTacToe.ScreenWidth / 2.0f + 100, TicTacToe.ScreenHeight / 2.0f);
            var offButton = new Button(offButtonSprite, _font, offButtonPosition, offButtonHeldSprite);
            
            Buttons.Add(returnButton);
            Buttons.Add(onButton);
            Buttons.Add(offButton);
            
            
            returnButton.OnClick += OnReturnButtonClicked;
            {
                AddScreen?.Invoke(new MenuScreen(_graphicsDeviceManager, _soundSystem));
            }

            onButton.OnClick += () => _soundSystem.Mute(false);

            offButton.OnClick += () => _soundSystem.Mute(true);
        }
        
        private void OnReturnButtonClicked()
        {
            RequestScreenClose?.Invoke();
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            
            spriteBatch.DrawString(_musicText, Music, new Vector2(TicTacToe.ScreenWidth / 2.0f - 120, TicTacToe.ScreenHeight / 2.0f - 15), Color.Chocolate);
            
            spriteBatch.DrawString(_musicText, On, new Vector2(TicTacToe.ScreenWidth / 2.0f - 15, TicTacToe.ScreenHeight / 2.0f - 15), Color.Chocolate);
            
            spriteBatch.DrawString(_musicText, Off, new Vector2(TicTacToe.ScreenWidth / 2.0f - -78, TicTacToe.ScreenHeight / 2.0f - 15), Color.Chocolate);
            
            spriteBatch.DrawString(_font, Return,
                new Vector2(TicTacToe.ScreenWidth / 2.0f - -267, TicTacToe.ScreenHeight / 2.0f - 215), Color.OrangeRed);
            
            spriteBatch.End();
        }

        protected override void OnUpdate(GameTime gameTime)
        {
        }
    }
}