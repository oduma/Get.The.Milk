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
    public class SelectAttackWeapon:OneObjectAction
    {
                public SelectAttackWeapon()
        {
            Name = new Verb {Infinitive = "To Select Attack Weapon", Past = "selected attack weapon", Present = "select attack weapon"};
            ActionType = ActionType.SelectAttackWeapon;
            StartingAction = false;

        }
        public override ActionResult Perform()
        {
            if (CanPerform())
            {
                ActionsHelper.SelectWeapon(ActiveCharacter,TargetObject as Weapon,WeaponType.Attack);
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
            return new SelectAttackWeapon();
        }


    }
}
