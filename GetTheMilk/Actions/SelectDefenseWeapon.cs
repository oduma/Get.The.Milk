using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.BaseCommon;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Actions
{
    public class SelectDefenseWeapon:OneObjectAction
    {
                        public SelectDefenseWeapon()
        {
            Name = new Verb {Infinitive = "To Select Defense Weapon", Past = "selected defense weapon", Present = "select defense weapon"};
            ActionType = ActionType.SelectDefenseWeapon;
            StartingAction = false;

        }
        public override ActionResult Perform()
        {
            if (CanPerform())
            {
                ActionsHelper.SelectWeapon(ActiveCharacter,TargetObject as Weapon,WeaponType.Deffense);
                return new ActionResult
                {
                    ForAction = this,
                    ResultType = ActionResultType.Ok
                };
            }
            return new ActionResult
            {
                ForAction = this,
                ResultType = ActionResultType.NotOk
            };

        }

        public override GameAction CreateNewInstance()
        {
            return new SelectDefenseWeapon();
        }



    }
}
