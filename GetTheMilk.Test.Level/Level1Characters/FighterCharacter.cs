using GetTheMilk.Actions;
using GetTheMilk.Actions.Interactions;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Utils;

namespace GetTheMilk.TestLevel.Level1Characters
{
    public class FighterCharacter:Character
    {
        public FighterCharacter():base()
        {
            BlockMovement = true;
            Walet.MaxCapacity = 1000;
            Walet.CurrentCapacity = 500;
        }

        public override Personality Personality
        {
            get
            {
                base.Personality.Type = PersonalityType.Foey;
                if(!base.Personality.InteractionRules.ContainsKey(GenericInteractionRulesKeys.CharacterSpecific))
                {
                    base.Personality.InteractionRules.Add(GenericInteractionRulesKeys.CharacterSpecific,new ActionReaction[]
                                                      {
                                                          new ActionReaction{Action=new Meet(),Reaction=new Attack()},
                                                          new ActionReaction
                                                              {
                                                                  Action = new Quit(),
                                                                  Reaction = new Attack()
                                                              },
                                                          new ActionReaction
                                                              {
                                                                  Action = new Quit(),
                                                                  Reaction = new Quit()
                                                              }
                                                      });
                }
                if(!base.Personality.InteractionRules.ContainsKey(GenericInteractionRulesKeys.PlayerResponses))
                {
                    base.Personality.InteractionRules.Add(GenericInteractionRulesKeys.PlayerResponses,
                                                          new ActionReaction[]
                                                              {
                                                                  new ActionReaction
                                                                      {
                                                                          Action = new Quit(),
                                                                          Reaction = new Attack()
                                                                      }
                                                              });
                }
                return base.Personality;
            }
        }

        public override string Name
        {
            get { return "Baddie"; }
        }

    }
}
