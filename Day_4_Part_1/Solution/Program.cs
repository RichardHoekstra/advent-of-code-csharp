using System;
using System.Collections.Generic;
using System.Linq;

/*
    PROBLEM STATEMENT:
        However, they do remember a few key facts about the password:

        It is a six-digit number.
        The value is within the range given in your puzzle input.
        Two adjacent digits are the same (like 22 in 122345).
        Going from left to right, the digits never decrease; they only ever increase or stay the same (like 111123 or 135679).

        Other than the range rule, the following are true:

            111111 meets these criteria (double 11, never decreases).
            223450 does not meet these criteria (decreasing pair of digits 50).
            123789 does not meet these criteria (no double).
    
    PROBLEM QUESTION:
        How many different passwords within the range given in your puzzle input meet these criteria?
*/

namespace Solution
{
    public class Program
    {
        static void Main(string[] args)
        {
            string input = "245182-790572";
            var min_value = Int32.Parse(input.Split("-")[0]);
            var max_value = Int32.Parse(input.Split("-")[1]);

            Console.WriteLine($"{min_value}|{max_value}");

            int plausiblePasswords = 0;
            for (int i = min_value+1; i < max_value; i++)
            { 
                if(Program.PlausiblePassword(i.ToString())){
                    Console.WriteLine(i);
                    plausiblePasswords++;
                }
            }        
            Console.WriteLine($"Possible passwords: {plausiblePasswords}");
        }

        public static int ExampleFunction(int x)
        {
            return x + 1;
        }

        public static int CharToInt(char c)
        {
            return (int)Char.GetNumericValue(c);
        }
        public static bool PlausiblePassword(string password, int min_value=-1, int max_value=-1)
        {           
            // It is a six-digit number.
            if(password.Length != 6)
            {
                return false;
            }
            //Console.WriteLine("It is a six-digit number.");

            // The value is within the range given in your puzzle input.
            int pass_value = Int32.Parse(password);
            if(min_value != -1 && pass_value < min_value)
            {
                return false;
            }

            if(max_value != -1 && pass_value > max_value)
            {
                return false;
            }
            //Console.WriteLine("The value is within the range given in your puzzle input.");

            // Two adjacent digits are the same (like 22 in 122345).
            bool hasEqualAdjacentDigits = false;
            for (int i = 0; i < password.Length-1; i++)
            {
                if(password[i] == password[i+1])
                {
                    hasEqualAdjacentDigits = true;
                }
            }

            if(!hasEqualAdjacentDigits)
            {
                return false;
            }
            //Console.WriteLine("Two adjacent digits are the same.");

            // Going from left to right, the digits never decrease; 
            // they only ever increase or stay the same (like 111123 or 135679).
            for (int i = 0; i < password.Length-1; i++)
            {
                //Console.WriteLine($"{Program.CharToInt(password[i])} > {Program.CharToInt(password[i+1])}");
                if(Program.CharToInt(password[i]) > Program.CharToInt(password[i+1]))
                {
                    return false;
                }
            }
            //Console.WriteLine("Going from left to right, the digits never decrease.");
            return true;
        }
    }
}
