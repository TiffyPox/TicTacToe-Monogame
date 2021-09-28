using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TicTacToe.Entities;
using TicTacToe.Graphics;
using TicTacToe.Renderers;
using TicTacToe.UI;

namespace TicTacToe.Screens
{
    public class GameScreen : BaseScreen
    {
        private int _count;
        private int _activePlayer;
        private (int, int)[] _winningCondition;
        
        private bool _isBlocked;
        private bool _newGame;
        
        private SpriteFont _font;
        private SpriteFont _winnerText;
        private SpriteFont _replayText;
        
        private const string Return = "Return to Menu";
        private const string YouWon = "Game Over!";
        private const string YouDraw = "You Draw!";
        private const string Replay = "Press [Space] to replay";
        
        private SoundEffect _gameSong;
        private SoundEffect _winSound;
        private SoundEffectInstance _stopSound;

        private readonly GraphicsDeviceManager _graphicsDeviceManager;
        
        private readonly SoundSystem _soundSystem;

        private readonly GridRenderer _gridRenderer;
        private readonly Grid _grid;
        
        private readonly Camera _camera;
        
        private readonly Texture2D _pixel;
        private readonly Texture2D _fade;
        private Texture2D _spriteSheet;
        
        private readonly PlayerToken[] _players = new PlayerToken[2];

        private KeyboardState _previousKeyboardState;
        
        public GameScreen(GraphicsDeviceManager graphicsDeviceManager, SoundSystem soundSystem)
        {
            BackgroundColor =  new Color(174,198, 207, 0);
            _graphicsDeviceManager = graphicsDeviceManager;
            _soundSystem = soundSystem;

            _fade = new Texture2D(graphicsDeviceManager.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            _fade.SetData(new[] { Color.White });

            _gridRenderer = new GridRenderer();
            _grid = new Grid(3, 3, 32, 32);
            _camera = new Camera(
                _graphicsDeviceManager.PreferredBackBufferWidth / 2.0f - _grid.GetCellWidth() * _grid.GetWidth() * 2.0f,
                _graphicsDeviceManager.PreferredBackBufferHeight / 2.0f - _grid.GetCellHeight() * _grid.GetHeight() * 2.0f, 4.0f);
            
            _pixel = new Texture2D(graphicsDeviceManager.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            _pixel.SetData(new[] { Color.White });
        }

        public override void OnShow() => _soundSystem.Play(_gameSong);

        public override void Load(ContentManager content)
        {
            _gridRenderer.Load(content, _graphicsDeviceManager);

            _spriteSheet = content.Load<Texture2D>("Tic-Tac-Toe-SpriteSheet");
            _font = content.Load<SpriteFont>("ReturnText");
            _winnerText = content.Load<SpriteFont>("WinnerText");
            _replayText = content.Load<SpriteFont>("ReplayText");

            _gameSong = content.Load<SoundEffect>("GameSong");
            _winSound = content.Load<SoundEffect>("WinSound");

            var returnButtonHeldSprite = new Sprite(_spriteSheet, 128, 16, 128, 48);
            var returnButtonSprite = new Sprite(_spriteSheet, 128, 80, 128, 48);
            var returnButtonPosition = new Vector2(TicTacToe.ScreenWidth - returnButtonSprite.RenderWidth + 50,
                TicTacToe.ScreenHeight - returnButtonSprite.RenderHeight + -400);
            var returnButton = new Button(returnButtonSprite, _font, returnButtonPosition, returnButtonHeldSprite);

            Buttons.Add(returnButton);

            returnButton.OnClick += OnReturnButtonClicked;
            {
                AddScreen?.Invoke(new MenuScreen(_graphicsDeviceManager, _soundSystem));
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            
            _newGame = true;

            var xToken = new Sprite(_spriteSheet,18, 18, 28, 28);
            var oToken = new Sprite(_spriteSheet,50, 18, 28, 28);

            _players[0] = new PlayerToken(xToken, 0);
            _players[1] = new PlayerToken(oToken, 1);

            _activePlayer = new Random().Next(0, 2);
        }

        private void OnReturnButtonClicked()
        {
            _soundSystem.Stop();
            RequestScreenClose?.Invoke();
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            if (!_isBlocked && _newGame)
            {
                _grid.SetSelected(null);

                var inverseCameraMatrix = Matrix.Invert(_camera.GetViewMatrix());
                var mousePosition = new Vector2(CurrentMouseState.X, CurrentMouseState.Y);
                var (x, y) = Vector2.Transform(mousePosition, inverseCameraMatrix);

                var cellAtMouse = _grid.GetCellAt((int) x, (int) y);
                _grid.SetSelected(cellAtMouse);

                if (CurrentMouseState.LeftButton != ButtonState.Pressed ||
                    LastMouseState.LeftButton != ButtonState.Released) return;

                if (cellAtMouse is {IsTaken: false})
                {
                    cellAtMouse.SetToken(GetActivePlayer());
                    _count++;

                    var (winCondition, winner) = _grid.CheckForWin();

                    if (winner != null)
                    {
                        _soundSystem.Stop();
                        _winSound.Play();
                        Console.WriteLine(winner.Id == 0 ? "X WINS" : "O WINS");
                        _winningCondition = winCondition;
                        BlockInputTemporarily();
                        winner?.AwardPoint(1);
                    }
                    else
                    {
                        _activePlayer = _activePlayer == 0 ? 1 : 0;
                        CheckForDraw();
                    }
                }
            }
            else
            {
                KeyboardState keyboardState = Keyboard.GetState();

                if (keyboardState.IsKeyDown(Keys.Space) && _previousKeyboardState.IsKeyUp(Keys.Space))
                {
                    Reset();
                }

                _previousKeyboardState = keyboardState;
            }
        }

        private PlayerToken GetActivePlayer() => _players[_activePlayer];
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: _camera.GetViewMatrix());

            _gridRenderer.Render(spriteBatch, _grid);
            
            spriteBatch.DrawString(_font, $"{_players[0].Points}", new Vector2(115, 10), Color.Black);
            spriteBatch.DrawString(_font, $"{_players[1].Points}", new Vector2(-30, 10), Color.Black);

            if (_winningCondition != null)
            {
                var (winStartX, winStartY) = _winningCondition[0];
                var (winEndX, winEndY) = _winningCondition[2];
                var startCell = _grid.GetCell(winStartX, winStartY);
                var endCell = _grid.GetCell(winEndX, winEndY);
                
                var renderPositionOfStart =
                    new Vector2(startCell.X * _grid.GetCellWidth() + _grid.GetCellWidth() / 2.0f, startCell.Y * _grid.GetCellHeight() + _grid.GetCellHeight() / 2.0f);
                var renderPositionOfEnd =
                    new Vector2(endCell.X * _grid.GetCellWidth() + _grid.GetCellWidth() / 2.0f, endCell.Y * _grid.GetCellHeight() + _grid.GetCellHeight() / 2.0f);
                
                var adjacent = renderPositionOfEnd.X - renderPositionOfStart.X;
                var opposite = renderPositionOfEnd.Y - renderPositionOfStart.Y;
                var hypotenuse = Math.Sqrt(Math.Pow(adjacent, 2) + Math.Pow(opposite, 2));
                var radians = (float) Math.Atan(opposite / adjacent);
                spriteBatch.Draw(_pixel, new Rectangle((int) renderPositionOfStart.X, (int) renderPositionOfStart.Y, (int) hypotenuse, 5), null, Color.Black * 0.8f,radians, new Vector2(0, 0.5f), SpriteEffects.None, 0.0f);
            }
            if (_isBlocked || _count == 9)
            {
                FadeScreen(spriteBatch);
            }

            spriteBatch.End();
            
            // Calls in to BaseScreen.Draw() to draw all the buttons
            base.Draw(spriteBatch);
            
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            
            spriteBatch.DrawString(_font, Return,
                new Vector2(TicTacToe.ScreenWidth / 2.0f - -267, TicTacToe.ScreenHeight / 2.0f - 215), Color.OrangeRed);

            spriteBatch.End();
        }

        private void BlockInputTemporarily()
        {
            _isBlocked = true;
        }

        private void FadeScreen(SpriteBatch spriteBatch)
        {
            _newGame = false;
            spriteBatch.Draw(_fade, new Rectangle(-60, -20, TicTacToe.ScreenWidth, TicTacToe.ScreenHeight), Color.AliceBlue * 0.75f);

            if (_isBlocked)
            {
                spriteBatch.DrawString(_winnerText, YouWon,
                new Vector2(-6, 37), Color.Black);
            }

            if (_count == 9 & !_isBlocked)
            {
                spriteBatch.DrawString(_winnerText, YouDraw, new Vector2(-2, 37), Color.Black);
            }
            
            spriteBatch.DrawString(_replayText, Replay, new Vector2(-13, 60), Color.Black);
        }

        private bool CheckForDraw()
        {
            if (_count == 9)
            {
                _soundSystem.Stop();
                _winSound.Play();
            }
            
            return _count == 9;
        }

        private void Reset()
        {
            _count = 0;
            _stopSound = _winSound.CreateInstance();
            _stopSound?.Stop();
            _soundSystem.Stop();
            _winningCondition = null;
            _isBlocked = false;
            _newGame = true;
            _grid.Reset();
        }
    }
}