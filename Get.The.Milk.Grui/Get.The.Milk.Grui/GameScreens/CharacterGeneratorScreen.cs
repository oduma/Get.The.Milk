using Get.The.Milk.X.Library;
using Get.The.Milk.X.Library.Controls;
using GetTheMilk.UI.ViewModels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.The.Milk.Grui.GameScreens
{
    public class CharacterGeneratorScreen : BaseGameState
    {
        #region Field Region

        LeftRightSelector walletSelector;
        LeftRightSelector experienceSelector;
        PictureBox backgroundImage;

        string[] walletItems = { "10", "20","50","100" };
        string[] experienceItems = { "20", "10", "5", "0" };

        #endregion

        #region Property Region
        #endregion

        #region Constructor Region

        public CharacterGeneratorScreen(GetTheMilkGameUI game, GameStateManager stateManager)
            : base(game, stateManager)
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

            CreateControls();
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

            ControlManager.Draw(GameRef.SpriteBatch);

            GameRef.SpriteBatch.End();
        }

        #endregion

        #region Method Region

        private void CreateControls()
        {
            Texture2D leftTexture = Game.Content.Load<Texture2D>(@"GUI\leftarrowUp");
            Texture2D rightTexture = Game.Content.Load<Texture2D>(@"GUI\rightarrowUp");
            Texture2D stopTexture = Game.Content.Load<Texture2D>(@"GUI\StopBar");

            backgroundImage = new PictureBox(
                Game.Content.Load<Texture2D>(@"Backgrounds\titlescreen"),
                GameRef.ScreenRectangle);
            ControlManager.Add(backgroundImage);

            Label label1 = new Label();

            label1.Value = "Who will search for the Eyes of the Dragon?";
            label1.Size = label1.SpriteFont.MeasureString(label1.Value.ToString());
            label1.Position = new Vector2(400, 150);

            ControlManager.Add(label1);

            walletSelector = new LeftRightSelector(leftTexture, rightTexture, stopTexture);
            walletSelector.SetItems(walletItems, 125);
            walletSelector.Position = new Vector2(label1.Position.X, 200);

            ControlManager.Add(walletSelector);

            experienceSelector = new LeftRightSelector(leftTexture, rightTexture, stopTexture);
            experienceSelector.SetItems(experienceItems, 125);
            experienceSelector.Position = new Vector2(label1.Position.X, 250);

            ControlManager.Add(experienceSelector);

            LinkLabel linkLabel1 = new LinkLabel();
            linkLabel1.Value = "Accept this character.";
            linkLabel1.Position = new Vector2(label1.Position.X, 300);
            linkLabel1.Selected += new EventHandler(linkLabel1_Selected);

            ControlManager.Add(linkLabel1);

            ControlManager.NextControl();
        }

        void linkLabel1_Selected(object sender, EventArgs e)
        {
            InputHandler.Flush();

            StateManager.PopState();
            StateManager.PushState(GameRef.GamePlayScreen);
        }

        #endregion
    }
}
