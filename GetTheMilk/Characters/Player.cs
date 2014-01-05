using System;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Factories;
using GetTheMilk.Settings;
using GetTheMilk.UI;
using GetTheMilk.Utils;

namespace GetTheMilk.Characters
{
    public class Player:Character,IPlayer
    {
        private string _name;
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

        public override string Name
        {
            get { return _name; }
            set { _name = value; }
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
            if (!base.Personality.InteractionRules.ContainsKey(targetCharacter.Name) 
                && targetCharacter.Personality.InteractionRules.ContainsKey(GenericInteractionRulesKeys.PlayerResponses))
                base.Personality.InteractionRules.Add(targetCharacter.Name,
                                                      targetCharacter.Personality.InteractionRules[
                                                          GenericInteractionRulesKeys.PlayerResponses]);

        }
    }
}
