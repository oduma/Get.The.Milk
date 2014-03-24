using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.UI;

namespace GetTheMilkTests.IntegrationTests
{
    public class StubTheInteractivity:IInteractivity
    {
        public NonCharacterObject[] SelectAnObject(NonCharacterObject[] availableObjects)
        {
            return availableObjects;
        }

        public ICharacter SelectACharacter(ICharacter[] availableCharacters)
        {
            throw new NotImplementedException();
        }

        public GameAction SelectAnAction(GameAction[] availableObjects, ICharacter targetCharacter)
        {
            return availableObjects[0];
        }

        public GameAction SelectAnActionAndAnObject(ExposeInventoryExtraData exposeInvetoryActionExtraData)
        {
            return new Buy {TargetObject = ((ExposeInventoryExtraData) exposeInvetoryActionExtraData).Contents.First()};
        }

        public void SelectWeapons(IEnumerable<Weapon> weapons, ref Weapon activeAttackWeapon, ref Weapon activeDefenseWeapon)
        {
            weapons.ForEach(w => w.IsCurrentAttack = false);
            activeAttackWeapon = weapons.FirstOrDefault(w => w.WeaponTypes.Contains(WeaponType.Attack));
            if (activeAttackWeapon != null)
                activeAttackWeapon.IsCurrentAttack = true;

            weapons.ForEach(w => w.IsCurrentDefense = false);
            activeDefenseWeapon = weapons.FirstOrDefault(w => w.WeaponTypes.Contains(WeaponType.Deffense));
            if (activeDefenseWeapon != null)
                activeDefenseWeapon.IsCurrentAttack = true;

        }

        public GameAction[] BuildActions(ExposeInventoryExtraData exposeInvetoryActionExtraData)
        {
            List<GameAction> gameActions=new List<GameAction>();
            //for(int i=0;i<exposeInvetoryActionExtraData.PossibleUses.Length;i++)
            //{
            //    if(exposeInvetoryActionExtraData.PossibleUses[i] is TakeFrom)
            //    {
            //        for(int j=0;j<exposeInvetoryActionExtraData.Contents.Length;j++)
            //        {
            //            gameActions.Add(new TakeFrom{UseableObject=exposeInvetoryActionExtraData.Contents[i]});
            //        }
            //    }
            //    if(exposeInvetoryActionExtraData.PossibleUses[i] is TakeMoneyFrom)
            //    {
            //        gameActions.Add(new TakeMoneyFrom{Amount=exposeInvetoryActionExtraData.Money});
            //    }
            //    if(exposeInvetoryActionExtraData.PossibleUses[i] is Kill)
            //    {
            //        gameActions.Add(new Kill());
            //    }
            //}
            return gameActions.ToArray();
        }
    }
}
