using System;
using System.Collections.Generic;
using System.Linq;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Characters.Base;
using GetTheMilk.Common;
using GetTheMilk.GameLevels;
using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Utils
{
    public static class CalculationStrategies
    {
        //weaponPower * by relativeExperience where relativeExperince is:
        //Novice=0 ~ 1/4 of maximum experience = coefficient 0.5
        //Medium=1/4+1 ~ 1/2 of maximum experience = coeffidient 1
        //Advanced=1/2+1 ~ 3/4 of maximum experience = coefficient 1.5
        //Expert = 3/4+1 ~ maximum experince =coefficient 2
        public static int CalculateAttackPower(int weaponPower, int currentExperience)
        {
            return (int)Math.Ceiling((double) weaponPower*CalculateExperienceCoefficient(
                                                ((double) ((currentExperience == 0) ? 1.0 : currentExperience))/(double) GameSettings.GetInstance().MaximumExperience));

        }

        private static double CalculateExperienceCoefficient(double relativeExperience)
        {
            if (relativeExperience == 1.0d) //maximum experience has been reached
                return 4.0;
            if(relativeExperience>0.5) //half of experience or more have been reached
                return 3.0;
            if(relativeExperience>0.25) //quarter of experience or more have been reached 
                return 2.0;
            if (relativeExperience > 0.1)//ten percent of experience have been reached
                return 1;
            if (relativeExperience > 0.001)//some experience
                return 0.5;
            return 0.25;
        }

        //1/2weaponPower * by relativeExperience where relativeExperince is:
        //Novice=0 ~ 1/4 of maximum experience = coefficient 0.5
        //Medium=1/4+1 ~ 1/2 of maximum experience = coeffidient 1
        //Advanced=1/2+1 ~ 3/4 of maximum experience = coefficient 1.5
        //Expert = 3/4+1 ~ maximum experince =coefficient 2
        public static int CalculateDefensePower(int weaponPower, int currentExperience)
        {
            return (int)Math.Ceiling(((double)weaponPower/2.0) * CalculateExperienceCoefficient(
                                                ((double)((currentExperience == 0) ? 1.0 : currentExperience))/(double)GameSettings.GetInstance().MaximumExperience));

        }

        public static void CalculateDamages(Hit hit, Hit counterHit, ICharacter attacker, ICharacter defender)
        {
            if(hit==null || counterHit==null)
                throw new Exception("Hits and counter hits cannot be null");
            
            defender.Health -= (hit.Power - counterHit.Power);
            counterHit.WithWeapon.Durability--;
            hit.WithWeapon.Durability--;
            attacker.Health -= (counterHit.Power);
        }


        public static Weapon SelectAWeapon(ICharacter character,WeaponType weaponType)
        {
            if (!character.Inventory.Any(w => w.ObjectCategory == ObjectCategory.Weapon))
                return null;
            return
                character.Inventory.Where(w => (w.ObjectCategory == ObjectCategory.Weapon))
                    .Select(w => (Weapon) w).FirstOrDefault(w => w.WeaponTypes.Contains(weaponType));
        }

        //take all the experience if the fromCharacter is at least as experienced as the toCharacter
        //take half of the experience if the fromCharacter is least than toCharacter but not lesser than 1/2
        //take a quater of the experience if from Character is least than half the toCharacter
        public static int CalculateWinExperience(ICharacter toCharacter, ICharacter fromCharacter)
        {
            if(fromCharacter.Experience>=toCharacter.Experience)
                return fromCharacter.Experience;
            if (fromCharacter.Experience <= toCharacter.Experience / 2)
                return fromCharacter.Experience/4;
            return fromCharacter.Experience/2;
        }

        public static bool CalculateSuccessOrFailure(ChanceOfSuccess chanceOfSuccess, int experience)
        {
            int idx = Randomizer.GetRandom(0, 99);

            return AllChances[chanceOfSuccess][idx];
        }

        private static Dictionary<ChanceOfSuccess, bool[]> _allChances;
 
        public static Dictionary<ChanceOfSuccess,bool[]>  AllChances
        {
            get
            {
                if(_allChances==null)
                {
                    bool[] vsChances = new bool[100];
                    vsChances[37] = true;
                    vsChances[42] = true;
                    vsChances[3] = true;
                    vsChances[53] = true;
                    vsChances[61] = true;
                    vsChances[79] = true;
                    vsChances[93] = true;
                    vsChances[88] = true;
                    vsChances[49] = true;
                    vsChances[17] = true;

                    bool[] sChances = new bool[100];
                    sChances[32] = true;
                    sChances[51] = true;
                    sChances[7] = true;
                    sChances[13] = true;
                    sChances[25] = true;
                    sChances[58] = true;
                    sChances[69] = true;
                    sChances[83] = true;
                    sChances[99] = true;
                    sChances[0] = true;
                    sChances[9] = true;
                    sChances[24] = true;
                    sChances[45] = true;
                    sChances[62] = true;
                    sChances[75] = true;
                    sChances[80] = true;
                    sChances[40] = true;
                    sChances[39] = true;
                    sChances[13] = true;
                    sChances[52] = true;
                    sChances[2] = true;
                    sChances[6] = true;
                    sChances[17] = true;
                    sChances[36] = true;
                    sChances[92] = true;

                    bool[] hChances = new bool[100];

                    for (int i = 0; i < 100; i++)
                    {
                        if (i % 2 == 0)
                        {
                            hChances[i] = true;
                        }
                    }

                    bool[] bChances = new bool[100];
                    for (int i = 0; i < 100; i++)
                    {
                        bChances[i] = !sChances[i];
                    }

                    bool[] vbChances = new bool[100];
                    for (int i = 0; i < 100; i++)
                    {
                        vbChances[i] = !vsChances[i];
                    }

                    bool[] fChances = new bool[100];
                    for (int i = 0; i < 100; i++)
                        fChances[i] = true;

                    _allChances = new Dictionary<ChanceOfSuccess, bool[]>
                                      {
                                          {ChanceOfSuccess.None, new bool[100]},
                                          {ChanceOfSuccess.VerySmall, vsChances},
                                          {ChanceOfSuccess.Small, sChances},
                                          {ChanceOfSuccess.Half, hChances},
                                          {ChanceOfSuccess.Big, bChances},
                                          {ChanceOfSuccess.VeryBig, vbChances},
                                          {ChanceOfSuccess.Full, fChances}
                                      };
                }

                return _allChances;
            }
        }

        public static int SelectAWeightedRandomTemplateAction(int start, int stop, string identifier)
        {
            if (identifier == "Quit")
                return stop;
            return Randomizer.GetRandom(start, stop);
        }
    }
}
