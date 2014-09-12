using Get.The.Milk.X.Library.TileEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.The.Milk.X.Library.Sprites
{
    public class AnimatedSprite
    {
        #region Field Region

        Dictionary<AnimationKey, Animation> _animations;
        float _speed = 2.0f;
        Vector2 _velocity;
        #endregion

        #region Property Region

        public Texture2D Texture { get; set; }

        public AnimationKey CurrentAnimation
        {
            get;
            set;
        }

        public bool IsAnimating
        {
            get;
            set;
        }

        public int Width
        {
            get { return _animations[CurrentAnimation].FrameWidth; }
        }

        public int Height
        {
            get { return _animations[CurrentAnimation].FrameHeight; }
        }

        public float Speed
        {
            get { return _speed; }
            set { _speed = MathHelper.Clamp(_speed, 1.0f, 16.0f); }
        }

        public Vector2 Position
        {
            get;
            set;
        }

        public Vector2 Velocity
        {
            get { return _velocity; }
            set
            {
                _velocity = value;
                if (_velocity != Vector2.Zero)
                    _velocity.Normalize();
            }
        }

        #endregion

        #region Constructor Region

        public AnimatedSprite(Texture2D sprite, Dictionary<AnimationKey, Animation> animation)
        {
            Texture = sprite;
            _animations = new Dictionary<AnimationKey, Animation>();

            foreach (AnimationKey key in animation.Keys)
                _animations.Add(key, (Animation)animation[key].Clone());

        }

        public AnimatedSprite(Texture2D sprite,Dictionary<AnimationKey,Animation> animation, Point tile,int verticalIndent)
            : this(sprite, animation)
        {
            this.Position = new Vector2(
                tile.X * Engine.TileWidth,
                tile.Y * Engine.TileHeight +verticalIndent);
        }

        #endregion

        #region Method Region

        public void Update(GameTime gameTime)
        {
            if (IsAnimating)
                _animations[CurrentAnimation].Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Camera camera)
        {
            spriteBatch.Draw(
                Texture,
                Position - camera.Position,
                _animations[CurrentAnimation].CurrentFrameRect,
                Color.White);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                Texture,
                Position,
                _animations[CurrentAnimation].CurrentFrameRect,
                Color.White);
        }

        #endregion
    }
}
