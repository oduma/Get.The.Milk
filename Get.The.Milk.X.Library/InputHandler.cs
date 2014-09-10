using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;


namespace Get.The.Milk.X.Library
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class InputHandler : Microsoft.Xna.Framework.GameComponent
    {
        #region Keyboard Property Region

        public static KeyboardState KeyboardState
        {
            get;
            private set;
        }

        public static KeyboardState LastKeyboardState
        {
            get;
            private set;
        }

        #endregion

        #region Mouse Property Region

        public static MouseState MouseState
        {
            get;
            private set;
        }

        public static MouseState LastMouseState
        {
            get;
            private set;
        }

        #endregion


        #region Game Pad Property Region

        public static GamePadState[] GamePadStates
        {
            get;
            private set;
        }

        public static GamePadState[] LastGamePadStates
        {
            get;
            private set;
        }

        #endregion

        #region Constructor Region

        public InputHandler(Game game)
            : base(game)
        {
            KeyboardState = Keyboard.GetState();

            GamePadStates = new GamePadState[Enum.GetValues(typeof(PlayerIndex)).Length];

            foreach (PlayerIndex index in Enum.GetValues(typeof(PlayerIndex)))
                GamePadStates[(int)index] = GamePad.GetState(index);
            MouseState = Mouse.GetState();
        }

        #endregion

        #region XNA methods

        public override void Initialize()
        {

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            LastKeyboardState = KeyboardState;
            KeyboardState = Keyboard.GetState();

            LastGamePadStates = (GamePadState[])GamePadStates.Clone();
            foreach (PlayerIndex index in Enum.GetValues(typeof(PlayerIndex)))
                GamePadStates[(int)index] = GamePad.GetState(index);
            LastMouseState = MouseState;
            MouseState = Mouse.GetState();
            base.Update(gameTime);
        }

        #endregion

        #region General Method Region

        public static void Flush()
        {
            LastKeyboardState = KeyboardState;
            LastMouseState = MouseState;
        }

        #endregion

        #region Keyboard Region

        public static bool KeyReleased(Keys key)
        {
            return KeyboardState.IsKeyUp(key) &&
                LastKeyboardState.IsKeyDown(key);
        }

        public static bool KeyPressed(Keys key)
        {
            return KeyboardState.IsKeyDown(key) &&
                LastKeyboardState.IsKeyUp(key);
        }

        public static bool KeyDown(Keys key)
        {
            return KeyboardState.IsKeyDown(key);
        }

        #endregion

        #region Mouse Region

        public static bool MouseClick()
        {
            return MouseState.LeftButton==ButtonState.Pressed;
        }

        #endregion
        #region Game Pad Region

        public static bool ButtonReleased(Buttons button, PlayerIndex index)
        {
            return GamePadStates[(int)index].IsButtonUp(button) &&
                LastGamePadStates[(int)index].IsButtonDown(button);
        }

        public static bool ButtonPressed(Buttons button, PlayerIndex index)
        {
            return GamePadStates[(int)index].IsButtonDown(button) &&
                LastGamePadStates[(int)index].IsButtonUp(button);
        }

        public static bool ButtonDown(Buttons button, PlayerIndex index)
        {
            return GamePadStates[(int)index].IsButtonDown(button);
        }

        #endregion
    }
}
