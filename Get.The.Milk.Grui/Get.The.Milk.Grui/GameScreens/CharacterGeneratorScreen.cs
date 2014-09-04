using Get.The.Milk.X.Library;
using Get.The.Milk.X.Library.TileEngine;
using GetTheMilk.Characters;
using GetTheMilk.Factories;
using GetTheMilk.GameLevels;
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
    public class CharacterGeneratorScreen : BaseGameState
    {
        #region Field Region


        Texture2D backgroundImage;

        Label _nameLabel;
        TextBox _nameText;
        Label _moneyLabel;
        TrackBar _moneyBar;
        Label _experienceLabel;
        TrackBar _experienceBar;
        Button _useCharacter;


        private Player _player;
        private int _maximumAvailableBonusPoints;

        #endregion

        #region Constructor Region

        public CharacterGeneratorScreen(GetTheMilkGameUI game, GameStateManager stateManager, Player player)
            : base(game, stateManager)
        {
            _player = player;
            _maximumAvailableBonusPoints = GameSettings.GetInstance().MaximumAvailableBonusPoints;

        }

        #endregion

        #region XNA Method Region

        public override void Initialize()
        {
            base.Initialize();

            CreateControls();
        }

        private void CreateControls()
        {
            _nameLabel = new Label(Manager);
            _nameLabel.Init();
            _nameLabel.Text = "Name";
            _nameLabel.Width = 180;
            _nameLabel.Height = 24;
            _nameLabel.Left = 350;
            _nameLabel.Top = 200;
            _nameLabel.Anchor = Anchors.Bottom;
            _nameLabel.TextColor = Color.Red;
            Manager.Add(_nameLabel);

            _nameText = new TextBox(Manager);
            _nameText.Init();
            _nameText.Width = 180;
            _nameText.Height = 24;
            _nameText.Left = 530;
            _nameText.Top = 200;
            _nameText.Anchor = Anchors.Bottom;
            Manager.Add(_nameText);

            _moneyLabel = new Label(Manager);
            _moneyLabel.Init();
            _moneyLabel.Text = "Money (" + _maximumAvailableBonusPoints/2+ ")";
            _moneyLabel.Width = 180;
            _moneyLabel.Height = 24;
            _moneyLabel.Left = 350;
            _moneyLabel.Top = 230;
            _moneyLabel.Anchor = Anchors.Bottom;
            _moneyLabel.TextColor = Color.Red;
            Manager.Add(_moneyLabel);

            _moneyBar = new TrackBar(Manager);
            _moneyBar.Init();
            _moneyBar.Width = 180;
            _moneyBar.Height = 24;
            _moneyBar.Left = 530;
            _moneyBar.Top = 230;
            _moneyBar.Anchor = Anchors.Bottom;
            _moneyBar.Movable = true;
            _moneyBar.Scale = true;
            _moneyBar.StepSize = 10;
            _moneyBar.Value = _maximumAvailableBonusPoints/2;
            _moneyBar.ValueChanged += _moneyBar_ValueChanged;
            Manager.Add(_moneyBar);

            _experienceLabel = new Label(Manager);
            _experienceLabel.Init();
            _experienceLabel.Text = "Experience (" + _maximumAvailableBonusPoints / 2 + ")";
            _experienceLabel.Width = 180;
            _experienceLabel.Height = 24;
            _experienceLabel.Left = 350;
            _experienceLabel.Top = 260;
            _experienceLabel.Anchor = Anchors.Bottom;
            _experienceLabel.TextColor = Color.Red;
            Manager.Add(_experienceLabel);

            _experienceBar = new TrackBar(Manager);
            _experienceBar.Init();
            _experienceBar.Width = 180;
            _experienceBar.Height = 24;
            _experienceBar.Left = 530;
            _experienceBar.Top = 260;
            _experienceBar.Anchor = Anchors.Bottom;
            _experienceBar.Movable = true;
            _experienceBar.Scale = true;
            _experienceBar.StepSize = 10;
            _experienceBar.Value = _maximumAvailableBonusPoints / 2;
            _experienceBar.ValueChanged += _experienceBar_ValueChanged;
            Manager.Add(_experienceBar);

            _useCharacter = new Button(Manager);
            _useCharacter.Init();
            _useCharacter.Width = 180;
            _useCharacter.Height = 24;
            _useCharacter.Left = 530;
            _useCharacter.Top = 290;
            _useCharacter.Anchor = Anchors.Bottom;
            _useCharacter.Text = "Start Game";
            _useCharacter.Click += _useCharacter_Click;
            Manager.Add(_useCharacter);


            
        }

        void _useCharacter_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            _player.SetPlayerName(_nameText.Text);
            _player.Walet.CurrentCapacity = _moneyBar.Value;
            _player.Experience = _experienceBar.Value;
            var objAction = ObjectActionsFactory.CreateObjectAction("Player");
            _player.AllowsTemplateAction = objAction.AllowsTemplateAction;
            _player.AllowsIndirectTemplateAction = objAction.AllowsIndirectTemplateAction;
            _player.Health = GameSettings.GetInstance().FullDefaultHealth;

            RpgGameCore gameCore = RpgGameCore.CreateNewGameInstance();
            gameCore.Player = _player;
            gameCore.Player.EnterLevel(gameCore.CurrentLevel);
            GameRef.GamePlayScreen.RpgGameCore = gameCore;

            StateManager.PopState();
            StateManager.PushState(GameRef.GamePlayScreen);


        }

        private void _experienceBar_ValueChanged(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            _experienceLabel.Text = "Experience (" + ((TrackBar)sender).Value + ")";
            _moneyBar.Value = _maximumAvailableBonusPoints - ((TrackBar)sender).Value;
            _moneyLabel.Text = "Money (" + _moneyBar.Value + ")";
        }

        void _moneyBar_ValueChanged(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            _moneyLabel.Text = "Money (" + ((TrackBar)sender).Value + ")";
            _experienceBar.Value = _maximumAvailableBonusPoints - ((TrackBar)sender).Value;
            _experienceLabel.Text = "Experience (" + _experienceBar.Value + ")";
        }

        protected override void LoadContent()
        {
            ContentManager content = GameRef.Content;
            backgroundImage = content.Load<Texture2D>(@"Backgrounds\titlescreen");

            base.LoadContent();

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Manager.BeginDraw(gameTime);
            GameRef.SpriteBatch.Begin();
            base.Draw(gameTime);
            GameRef.SpriteBatch.Draw(
                backgroundImage,
                GameRef.ScreenRectangle,
                Color.White);

            GameRef.SpriteBatch.End();
            Manager.EndDraw();
        }

        #endregion

    }
}
