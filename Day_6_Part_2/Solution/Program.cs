using System;
using System.Collections.Generic;
using System.Linq;

namespace Solution
{

    public class Node
    {
        public Node parent;
        public List<Node> children = new List<Node>();

        public string id;
        public int depth = 0;

        public Node(string _id, Node _parent = null)
        {
            parent = _parent;
            id = _id;
        }

        public void CalcDepths()
        {

        }
    }

    public class Tree
    {
        public Dictionary<string, Node> nodes = new Dictionary<string, Node>();
        public Node root;

        public Tree(Node _root = null)
        {
            root = _root;
        }

        public void AddNode(Node node)
        {
            if (Contains(node.id))
            {
                throw new NotSupportedException("Node ID is not unique.");
            }
            nodes[node.id] = node;

            if (root == null && node.id == "COM")
            {
                root = node;
            }
        }

        public bool Contains(string id)
        {
            return nodes.Keys.Contains(id);
        }

        public Node GetNode(string id)
        {
            return nodes[id];
        }

        public void AddRelation(string centerId, string satelliteId)
        {
            if (!Contains(centerId))
            {
                AddNode(new Node(centerId));
            }

            if (!Contains(satelliteId))
            {
                AddNode(new Node(satelliteId, nodes[centerId]));
            }

            nodes[centerId].children.Add(nodes[satelliteId]);
        }
        public List<Node> FlattenBFS()
        {
            Queue<Node> q = new Queue<Node>();
            List<Node> discovered = new List<Node>() { root };
            q.Enqueue(root);

            while (q.Count > 0)
            {
                Node node = q.Dequeue();
                foreach (var child in node.children)
                {
                    if (!discovered.Contains(child))
                    {
                        discovered.Add(child);
                        q.Enqueue(child);
                    }
                }
            }
            return discovered;
        }

        public List<Node> FlattenDFAndSetDepth(Node start, List<Node> visited = null, int depth = 0)
        {
            // NOTE: Ideally should be split in a method 'FlattenDF' and 'SetDepth'
            if (visited == null)
            {
                visited = new List<Node>();
            }
            visited.Add(start);
            start.depth = depth;

            foreach (var child in start.children)
            {
                if (!visited.Contains(child))
                {
                    FlattenDFAndSetDepth(child, visited, depth + 1);
                }
            }
            return visited;
        }
    }


    public class Program
    {
        static void Main(string[] args)
        {
            string map = System.IO.File.ReadAllText("input.txt");
            //string map = "COM)B\nB)C\nC)D\nD)E\nE)F\nB)G\nG)H\nD)I\nE)J\nJ)K\nK)L";

            Tree orbitTree = new Tree();

            string[] orbits = map.Split("\n");
            foreach (string orbit in orbits)
            {
                string[] objects = orbit.Split(")");
                string center_id = objects[0];
                string satellite_id = objects[1];
                orbitTree.AddRelation(center_id, satellite_id);

            }

            int sum = 0;
            foreach (var node in orbitTree.FlattenDFAndSetDepth(orbitTree.root))
            {
                sum += node.depth;
                Console.WriteLine($"id: {node.id} depth: {node.depth}");
            }
            Console.WriteLine(sum);
        }

        public static int ExampleFunction(string map)
        {
            return 0;
        }
    }
}
