using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.The.Milk.X.Library.TileEngine
{
    public class Engine
    {
        #region Field Region

        #endregion

        #region Property Region

        public static int TileWidth
        {
            get;
            private set;
        }

        public static int TileHeight
        {
            get;
            private set;
        }

        #endregion

        #region Constructors

        public Engine(int tileWidth, int tileHeight)
        {
            Engine.TileWidth = tileWidth;
            Engine.TileHeight = tileHeight;
        }

        #endregion

        #region Methods

        public static Point VectorToPoint(Vector2 position)
        {
            return new Point((int)position.X / TileWidth, (int)position.Y / TileHeight);
        }
        public static Point CellToPoint(int cellNumber,int mapSize)
        {
            return new Point((int)cellNumber % mapSize, (int)cellNumber / mapSize);
        }
        #endregion
    }
}
