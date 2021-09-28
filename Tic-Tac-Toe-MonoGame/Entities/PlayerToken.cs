using TicTacToe.Graphics;

namespace TicTacToe.Entities
{
    public class PlayerToken
    {
        public Sprite Sprite { get; }
        public int Id { get; }

        public int Points { get; private set; }

        public PlayerToken(Sprite sprite, int id)
        {
            Sprite = sprite;
            Id = id;
        }

        public void AwardPoint(int point)
        {
            Points += point;
        }
        
    }
}
