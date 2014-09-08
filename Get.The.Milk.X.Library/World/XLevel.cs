using Get.The.Milk.X.Library.Characters;
using Get.The.Milk.X.Library.Objects;
using Get.The.Milk.X.Library.TileEngine;
using GetTheMilk.GameLevels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.The.Milk.X.Library.World
{
    public class XLevel
    {
        private Game _game;
        #region Property Region

        public TileMap Map
        {
            get;private set;
        }

        public List<XCharacter> Characters
        {
            get; private set;
        }

        public List<XObject> Objects
        {
            get; private set;
        }

        public Level Level { get; private set; }
        #endregion

        #region Constructor Region

        public XLevel(Game game, Level level, Rectangle? sourceRectangle)
        {
            Level = level;
            _game = game;
            Characters = Level.Characters.Select(c => new XCharacter(c, _game)).ToList();
            Objects = Level.Inventory.Select(o => new XObject(o, _game, sourceRectangle)).ToList();
        }

        #endregion

        #region Method Region

        public void Update(GameTime gameTime)
        {
            foreach (var character in Characters)
                character.Update(gameTime);

            foreach (var xObj in Objects)
                xObj.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Camera camera)
        {
            Map.Draw(spriteBatch, camera);

            foreach (var character in Characters)
                character.Draw(gameTime, spriteBatch);

            foreach (var xObj in Objects)
                xObj.Draw(gameTime, spriteBatch);
        }

        #endregion


        public void LoadContent()
        {
            var tilesetName = @"Tilesets\" + ((string.IsNullOrEmpty(Level.CurrentMap.Tileset))
                ? "tileset1" : Level.CurrentMap.Tileset);

            Texture2D tilesetTexture = _game.Content.Load<Texture2D>(tilesetName);
            var tileset = new Tileset(tilesetTexture, 8, 8, 32, 32);

            Map = new TileMap(tileset, Level.CurrentMap);

            foreach (var character in Characters)
                character.LoadContent();

            foreach (var xObj in Objects)
                xObj.LoadContent();

        }
    }
}
