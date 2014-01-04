using System;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Actions.Interactions;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.Utils;

namespace GetTheMilk.TestLevel.Level1Characters
{
    public class ShopKeeperCharacter:Character
    {
        public ShopKeeperCharacter():base()
        {
            Walet.MaxCapacity = 1000;
            Walet.CurrentCapacity = 200;
            BlockMovement = true;
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
                                                                      Action = new Meet(),
                                                                      Reaction =
                                                                          new CommunicateAction
                                                                              {
                                                                                  Message =
                                                                                      "How are you? Beautifull day out there better buy something!"
                                                                              }
                                                                  },
                                                              new ActionReaction
                                                                  {
                                                                      Action = new CommunicateAction {Message = "Yes"},
                                                                      Reaction =
                                                                          new ExposeInventory{AllowedNextActions=new GameAction[]{new Buy()}}
                                                                  },
                                                              new ActionReaction
                                                                  {
                                                                      Action = new CommunicateAction {Message = "No"},
                                                                      Reaction =
                                                                          new CommunicateAction
                                                                              {Message = "Why oh Why!?"}
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
                                                                          Action =
                                                                              new CommunicateAction
                                                                                  {
                                                                                      Message =
                                                                                          "How are you? Beautifull day out there better buy something!"
                                                                                  },
                                                                          Reaction =
                                                                              new CommunicateAction {Message = "Yes"}
                                                                      },
                                                                  new ActionReaction
                                                                      {
                                                                          Action =
                                                                              new CommunicateAction
                                                                                  {
                                                                                      Message =
                                                                                          "How are you? Beautifull day out there better buy something!"
                                                                                  },
                                                                          Reaction =
                                                                              new CommunicateAction {Message = "No"}
                                                                      }

                                                              });
                }
                return base.Personality;
            }
        }
        public override string Name
        {
            get { return "John the Shop Keeper"; }
        }

        public override bool AllowsIndirectAction(GameAction a, IPositionableObject o)
        {
            return (a is Buy && o.StorageContainer.Owner.Name==Name) ||(a is Meet) || (a is CommunicateAction) || (a is Sell);
        }
    }
}
