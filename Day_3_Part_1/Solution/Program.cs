using System;
using System.Collections.Generic;
using System.Linq;

namespace Solution
{

    public class Program
    {
        static void Main(string[] args)
        {
            string input = System.IO.File.ReadAllText("input.txt");
            Console.WriteLine($"Closest: {Program.ExampleFunction(input)}");
        }

        public static int ExampleFunction(string paths)
        {
            var inter = Program.PathsIntersections(paths);
            return Program.IntersectionClosestToCenter(inter);
        }

        public static int IntersectionClosestToCenter(HashSet<(int, int)> intersections)
        {   
            // ALTERNATIVE: Change the intersections to a list of distances to the center 
            // and just use <List>.Min()

            bool initialized = false;
            int min_manhattan_distance = 0;
            foreach (var intersection in intersections)
            {
                var manhattan_distance = Math.Abs(intersection.Item1) + Math.Abs(intersection.Item2);
                if(!initialized){
                    min_manhattan_distance = manhattan_distance;
                    initialized = true;
                } else {
                    if(manhattan_distance < min_manhattan_distance)
                    {
                        min_manhattan_distance = manhattan_distance;
                    }
                }
            }

            return min_manhattan_distance;
        }

        public static HashSet<(int, int)> PathsIntersections(string _paths)
        {
            string[] paths = _paths.Split('\n');
            
            var intersections = Program.PathToSet(paths[0]);
            for (int i = 1; i < paths.Length; i++)
            {
                intersections.IntersectWith(Program.PathToSet(paths[i]));
            }
            return intersections;
        }

        public static HashSet<(int,int)> PathToSet(string path)
        {
            var bitmap = new HashSet<(int,int)>();

            var currLocation = (x: 0, y: 0);

            var vectors = path.Split(",");
            foreach (var v in vectors)
            {
                char direction = v[0];
                int magnitude = Int32.Parse(v.Substring(1));
                for (int i = 0; i < magnitude; i++)
                {
                    switch (direction)
                    {
                        case 'L':
                            currLocation.x--;
                            break;
                        case 'R':
                            currLocation.x++;
                            break;
                        case 'U':
                            currLocation.y++;
                            break;
                        case 'D':
                            currLocation.y--;
                            break;
                        default:
                            throw new System.InvalidProgramException("Invalid direction.");
                    }
                    bitmap.Add(currLocation);
                }
            }
            return bitmap;
        }
    }
}
