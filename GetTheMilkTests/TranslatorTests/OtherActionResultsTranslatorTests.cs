using GetTheMilk.Actions;
using GetTheMilk.UI.Translators;
using NUnit.Framework;

namespace GetTheMilkTests.TranslatorTests
{
    [TestFixture]
    public class OtherActionResultsTranslatorTests
    {
        [Test, TestCaseSource(typeof(DataGeneratorForTranslator), "TestCasesForTranslatorCommunicate")]
        public string CommunicateOk(ActionResult actionResult)
        {
            ActionResultToHuL actionResultToHuL = new ActionResultToHuL();
            return actionResultToHuL.TranslateActionResult(actionResult);
        }

    }
}
