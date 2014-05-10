using System.Collections.ObjectModel;
using System.Linq;
using GetTheMilk.BaseCommon;
using GetTheMilk.Factories;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.LevelBuilder.ViewModels
{
    public class WeaponObjectViewModel:ObjectViewModelBase<NonCharacterObject>
    {
        public WeaponObjectViewModel(Weapon weapon)
        {
            if(weapon.Name==null)
                weapon.Name=new Noun();
            if(weapon.WeaponTypes==null)
                weapon.WeaponTypes=new WeaponType[0];
            Value = weapon;
            
            var objectTypes = ObjectActionsFactory.GetFactory().ListAllRegisterNames(ObjectCategory.Weapon);
            if(AllObjectTypes==null)
                AllObjectTypes= new ObservableCollection<string>();
            foreach (var objectType in objectTypes)
                AllObjectTypes.Add(objectType);
        }

        private NonCharacterObject _value;
        public override NonCharacterObject Value
        {
            get { return _value; }
            set
            {
                if (value != _value)
                {
                    _value = value;
                    RaisePropertyChanged("Value");
                }
            }
        }

        public override ObjectViewModelBase<NonCharacterObject> Clone()
        {

            return
                new WeaponObjectViewModel(new Weapon
                                              {
                                                  AllowsAction = Value.AllowsAction,
                                                  AllowsIndirectAction = Value.AllowsIndirectAction,
                                                  AttackPower = ((Weapon) Value).AttackPower,
                                                  BlockMovement = ((Weapon) Value).BlockMovement,
                                                  BuyPrice = ((Weapon) Value).BuyPrice,
                                                  CloseUpMessage = ((Weapon) Value).CloseUpMessage,
                                                  DefensePower = ((Weapon) Value).DefensePower,
                                                  Durability = ((Weapon) Value).Durability,
                                                  Name =
                                                      new Noun
                                                          {
                                                              Main = ((Weapon) Value).Name.Main,
                                                              Narrator = ((Weapon) Value).Name.Narrator
                                                          },
                                                  ObjectTypeId = ((Weapon) Value).ObjectTypeId,
                                                  SellPrice = ((Weapon) Value).SellPrice,
                                                  WeaponTypes = ((Weapon) Value).WeaponTypes
                                              });
        }


        public bool IsAttackEnabled
        {
            get { return ((Weapon) Value).WeaponTypes.Any(w=>w==WeaponType.Attack); }
            set
            {
                if(((Weapon) Value).WeaponTypes.Any(w=>w==WeaponType.Attack)!=value)
                {
                    var weaponTypes = ((Weapon)Value).WeaponTypes.ToList();
                    if (value)
                    {
                        weaponTypes.Add(WeaponType.Attack);
                    }
                    else
                    {
                        weaponTypes.Remove(WeaponType.Attack);
                    }
                    ((Weapon)Value).WeaponTypes = weaponTypes.ToArray();
                    RaisePropertyChanged("IsAttackEnabled");
                }
            }
        }
        public bool IsDefenseEnabled
        {
            get { return ((Weapon)Value).WeaponTypes.Any(w => w == WeaponType.Deffense); }
            set
            {
                if (((Weapon)Value).WeaponTypes.Any(w => w == WeaponType.Deffense) != value)
                {
                    var weaponTypes = ((Weapon)Value).WeaponTypes.ToList();
                    if (value)
                    {
                        weaponTypes.Add(WeaponType.Deffense);
                    }
                    else
                    {
                        weaponTypes.Remove(WeaponType.Deffense);
                    }
                    ((Weapon)Value).WeaponTypes = weaponTypes.ToArray();
                    RaisePropertyChanged("IsDefenseEnabled");
                }
            }
        }

    }
}
