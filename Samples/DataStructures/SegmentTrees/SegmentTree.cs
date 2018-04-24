using System;

namespace Samples.DataStructures.SegmentTrees
{
    public class SegmentTree
    {
        private Node<int> [] _segments;

        

        public SegmentTree(int [] items )
        {
            _segments = new Node<int>[ 2 * ((1 << (int) Math.Ceiling(Math.Log(items.Length, 2)))) -1 ];
             PreProcess(items);
        }


        private void PreProcess(int [] items)
        {
            _segments[0] = new Node<int>(PreProcess(items, 0, items.Length - 1, 0), 0, items.Length - 1);
        }

        private int   PreProcess(int [] items, int ini , int fin, int node )
        {

            if (ini > fin)
                return 0;

            if (ini == fin)
            {
                _segments[node] = new Node<int>(items[ini], ini, fin);
                return items[ini];
            }

            int middle = (ini + fin) / 2;


            int value1 =PreProcess(items, ini, middle , LeftChild(node));
            int value2 =PreProcess(items, middle + 1, fin, RightChild(node));

            _segments[node] = new Node<int>(value1+value2 , ini , fin);

            return _segments[node].Value;
        }


        public void ChangeValue(int index, int newValue)
        {
            ChangeValue(index,  newValue, 0);
        }


        public int ChangeValue(int index, int newValue, int nodeIndex)
        {
            var node = _segments[nodeIndex];

            if (node == null)
                return 0;


            if (_segments[nodeIndex].IsLeaf)
            {
                _segments[nodeIndex].Value = newValue;
                return newValue;
            } 

            if (LeftNode(nodeIndex).Belongs(index))
                  return _segments[nodeIndex].Value = ChangeValue(index, newValue, LeftChild(nodeIndex)) + RightNode(nodeIndex).Value;

            if (RightNode(nodeIndex).Belongs(index))
                return _segments[nodeIndex].Value = ChangeValue(index, newValue, RightChild(nodeIndex)) + LeftNode(nodeIndex).Value;

            return 0;
        }

        public virtual int Query(int ini, int fin)
        {
            return Query(ini, fin, 0);
        }

        private int Query(int ini, int fin, int nodeIndex)
        {
            
            var node = _segments[nodeIndex];

            if (node == null)
                return 0;

            if (ini <= node.Left && node.Right <= fin)
                return node.Value;

            if (fin < node.Left || node.Right < ini)
                return 0;

            return Query(ini, fin, LeftChild(nodeIndex)) + Query(ini, fin, RightChild(nodeIndex));

        }

        private Node<int> RightNode(int node)
        {
            return _segments[RightChild(node)];
        }
        private Node<int> LeftNode(int node)
        {
            return _segments[LeftChild(node)] ;
        }

        private static int RightChild(int nodeIndex)
        {
            return 2 * nodeIndex + 2;
        }

        private static int LeftChild(int nodeIndex)
        {
            return 2 * nodeIndex + 1;
        }
    }


    public class Node<T>
    {
        public T Value { get; set; }

        public int Left { get; set; }

        public int Right { get; set; }

        public Node(T value, int left, int right)
        {
            Value = value;
            Left = left;
            Right = right;
        }

        public bool Belongs(int index)
        {
            return Left <= index && index <= Right;
        }


        public bool IsLeaf => Left == Right;
    }

}
