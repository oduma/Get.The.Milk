using GetTheMilk.Actions;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Levels;
using GetTheMilk.UI.Translators;
using NUnit.Framework;

namespace GetTheMilkTests.TranslatorTests
{
    [TestFixture]
    public class MovementResultsTranslationTests
    {
        [Test,TestCaseSource(typeof(DataGeneratorForTranslator),"TestCasesForTranslatorMovement")]
        public string MoveOk(ActionResult actionResult)
        {
            ActionResultToHuL actionResultToHuL= new ActionResultToHuL();
            return actionResultToHuL.TranslateMovementResult(actionResult);
        }

        [Test,TestCaseSource(typeof(DataGeneratorForTranslator),"TestCasesForTranslatorMovementError")]
        public string OriginNotOnTheMap(ActionResult actionResult)
        {
            ActionResultToHuL actionResultToHuL = new ActionResultToHuL();
            return actionResultToHuL.TranslateMovementResult(actionResult);
        }

        [Test, TestCaseSource(typeof(DataGeneratorForTranslator), "TestCasesForTranslatorMovementOutOfTheWorld")]
        public string OutOfTheMap(ActionResult actionResult)
        {
            ActionResultToHuL actionResultToHuL = new ActionResultToHuL();
            return actionResultToHuL.TranslateMovementResult(actionResult);
        }

        [Test, TestCaseSource(typeof(DataGeneratorForTranslator), "TestCasesForTranslatorMovementBlockedByObjs")]
        public string BlockedByObjects(ActionResult actionResult)
        {
            ActionResultToHuL actionResultToHuL = new ActionResultToHuL();
            return actionResultToHuL.TranslateMovementResult(actionResult);
        }

        [Test, TestCaseSource(typeof(DataGeneratorForTranslator), "TestCasesForTranslatorMovementBlockedByChars")]
        public string BlockedByCharacters(ActionResult actionResult)
        {
            ActionResultToHuL actionResultToHuL = new ActionResultToHuL();
            return actionResultToHuL.TranslateMovementResult(actionResult);
        }

        [Test, TestCaseSource(typeof(DataGeneratorForTranslator), "TestCasesForTranslatorMovementNoActiveCharacter")]
        public string NoActiveCharacter(ActionResult actionResult)
        {
            ActionResultToHuL actionResultToHuL = new ActionResultToHuL();
            return actionResultToHuL.TranslateMovementResult(actionResult);
        }

        [Test, TestCaseSource(typeof(DataGeneratorForTranslator), "TestCasesForTranslatorMovementWrongAction")]
        public string NotMovementAction(ActionResult actionResult)
        {
            ActionResultToHuL actionResultToHuL = new ActionResultToHuL();
            return actionResultToHuL.TranslateMovementResult(actionResult);
        }

        [Test, TestCaseSource(typeof(DataGeneratorForTranslator), "TestCasesForTranslatorMovementExtraData")]
        public string MovementExtraDataTest(ActionResult actionResult, IPlayer active, Level level)
        {
            ActionResultToHuL actionResultToHuL = new ActionResultToHuL();
            return actionResultToHuL.TranslateMovementExtraData(actionResult.ExtraData as MovementActionExtraData, active,level);
            
        }
    }
}
