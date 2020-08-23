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

        private void InsConsoleInput(List<Mode> modes)
        {
            // TODO
        }

        private void InsConsoleOutput(List<Mode> modes)
        {
            // TOOD
        }
        public void Execute()
        {
            // Execute the program that is loaded in memory
            var (OPCODE, MODES) = ReadOpCode();
            while(OPCODE != OpCode.HALT)
            {
                switch(OPCODE)
                {
                    case OpCode.ADD:
                        InsAdd(MODES);
                        break;
                    case OpCode.MULTIPLY:
                        InsMultiply(MODES);
                        break;
                    case OpCode.CONSOLE_INPUT:
                        InsConsoleInput(MODES);
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

            int result = 0;
            int result_noun = 0;
            int result_verb = 0;
            string program = "";    
            const int _range = 100;
           
            for (int noun = 0; noun < _range; noun++)
            {
                for (int verb = 0; verb < _range; verb++)
                {
                    // Replace instructions
                    program = Program.ReplaceInstructions(constProgram, noun, verb); 
                    // Execute and read the result at instruction 0
                    result = Int32.Parse(ReadResult(Program.RunIntCode(program)));
                    if(result == 19690720)
                    {
                        result_noun = noun;
                        result_verb = verb;
                        break;
                    }
                }
                if(result == 19690720)
                {
                    break;
                }
            }
            Console.WriteLine($"What is 100 * noun + verb? 100 * {result_noun.ToString()} + {result_verb.ToString()} = {100*result_noun+result_verb}");
        }

        public static string ExampleFunction(string program)
        {
            var computer = new IntcodeComputer(program);
            computer.Execute();
            return computer.MemoryDump();
        }

        public static string ReplaceInstructions(string program, int noun, int verb)
        {
            var instructions = program.Split(",");
            instructions[1] = noun.ToString();
            instructions[2] = verb.ToString();
            return String.Join(",", instructions);
        }

        public static string ReadResult(string program)
        {
            return program.Split(",")[0];
        }

        public static string RunIntCode(string IntCode)
        {
            // OPCODES:
            //  1: ADDITION
            //  2: MULTIPLICATION

            // STRUCTURE: 
            //  OPCODE, ADDRESS_X, ADDRESS_Y, ADDRESS_RESULT
            var instructions = IntCode.Split(",");
            
            int instructionPointer = 0;
            bool programExit = false;
            while(!programExit){
                // READ THE INSTRUCTION
                int OPCODE = Int32.Parse(instructions[instructionPointer]);

                // HALT IMMEDIATELY UPON READING OPCODE 99
                if(OPCODE==99){
                    programExit = true;
                    break;
                }

                int ADDRESS_X = Int32.Parse(instructions[instructionPointer+1]);
                int ADDRESS_Y = Int32.Parse(instructions[instructionPointer+2]);
                int ADDRESS_RESULT = Int32.Parse(instructions[instructionPointer+3]);

                // READ THE ADDRESS
                int X = Int32.Parse(instructions[ADDRESS_X]);
                int Y = Int32.Parse(instructions[ADDRESS_Y]);

                // EXECUTE OPCODE
                int RESULT = 0;
                switch (OPCODE)
                {
                    case 1: // ADDITION
                        RESULT = X + Y;
                        break;
                    case 2: // MULTIPLICATION
                        RESULT = X * Y;
                        break;
                    default:
                        throw new System.InvalidProgramException("OPCODE NOT SUPPORTED");
                }
            
                // WRITE RESULT TO ADDRESS
                instructions[ADDRESS_RESULT] = RESULT.ToString();

                // INCREMENT INSTRUCTION POINTER TO READ THE NEXT INSTRUCTION
                instructionPointer += 4;
            }

            // RETURN THE EXECUTED INSTRUCTIONS
            return String.Join(",", instructions);
        }
    }
}
