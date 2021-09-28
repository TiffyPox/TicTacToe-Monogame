using System;

namespace TicTacToe.Entities
{
    public class Cell
    {
        private PlayerToken _playerToken;
        public bool IsTaken => _playerToken != null;
        
        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }

        public int Y { get; }

        public void SetToken(PlayerToken playerToken) => _playerToken = playerToken;
        public PlayerToken GetToken() => _playerToken;
        
    }
}