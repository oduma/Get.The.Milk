using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace GetTheMilkTests.TranslatorTests
{
    public class DataGeneratorForLoader
    {
        public static IEnumerable TestCasesForLoader
        {
            get 
            { 
                yield return new TestCaseData("").Returns(0);
                yield return new TestCaseData("non existent folder").Returns(0);
                yield return
                    new TestCaseData(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"UI\Data")).Returns(7);
            }
        }
    }
}
