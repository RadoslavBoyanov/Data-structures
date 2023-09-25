namespace Tree
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Program
    {
        public static void Main(string[] args)
        {
            var tree = new Tree<int>(5,
                            new Tree<int>(3),
                            new Tree<int>(100, 
                                new Tree<int>(1000)),
                            new Tree<int>(55,
                                new Tree<int>(11),
                                new Tree<int>(77),
                                new Tree<int>(8))
                            );
            
        }
    }
}
