using Get.The.Milk.X.Library.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.The.Milk.X.Library.TileEngine
{
    public enum CameraMode { Free, Follow }

    public class Camera
    {
        #region Field Region

        Vector2 position;
        float speed;
        float zoom;
        Rectangle viewportRectangle;
        private TileMap _tileMap;
        CameraMode mode;

        #endregion

        #region Property Region

        public Vector2 Position
        {
            get { return position; }
            private set { position = value; }
        }

        public float Speed
        {
            get { return speed; }
            set
            {
                speed = (float)MathHelper.Clamp(speed, 1f, 16f);
            }
        }

        public float Zoom
        {
            get { return zoom; }
        }

        public CameraMode CameraMode
        {
            get { return mode; }
        }


        #endregion

        #region Constructor Region

        public Camera(Rectangle viewportRect,TileMap tileMap)
        {
            speed = 4f;
            zoom = 1f;
            viewportRectangle = viewportRect;
            _tileMap = tileMap;
            mode = CameraMode.Follow;

        }

        public Camera(Rectangle viewportRect, Vector2 position, TileMap tileMap)
        {
            speed = 4f;
            zoom = 1f;
            viewportRectangle = viewportRect;
            Position = position;
            _tileMap = tileMap;
            mode = CameraMode.Follow;
        }

        #endregion

        #region Method Region

        public void Update(GameTime gameTime)
        {
            if (mode == CameraMode.Follow)
                return;

            Vector2 motion = Vector2.Zero;

            if (InputHandler.KeyDown(Keys.Left) ||
                InputHandler.ButtonDown(Buttons.RightThumbstickLeft, PlayerIndex.One))
                motion.X = -speed;
            else if (InputHandler.KeyDown(Keys.Right) ||
                InputHandler.ButtonDown(Buttons.RightThumbstickRight, PlayerIndex.One))
                motion.X = speed;

            if (InputHandler.KeyDown(Keys.Up) ||
                InputHandler.ButtonDown(Buttons.RightThumbstickUp, PlayerIndex.One))
                motion.Y = -speed;
            else if (InputHandler.KeyDown(Keys.Down) ||
                InputHandler.ButtonDown(Buttons.RightThumbstickDown, PlayerIndex.One))
                motion.Y = speed;

            if (motion != Vector2.Zero)
            {
                motion.Normalize();
                position += motion * speed;
                LockCamera();
            }
        }

        private void LockCamera()
        {
            position.X = MathHelper.Clamp(position.X,
                0,
                _tileMap.WidthInPixels - viewportRectangle.Width);
            position.Y = MathHelper.Clamp(position.Y,
                0,
                _tileMap.HeightInPixels - viewportRectangle.Height);
        }

        public void LockToSprite(AnimatedSprite sprite)
        {
            position.X = sprite.Position.X + sprite.Width / 2
                            - (viewportRectangle.Width / 2);
            position.Y = sprite.Position.Y + sprite.Height / 2
                            - (viewportRectangle.Height / 2);
            LockCamera();
        }

        public void ToggleCameraMode()
        {
            if (mode == CameraMode.Follow)
                mode = CameraMode.Free;
            else if (mode == CameraMode.Free)
                mode = CameraMode.Follow;
        }
        #endregion
    }
}
