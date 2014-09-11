using Get.The.Milk.Grui.GameScreens;
using Get.The.Milk.X.Library;
using Get.The.Milk.X.Library.Sprites;
using Get.The.Milk.X.Library.TileEngine;
using Get.The.Milk.X.Library.World;
using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Characters;
using GetTheMilk.GameLevels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.The.Milk.Grui.Components
{
    public class PlayerComponent
    {
        public event EventHandler<RequestScreenChangeEventArgs> RequestScreenChange;
        #region Property Region

        public Camera Camera
        { 
            get; 
            set;
        }

        AnimatedSprite _sprite;
        private RpgGameCore _rpgGameCore;
        MovementActionTemplate _teleport;
        private XLevel _xLevel;
        private ExposeInventoryActionTemplate _selfInventory;

        #endregion

        #region Constructor Region

        public PlayerComponent(Game game,RpgGameCore rpgGameCore,XLevel xLevel)
        {
            GameRef = (GetTheMilkGameUI)game;
            Camera = new Camera(GameRef.ScreenRectangle,GameRef.CurrentLevelTileMap);
            _rpgGameCore = rpgGameCore;
            _teleport=_rpgGameCore.Player.CreateNewInstanceOfAction<MovementActionTemplate>("Teleport");
            _xLevel = xLevel;
            _selfInventory = _rpgGameCore.Player.CreateNewInstanceOfAction<ExposeInventoryActionTemplate>("ExposeInventory");

        }

        #endregion

        #region Method Region

        public void Update(GameTime gameTime)
        {
            Camera.Update(gameTime);
            _sprite.Update(gameTime);
            var direction = Direction.None;

            Vector2 motion = new Vector2();
            _teleport.CurrentMap = _rpgGameCore.CurrentLevel.CurrentMap;
            if (InputHandler.KeyDown(Keys.W) ||
                InputHandler.ButtonDown(Buttons.LeftThumbstickUp, PlayerIndex.One))
            {
                _sprite.CurrentAnimation = AnimationKey.Up;
                direction = Direction.North;
                motion.Y = -1;
            }
            else if (InputHandler.KeyDown(Keys.S) ||
                InputHandler.ButtonDown(Buttons.LeftThumbstickDown, PlayerIndex.One))
            {
                _sprite.CurrentAnimation = AnimationKey.Down;
                direction = Direction.South;
                motion.Y = 1;
            }

            if (InputHandler.KeyDown(Keys.A) ||
                InputHandler.ButtonDown(Buttons.LeftThumbstickLeft, PlayerIndex.One))
            {
                _sprite.CurrentAnimation = AnimationKey.Left;
                direction = Direction.West;
                motion.X = -1;
            }
            else if (InputHandler.KeyDown(Keys.D) ||
                InputHandler.ButtonDown(Buttons.LeftThumbstickRight, PlayerIndex.One))
            {
                _sprite.CurrentAnimation = AnimationKey.Right;
                direction = Direction.East;
                motion.X = 1;
            }
            if(InputHandler.KeyDown(Keys.I))
            {
                motion = Vector2.Zero;
                var actionResult = _rpgGameCore.Player.PerformAction(_selfInventory);
                if(actionResult.ResultType==ActionResultType.Ok)
                {
                    GameRef.InventoryScreen.InventoryExtraData = (InventoryExtraData)actionResult.ExtraData;
                    if (RequestScreenChange != null)
                        RequestScreenChange(this, new RequestScreenChangeEventArgs(GameRef.InventoryScreen));
                }
            }
            if (motion != Vector2.Zero)
            {

                    _sprite.IsAnimating = true;
                    motion.Normalize();

                    var targetPosition = _sprite.Position + motion * _teleport.DefaultDistance;
                    targetPosition = targetPosition.LockToMap(GameRef.CurrentLevelTileMap,_sprite.Width,_sprite.Height);
                    var newCellNumber = targetPosition.ConvertToCellNumber(
                        (int)_rpgGameCore.CurrentLevel.SizeOfLevel,
                        _sprite.Width,
                        _sprite.Height,
                        direction);
                    if (newCellNumber != _rpgGameCore.Player.CellNumber)
                    {
                        _teleport.TargetCell = newCellNumber;
                        var actionResult=_rpgGameCore.Player.PerformAction(_teleport);
                        MarkReachable((MovementActionTemplateExtraData)actionResult.ExtraData);
                        if(actionResult.ResultType==ActionResultType.Ok)
                        {
                            _sprite.Position = targetPosition;
                        }
                    }
                    else
                    {
                        _sprite.Position = targetPosition;
                    }
                    if (Camera.CameraMode == CameraMode.Follow)
                        Camera.LockToSprite(_sprite);
            }
            else
            {
                _sprite.IsAnimating = false;
            }
            if (InputHandler.KeyReleased(Keys.F) ||
                InputHandler.ButtonReleased(Buttons.RightStick, PlayerIndex.One))
            {
                Camera.ToggleCameraMode();
                if (Camera.CameraMode == CameraMode.Follow)
                    Camera.LockToSprite(_sprite);
            }

            if (Camera.CameraMode != CameraMode.Follow)
            {
                if (InputHandler.KeyReleased(Keys.C) ||
                    InputHandler.ButtonReleased(Buttons.LeftStick, PlayerIndex.One))
                {
                    Camera.LockToSprite(_sprite);
                }
            }
        }

        private void MarkReachable(MovementActionTemplateExtraData extraData)
        {
            foreach (var xChar in _xLevel.Characters)
                xChar.Reachable = false;
            foreach (var xChar in _xLevel.Characters.Where(x=>extraData.CharactersBlocking.Contains(x.Character) || extraData.CharactersInRange.Contains(x.Character)))
            {
                xChar.AvailableActions = extraData.AvailableActionTemplates.Where(a => a.TargetCharacter == xChar.Character).ToList();
                xChar.Reachable = true;
            }
            foreach (var xObj in _xLevel.Objects)
                xObj.Reachable = false;
            foreach (var xObj in _xLevel.Objects.Where(x => extraData.ObjectsBlocking.Contains(x.Object) 
                || extraData.ObjectsInCell.Contains(x.Object)
                || extraData.ObjectsInRange.Contains(x.Object)))
            {
                xObj.AvailableActions = extraData.AvailableActionTemplates.Where(a => a.TargetObject == xObj.Object || a.ActiveObject == xObj.Object).ToList();
                xObj.Reachable = true;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _sprite.Draw(gameTime, spriteBatch,Camera);
        }

        #endregion

        public GetTheMilkGameUI GameRef { get; set; }

        internal void LoadContent(Game gameRef)
        {
            Texture2D spriteSheet = gameRef.Content.Load<Texture2D>(@"CharacterSprites\malefighter");
            Dictionary<AnimationKey, Animation> animations = new Dictionary<AnimationKey, Animation>();

            Animation animation = new Animation(3, 32, 32, 0, 0);
            animations.Add(AnimationKey.Down, animation);

            animation = new Animation(3, 32, 32, 0, 32);
            animations.Add(AnimationKey.Left, animation);

            animation = new Animation(3, 32, 32, 0, 64);
            animations.Add(AnimationKey.Right, animation);

            animation = new Animation(3, 32, 32, 0, 96);
            animations.Add(AnimationKey.Up, animation);

            _sprite = new AnimatedSprite(spriteSheet, animations);
        }

    }
}
