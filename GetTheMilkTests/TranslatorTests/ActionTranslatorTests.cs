using GetTheMilk.Actions.ActionTemplates;
using NUnit.Framework;

namespace GetTheMilkTests.TranslatorTests
{
    [TestFixture]
    public class ActionTranslatorTests
    {
        [Test, TestCaseSource(typeof(DataGeneratorForTranslator), "TestCasesForActionTranslator")]
        public string ActionToHuLTests(BaseActionTemplate action)
        {
            return action.ToString();
        }
    }
}
