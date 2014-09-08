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

        #endregion

        #region Property Region

        public int WidthInPixels
        {
            get { return (int)Map.Parent.SizeOfLevel * Engine.TileWidth; }
        }

        public int HeightInPixels
        {
            get { return (int)Map.Parent.SizeOfLevel * Engine.TileHeight; }
        }

        public  Map Map{get;set;}


        #endregion

        #region Constructor Region

        public TileMap(Tileset tileset, Map map)
        {
            _tileset = tileset;

            Map = map;
        }

        #endregion

        #region Method Region

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            Rectangle destination = new Rectangle(0, 0, Engine.TileWidth, Engine.TileHeight);
            Cell cell;

            for (int y = 0; y < Map.Size; y++)
            {
                destination.Y = y * Engine.TileHeight - (int)camera.Position.Y;

                for (int x = 0; x < Map.Size; x++)
                {
                    cell = Map.Cells[y * Map.Size + x];
                    
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

        public Vector2 GetPosition(int cellNumber)
        {
            return new Vector2((float)(cellNumber % (int)Map.Parent.SizeOfLevel), (float)(cellNumber / (int)Map.Parent.SizeOfLevel));
        }
    }
}
