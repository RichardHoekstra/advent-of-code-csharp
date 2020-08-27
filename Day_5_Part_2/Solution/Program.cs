using System;
using System.Collections.Generic;
using System.Linq;


namespace Solution
{
    public class IntcodeComputer
    {
        public enum OpCode
        {
            ADD = 1,
            MULTIPLY = 2,
            CONSOLE_INPUT = 3,
            CONSOLE_OUTPUT = 4,
            JUMP_NOT_ZERO = 5,
            JUMP_ZERO = 6,
            COMPARE_LESS_THAN = 7,
            COMPARE_EQUAL = 8,
            HALT = 99
        }

        public enum Mode
        {
            POSITION = 0, // Read value from address
            IMMEDIATE = 1 // Read value
        }
        public string[] Memory;
        private int instructionPointer = 0;

        public bool TestMode = false;
        public int TestInput;
        public int TestOutput;

        public IntcodeComputer(string program, bool testMode = false)
        {
            // Load program into memory, assumes a comma-seperated IntCode program.
            Memory = program.Split(",");
            Console.WriteLine($"Memory at boot: {MemoryDump()}");

            // Set the testing mode flag
            TestMode = testMode;
        }

        public (OpCode, List<Mode>) ReadOpCode(int position = -1)
        {
            if (position == -1)
            {
                position = instructionPointer;
            }

            string raw_val = ReadAddress(position).ToString();
            OpCode OPCODE = (OpCode)Int32.Parse(raw_val.Substring(Math.Max(raw_val.Length - 2, 0), Math.Min(raw_val.Length, 2)));

            var MODES = new List<Mode>();
            if (raw_val.Length > 2)
            {
                string RAW_MODES = raw_val.Substring(0, raw_val.Length - 2);
                if (RAW_MODES.Length > 0)
                {
                    for (int i = RAW_MODES.Length - 1; i >= 0; i--)
                    {
                        MODES.Add((Mode)Char.GetNumericValue(RAW_MODES[i]));
                    }
                }
            }
            return (OPCODE, MODES);
        }

        public string MemoryDump()
        {
            // Return a correctly formatted IntCode program
            return String.Join(",", Memory);
        }

        private int ReadAddress(int address)
        {
            if (address > Memory.Count() - 1)
                throw new IndexOutOfRangeException($"Tried reading an out-of-bounds address. Address: {address} Memory: {MemoryDump()}");
            return Int32.Parse(Memory[address]);
        }

        private void WriteAddress(int address, int value)
        {

            Memory[address] = value.ToString();
        }

        private void InsAdd(List<Mode> modes)
        {
            // Read the parameters (by value)
            int par1 = ReadAddress(instructionPointer + 1);
            int par2 = ReadAddress(instructionPointer + 2);
            int par3 = ReadAddress(instructionPointer + 3);

            // Execute position mode(s), if set.
            if (modes.Count == 0 || (modes.Count >= 1 && modes[0] == Mode.POSITION))
            {
                // Read the address and write it back to X
                par1 = ReadAddress(par1);
            }

            if (modes.Count < 2 || (modes.Count >= 2 && modes[1] == Mode.POSITION))
            {
                par2 = ReadAddress(par2);
            }

            // Execute the OPCODE
            int result = par1 + par2;

            // Write to address of Z
            WriteAddress(par3, result);

            // Increment instruction pointer by the amount of parameters, including the OpCode.
            instructionPointer += 4;
        }

        private void InsMultiply(List<Mode> modes)
        {
            // Read the parameters (by value)
            int par1 = ReadAddress(instructionPointer + 1);
            int par2 = ReadAddress(instructionPointer + 2);
            int par3 = ReadAddress(instructionPointer + 3);

            // Execute position mode(s), if set.
            if (modes.Count == 0 || (modes.Count >= 1 && modes[0] == Mode.POSITION))
            {
                // Read the address and write it back to X
                par1 = ReadAddress(par1);
            }

            if (modes.Count < 2 || (modes.Count >= 2 && modes[1] == Mode.POSITION))
            {
                par2 = ReadAddress(par2);
            }

            // Execute the OPCODE
            int result = par1 * par2;

            // Write to address of Z
            WriteAddress(par3, result);

            // Increment instruction pointer by the amount of parameters, including the OpCode.
            instructionPointer += 4;
        }

        private void InsJumpNotZero(List<Mode> modes)
        {
            // If the first parameter is non-zero, it sets the instruction pointer to the value 
            // from the second parameter. Otherwise, it does nothing.

            // Read parameters (by value)
            int par1 = ReadAddress(instructionPointer + 1);
            int par2 = ReadAddress(instructionPointer + 2);

            // Execute position mode(s), if set.
            if (modes.Count == 0 || (modes.Count >= 1 && modes[0] == Mode.POSITION))
            {
                // Read the address and write it back to X
                par1 = ReadAddress(par1);
            }

            if (modes.Count < 2 || (modes.Count >= 2 && modes[1] == Mode.POSITION))
            {
                par2 = ReadAddress(par2);
            }

            // Execute the OPCODE
            if (par1 != 0)
            {
                // Jump to the value from the second parameter
                instructionPointer = par2;
            }
            else
            {
                // Step to the next instruction
                instructionPointer += 3;
            }
        }

        private void InsJumpZero(List<Mode> modes)
        {
            // If the first parameter is zero, it sets the instruction pointer to the value 
            // from the second parameter. Otherwise, it does nothing.

            // Read parameters (by value)
            int par1 = ReadAddress(instructionPointer + 1);
            int par2 = ReadAddress(instructionPointer + 2);

            // Execute position mode(s), if set.
            if (modes.Count == 0 || (modes.Count >= 1 && modes[0] == Mode.POSITION))
            {
                // Read the address and write it back to X
                par1 = ReadAddress(par1);
            }

            if (modes.Count < 2 || (modes.Count >= 2 && modes[1] == Mode.POSITION))
            {
                par2 = ReadAddress(par2);
            }

            Console.WriteLine($"Par1: {par1} Par2: {par2}");

            // Execute the OPCODE
            if (par1 == 0)
            {
                // Jump to the value from the second parameter
                instructionPointer = par2;
            }
            else
            {
                // Step to the next instruction
                instructionPointer += 3;
            }
        }

        private void InsCmpLessThan(List<Mode> modes)
        {
            // If the first parameter is less than the second parameter, 
            // it stores 1 in the position given by the third parameter. Otherwise, it stores 0.

            // Read parameters (by value)
            int par1 = ReadAddress(instructionPointer + 1);
            int par2 = ReadAddress(instructionPointer + 2);
            int par3 = ReadAddress(instructionPointer + 3);

            // Execute position mode(s), if set.
            if (modes.Count == 0 || (modes.Count >= 1 && modes[0] == Mode.POSITION))
            {
                // Read the address and write it back to X
                par1 = ReadAddress(par1);
            }

            if (modes.Count < 2 || (modes.Count >= 2 && modes[1] == Mode.POSITION))
            {
                par2 = ReadAddress(par2);
            }

            /*
            if(modes.Count < 3 || (modes.Count >= 3 && modes[2] == Mode.POSITION))
            {
                par3 = ReadAddress(par3);
            }
            */

            // Execute the OPCODE
            WriteAddress(par3, (par1 < par2) ? 1 : 0);

            // Step to the next instruction
            instructionPointer += 4;
        }

        private void InsCmpEqual(List<Mode> modes)
        {
            // If the first parameter is equal to the second parameter, 
            // it stores 1 in the position given by the third parameter. Otherwise, it stores 0.

            // Read parameters (by value)
            int par1 = ReadAddress(instructionPointer + 1);
            int par2 = ReadAddress(instructionPointer + 2);
            int par3 = ReadAddress(instructionPointer + 3);

            Console.WriteLine($"Par1: {par1} Par2: {par2} Par3: {par3}");

            // Execute position mode(s), if set.
            if (modes.Count == 0 || (modes.Count >= 1 && modes[0] == Mode.POSITION))
            {
                // Read the address and write it back to X
                par1 = ReadAddress(par1);
            }

            if (modes.Count < 2 || (modes.Count >= 2 && modes[1] == Mode.POSITION))
            {
                par2 = ReadAddress(par2);
            }

            /*
            if(modes.Count < 3 || (modes.Count >= 3 && modes[2] == Mode.POSITION))
            {
                par3 = ReadAddress(par3);
            }*/

            Console.WriteLine($"Par1: {par1} Par2: {par2} Par3: {par3}");

            // Execute the OPCODE
            WriteAddress(par3, (par1 == par2) ? 1 : 0);

            // Step to the next instruction
            instructionPointer += 4;
        }


        private void InsConsoleInput()
        {
            // Opcode 3 takes a single integer as input and saves it to the position given by its only parameter.

            string input;
            if (TestMode)
            {
                input = TestInput.ToString();
            }
            else
            {
                Console.Write("CONSOLE INPUT | ENTER AN INTEGER VALUE: ");
                input = Console.ReadLine();
            }

            // Get the value of the first (and only) parameter
            int x = ReadAddress(instructionPointer + 1);

            // Write the user input to the address x
            WriteAddress(x, Int32.Parse(input));

            // Increment instruction pointer by the amount of parameters, including the OpCode.
            instructionPointer += 2;
        }

        private void InsConsoleOutput(List<Mode> modes)
        {
            // Opcode 4 outputs the value of its only parameter.

            int x = ReadAddress(instructionPointer + 1);

            // Execute position mode(s), if set.
            if (modes.Count == 0 || (modes.Count >= 1 && modes[0] == Mode.POSITION))
            {
                // Read the address and write it back to X
                x = ReadAddress(x);
            }

            if (TestMode)
            {
                TestOutput = x;
            }
            else
            {
                Console.WriteLine("CONSOLE OUTPUT: " + x.ToString());
            }

            // Increment instruction pointer by the amount of parameters, including the OpCode.
            instructionPointer += 2;
        }

        public void Execute()
        {
            // Execute the program that is loaded in memory
            var (OPCODE, MODES) = ReadOpCode();
            bool HALT = false;
            while (OPCODE != OpCode.HALT && !HALT)
            {
                string strMODES = "";
                foreach (var item in MODES)
                {
                    strMODES += item + " ";
                }
                Console.WriteLine($"OpCode: {OPCODE}    Modes: {strMODES}");
                Console.WriteLine($"IP: {instructionPointer}    Memory: {MemoryDump()}");
                switch (OPCODE)
                {
                    case OpCode.ADD:
                        InsAdd(MODES);
                        break;
                    case OpCode.MULTIPLY:
                        InsMultiply(MODES);
                        break;
                    case OpCode.CONSOLE_INPUT:
                        InsConsoleInput();
                        break;
                    case OpCode.CONSOLE_OUTPUT:
                        InsConsoleOutput(MODES);
                        break;
                    case OpCode.JUMP_NOT_ZERO:
                        InsJumpNotZero(MODES);
                        break;
                    case OpCode.JUMP_ZERO:
                        InsJumpZero(MODES);
                        break;
                    case OpCode.COMPARE_LESS_THAN:
                        InsCmpLessThan(MODES);
                        break;
                    case OpCode.COMPARE_EQUAL:
                        InsCmpEqual(MODES);
                        break;
                    case OpCode.HALT:
                        HALT = true;
                        break;
                    default:
                        throw new NotImplementedException($"Tried to execute an OpCode that is not implemented. OpCode: {OPCODE} Memory dump: {MemoryDump()} Instruction Pointer: {instructionPointer}");
                }
                (OPCODE, MODES) = ReadOpCode();
            }
        }
    }
    public class Program
    {
        static void Main(string[] args)
        {
            var constProgram = System.IO.File.ReadAllText("input.txt");
            IntcodeComputer computer = new IntcodeComputer(constProgram);
            computer.Execute();
        }
    }
}
