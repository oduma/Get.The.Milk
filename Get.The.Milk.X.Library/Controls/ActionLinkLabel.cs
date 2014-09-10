using GetTheMilk.Actions.ActionTemplates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.The.Milk.X.Library.Controls
{
    public class ActionLinkLabel : Control
    {
        public event EventHandler<ActionSelectedEventArgs> OnActionSelected;
        #region Fields and Properties

        Color selectedColor = Color.Red;

        public BaseActionTemplate AssociatedAction { get; set; }

        public Color SelectedColor
        {
            get { return selectedColor; }
            set { selectedColor = value; }
        }

        #endregion

        #region Constructor Region

        public ActionLinkLabel()
        {
            TabStop = false;
            HasFocus = true;
            Position = Vector2.Zero;
            Color = Color.Black;
        }

        #endregion

        #region Abstract Methods

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (HasFocus)
                spriteBatch.DrawString(SpriteFont, Value.ToString(), Position, selectedColor);
            else
                spriteBatch.DrawString(SpriteFont, Value.ToString(), Position, Color);
        }

        public override void HandleInput(PlayerIndex playerIndex)
        {
            if (InputHandler.MouseClick())
                if(IsOnLinkLabel())
                if (OnActionSelected != null)
                    OnActionSelected(this, new ActionSelectedEventArgs(AssociatedAction));
        }

        private bool IsOnLinkLabel()
        {
            var horizMargin=10;
            var vertMargin =2;
            var leftStart=Position.X+horizMargin;
            var rightStop=Position.X+Size.X-horizMargin;
            var topStart=Position.Y+vertMargin;
            var bottomStop=Position.Y+Size.Y-vertMargin;

            return (InputHandler.MouseState.X > leftStart && InputHandler.MouseState.X < rightStop) && (InputHandler.MouseState.Y > topStart && InputHandler.MouseState.Y < bottomStop);
        }

        #endregion
    }
}
