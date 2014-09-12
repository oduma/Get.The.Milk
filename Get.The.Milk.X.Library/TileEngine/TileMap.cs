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
        public int VerticalIndent { get; private set; }

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

        public TileMap(Tileset tileset, Map map, int verticalIndent)
        {
            _tileset = tileset;
            VerticalIndent = verticalIndent;

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
                destination.Y = VerticalIndent + y * Engine.TileHeight - (int)camera.Position.Y;

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
            return new Vector2((float)(cellNumber % (int)Map.Parent.SizeOfLevel), VerticalIndent + (float)(cellNumber / (int)Map.Parent.SizeOfLevel));
        }

        internal int GetCellFromPoint(Point point)
        {
            var row = (point.Y - VerticalIndent)/ Engine.TileHeight;
            var col = point.X / Engine.TileWidth;

            return row * Map.Size + col;
        }
    }
}
