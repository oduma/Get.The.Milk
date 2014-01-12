using System.Collections;
using GetTheMilk.Actions;
using GetTheMilk.Characters;
using GetTheMilk.Navigation;
using NUnit.Framework;

namespace GetTheMilkTests.TranslatorTests
{
    public class DataGeneratorForTranslator
    {
        public static IEnumerable TestCasesForTranslatorMovement
        {
            get
            {
                yield return
                    new TestCaseData(
                        new ActionResult
                            {ResultType = ActionResultType.Ok, ForAction = new Walk {Direction = Direction.South}},
                        Player.GetNewInstance()).Returns("You walked South.");
            }
        }
    }
}
