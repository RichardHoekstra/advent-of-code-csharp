using System;
using System.Collections.Generic;
using System.Linq;

/*
    PROBLEM STATEMENT:
        An Elf just remembered one more important detail: the two adjacent matching digits are not part of a larger group of matching digits.

        Given this additional criterion, but still ignoring the range rule, the following are now true:

            112233 meets these criteria because the digits never decrease and all repeated digits are exactly two digits long.
            123444 no longer meets the criteria (the repeated 44 is part of a larger group of 444).
            111122 meets the criteria (even though 1 is repeated more than twice, it still contains a double 22).

    
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
            for (int i = min_value; i < max_value; i++)
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
            
            // Two adjacent digits are the same but not part of a
            // larger group of matching digits
            bool hasEqualAdjacentDigits = false;
            int matchingDigits = 0;
            for (int i = 0; i < password.Length-1; i++)
            {
                //Console.WriteLine($"{password[i]} == {password[i+1]}");
                if(password[i] == password[i+1])
                {
                    matchingDigits++;
                    hasEqualAdjacentDigits = true;    
                } else {
                    // If the digits have changed
                    // but, we found a double
                    // break early.
                    if(matchingDigits == 1){
                        break;
                    }
                    matchingDigits = 0;
                }
                
                // Fail on more than one match because
                // in the example sequence 3444, 
                // the produced matches are
                //  4 == 3
                //  4 == 4
                //  4 == 4
                if(matchingDigits > 1){
                    hasEqualAdjacentDigits = false;
                }   
            }

            if(!hasEqualAdjacentDigits)
            {
                //Console.WriteLine($"Rejected on adjacency: {password}");
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
