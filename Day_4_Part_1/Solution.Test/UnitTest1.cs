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
            var testCases = new Dictionary<int, int>
            {
                {1, 2},
                {2, 3},
                {3, 4},
            };
            
            foreach (KeyValuePair<int, int> testCase in testCases)
            {
                var X = testCase.Key;
                var Y = testCase.Value;

                var result = Program.ExampleFunction(X);
                Assert.IsTrue(result==Y, $"Function({X} should return {Y}, function returned {result}");
            }
        }
    }
}