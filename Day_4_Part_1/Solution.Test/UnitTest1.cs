using System.Collections.Generic;

using NUnit.Framework;

using Solution;


namespace Solution.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ExampleTest()
        {
            var testCases = new Dictionary<string, bool>
            {
                {"111111", true},
                {"223450", false},
                {"123789", false},
            };
            
            foreach (KeyValuePair<string, bool> testCase in testCases)
            {
                var X = testCase.Key;
                var Y = testCase.Value;

                var result = Program.PlausiblePassword(X);
                Assert.IsTrue(result==Y, $"Function({X} should return {Y}, function returned {result}");
            }
        }
    }
}