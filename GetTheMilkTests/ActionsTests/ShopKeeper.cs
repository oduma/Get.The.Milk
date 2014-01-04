using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Actions.Interactions;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.Utils;

namespace GetTheMilkTests.ActionsTests
{
    public class ShopKeeper:Character
    {
        public override string Name
        {
            get { return "Shop Keeper"; }
        }

        public override bool AllowsAction(GameAction a)
        {
            return false;
        }

        public override bool AllowsIndirectAction(GameAction a,IPositionableObject o)
        {
                if (a is Buy && ((o is AnyKey) || (o is ScrewDriver)))
                    return true;
                if (a is Meet)
                    return true;
                return false;
        }

        public ShopKeeper()
        {
            BlockMovement = false;
        }

        public override Personality Personality
        {
            get
            {
                base.Personality.Type = PersonalityType.Neutral;
                if (!base.Personality.InteractionRules.ContainsKey(GenericInteractionRulesKeys.CharacterSpecific))
                {
                    base.Personality.InteractionRules.Add(GenericInteractionRulesKeys.CharacterSpecific,
                                                          new ActionReaction[]
                                                              {
                                                                  new ActionReaction
                                                                      {
                                                                          Action = new CommunicateAction(),
                                                                          Reaction =
                                                                              new CommunicateAction
                                                                                  {
                                                                                      Message =
                                                                                          "I'm just a poor shop keeper. Do you want those sunglasses?"
                                                                                  }
                                                                      },
                                                                  new ActionReaction
                                                                      {
                                                                          Action = new Attack(),
                                                                          Reaction =
                                                                              new CommunicateAction
                                                                                  {Message = "Why oh Why!?"}
                                                                      }
                                                              });
                }
                return base.Personality;
            }
        }
    }
}
