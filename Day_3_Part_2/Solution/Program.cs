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
            Console.WriteLine($"Least combined steps: {Program.ExampleFunction(input)}");
        }

        public static int ExampleFunction(string paths)
        {
            return LeastSumDistanceIntersection(paths);
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

        public static int StepsToIntersection(string path, (int, int) intersection)
        {
            // NOTE: Not DRY, duplicate code with PathToSet
            //  I see no simple way to refactor and the time spent
            //  refactoring will outweigh the amount of times I am going
            //  to re-use this code.

            int steps = 0;
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
                    steps++;
                    if(currLocation == intersection){
                        break;
                    }
                }
                if(currLocation == intersection){
                    break;
                }
            }
            return steps;
        }

        public static int LeastSumDistanceIntersection(string paths){
            var intersections = Program.PathsIntersections(paths);
            var intersection_distances = new List<int>();

            foreach (var intersection in intersections)
            {
                int sum_distance = 0;
                foreach (var path in paths.Split('\n'))
                {
                    sum_distance += Program.StepsToIntersection(path, intersection);
                }                
                intersection_distances.Add(sum_distance);
            }

            return intersection_distances.Min();
        }
    }
}
