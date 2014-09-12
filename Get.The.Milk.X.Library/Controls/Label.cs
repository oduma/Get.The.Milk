using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.The.Milk.X.Library.Controls
{
    public class Label : Control
    {
        #region Constructor Region

        public Label()
        {
            TabStop = false;
        }

        #endregion

        #region Abstract Methods

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(SpriteFont, ((Value!=null)?Value.ToString():string.Empty), Position, Color);
        }

        public override void HandleInput(PlayerIndex playerIndex)
        {
        }

        #endregion
    }
}
