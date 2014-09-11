using Get.The.Milk.Grui.Components;
using Get.The.Milk.X.Library;
using Get.The.Milk.X.Library.Controls;
using Get.The.Milk.X.Library.Sprites;
using Get.The.Milk.X.Library.TileEngine;
using Get.The.Milk.X.Library.World;
using GetTheMilk.GameLevels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.The.Milk.Grui.GameScreens
{
    [System.Runtime.InteropServices.GuidAttribute("21E67048-CC3B-4A37-81BF-4BD356AC431A")]
    public class GamePlayScreen : BaseGameState
    {
        #region Field Region

        private PlayerComponent _playerComponent;
        private XLevel _xLevel;

        #endregion

        #region Property Region
        #endregion

        #region Constructor Region

        public GamePlayScreen(Game game, GameStateManager manager)
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
            _xLevel.LoadContent();
            _playerComponent.LoadContent(GameRef);
            
            GameRef.CurrentLevelTileMap = _xLevel.Map;
            _playerComponent.Camera = new Camera(GameRef.ScreenRectangle, _xLevel.Map);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            var displayTime = DateTime.Now.Subtract(_startDisplayActionTime);
            if (displayTime.Seconds > 2)
                ControlManager.Clear();
            ControlManager.Update(gameTime, PlayerIndex.One);
            _playerComponent.Update(gameTime);
            _xLevel.Update(gameTime);
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

            _xLevel.Draw(gameTime, GameRef.SpriteBatch, _playerComponent.Camera);
            _playerComponent.Draw(gameTime, GameRef.SpriteBatch);
            ControlManager.Draw(GameRef.SpriteBatch);

            base.Draw(gameTime);

            GameRef.SpriteBatch.End();
        }

        #endregion

        private RpgGameCore _rpgGameCore;
        private DateTime _startDisplayActionTime;
        public RpgGameCore RpgGameCore 
        { 
            get 
            { 
                return _rpgGameCore; 
            } 
            set 
            { 
                _rpgGameCore = value;
                _xLevel = new XLevel(GameRef, RpgGameCore.CurrentLevel, GameRef.ScreenRectangle);
                _xLevel.PointAndClick -= _xLevel_PointAndClick;
                _xLevel.PointAndClick += _xLevel_PointAndClick;
                _playerComponent = new PlayerComponent(GameRef, _rpgGameCore,_xLevel);
                _playerComponent.RequestScreenChange += _playerComponent_RequestScreenChange;
            }
        }

        void _playerComponent_RequestScreenChange(object sender, RequestScreenChangeEventArgs e)
        {
            StateManager.PopState();
            StateManager.PushState(e.TargetScreen);
        }

        void _xLevel_PointAndClick(object sender, PointAndClickEventArgs e)
        {
            if (e.Actions.Any() || e.Messages.Any())
                _startDisplayActionTime = DateTime.Now;
            var startingPosition = new Vector2(e.ClickSource.X, e.ClickSource.Y);
            foreach(var action in e.Actions)
            {
                var linkLabel = new Get.The.Milk.X.Library.Controls.ActionLinkLabel { Position = startingPosition, Value = action.ToString(), Color = Color.Black,AssociatedAction=action,Size=ControlManager.SpriteFont.MeasureString(action.ToString()) };
                linkLabel.OnActionSelected += linkLabel_OnActionSelected;
                ControlManager.Add(linkLabel);
                startingPosition.Y += linkLabel.Size.Y;
            }
            foreach (var message in e.Messages)
            {
                var label = new Get.The.Milk.X.Library.Controls.Label { Position = new Vector2(e.ClickSource.X, e.ClickSource.Y), Value = message.ToString(), Color = Color.Black, Size = ControlManager.SpriteFont.MeasureString(message) };
                ControlManager.Add(label);
                startingPosition.Y += label.Size.Y;
            }

        }

        void linkLabel_OnActionSelected(object sender, ActionSelectedEventArgs e)
        {
            RpgGameCore.Player.PerformAction(e.Action);
        }

    }
}
