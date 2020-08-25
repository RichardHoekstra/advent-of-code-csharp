using System.Collections.Generic;
using System.IO;
using System;

using NUnit.Framework;

using Solution;


namespace Solution.Test
{
    public class Tests
    {
        private StringWriter _sw;
        private StringReader _sr;


        [SetUp]
        public void Setup()
        {
            // Take control of cin/cout for the purpose of
            // unit testing
            _sw = new StringWriter();
            Console.SetOut(_sw);
            _sr = new StringReader("100");
            Console.SetIn(_sr);
        }

        [TearDown]
        public void TearDown()
        {
            var standardOut = new StreamWriter(Console.OpenStandardOutput());
            standardOut.AutoFlush = true;
            Console.SetOut(standardOut);
            Console.SetIn(new StreamReader(Console.OpenStandardInput()));
        }

        [Test]
        public void ExampleTest()
        {
            var testCases = new Dictionary<string, string>
            {
                {"1,0,0,0,99", "2,0,0,0,99"},
                {"2,3,0,3,99", "2,3,0,6,99"},
                {"2,4,4,5,99,0", "2,4,4,5,99,9801"},
                {"1,1,1,4,99,5,6,0,99", "30,1,1,4,2,5,6,0,99"}
            };
            
            foreach (KeyValuePair<string, string> testCase in testCases)
            {
                var X = testCase.Key;
                var Y = testCase.Value;
                
                var computer = new IntcodeComputer(X);
                computer.Execute();
                var result = computer.MemoryDump();
                Assert.IsTrue(result==Y, $"Function({X} should return {Y}, function returned {result}");
            }
        }

        [Test]
        public void ReadOpCodeTest()
        {
            var computer = new IntcodeComputer("1002,4,3,4,33");
            var (OPCODE, MODES) = computer.ReadOpCode(0);
            Assert.IsTrue(OPCODE==IntcodeComputer.OpCode.MULTIPLY);
            Assert.IsTrue(MODES[0]==IntcodeComputer.Mode.POSITION);
            Assert.IsTrue(MODES[1]==IntcodeComputer.Mode.IMMEDIATE);
        }
    }
}