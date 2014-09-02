using System;
using System.Linq;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Characters.Base;
using GetTheMilk.Common;
using GetTheMilk.Objects.Base;

namespace GetTheMilk.Actions.ActionPerformers.Base
{
    public class TwoCharactersActionTemplatePerformer:BaseActionResponsePerformer,IActionTemplatePerformer
    {
        public virtual string PerformerType
        {
            get { return GetType().Name; }
        }
        public event EventHandler<FeedbackEventArgs> FeedbackFromSubAction;

        public virtual bool CanPerform(BaseActionTemplate actionTemplate)
        {
            if (actionTemplate.ActiveCharacter == null || actionTemplate.TargetCharacter == null)
                return false;
            return actionTemplate.ActiveCharacter.AllowsTemplateAction(actionTemplate)
                   && actionTemplate.TargetCharacter.AllowsIndirectTemplateAction(actionTemplate,
                                                                           actionTemplate.ActiveCharacter);

        }

        public virtual PerformActionResult Perform(BaseActionTemplate actionTemplate)
        {
            if (!CanPerform(actionTemplate))
                return new PerformActionResult { ForAction = actionTemplate, ResultType = ActionResultType.NotOk };

            return (PerformResponseAction(actionTemplate)) ??
                   new PerformActionResult
                       {
                           ForAction = actionTemplate,
                           ResultType = ActionResultType.Ok,
                           ExtraData = GetAvailableReactions(actionTemplate)
                       };

        }

        protected void PileageCharacter(Character pileager, Character pileagee)
        {
            var takeFrom =
                pileager.CreateNewInstanceOfAction<ObjectTransferActionTemplate>("TakeFrom");
            if (takeFrom != null)
            {
                var pileageeInventory = pileagee.Inventory.ToList();
                foreach (var o in pileageeInventory)
                {
                    if (pileager.Inventory.MaximumCapacity >= pileager.Inventory.Count)
                    {
                        takeFrom.TargetCharacter = pileagee;
                        if (o.ObjectCategory == ObjectCategory.Weapon)
                        {
                            if (((Weapon)o).IsCurrentAttack)
                                pileagee.ActiveAttackWeapon = null;
                            if (((Weapon)o).IsCurrentDefense)
                                pileagee.ActiveDefenseWeapon = null;
                        }

                        takeFrom.TargetObject = o;
                        PerformActionResult actionResult = pileager.PerformAction(takeFrom);
                        if (FeedbackFromSubAction != null)
                            FeedbackFromSubAction(this, new FeedbackEventArgs(actionResult));
                    }
                    else
                    {
                        break;
                    }
                }
            }

            pileagee.Walet.PerformTransaction(pileager, (pileager.Walet.MaxCapacity - pileager.Walet.CurrentCapacity >
                                        pileagee.Walet.CurrentCapacity)
                    ? pileagee.Walet.CurrentCapacity
                    : (pileager.Walet.MaxCapacity - pileager.Walet.CurrentCapacity));
        }


        public void TwoCharactersActionFeedbackFromSubAction(object sender, FeedbackEventArgs e)
        {
            if (FeedbackFromSubAction != null)
                FeedbackFromSubAction(this, e);
        }


        public string Category
        {
            get { return CategorysCatalog.TwoCharactersCategory; }
        }
    }
}
