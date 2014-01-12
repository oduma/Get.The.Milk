using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GetTheMilk.Actions;
using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.UI.Translators;
using NUnit.Framework;

namespace GetTheMilkTests.TranslatorTests
{
    [TestFixture]
    public class MovementResultsTranslationTests
    {
        [Test,TestCaseSource(typeof(DataGeneratorForTranslator),"TestCasesForTranslatorMovement")]
        public string MoveOk(ActionResult actionResult, IPlayer active)
        {
            ActionResultToHuL actionResultToHuL= new ActionResultToHuL();
            return actionResultToHuL.TranslateMovementResult(actionResult, active);
        }

        [Test]
        public void OriginNotOnTheMap()
        {
            Assert.Fail();
        }

        [Test]
        public void OutOfTheMap()
        {
            Assert.Fail();
        }

        [Test]
        public void BlockedByObjects()
        {
            Assert.Fail();
        }

        [Test]
        public void BlockedByCharacters()
        {
            Assert.Fail();
        }

        [Test]
        public void NoActiveCharacter()
        {
            Assert.Fail();
        }

        [Test]
        public void NotMovementAction()
        {
            Assert.Fail();
        }
    }
}
