using System;
using GetTheMilk.Actions;
using GetTheMilk.Characters;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Settings;

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
            return (int)Math.Ceiling((double) weaponPower*CalculateExperienceCoefficient(((double) GameSettings.GetInstance().MaximumExperience/
                                                ((double) ((currentExperience == 0) ? 1.0 : currentExperience)))));

        }

        private static double CalculateExperienceCoefficient(double relativeExperience)
        {
            if (relativeExperience > 4.0)
                return 0.5;
            if(relativeExperience>2.0)
                return 1.0;
            if(relativeExperience>1.33)
                return 1.5;
            if (relativeExperience > 1.0)
                return 2.0;
            return 0.0;
        }

        //1/2weaponPower * by relativeExperience where relativeExperince is:
        //Novice=0 ~ 1/4 of maximum experience = coefficient 0.5
        //Medium=1/4+1 ~ 1/2 of maximum experience = coeffidient 1
        //Advanced=1/2+1 ~ 3/4 of maximum experience = coefficient 1.5
        //Expert = 3/4+1 ~ maximum experince =coefficient 2
        public static int CalculateDefensePower(int weaponPower, int currentExperience)
        {
            return (int)Math.Ceiling(((double)weaponPower/2.0) * CalculateExperienceCoefficient(((double)GameSettings.GetInstance().MaximumExperience /
                                                ((double)((currentExperience == 0) ? 1.0 : currentExperience)))));

        }



        public static void CalculateDamages(Hit hit, Hit counterHit, ICharacter attacker, ICharacter defender)
        {
            if (counterHit == null)
            {
                hit.WithWeapon.Durability--;
                defender.Health -= hit.Power;
            }
            else
            {
                defender.Health -= (hit.Power - counterHit.Power);
                counterHit.WithWeapon.Durability--;
                hit.WithWeapon.Durability--;
                attacker.Health -= (counterHit.Power);
            }
        }



    }
}
