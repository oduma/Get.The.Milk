using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Get.The.Milk.X.Library.Controls
{
    public abstract class Control
    {
        #region Event Region

        public event EventHandler Selected;

        #endregion

        #region Property Region

        public string Name
        {
            get;
            set;
        }

        public Vector2 Size
        {
            get;
            set;
        }

        public Vector2 Position
        {
            get;
            set;
        }

        public object Value
        {
            get;
            set;
        }

        public bool HasFocus
        {
            get;
            set;
        }

        public bool Enabled
        {
            get;
            set;
        }

        public bool Visible
        {
            get;
            set;
        }

        public bool TabStop
        {
            get;
            set;
        }

        public SpriteFont SpriteFont
        {
            get;
            set;
        }

        public Color Color
        {
            get;
            set;
        }

        public string Type
        {
            get;
            set;
        }

        #endregion

        #region Constructor Region

        public Control()
        {
            Color = Color.White;
            Enabled = true;
            Visible = true;
            SpriteFont = ControlManager.SpriteFont;
        }

        #endregion

        #region Abstract Methods

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void HandleInput(PlayerIndex playerIndex);

        #endregion

        #region Virtual Methods

        protected virtual void OnSelected(EventArgs e)
        {
            if (Selected != null)
            {
                Selected(this, e);
            }
        }

        #endregion
    }
}
