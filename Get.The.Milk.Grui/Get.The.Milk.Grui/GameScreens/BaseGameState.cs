using Get.The.Milk.X.Library;
using Get.The.Milk.X.Library.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TomShane.Neoforce.Controls;

namespace Get.The.Milk.Grui.GameScreens
{
    public abstract partial class BaseGameState : GameState
    {
        #region Fields region

        protected GetTheMilkGameUI GameRef;

        protected ControlManager ControlManager;

        protected PlayerIndex PlayerIndexInControl;

        protected Manager Manager;

        #endregion

        #region Constructor Region

        protected BaseGameState(Game game, GameStateManager manager)
            : base(game, manager)
        {
            GameRef = (GetTheMilkGameUI)game;

            PlayerIndexInControl = PlayerIndex.One;
        }

        #endregion

        #region XNA Method Region

        protected override void LoadContent()
        {
            ContentManager content = Game.Content;

            var menuFont = content.Load<SpriteFont>(@"Fonts\ControlFont");
            ControlManager = new ControlManager(menuFont);
            base.LoadContent();
            Manager = new Manager(GameRef, GameRef.Graphics);

            Manager.SkinDirectory = @"Content\Skins\";
            Manager.SetSkin("Default");

        }

        public override void Initialize()
        {
            base.Initialize();
            Manager.Initialize();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Manager.Update(gameTime);

        }
        #endregion
    }
}
