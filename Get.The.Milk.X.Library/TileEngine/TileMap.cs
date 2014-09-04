using GetTheMilk.GameLevels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Get.The.Milk.X.Library.TileEngine
{
    public class TileMap
    {
        #region Field Region

        Tileset _tileset;
        private Map _map;

        #endregion

        #region Property Region

        public int WidthInPixels
        {
            get { return (int)_map.Parent.SizeOfLevel * Engine.TileWidth; }
        }

        public int HeightInPixels
        {
            get { return (int)_map.Parent.SizeOfLevel * Engine.TileHeight; }
        }


        #endregion

        #region Constructor Region

        public TileMap(Tileset tileset, Map map)
        {
            _tileset = tileset;

            _map = map;
        }

        #endregion

        #region Method Region

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            Rectangle destination = new Rectangle(0, 0, Engine.TileWidth, Engine.TileHeight);
            Cell cell;

            for (int y = 0; y < _map.Size; y++)
            {
                destination.Y = y * Engine.TileHeight - (int)camera.Position.Y;

                for (int x = 0; x < _map.Size; x++)
                {
                    cell = _map.Cells[y * _map.Size + x];
                    
                    if (cell.TileIndex == -1)
                        continue;

                    destination.X = x * Engine.TileWidth - (int)camera.Position.X;

                    spriteBatch.Draw(
                        _tileset.Texture,
                        destination,
                        _tileset.SourceRectangles[cell.TileIndex],
                        Color.White);
                }
            }
        }
        #endregion
    }
}
