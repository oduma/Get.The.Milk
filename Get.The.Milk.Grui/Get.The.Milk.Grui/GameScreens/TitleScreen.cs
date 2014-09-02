using Get.The.Milk.X.Library;
using Get.The.Milk.X.Library.Controls;
using GetTheMilk.UI.ViewModels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.The.Milk.Grui.GameScreens
{
    public class TitleScreen : BaseGameState
    {
        #region Field region

        Texture2D backgroundImage;
        private TitleViewModel _viewModel;

        #endregion

        #region Constructor region

        public TitleScreen(Game game, GameStateManager manager,TitleViewModel viewModel)
            : base(game, manager)
        {
            _viewModel = viewModel;
        }

        #endregion

        #region XNA Method region

        protected override void LoadContent()
        {

            ContentManager Content = GameRef.Content;
            backgroundImage = Content.Load<Texture2D>(@"Backgrounds\titlescreen");

            base.LoadContent();

            ControlManager.Add(new Label { Position = new Vector2(350, 100), Value = _viewModel.Title, Color = Color.Black });
            LinkLabel startLabel = new LinkLabel { Position = new Vector2(350, 400), Value = _viewModel.ContinueText, Color = Color.White, TabStop = true, HasFocus = true };
            startLabel.Selected += new EventHandler(startLabel_Selected);
            ControlManager.Add(startLabel);
        }

        public override void Update(GameTime gameTime)
        {
            ControlManager.Update(gameTime, PlayerIndex.One);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GameRef.SpriteBatch.Begin();

            base.Draw(gameTime);

            GameRef.SpriteBatch.Draw(
                backgroundImage,
                GameRef.ScreenRectangle,
                Color.White);

            ControlManager.Draw(GameRef.SpriteBatch);

            GameRef.SpriteBatch.End();
        }

        #endregion

        #region Title Screen Methods

        private void startLabel_Selected(object sender, EventArgs e)
        {
            StateManager.PushState(GameRef.StartMenuScreen);
        }

        #endregion
    }
}
