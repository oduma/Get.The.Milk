using GetTheMilk.GameLevels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Get.The.Milk.X.Library.TileEngine
{
    public class TileMap
    {
        #region Field Region

        List<Tileset> tilesets;
        private Map _map;

        #endregion

        #region Property Region
        #endregion

        #region Constructor Region

        public TileMap(List<Tileset> tilesets, Map map)
        {
            this.tilesets = tilesets;
            _map = map;
        }

        public TileMap(Tileset tileset, Map map)
        {
            tilesets = new List<Tileset>();
            tilesets.Add(tileset);

            _map = map;
        }

        #endregion

        #region Method Region

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle destination = new Rectangle(0, 0, Engine.TileWidth, Engine.TileHeight);
            Cell cell;

            for (int y = 0; y < _map.Size; y++)
            {
                destination.Y = y * Engine.TileHeight;

                for (int x = 0; x < _map.Size; x++)
                {
                    cell = _map.Cells[y * _map.Size + x];

                    destination.X = x * Engine.TileWidth;

                    spriteBatch.Draw(
                        tilesets[cell.Tileset].Texture,
                        destination,
                        tilesets[cell.Tileset].SourceRectangles[cell.TileIndex],
                        Color.White);
                }
            }
        }

        #endregion
    }
}
