﻿using Get.The.Milk.X.Library;
using Get.The.Milk.X.Library.Controls;
using GetTheMilk.UI.ViewModels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TomShane.Neoforce.Controls;

namespace Get.The.Milk.Grui.GameScreens
{
    public class TitleScreen : BaseGameState
    {
        #region Field region

        Texture2D backgroundImage;
        private TitleViewModel _viewModel;

        private Manager _manager;
        private Button _button;

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
            GameRef.IsMouseVisible = true;
            ContentManager Content = GameRef.Content;
            backgroundImage = Content.Load<Texture2D>(@"Backgrounds\titlescreen");

            base.LoadContent();

            ControlManager.Add(new Get.The.Milk.X.Library.Controls.Label { Position = new Vector2(350, 100), Value = _viewModel.Title, Color = Color.Black });

            _manager = new Manager(GameRef, GameRef.Graphics);

            _manager.SkinDirectory = @"Content\Skins\";
            _manager.SetSkin("Default");

        }

        public override void Initialize()
        {
            base.Initialize();
            _manager.Initialize();

            _button = new Button(_manager);
            _button.Init();
            _button.Text = _viewModel.ContinueText;
            _button.Width = 172;
            _button.Height = 24;
            _button.Left = 350;
            _button.Top = 400;
            _button.Anchor = Anchors.Bottom;
            _button.Focused = true;
            _button.Click += _button_Click;

            _manager.Add(_button);
        }

        void _button_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            StateManager.PushState(GameRef.StartMenuScreen);
        }
        public override void Update(GameTime gameTime)
        {
            ControlManager.Update(gameTime, PlayerIndex.One);

            base.Update(gameTime);
            _manager.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _manager.BeginDraw(gameTime);
            GameRef.SpriteBatch.Begin();

            base.Draw(gameTime);

            GameRef.SpriteBatch.Draw(
                backgroundImage,
                GameRef.ScreenRectangle,
                Color.White);

            ControlManager.Draw(GameRef.SpriteBatch);

            GameRef.SpriteBatch.End();
            _manager.EndDraw();
        }

        #endregion

    }
}
