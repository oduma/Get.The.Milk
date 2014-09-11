using Get.The.Milk.X.Library;
using GetTheMilk.Actions.ActionTemplates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.The.Milk.Grui.GameScreens
{
    public class InventoryScreen:BaseGameState
    {
        #region Property Region
        public InventoryExtraData InventoryExtraData{get;set;}
        #endregion

        #region Constructor Region

        public InventoryScreen(Game game, GameStateManager manager)
            : base(game, manager)
        {

        }

        #endregion

        #region XNA Method Region

        public override void Initialize()
        {
            base.Initialize();
            
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GameRef.SpriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null,
                null,
                null,
                Matrix.Identity);

            base.Draw(gameTime);

            GameRef.SpriteBatch.End();
        }

        #endregion

    }
}
