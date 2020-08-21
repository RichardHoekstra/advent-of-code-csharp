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
                {12, 2},
                {14, 2},
                {1969, 654},
                {100756, 33583}
            };
            
            foreach (KeyValuePair<int, int> testCase in testCases)
            {
                var mass = testCase.Key;
                var fuel = testCase.Value;

                var result = Program.RequiredFuelForMass(mass);
                Assert.IsTrue(result==fuel, $"Mass of {mass} requires {fuel} fuel, function returned {result}");
            }
        }
    }
}