﻿namespace Demo
{
    using System;
    using Tree;

    class Program
    {
        static void Main(string[] args)
        {
            var input = new string[] { "9 17", "9 4", "9 14", "4 36", "14 53", "14 59", "53 67", "53 73" };

            var treeFactory = new TreeFactory();

            var tree = treeFactory.CreateTreeFromStrings(input);

            //Console.WriteLine(tree.GetAsString());
            Console.WriteLine($"Leaf keys: {string.Join(", ", tree.GetLeafKeys())}");
            Console.WriteLine($"Internal keys: {string.Join(", ", tree.GetMiddleKeys())}");
        }
    }
}
