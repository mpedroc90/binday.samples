using System;

namespace Sample.DataStructures.SegmentTrees
{

    public delegate T SegmentTreeMerge<T>(T LeftChild, T RightChild);

    public delegate T DefaultValue<out T>();

    public interface ISegmentTree<T>
    {
        void PreProcess(T [] items);
        void ChangeValue(int index, T newValue);
        T Query(int ini, int fin);
    }

    public class SegmentTree<T> : ISegmentTree<T>
    {

        protected class Node
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
        protected readonly SegmentTreeMerge<T> _mergeFunction;
        protected readonly DefaultValue<T> _defaultValue;
        protected  Node [] _segments;

        public SegmentTree(T [] items , SegmentTreeMerge<T> mergeFunction , DefaultValue<T> defaultValue= null)
        {
            _mergeFunction = mergeFunction;
            _defaultValue = defaultValue ?? (() => default(T));
            // ReSharper disable once VirtualMemberCallInConstructor
            PreProcess(items);
        }


        public virtual void PreProcess(T [] items)
        {
            _segments = new Node[2 * (1 << (int)Math.Ceiling(Math.Log(items.Length, 2))) - 1];
            _segments[0] = new Node(PreProcess(items, 0, items.Length - 1, 0), 0, items.Length - 1);
        }

        private T PreProcess(T [] items, int ini , int fin, int node )
        {

            if (ini > fin)
                return _defaultValue();

            if (ini == fin)
            {
                _segments[node] = new Node(items[ini], ini, fin);
                return items[ini];
            }


            int middle = (ini + fin) / 2;


            T leftResult =PreProcess(items, ini, middle , LeftChildIndex(node));
            T rightResult =PreProcess(items, middle + 1, fin, RightChildIndex(node));
            T newValue = _mergeFunction(leftResult, rightResult);


            _segments[node] = new Node(newValue, ini , fin);

            return _segments[node].Value;
        }

        public virtual void ChangeValue(int index, T newValue)
        {
            ChangeValue(index,  newValue, 0);
        }

        private  T ChangeValue(int index, T newValue, int nodeIndex)
        {
            var node = _segments[nodeIndex];

            if (node == null)
                  return _defaultValue();


            if (_segments[nodeIndex].IsLeaf)
            {
                _segments[nodeIndex].Value = newValue;
                return newValue;
            } 

            if (LeftNode(nodeIndex).Belongs(index))
                  return _segments[nodeIndex].Value = _mergeFunction(ChangeValue(index, newValue, LeftChildIndex(nodeIndex)) , RightNode(nodeIndex).Value);

            if (RightNode(nodeIndex).Belongs(index))
                return _segments[nodeIndex].Value = _mergeFunction(LeftNode(nodeIndex).Value ,  ChangeValue(index, newValue, RightChildIndex(nodeIndex))) ;

            return _defaultValue();
        }

        public virtual T Query(int ini, int fin)
        {
            return Query(ini, fin, 0);
        }

        private T Query(int ini, int fin, int nodeIndex)
        {
            
            var node = _segments[nodeIndex];

            if (node == null)
                return _defaultValue();

            if (ini <= node.Left && node.Right <= fin)
                return node.Value;

            if (fin < node.Left || node.Right < ini)
                return _defaultValue();

            return _mergeFunction(Query(ini, fin, LeftChildIndex(nodeIndex)) , Query(ini, fin, RightChildIndex(nodeIndex)));

        }

        protected virtual Node RightNode(int node)
        {
            return _segments[RightChildIndex(node)];
        }

        protected virtual Node LeftNode(int node)
        {
            return _segments[LeftChildIndex(node)] ;
        }

        protected virtual  int RightChildIndex(int nodeIndex)
        {
            return 2 * nodeIndex + 2;
        }

        protected virtual  int LeftChildIndex(int nodeIndex)
        {
            return 2 * nodeIndex + 1;
        }
    }




}
