using System;
using System.Linq;
using GetTheMilk.Actions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Factories;
using GetTheMilk.Levels;
using GetTheMilk.Navigation;
using GetTheMilk.Settings;
using GetTheMilk.UI;
using GetTheMilk.Utils;

namespace GetTheMilk.Characters
{
    public class Player:Character,IPlayer
    {
        private static Player _instance;
        private static object _lock = new object();

        private Player():base()
        {
            BlockMovement = true;
            Experience = GameSettings.MinimumStartingExperience;
            Walet.MaxCapacity = GameSettings.DefaultWalletMaxCapacity;
        }

        public static Player GetNewInstance()
        {
            if(_instance==null)
            {
                lock(_lock)
                {
                    if (_instance == null)
                    {
                        _instance= new Player();
                        _instance.Interactivity =
                            (new ObjectsFactory(new InteractivityProvidersInstaller())).CreateObject<IInteractivity>(
                                GameSettings.InteractiveMode);
                    }
                }
            }
            return _instance;
        }

        public static Player GetNewInstance(IInteractivity interactivityProvider)
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new Player();
                        _instance.Interactivity = interactivityProvider;
                    }
                }
            }
            return _instance;
        }

        public static void Destroy()
        {
            lock(_lock)
            {
                _instance = null;
            }
        }

        public override Personality Personality
        {
            get
            {
                base.Personality.Type=PersonalityType.Neutral;
                return base.Personality;
            }
        }

        public void LoadPlayer()
        {
            throw new NotImplementedException();
        }

        public void SavePlayer()
        {
            throw new NotImplementedException();
        }

        public void LoadInteractionsWithPlayer(ICharacter targetCharacter)
        {
            if (!base.Personality.InteractionRules.ContainsKey(targetCharacter.Name.Main) 
                && targetCharacter.Personality.InteractionRules.ContainsKey(GenericInteractionRulesKeys.PlayerResponses))
                base.Personality.InteractionRules.Add(targetCharacter.Name.Main,
                                                      targetCharacter.Personality.InteractionRules[
                                                          GenericInteractionRulesKeys.PlayerResponses]);

        }

        public ActionResult EnterLevel(ILevel level)
        {
            level.Player = this;
            MapNumber = level.StartingMap;
            CellNumber =level.StartingCell;
            return TryPerformMove(new EnterLevel {Direction = Direction.None, TargetCell = level.StartingCell},
                                  level.Maps.FirstOrDefault(m=>m.Number==level.StartingMap), level.PositionableObjects.Objects,
                                  level.Characters.Objects);
        }

        public void SetPlayerName(string name)
        {
            Name = name != null 
                ? new Noun {Main = name, Narrator=GameSettings.DefaultNarratorAddressingForPlayer} 
            : new Noun {Main = "Payer 1", Narrator=GameSettings.DefaultNarratorAddressingForPlayer};
        }
    }
}
