using System;
using System.Collections.Generic;
using System.Linq;

/*
    PROBLEM DESCRIPTION:
        Fuel itself requires fuel just like a module - take its mass, divide by three, round down, and subtract 2. However, that fuel also requires fuel, and that fuel requires fuel, and so on. Any mass that would require negative fuel should instead be treated as if it requires zero fuel; the remaining mass, if any, is instead handled by wishing really hard, which has no mass and is outside the scope of this calculation.

        So, for each module mass, calculate its fuel and add it to the total. Then, treat the fuel amount you just calculated as the input mass and repeat the process, continuing until a fuel requirement is zero or negative. For example:

            A module of mass 14 requires 2 fuel. This fuel requires no further fuel (2 divided by 3 and rounded down is 0, which would call for a negative fuel), so the total fuel required is still just 2.
            At first, a module of mass 1969 requires 654 fuel. Then, this fuel requires 216 more fuel (654 / 3 - 2). 216 then requires 70 more fuel, which requires 21 fuel, which requires 5 fuel, which requires no further fuel. So, the total fuel required for a module of mass 1969 is 654 + 216 + 70 + 21 + 5 = 966.
            The fuel required by a module of mass 100756 and its fuel is: 33583 + 11192 + 3728 + 1240 + 411 + 135 + 43 + 12 + 2 = 50346.


    PROBLEM QUESTION:
        What is the sum of the fuel requirements for all of the modules on your spacecraft when also taking into account the mass of the added fuel? 
        (Calculate the fuel requirements for each module separately, then add them all up at the end.) 
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
                requiredFuel += Program.RequiredFuelForMassAndFuel(Int32.Parse(moduleMass));    
            }

            Console.WriteLine($"The sum of the fuel requirements is: {requiredFuel}");
            
        }

        public static int RequiredFuelForMass(int mass)
        {
            // Relies on integer division truncating any decimals
            return (mass / 3) - 2;
        }

        public static int RequiredFuelForMassAndFuel(int mass)
        {
            int moduleFuel = Program.RequiredFuelForMass(mass);

            // Calculate the fuel needed for the fuel itself
            // Iterate the required fuel until it returns zero or negative
            int fuelFuel = 0;
            int fuel = Program.RequiredFuelForMass(moduleFuel);
            
            while (fuel > 0)
            {
                fuelFuel += fuel;
                fuel = Program.RequiredFuelForMass(fuel);
            }

            return moduleFuel + fuelFuel;
        }
    }
}
