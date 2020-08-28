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
            string map = "COM)B\nB)C\nC)D\nD)E\nE)F\nB)G\nG)H\nD)I\nE)J\nJ)K\nK)L";
            int checksum = 42;
            var result = Program.ExampleFunction(map);
            Assert.IsTrue(result == checksum, $"Function({map} should return {checksum}, function returned {result}");

        }
    }
}