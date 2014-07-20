using GetTheMilk.Actions.ActionPerformers;
using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.UI.Translators;
using NUnit.Framework;

namespace GetTheMilkTests.TranslatorTests
{
    [TestFixture]
    public class OtherActionResultsTranslatorTests
    {
        [Test, TestCaseSource(typeof(DataGeneratorForTranslator), "TestCasesForTranslatorCommunicate")]
        public string CommunicateOk(PerformActionResult actionResult)
        {
            ActionResultToHuL actionResultToHuL = new ActionResultToHuL();
            return actionResultToHuL.TranslateActionResult(actionResult);
        }

    }
}
