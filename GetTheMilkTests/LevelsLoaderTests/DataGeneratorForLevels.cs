using System;
using System.Collections;
using NUnit.Framework;

namespace GetTheMilkTests.LevelsLoaderTests
{
    public class DataGeneratorForLevels
    {
        public static IEnumerable TestCases
        {
            get 
            { 
                yield return new TestCaseData(1,4,2).Returns(9);
                yield return new TestCaseData(2,0,0).Returns(8);
                yield return new TestCaseData(3,0,0).Throws(typeof (InvalidOperationException));
            }
        }

        public static IEnumerable LoadTestCases
        {
            get { yield return new TestCaseData(1, null).Returns(4); }
        }
    }
}
