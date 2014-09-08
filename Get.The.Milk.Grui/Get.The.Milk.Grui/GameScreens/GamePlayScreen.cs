using Get.The.Milk.Grui.Components;
using Get.The.Milk.X.Library;
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
            _playerComponent.LoadContent(GameRef);
            _xLevel.LoadContent();
            
            GameRef.CurrentLevelTileMap = _xLevel.Map;
            _playerComponent.Camera = new Camera(GameRef.ScreenRectangle, _xLevel.Map);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
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

            _playerComponent.Draw(gameTime, GameRef.SpriteBatch);
            _xLevel.Draw(gameTime, GameRef.SpriteBatch, _playerComponent.Camera);
            base.Draw(gameTime);

            GameRef.SpriteBatch.End();
        }

        #endregion

        private RpgGameCore _rpgGameCore;
        public RpgGameCore RpgGameCore 
        { 
            get 
            { 
                return _rpgGameCore; 
            } 
            set 
            { 
                _rpgGameCore = value;
                _playerComponent = new PlayerComponent(GameRef,_rpgGameCore);
                _xLevel = new XLevel(GameRef, RpgGameCore.CurrentLevel, GameRef.ScreenRectangle);
            }
        }
    }
}
