using TicTacToe.Graphics;

namespace TicTacToe.Entities
{
    public class PlayerToken
    {
        public Sprite Sprite { get; }
        public int Id { get; }

        private int _points;
        public int Points => _points;

        public PlayerToken(Sprite sprite, int ID)
        {
            Sprite = sprite;
            Id = ID;
        }

        public void AwardPoint(int point)
        {
            _points += point;
        }
        
    }
}
