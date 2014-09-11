using Get.The.Milk.X.Library.Characters;
using Get.The.Milk.X.Library.Objects;
using Get.The.Milk.X.Library.TileEngine;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Common;
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
        public event EventHandler<PointAndClickEventArgs> PointAndClick;
        private Game _game;
        Engine engine = new Engine(32, 32);
        private Rectangle? _sourceRectangle;
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
            _sourceRectangle = sourceRectangle;
            Characters = Level.Characters.Select(c => new XCharacter(c, _game)).ToList();
            Objects = Level.Inventory.Select(o => new XObject(o, _game, _sourceRectangle)).ToList();
        }

        #endregion

        #region Method Region

        public void Update(GameTime gameTime)
        {
            ReEvaluateLevel();
            if(InputHandler.MouseClick())
            {
                var pointAndClickActions = GetPointAndClickActions(InputHandler.MouseState.X, InputHandler.MouseState.Y);
                if(PointAndClick != null)
                    PointAndClick(this, new PointAndClickEventArgs(pointAndClickActions,
                        new Point(InputHandler.MouseState.X, InputHandler.MouseState.Y)));
            }
            foreach (var character in Characters)
                character.Update(gameTime);

            foreach (var xObj in Objects)
                xObj.Update(gameTime);
        }

        private IEnumerable<object> GetPointAndClickActions(int x, int y)
        {
            if ((x >= 0 && y <= Map.WidthInPixels) && (y >= 0 && y <= Map.HeightInPixels))
            {
                var cellNumber = Map.GetCellFromPoint(new Point(x, y));
                foreach (var xChar in Characters.Where(xc => xc.Reachable && xc.Character.CellNumber == cellNumber))
                {
                    if (!xChar.AvailableActions.Any())
                        yield return xChar.Character.Name.Description;
                    else
                    foreach (var act in xChar.AvailableActions)
                        yield return act;
                    
                }
                foreach (var xObj in Objects.Where(xo => xo.Reachable && xo.Object.CellNumber == cellNumber))
                {
                    if (!xObj.AvailableActions.Any())
                        yield return xObj.Object.Name.Description;
                    foreach (var act in xObj.AvailableActions)
                        yield return act;
                }
            }
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

        private void ReEvaluateLevel()
        {
            if(Characters.Count!=Level.Characters.Count)
            {
                foreach( var xChar in Characters.Where(x=>!Level.Characters.Contains(x.Character)))
                {
                    Characters.Remove(xChar);
                }
                foreach (var character in Level.Characters.Where(c => !Characters.Any(x=>x.Character==c)))
                {
                    var xChar= new XCharacter(character,_game);
                    xChar.LoadContent();
                    Characters.Add(xChar);
                }
            }
            if(Objects.Count != Level.Inventory.Count)
            {
                var objectsGone=Objects.Where(x=>!Level.Inventory.Contains(x.Object)).ToList();
                foreach( var xObj in objectsGone)
                {
                    Objects.Remove(xObj);
                }
                foreach (var obj in Level.Inventory.Where(c => !Objects.Any(x=>x.Object==c)))
                {
                    var xObj= new XObject(obj,_game,_sourceRectangle);
                    xObj.LoadContent();
                    Objects.Add(xObj);
                }
            }
        }
    }
}
