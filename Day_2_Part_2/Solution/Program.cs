using System;
using System.Collections.Generic;
using System.Linq;

namespace Solution
{
    public class Program
    {
        static void Main(string[] args)
        {
            var program = System.IO.File.ReadAllText("input.txt");

            // RESTORE STATE FROM BEFORE CRASH
            var instructions = program.Split(",");
            instructions[1] = "12";
            instructions[2] = "2";
            program = String.Join(",", instructions);

            // FEED THE INSTRUCTIONS INTO THE MACHINE
            var result = Program.RunIntCode(program);
            
            Console.WriteLine($"What value is left at position 0 after the program halts? Answer: {result.Split(",")[0]}");
        }

        public static int ExampleFunction(int x)
        {
            return x + 1;
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
