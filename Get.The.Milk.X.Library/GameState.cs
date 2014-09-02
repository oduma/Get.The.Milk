using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;


namespace Get.The.Milk.X.Library
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public abstract partial class GameState : DrawableGameComponent
    {
        #region Fields and Properties

        public List<GameComponent> Components { get; private set; }

        public GameState Tag { get; private set; }

        protected GameStateManager StateManager;

        #endregion

        #region Constructor Region

        protected GameState(Game game, GameStateManager manager)
            : base(game)
        {
            StateManager = manager;

            Components = new List<GameComponent>();
            Tag = this;
        }

        #endregion

        #region XNA Drawable Game Component Methods

        public override void Update(GameTime gameTime)
        {
            foreach (GameComponent component in Components)
            {
                if (component.Enabled)
                    component.Update(gameTime);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (GameComponent component in Components)
            {
                if (component is DrawableGameComponent)
                {
                    var drawComponent = component as DrawableGameComponent;

                    if (drawComponent.Visible)
                        drawComponent.Draw(gameTime);
                }
            }

            base.Draw(gameTime);
        }

        #endregion

        #region GameState Method Region

        internal protected virtual void StateChange(object sender, EventArgs e)
        {
            if (StateManager.CurrentState == Tag)
                Show();
            else
                Hide();
        }

        protected virtual void Show()
        {
            Visible = true;
            Enabled = true;
            foreach (GameComponent component in Components)
            {
                component.Enabled = true;
                if (component is DrawableGameComponent)
                    ((DrawableGameComponent)component).Visible = true;
            }
        }

        protected virtual void Hide()
        {
            Visible = false;
            Enabled = false;
            foreach (GameComponent component in Components)
            {
                component.Enabled = false;
                if (component is DrawableGameComponent)
                    ((DrawableGameComponent)component).Visible = false;
            }
        }

        #endregion
    }
}
