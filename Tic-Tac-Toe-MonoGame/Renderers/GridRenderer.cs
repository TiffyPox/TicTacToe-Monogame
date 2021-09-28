using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using TicTacToe.Entities;

namespace TicTacToe.Renderers
{
    public sealed class GridRenderer
    {
        private Texture2D _pixel;

        public void Render(SpriteBatch spriteBatch, Grid grid)
        {
            var cellWidth = grid.GetCellWidth();
            var cellHeight = grid.GetCellHeight();
            var selectedCell = grid.GetSelectedCell();
            
            grid.ForEachCell((Cell cell) =>
            {
                var cellRenderX = cell.X * cellWidth;
                var cellRenderY = cell.Y * cellHeight;

                // Render inside
                if (cell == selectedCell)
                {
                    spriteBatch.Draw(_pixel, new Rectangle(cellRenderX, cellRenderY, cellWidth, cellHeight), Color.White * 0.9f);
                }
                else
                {
                    spriteBatch.Draw(_pixel, new Rectangle(cellRenderX, cellRenderY, cellWidth, cellHeight), Color.White * 0.5f);
                }

                // Render border
                RenderLineAt(spriteBatch, cellRenderX, cellRenderY, cellWidth, 1);
                RenderLineAt(spriteBatch, cellRenderX + cellWidth - 1, cellRenderY, 1, cellHeight);
                RenderLineAt(spriteBatch, cellRenderX, cellRenderY, 1, cellHeight);
                RenderLineAt(spriteBatch, cellRenderX, cellRenderY + cellHeight - 1, cellWidth, 1);

                var cellToken = cell.GetToken();
                if (cellToken != null)
                {
                    var cellCentre = new Vector2(cellRenderX + cellWidth / 2.0f, cellRenderY + cellHeight / 2.0f);
                    
                    cell.GetToken().Sprite?.Draw(spriteBatch, new Vector2(cellCentre.X, cellCentre.Y), Color.White);    
                }
            });
        }

        private void RenderLineAt(SpriteBatch spriteBatch, int x, int y, int w, int h)
        {
            spriteBatch.Draw(_pixel, new Rectangle(x, y, w, h), Color.Black);
        }

        internal void Load(ContentManager content, GraphicsDeviceManager graphicsDeviceManager)
        {
            Console.WriteLine("Loading Grid Renderer");
            _pixel = new Texture2D(graphicsDeviceManager.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            _pixel.SetData(new[] { Color.White });
        }
    }
}
