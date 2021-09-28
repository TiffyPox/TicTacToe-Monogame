using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TicTacToe.Screens;

namespace TicTacToe
{
    public class SoundSystem
    {
        private bool _isMuted;

        private SoundEffect _currentSoundEffect;
        private SoundEffectInstance _activeSoundEffect;
        
        public void Play(SoundEffect soundEffect)
        {
            if (_isMuted)
            {
                return;
            }
            
            if (_currentSoundEffect != null && soundEffect == _currentSoundEffect)
            {
                _activeSoundEffect?.Play();
                return;
            }
            
            _activeSoundEffect?.Pause();
            _activeSoundEffect = soundEffect.CreateInstance();
            _activeSoundEffect.Play();
            _currentSoundEffect = soundEffect;
        }

        public void Stop() => _activeSoundEffect?.Pause();

        private void Start() => _activeSoundEffect?.Play();

        public void Mute(bool isMuted)
        {
            _isMuted = isMuted;

            if (isMuted)
            {
                Stop();
            }
            else
            {
                Start();
            }
        }

        public void Restart() => _activeSoundEffect = _currentSoundEffect.CreateInstance();
    }
    public class TicTacToe : Game
    {
        private const string GameTitle = "Tiff-Tic-Tac-Toe";
        public static int ScreenWidth;
        public static int ScreenHeight;

        private readonly GraphicsDeviceManager _graphics;

        private SpriteBatch _spriteBatch;

        private readonly Stack<IScreen> _screens = new Stack<IScreen>();
        private readonly SoundSystem _soundSystem = new SoundSystem();

        public TicTacToe()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            ScreenWidth = _graphics.PreferredBackBufferWidth;
            ScreenHeight = _graphics.PreferredBackBufferHeight;
        }

        protected override void Initialize()
        {
            base.Initialize();

            Window.Title = GameTitle;

            // Add the menu screen to the stack
            AddScreen(new MenuScreen(_graphics, _soundSystem));
        }

        private void AddScreen(IScreen screen)
        {
            screen.Load(Content);
            screen.Initialize();
            screen.OnShow();
            screen.AddScreen += AddScreen;
            screen.RequestScreenClose += PopScreen;
            _screens.Push(screen);
        }

        private void PopScreen()
        {
            _screens.Pop();
            _screens.Peek()?.OnShow();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _screens.Peek()?.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(_screens.Peek().BackgroundColor);

            _screens.Peek()?.Draw(_spriteBatch);

            base.Draw(gameTime);
        }
    }
}
