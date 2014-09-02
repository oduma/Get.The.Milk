﻿using GetTheMilk.Actions.ActionPerformers.Base;
using GetTheMilk.Actions.ActionTemplates;
using GetTheMilk.Common;
using GetTheMilk.Objects.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetTheMilk.NewActions.Tests.SingleActionPerformedTests
{
    [TestFixture]
    public class ObjectPerformObjectResponseActionTests
    {

        [Test]
        public void ObjectPerformResponse()
        {
            var activeObject = new NonCharacterObject { Name = new Noun { Main = "Dawn", Narrator = "the dawn" }, AllowsTemplateAction = TestHelper.AllowsEverything };
            activeObject.AddAvailableAction(new ObjectResponseActionTemplate { Name = new Verb { UniqueId = "Crack", Past = "cracked", Present = "crack" }, PerformerType = typeof(ObjectResponseActionTemplatePerformer) });
            var useAction = activeObject.CreateNewInstanceOfAction<ObjectResponseActionTemplate>("Crack");
            Assert.IsNotNull(useAction);
            Assert.AreEqual(typeof(ObjectResponseActionTemplate), useAction.GetType());

            Assert.AreEqual(ActionResultType.Ok,activeObject.PerformAction(useAction).ResultType);
        }
    }
}
