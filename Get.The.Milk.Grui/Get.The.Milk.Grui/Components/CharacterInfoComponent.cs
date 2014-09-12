using Get.The.Milk.X.Library.Controls;
using Get.The.Milk.X.Library.Sprites;
using GetTheMilk.Characters.Base;
using GetTheMilk.GameLevels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.The.Milk.Grui.Components
{
    public class CharacterInfoComponent
    {
        private Rectangle? _sourceRectangle;
        private X.Library.Controls.Label _healthLabel;
        private ControlManager _controlManager;
        private Character _character;
        private Vector2 _position;

        public CharacterInfoComponent(Character character, Rectangle? sourceRectangle,Vector2 position)
        {
            _character = character;
            _sourceRectangle = sourceRectangle;
            _position = position;
        }

        internal void Update(GameTime gameTime)
        {
            _healthLabel.Value = _character.Health;
            _controlManager.Update(gameTime, PlayerIndex.One);

        }

        internal void LoadContent(Game gameRef,ControlManager controlManager)
        {
            Texture2D spriteSheet = gameRef.Content.Load<Texture2D>(@"CharacterSprites\health");
            _sprite = new BaseSprite(spriteSheet,_sourceRectangle,new Point((int)_position.X,(int)_position.Y),0);
            _healthLabel = new Get.The.Milk.X.Library.Controls.Label { Position = new Vector2(_position.X+36, _position.Y), Color = Color.Black };
            _controlManager = controlManager;
            _controlManager.Add(_healthLabel);

        }

        BaseSprite _sprite;

        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            _sprite.Draw(gameTime, spriteBatch);
            _controlManager.Draw(spriteBatch);
        }
    }
}
