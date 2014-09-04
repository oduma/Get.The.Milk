﻿using Get.The.Milk.Grui.Components;
using Get.The.Milk.X.Library;
using Get.The.Milk.X.Library.TileEngine;
using GetTheMilk.GameLevels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        Engine engine = new Engine(32, 32);

        Tileset tileset;
        TileMap map;
        PlayerComponent playerComponent;

        #endregion

        #region Property Region
        #endregion

        #region Constructor Region

        public GamePlayScreen(Game game, GameStateManager manager)
            : base(game, manager)
        {
            playerComponent = new PlayerComponent(game);
        }

        #endregion

        #region XNA Method Region

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            var tilesetName = @"Tilesets\" + ((string.IsNullOrEmpty(RpgGameCore.CurrentLevel.CurrentMap.Tileset)) 
                ? "tileset1" : RpgGameCore.CurrentLevel.CurrentMap.Tileset);

            Texture2D tilesetTexture = Game.Content.Load<Texture2D>(tilesetName);
            tileset = new Tileset(tilesetTexture, 8, 8, 32, 32);

            map = new TileMap(tileset, RpgGameCore.CurrentLevel.CurrentMap);
            GameRef.CurrentLevelTileMap = map;
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

            map.Draw(GameRef.SpriteBatch,playerComponent.Camera);

            base.Draw(gameTime);

            GameRef.SpriteBatch.End();
        }

        #endregion

        
        public RpgGameCore RpgGameCore { get; set; }
    }
}
