using GetTheMilk.Actions;
using GetTheMilk.Actions.ActionPerformers;
using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;
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
        public string MoveOk(PerformActionResult actionResult)
        {
            ActionResultToHuL actionResultToHuL= new ActionResultToHuL();
            return actionResultToHuL.TranslateMovementResult(actionResult);
        }

        [Test,TestCaseSource(typeof(DataGeneratorForTranslator),"TestCasesForTranslatorMovementError")]
        public string OriginNotOnTheMap(PerformActionResult actionResult)
        {
            ActionResultToHuL actionResultToHuL = new ActionResultToHuL();
            return actionResultToHuL.TranslateMovementResult(actionResult);
        }

        [Test, TestCaseSource(typeof(DataGeneratorForTranslator), "TestCasesForTranslatorMovementOutOfTheWorld")]
        public string OutOfTheMap(PerformActionResult actionResult)
        {
            ActionResultToHuL actionResultToHuL = new ActionResultToHuL();
            return actionResultToHuL.TranslateMovementResult(actionResult);
        }

        [Test, TestCaseSource(typeof(DataGeneratorForTranslator), "TestCasesForTranslatorMovementBlockedByObjs")]
        public string BlockedByObjects(PerformActionResult actionResult)
        {
            ActionResultToHuL actionResultToHuL = new ActionResultToHuL();
            return actionResultToHuL.TranslateMovementResult(actionResult);
        }

        [Test, TestCaseSource(typeof(DataGeneratorForTranslator), "TestCasesForTranslatorMovementBlockedByChars")]
        public string BlockedByCharacters(PerformActionResult actionResult)
        {
            ActionResultToHuL actionResultToHuL = new ActionResultToHuL();
            return actionResultToHuL.TranslateMovementResult(actionResult);
        }

        [Test, TestCaseSource(typeof(DataGeneratorForTranslator), "TestCasesForTranslatorMovementNoActiveCharacter")]
        public string NoActiveCharacter(PerformActionResult actionResult)
        {
            ActionResultToHuL actionResultToHuL = new ActionResultToHuL();
            return actionResultToHuL.TranslateMovementResult(actionResult);
        }

        [Test, TestCaseSource(typeof(DataGeneratorForTranslator), "TestCasesForTranslatorMovementExtraData")]
        public string MovementExtraDataTest(PerformActionResult actionResult, IPlayer active, Level level)
        {
            ActionResultToHuL actionResultToHuL = new ActionResultToHuL();
            return actionResultToHuL.TranslateMovementExtraData(actionResult.ExtraData as MovementActionTemplateExtraData, active,level);
            
        }
    }
}
