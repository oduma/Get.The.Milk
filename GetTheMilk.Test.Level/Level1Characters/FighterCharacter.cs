using GetTheMilk.Actions;
using GetTheMilk.Actions.Interactions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Utils;

namespace GetTheMilk.Test.Level.Level1Characters
{
    public class FighterCharacter:Character
    {
        private Noun _name;

        public FighterCharacter():base()
        {
            BlockMovement = true;
            Walet.MaxCapacity = 1000;
            Walet.CurrentCapacity = 500;
            Name = new Noun { Main = "Baddie", Narrator = "the Baddie" };
        }

        public override Noun Name
        {
            get
            {
                return _name = (_name) ?? new Noun { Main = "Baddie", Narrator = "the Baddie" };
            }
            protected set { _name = value; }
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
    }
}
