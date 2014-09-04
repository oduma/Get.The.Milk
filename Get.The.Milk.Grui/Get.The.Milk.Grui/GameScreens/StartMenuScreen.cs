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
        Button _startGame;
        Button _loadGame;
        Button _exitGame;
        Button _saveGame;
        private StartMenuViewModel _viewModel;

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

            _startGame = new Button(Manager);
            _startGame.Init();
            _startGame.Text = _viewModel.StartNew;
            _startGame.Width = 172;
            _startGame.Height = 24;
            _startGame.Left = 350;
            _startGame.Top = 200;
            _startGame.Anchor = Anchors.Bottom;
            _startGame.Focused = true;
            _startGame.Click += _startGame_Click;

            Manager.Add(_startGame);

            _loadGame = new Button(Manager);
            _loadGame.Init();
            _loadGame.Text = _viewModel.Load;
            _loadGame.Width = 172;
            _loadGame.Height = 24;
            _loadGame.Left = 350;
            _loadGame.Top = 230;
            _loadGame.Anchor = Anchors.Bottom;
            _loadGame.Focused = true;
            _loadGame.Click += _loadGame_Click;

            Manager.Add(_loadGame);

            _saveGame = new Button(Manager);
            _saveGame.Init();
            _saveGame.Text = _viewModel.Save;
            _saveGame.Width = 172;
            _saveGame.Height = 24;
            _saveGame.Left = 350;
            _saveGame.Top = 260;
            _saveGame.Anchor = Anchors.Bottom;
            _saveGame.Focused = true;
            _saveGame.Click += _saveGame_Click;

            Manager.Add(_saveGame);

            _exitGame = new Button(Manager);
            _exitGame.Init();
            _exitGame.Text = _viewModel.Exit;
            _exitGame.Width = 172;
            _exitGame.Height = 24;
            _exitGame.Left = 350;
            _exitGame.Top = 290;
            _exitGame.Anchor = Anchors.Bottom;
            _exitGame.Focused = true;
            _exitGame.Click += _exitGame_Click;

            Manager.Add(_exitGame);
        }

        private void _exitGame_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            GameRef.Exit();
        }

        private void _saveGame_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void _loadGame_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void _startGame_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            StateManager.PushState(GameRef.CharacterGeneratorScreen);
        }

        protected override void LoadContent()
        {
            backgroundImage = GameRef.Content.Load<Texture2D>(@"Backgrounds\titlescreen");

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Manager.BeginDraw(gameTime);
            GameRef.SpriteBatch.Begin();
            base.Draw(gameTime);
            GameRef.SpriteBatch.Draw(
                backgroundImage,
                GameRef.ScreenRectangle,
                Color.White);

            GameRef.SpriteBatch.End();
            Manager.EndDraw();
        }

        
        #endregion
    }
}
