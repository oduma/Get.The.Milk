using Get.The.Milk.X.Library.Sprites;
using Get.The.Milk.X.Library.TileEngine;
using GetTheMilk.GameLevels;
using GetTheMilk.Objects.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.The.Milk.X.Library.Objects
{
    public class XObject
    {
        private Game _game;
        private Rectangle? _sourceRectangle;
        #region Property Region

        public INonCharacterObject Object
        {
            get;
            private set;
        }

        public BaseSprite Sprite
        {
            get;
            set;
        }

        #endregion

        #region Constructor Region

        public XObject(INonCharacterObject o,Game game,Rectangle? sourceRectangle)
        {
            Object = o;
            _game = game;
            _sourceRectangle = sourceRectangle;
        }

        #endregion

        #region Method Region
        #endregion

        #region Virtual Method region

        public virtual void Update(GameTime gameTime)
        {
            Sprite.Update(gameTime);
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            Sprite.Draw(gameTime, spriteBatch);
        }

        public virtual void LoadContent()
        {
            var spriteName = @"ObjectSprites\" + ((string.IsNullOrEmpty(Object.SpriteName))
                ? "containers" : Object.SpriteName);
            Texture2D spriteSheet = _game.Content.Load<Texture2D>(spriteName);
            Dictionary<AnimationKey, Animation> animations = new Dictionary<AnimationKey, Animation>();

            Animation animation = new Animation(3, 32, 32, 0, 0);
            animations.Add(AnimationKey.Down, animation);

            animation = new Animation(3, 32, 32, 0, 32);
            animations.Add(AnimationKey.Left, animation);

            animation = new Animation(3, 32, 32, 0, 64);
            animations.Add(AnimationKey.Right, animation);

            animation = new Animation(3, 32, 32, 0, 96);
            animations.Add(AnimationKey.Up, animation);

            Sprite = new BaseSprite(spriteSheet,
                _sourceRectangle,
                Engine.CellToPoint(Object.CellNumber,(int)((Level)Object.StorageContainer.Owner).SizeOfLevel));
        }

        #endregion

        public bool Reachable { get; set; }
    }
}
