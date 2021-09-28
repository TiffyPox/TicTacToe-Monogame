using Microsoft.Xna.Framework;

namespace TicTacToe
{
    public class Camera
    {
        private readonly float _xPosition;
        private readonly float _yPosition;
        private float _zoom;

        public Camera(float xPosition, float yPosition, float zoom)
        {
            _xPosition = xPosition;
            _yPosition = yPosition;
            _zoom = zoom;
        }
        
        public Matrix GetViewMatrix() => Matrix.CreateScale(_zoom) * Matrix.CreateTranslation(_xPosition, _yPosition, 0.0f);
    }
}