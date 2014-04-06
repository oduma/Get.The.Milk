using System.Linq;
using GetTheMilk.Actions;
using GetTheMilk.Actions.BaseActions;
using GetTheMilk.Characters;
using GetTheMilk.Factories;
using GetTheMilk.Levels;
using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.UI.Translators;
using NUnit.Framework;

namespace GetTheMilkTests.TranslatorTests
{
    [TestFixture]
    public class ActionTranslatorTests
    {
        [Test, TestCaseSource(typeof(DataGeneratorForTranslator), "TestCasesForActionTranslator")]
        public string ActionToHuLTests(GameAction action)
        {
            return ActionToHuL.TranslateAction(action);
        }
    }
}
