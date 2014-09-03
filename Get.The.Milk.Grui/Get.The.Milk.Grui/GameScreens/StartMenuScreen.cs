using Get.The.Milk.X.Library;
using Get.The.Milk.X.Library.Controls;
using GetTheMilk.UI.ViewModels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TomShane.Neoforce.Controls;
using Control = Get.The.Milk.X.Library.Controls.Control;
using EventArgs = System.EventArgs;
using EventHandler = System.EventHandler;

namespace Get.The.Milk.Grui.GameScreens
{
    public class StartMenuScreen : BaseGameState
    {
        #region Field region

        Texture2D backgroundImage;
        Button startGame;
        Button loadGame;
        Button exitGame;
        Button saveGame;
        private StartMenuViewModel _viewModel;
        private Manager _manager;

        #endregion

        #region Constructor Region

        public StartMenuScreen(Game game, GameStateManager manager,StartMenuViewModel viewModel)
            : base(game, manager)
        {
            _viewModel = viewModel;
        }

        #endregion

        #region XNA Method Region

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            ContentManager content = GameRef.Content;
            backgroundImage = content.Load<Texture2D>(@"Backgrounds\titlescreen");

            base.LoadContent();
            
            _manager = new Manager(GameRef, GameRef.Graphics);

            _manager.SkinDirectory = @"Content\Skins\";
            _manager.SetSkin("Default");


            ControlManager.Add(new LinkLabel{Value = _viewModel.StartNew});

            ControlManager.Add(new LinkLabel { Value = _viewModel.Load});

            ControlManager.Add(new LinkLabel { Value = _viewModel.Save});

            ControlManager.Add(new LinkLabel { Value = _viewModel.Exit});

            ControlManager.NextControl();

            ControlManager.FocusChanged += new EventHandler(ControlManager_FocusChanged);

            Vector2 position = new Vector2(350, 200);
            foreach (Control c in ControlManager)
            {
                if (c is LinkLabel)
                {
                    if (c.Size.X > maxItemWidth)
                        maxItemWidth = c.Size.X;

                    c.Position = position;
                    position.Y += c.Size.Y + 5f;
                    c.Selected+=menuItem_Selected;
                    c.Size = c.SpriteFont.MeasureString(c.Value.ToString());
                }
            }

            ControlManager_FocusChanged(startGame, null);
        }

        void ControlManager_FocusChanged(object sender, EventArgs e)
        {
            Control control = sender as Control;
            Vector2 position = new Vector2(control.Position.X + maxItemWidth + 10f, control.Position.Y);
        }

        private void menuItem_Selected(object sender, EventArgs e)
        {
            if (sender == startGame)
            {
                StateManager.PushState(GameRef.CharacterGeneratorScreen);
            }

            if (sender == loadGame)
            {
                StateManager.PushState(GameRef.GamePlayScreen);
            }

            if (sender == exitGame)
            {
                GameRef.Exit();
            }
        }

        public override void Update(GameTime gameTime)
        {
            ControlManager.Update(gameTime, PlayerIndexInControl);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GameRef.SpriteBatch.Begin();

            base.Draw(gameTime);

            ControlManager.Draw(GameRef.SpriteBatch);

            GameRef.SpriteBatch.End();
        }

        #endregion

        #region Game State Method Region
        #endregion

    }
}
