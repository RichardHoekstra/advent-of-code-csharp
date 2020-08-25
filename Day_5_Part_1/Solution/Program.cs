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
            HALT = 99
        }
        
        public enum Mode 
        {
            POSITION = 0, // Read value from address
            IMMEDIATE = 1 // Read value
        }
        public string[] Memory;
        private int instructionPointer = 0;

        public IntcodeComputer(string program)
        {
            // Load program into memory, assumes a comma-seperated IntCode program.
            Memory = program.Split(",");
        }

        public (OpCode, List<Mode>) ReadOpCode(int position=-1)
        {
            if(position == -1)
            {
                position = instructionPointer;
            }

            string raw_val = ReadAddress(position).ToString();
            OpCode OPCODE = (OpCode)Int32.Parse(raw_val.Substring(Math.Max(raw_val.Length-2, 0), Math.Min(raw_val.Length, 2)));
            
            var MODES = new List<Mode>();
            if(raw_val.Length > 2){
                string RAW_MODES = raw_val.Substring(0, raw_val.Length - 2);
                if(RAW_MODES.Length > 0)
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
            return Int32.Parse(Memory[address]);
        }

        private void WriteAddress(int address, int value)
        {
            
            Memory[address] = value.ToString();
        }

        private void InsAdd(List<Mode> modes)
        {
            // X, Y, Z
            // X and Y can be in position mode or immediate mode
            // Z can only be in position mode
            // Write X + Y to position Z

            int x = ReadAddress(instructionPointer+1);
            int y = ReadAddress(instructionPointer+2);
            int z = ReadAddress(instructionPointer+3);

            // Execute position mode(s), if set.
            if(modes.Count == 0 || (modes.Count >= 1 && modes[0] == Mode.POSITION))
            {
                // Read the address and write it back to X
                x = ReadAddress(x);
            }

            if(modes.Count < 2 || (modes.Count >= 2 && modes[1] == Mode.POSITION))
            {
                y = ReadAddress(y);
            }

            // Execute the OPCODE
            int result = x + y;

            // Write to address of Z
            WriteAddress(z, result);
            
            // Increment instruction pointer by the amount of parameters, including the OpCode.
            instructionPointer += 4;
        }

        private void InsMultiply(List<Mode> modes)
        {
            // X, Y, Z
            // X and Y can be in position mode or immediate mode
            // Z can only be in position mode
            // Write X * Y to position Z

            int x = ReadAddress(instructionPointer+1);
            int y = ReadAddress(instructionPointer+2);
            int z = ReadAddress(instructionPointer+3);

           
            // Execute position mode(s), if set.
            if(modes.Count == 0 || (modes.Count >= 1 && modes[0] == Mode.POSITION))
            {
                // Read the address and write it back to X
                x = ReadAddress(x);
            }

            if(modes.Count < 2 || (modes.Count >= 2 && modes[1] == Mode.POSITION))
            {
                y = ReadAddress(y);
            }

            // Execute the OPCODE
            int result = x * y;

            // Write to address of Z
            WriteAddress(z, result);

            // Increment instruction pointer by the amount of parameters, including the OpCode.
            instructionPointer += 4;
        }

        private void InsConsoleInput()
        {
            // Opcode 3 takes a single integer as input and saves it to the position given by its only parameter.
            // Get user input
            Console.Write("CONSOLE INPUT | ENTER AN INTEGER VALUE: ");
            string input = Console.ReadLine();

            // Get the value of the first (and only) parameter
            int x = ReadAddress(instructionPointer+1);
            
            // Write the user input to the address x
            WriteAddress(x, Int32.Parse(input));

            // Increment instruction pointer by the amount of parameters, including the OpCode.
            instructionPointer += 2;
        }

        private void InsConsoleOutput(List<Mode> modes)
        {
            // Opcode 4 outputs the value of its only parameter.
            int x = ReadAddress(instructionPointer+1);

            // Execute position mode(s), if set.
            if(modes.Count == 0 || (modes.Count >= 1 && modes[0] == Mode.POSITION))
            {
                // Read the address and write it back to X
                x = ReadAddress(x);
            }

            Console.WriteLine("CONSOLE OUTPUT: " + x.ToString());
            
            // Increment instruction pointer by the amount of parameters, including the OpCode.
            instructionPointer += 2;
        }
        public void Execute()
        {
            // Execute the program that is loaded in memory
            var (OPCODE, MODES) = ReadOpCode();
            while(OPCODE != OpCode.HALT)
            {
                // Console.WriteLine($"{OPCODE}");
                switch(OPCODE)
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
                    case OpCode.HALT:
                        break;
                    default:
                        break;
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
