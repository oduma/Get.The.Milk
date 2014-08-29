using Get.The.Milk.X.Library;
using Get.The.Milk.X.Library.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.The.Milk.Grui.GameScreens
{
    public abstract partial class BaseGameState : GameState
    {
        #region Fields region

        protected GetTheMilkGameUI GameRef;

        protected ControlManager ControlManager;

        protected PlayerIndex playerIndexInControl;

        #endregion

        #region Properties region
        #endregion

        #region Constructor Region

        public BaseGameState(Game game, GameStateManager manager)
            : base(game, manager)
        {
            GameRef = (GetTheMilkGameUI)game;

            playerIndexInControl = PlayerIndex.One;
        }

        #endregion

        #region XNA Method Region

        protected override void LoadContent()
        {
            ContentManager Content = Game.Content;

            SpriteFont menuFont = Content.Load<SpriteFont>(@"Fonts\ControlFont");
            ControlManager = new ControlManager(menuFont);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        #endregion
    }
}
