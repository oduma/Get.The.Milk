using System;
using System.Collections.Generic;
using System.Linq;
using GetTheMilk.GameLevels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Get.The.Milk.X.Library;
using Get.The.Milk.Grui.GameScreens;
using GetTheMilk.Factories;

namespace Get.The.Milk.Grui
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GetTheMilkGameUI : Microsoft.Xna.Framework.Game
    {
        #region XNA Field Region

        GraphicsDeviceManager graphics;
        public SpriteBatch SpriteBatch;

        #endregion

        #region Game State Region

        GameStateManager stateManager;

        public TitleScreen TitleScreen;
        public StartMenuScreen StartMenuScreen;
        public GamePlayScreen GamePlayScreen;
        public CharacterGeneratorScreen CharacterGeneratorScreen;


        #endregion

        #region Screen Field Region

        const int screenWidth = 1068;
        const int screenHeight = 600;

        public readonly Rectangle ScreenRectangle;

        #endregion

        public GetTheMilkGameUI()
        {

            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;

            ScreenRectangle = new Rectangle(
                0,
                0,
                screenWidth,
                screenHeight);

            Content.RootDirectory = "Content";

            Components.Add(new InputHandler(this));

            stateManager = new GameStateManager(this);
            Components.Add(stateManager);

            TitleScreen = new TitleScreen(this, stateManager);
            StartMenuScreen = new StartMenuScreen(this, stateManager);
            GamePlayScreen = new GamePlayScreen(this, stateManager);
            CharacterGeneratorScreen = new CharacterGeneratorScreen(this, stateManager);

            stateManager.ChangeState(TitleScreen);
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }

        internal void LoadNewRpgEngine()
        {
            new BootstrapRegister().RegisterAllComponents();
            RpgGameCore gameCore = RpgGameCore.CreateNewGameInstance();
        }
    }
}
