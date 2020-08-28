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
            string map = "COM)B\nB)C\nC)D\nD)E\nE)F\nB)G\nG)H\nD)I\nE)J\nJ)K\nK)L\nK)YOU\nI)SAN";
            int orbitalTransfers = 4;
            var result = Program.ExampleFunction(map);
            Assert.IsTrue(result == orbitalTransfers, $"Function({map} should return {orbitalTransfers}, function returned {result}");
        }
    }
}