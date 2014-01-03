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
        public Player(IInteractivity interactivity):base()
        {
            Interactivity = interactivity;
            Initialize();
        }
        public Player():base()
        {
            Interactivity =
    (new ObjectsFactory(new InteractivityProvidersInstaller())).CreateObject<IInteractivity>(
        GameSettings.InteractiveMode);
            Initialize();

        }

        public override string Name
        {
            get { return "Me"; }
        }

        private void Initialize()
        {
            BlockMovement = true;
            Experience = 1;
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
            if (!base.Personality.InteractionRules.ContainsKey(targetCharacter.Name) 
                && targetCharacter.Personality.InteractionRules.ContainsKey(GenericInteractionRulesKeys.PlayerResponses))
                base.Personality.InteractionRules.Add(targetCharacter.Name,
                                                      targetCharacter.Personality.InteractionRules[
                                                          GenericInteractionRulesKeys.PlayerResponses]);

        }
    }
}
