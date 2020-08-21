using System;
using System.Collections.Generic;
using System.Linq;

/*
    PROBLEM DESCRIPTION:
        Fuel required to launch a given module is based on its mass. Specifically, to find the fuel required for a module, take its mass, divide by three, round down, and subtract 2.

        For example:

            For a mass of 12, divide by 3 and round down to get 4, then subtract 2 to get 2.
            For a mass of 14, dividing by 3 and rounding down still yields 4, so the fuel required is also 2.
            For a mass of 1969, the fuel required is 654.
            For a mass of 100756, the fuel required is 33583.

        The Fuel Counter-Upper needs to know the total fuel requirement. To find it, individually calculate the fuel needed for the mass of each module (your puzzle input), then add together all the fuel values.

    PROBLEM QUESTION:
        What is the sum of the fuel requirements for all of the modules on your spacecraft?     
*/



namespace Solution
{
    public class Program
    {
        static void Main(string[] args)
        {
            int requiredFuel = 0;

            var moduleMasses = System.IO.File.ReadLines("input.txt");
            foreach (var moduleMass in moduleMasses)
            {
                requiredFuel += Program.RequiredFuelForMass(Int32.Parse(moduleMass));    
            }

            Console.WriteLine($"The sum of the fuel requirements is: {requiredFuel}");
            
        }

        public static int RequiredFuelForMass(int mass)
        {
            // Relies on integer division truncating any decimals
            return (mass / 3) - 2;
        }
    }
}
