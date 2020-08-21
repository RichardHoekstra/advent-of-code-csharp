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
            var result = Program.ExampleFunction(10);
            Assert.IsTrue(result==11, $"10+1 should equal 11, function returned {result}");
        }
    }
}