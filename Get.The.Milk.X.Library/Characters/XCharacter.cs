using Get.The.Milk.X.Library.Sprites;
using Get.The.Milk.X.Library.TileEngine;
using GetTheMilk.Characters.Base;
using GetTheMilk.GameLevels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.The.Milk.X.Library.Characters
{
    public class XCharacter
    {
        private Game _game;
        #region Property Region

        public Character Character
        {
            get;
            private set;
        }

        public AnimatedSprite Sprite
        {
            get;
            private set;
        }

        #endregion

        #region Constructor Region

        public XCharacter(Character character,Game game)
        {
            Character = character;
            _game = game;
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
            var spriteName= @"CharacterSprites\" + ((string.IsNullOrEmpty(Character.SpriteName))
                ? "malefighter" : Character.SpriteName);
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

            Sprite = new AnimatedSprite(spriteSheet, animations, Engine.CellToPoint(Character.CellNumber, (int)((Level)Character.StorageContainer.Owner).SizeOfLevel));

        }
        #endregion
    }
}
