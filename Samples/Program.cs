using System;
using Samples.DataStructures.SegmentTrees;

namespace Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] items =  {1, 2, 3, 4, 5, 6, 7, 8, 9};

            var segmentTree = new SegmentTree<int>(items,  (a,b)=> a+b);
            Console.WriteLine("18 == "+segmentTree.Query(2, 5));
            Console.WriteLine("30 == " + segmentTree.Query(5, 8));
            Console.WriteLine("22 == " + segmentTree.Query(3, 6));
            segmentTree.ChangeValue(4, 0);
            Console.WriteLine("13 == " + segmentTree.Query(2, 5));
            Console.WriteLine("17 == " + segmentTree.Query(3, 6));
            Console.ReadLine();
        }
    }
}
