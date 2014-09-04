using Get.The.Milk.X.Library.TileEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.The.Milk.Grui.Components
{
    public class PlayerComponent
    {

        #region Property Region

        public Camera Camera{ get; set;}

        #endregion

        #region Constructor Region

        public PlayerComponent(Game game)
        {
            GameRef = (GetTheMilkGameUI)game;
            Camera = new Camera(GameRef.ScreenRectangle,GameRef.CurrentLevelTileMap);
        }

        #endregion

        #region Method Region

        public void Update(GameTime gameTime)
        {
            Camera.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }

        #endregion

        public GetTheMilkGameUI GameRef { get; set; }
    }
}
