﻿using GetTheMilk.GameLevels;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.The.Milk.X.Library.TileEngine
{
    public static class ExtensionMethods
    {
        public static Vector2 LockToMap(this Vector2 position,TileMap tileMap,int width, int height)
        {
            position.X = MathHelper.Clamp(position.X, 0, tileMap.WidthInPixels - width);
            position.Y = MathHelper.Clamp(position.Y, 0, tileMap.HeightInPixels - height);
            return position;
        }

        public static int ConvertToCellNumber(this Vector2 position, int mapSize, int width, int height,Direction direction)
        {
            var pos = new Vector2(position.X, position.Y);
            switch(direction)
            {
                case Direction.East:
                    pos.X += width;
                    break;
                case Direction.South:
                    pos.Y += height;
                    break;
                case Direction.West:
                    pos.X -= width;
                    break;
                case Direction.North:
                    pos.Y -= height;
                    break;
            }
            var point = Engine.VectorToCell(pos);
            return point.Y * mapSize + point.X;
        }
    }
}
