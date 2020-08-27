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
            /*
            _sw = new StringWriter();
            Console.SetOut(_sw);
            _sr = new StringReader("100");
            Console.SetIn(_sr);
            */
        }

        [TearDown]
        public void TearDown()
        {
            /*
            var standardOut = new StreamWriter(Console.OpenStandardOutput());
            standardOut.AutoFlush = true;
            Console.SetOut(standardOut);
            Console.SetIn(new StreamReader(Console.OpenStandardInput()));
            */
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
                Assert.IsTrue(result == Y, $"Function({X} should return {Y}, function returned {result}");
            }
        }

        [Test]
        public void LargerExampleTest()
        {

            var testCases = new List<(string, int, int)>
            {
                // Is input smaller than 8?
                ("3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99", 7, 999), 
                // Is input equal to 8?
                ("3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99", 8, 1000), 
                // Is input bigger than 8?
                ("3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99", 9, 1001),
            };

            foreach ((string, int, int) testCase in testCases)
            {
                var program = testCase.Item1;
                var testInput = testCase.Item2;
                var expectedTestOutput = testCase.Item3;

                var computer = new IntcodeComputer(program, true);
                computer.TestInput = testInput;
                computer.Execute();
                var result = computer.MemoryDump();
                Assert.IsTrue(result != "");
                Assert.IsTrue(computer.TestOutput == expectedTestOutput, $"Function({program}) should return {expectedTestOutput}, function returned {computer.TestOutput}");
            }
        }

        [Test]
        public void CmpOpCodeTest()
        {
            var testCases = new List<(string, int, int)>
            {
                // Is input equal to 8?
                ("3,9,8,9,10,9,4,9,99,-1,8", 8, 1),
                ("3,9,8,9,10,9,4,9,99,-1,8", 7, 0), 

                // Is input less than 8?
                ("3,9,7,9,10,9,4,9,99,-1,8", 7, 1),
                ("3,9,7,9,10,9,4,9,99,-1,8", 8, 0),

                // Is input equal to 8? (Immediate mode)
                ("3,3,1108,-1,8,3,4,3,99", 8, 1),
                ("3,3,1108,-1,8,3,4,3,99", 7, 0),

                // Is input less than 8? (Immediate mode)
                ("3,3,1107,-1,8,3,4,3,99", 7, 1),
                ("3,3,1107,-1,8,3,4,3,99", 8, 0),
            };

            foreach ((string, int, int) testCase in testCases)
            {
                var program = testCase.Item1;
                var testInput = testCase.Item2;
                var expectedTestOutput = testCase.Item3;

                var computer = new IntcodeComputer(program, true);
                computer.TestInput = testInput;
                computer.Execute();
                var result = computer.MemoryDump();
                Assert.IsTrue(result != "");
                Assert.IsTrue(computer.TestOutput == expectedTestOutput, $"Function({program} should return {testInput}, function returned {computer.TestOutput}");
            }
        }

        [Test]
        public void JumpOpCodeTest()
        {
            var testCases = new List<(string, int, int)>
            {
                ("3,12,6,12,15,1,13,14,13,4,13,99,-1,0,1,9", 0, 0),
                ("3,12,6,12,15,1,13,14,13,4,13,99,-1,0,1,9", 1, 1),
                ("3,3,1105,-1,9,1101,0,0,12,4,12,99,1", 0, 0),
                ("3,3,1105,-1,9,1101,0,0,12,4,12,99,1", 1, 1),
            };

            foreach ((string, int, int) testCase in testCases)
            {
                var program = testCase.Item1;
                var testInput = testCase.Item2;
                var expectedTestOutput = testCase.Item3;

                var computer = new IntcodeComputer(program, true);
                computer.TestInput = testInput;
                computer.Execute();
                var result = computer.MemoryDump();
                Assert.IsTrue(result != "");
                Assert.IsTrue(computer.TestOutput == expectedTestOutput, $"Function({program} should return {testInput}, function returned {computer.TestOutput}");
            }
        }

        [Test]
        public void ReadOpCodeTest()
        {
            var computer = new IntcodeComputer("1002,4,3,4,33");
            var (OPCODE, MODES) = computer.ReadOpCode(0);
            Assert.IsTrue(OPCODE == IntcodeComputer.OpCode.MULTIPLY);
            Assert.IsTrue(MODES[0] == IntcodeComputer.Mode.POSITION);
            Assert.IsTrue(MODES[1] == IntcodeComputer.Mode.IMMEDIATE);
        }
    }
}