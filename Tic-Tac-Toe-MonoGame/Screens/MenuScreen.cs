using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TicTacToe.Graphics;
using TicTacToe.UI;

namespace TicTacToe.Screens
{
    public class MenuScreen : BaseScreen
    {
        private readonly GraphicsDeviceManager _graphicsDeviceManager;
        private readonly SoundSystem _soundSystem;
        
        private Sprite _titleSprite;

        private Vector2 _titlePosition;
        private SpriteFont _font;
        private const string Play = "Play";
        private const string Credits = "Credits";
        private const string Sound = "Sound";

        private SoundEffect _menuSong;
        
        public MenuScreen(GraphicsDeviceManager graphicsDeviceManager, SoundSystem soundSystem)
        {
            _graphicsDeviceManager = graphicsDeviceManager;
            _soundSystem = soundSystem;
            BackgroundColor = new Color(25, 25, 112, 255);
        }

        public override void OnShow() => _soundSystem.Play(_menuSong);

        public override void Load(ContentManager content)
        {
            var spriteSheet = content.Load<Texture2D>("Tic-Tac-Toe-SpriteSheet");
            var titleSpriteSheet = content.Load<Texture2D>("Tic-Tac-Toe-Title");
            _font = content.Load<SpriteFont>("PlayText");

            _menuSong = content.Load<SoundEffect>("MenuSong");

            _titleSprite = new Sprite(titleSpriteSheet, 0, 16, 800, 177);
            _titlePosition = new Vector2(TicTacToe.ScreenWidth / 2.0f, 32.0f * 3.5f);
            
            var playButtonHeldSprite = new Sprite(spriteSheet, 128, 16, 128, 48);
            var playButtonSprite = new Sprite(spriteSheet, 128, 80, 128, 48);
            var playButtonPosition = new Vector2(TicTacToe.ScreenWidth / 2.0f, TicTacToe.ScreenHeight / 2.0f + 3);
            var playButton = new Button(playButtonSprite, _font, playButtonPosition, playButtonHeldSprite);

            var creditButtonHeldSprite = new Sprite(spriteSheet, 128, 16, 128, 48);
            var creditButtonSprite = new Sprite(spriteSheet, 128, 80, 128, 48);
            var creditButtonPosition = new Vector2(TicTacToe.ScreenWidth / 2.0f, TicTacToe.ScreenHeight / 2 + 70);
            var creditButton = new Button(creditButtonSprite, _font, creditButtonPosition, creditButtonHeldSprite);
            
            var soundButtonHeldSprite = new Sprite(spriteSheet, 128, 16, 128, 48);
            var soundButtonSprite = new Sprite(spriteSheet, 128, 80, 128, 48);
            var soundButtonPosition = new Vector2(TicTacToe.ScreenWidth / 2.0f, TicTacToe.ScreenHeight / 2 + 137);
            var soundButton = new Button(soundButtonSprite, _font, soundButtonPosition, soundButtonHeldSprite);

            Buttons.Add(playButton);
            Buttons.Add(creditButton);
            Buttons.Add(soundButton);

            playButton.OnClick += () =>
            {
                _soundSystem.Stop();
                AddScreen?.Invoke(new GameScreen(_graphicsDeviceManager, _soundSystem));
            };
            
            creditButton.OnClick += () =>
            {
                AddScreen?.Invoke(new CreditsScreen(_graphicsDeviceManager, _soundSystem));
            };

            soundButton.OnClick += () =>
            {
                AddScreen?.Invoke(new SoundScreen(_graphicsDeviceManager, _soundSystem));
            };
        }

        public override void Initialize()
        {
        }

        protected override void OnUpdate(GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Calls in to BaseScreen.Draw() to draw all the buttons
            base.Draw(spriteBatch);
            
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _titleSprite.Draw(spriteBatch, _titlePosition, Color.White);

            var (x, y) = _font.MeasureString(Play);
            spriteBatch.DrawString(_font, Play, new Vector2(TicTacToe.ScreenWidth / 2.0f - x / 2, TicTacToe.ScreenHeight / 2.0f - y / 2.5f), Color.Chocolate);
            
            spriteBatch.DrawString(_font, Credits, new Vector2(TicTacToe.ScreenWidth / 2.0f - x / 2 - 20, TicTacToe.ScreenHeight / 2.0f + 58), Color.Chocolate);
            
            spriteBatch.DrawString(_font, Sound, new Vector2(TicTacToe.ScreenWidth / 2.0f - x / 2 - 7, TicTacToe.ScreenHeight / 2.0f + 125), Color.Chocolate);

            spriteBatch.End();
        }
    }
}
