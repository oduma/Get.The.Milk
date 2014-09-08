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
using GetTheMilk.UI.ViewModels;
using TomShane.Neoforce.Controls;
using GetTheMilk.Characters;
using Get.The.Milk.X.Library.TileEngine;

namespace Get.The.Milk.Grui
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GetTheMilkGameUI : Game
    {
        #region XNA Field Region

        public SpriteBatch SpriteBatch;

        #endregion

        #region Game State Region

        public TitleScreen TitleScreen;
        public StartMenuScreen StartMenuScreen;
        public GamePlayScreen GamePlayScreen;
        public CharacterGeneratorScreen CharacterGeneratorScreen;


        #endregion

        #region Screen Field Region

        const int ScreenWidth = 1068;
        const int ScreenHeight = 600;

        public readonly Rectangle ScreenRectangle;

        #endregion

        public GetTheMilkGameUI()
        {
            Graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = ScreenWidth,
                PreferredBackBufferHeight = ScreenHeight
            };

            
            ScreenRectangle = new Rectangle(
                0,
                0,
                ScreenWidth,
                ScreenHeight);

            Content.RootDirectory = "Content";

            Components.Add(new InputHandler(this));

            var stateManager = new GameStateManager(this);
            Components.Add(stateManager);

            new BootstrapRegister().RegisterAllComponents();

            TitleScreen = new TitleScreen(this, stateManager,new TitleViewModel());
            StartMenuScreen = new StartMenuScreen(this, stateManager,new StartMenuViewModel());
            GamePlayScreen = new GamePlayScreen(this, stateManager);
            CharacterGeneratorScreen = new CharacterGeneratorScreen(this, stateManager, new Player());

            stateManager.ChangeState(TitleScreen);
        }

        public GraphicsDeviceManager Graphics { get; set; }

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

        public TileMap CurrentLevelTileMap { get; set; }
    }
}
