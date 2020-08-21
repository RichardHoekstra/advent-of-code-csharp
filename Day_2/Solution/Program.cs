using System;
using System.Collections.Generic;
using System.Linq;

namespace Solution
{
    public class Program
    {
        static void Main(string[] args)
        {
            const var constProgram = System.IO.File.ReadAllText("input.txt");

            int result = 0;
            string program = "";
            int noun = 0;
            int verb = 0;
            bool toggle = false;
            bool notFirstIter = false;

            do
            {
                if(notFirstIter){
                    if(toggle){
                        noun += 1;
                    } else {
                        verb += 1;
                    }
                    toggle = !toggle;
                }
                program = Program.ReplaceInstructions(constProgram, noun, verb); 
                result = ReadResult(program).ToString();
                notFirstIter = true;
                Console.WriteLine($"Noun({noun.ToString()}) and Verb({verb.ToString()}) = Result({result})");
            } while (result != 19690720);
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
