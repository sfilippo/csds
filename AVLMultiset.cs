using System;

namespace csds
{
    // With size commented, runs 5 times slower than OrderedSet
    // Otherwise, about 6.
    class AVLMultiset <T> : Multiset<T> where T: IComparable
    {
        public class AVLNode
        {
            public T Value {get; private set;}
            public UInt32 Size {get; private set;}
            public SByte Height {get; private set;}
            private AVLNode[] nodes;
            public AVLNode Left {get => nodes[0]; private set => nodes[0] = value; }
            public AVLNode Right {get => nodes[1]; private set => nodes[1] = value; }
            //Balance is difference between left and right childs height. Should stay in [-1, 1]
            public SByte Balance
            {
                get
                {
                    return (SByte) ((Right == null ? (SByte)0 : Right.Height) - (Left == null ? (SByte)0 : Left.Height));
                }
            }
            public AVLNode(T val)
            {
                this.Value = val;
                this.Size = 1;
                this.Height = 1;
                this.nodes = new AVLNode[]{null, null};
            }
            private void Update()
            {
                this.Size = 1 + (Left != null ? Left.Size : 0) + (Right != null ? Right.Size : 0);
                this.Height = (SByte)(1 + Math.Max(Left != null ? Left.Height : (SByte)0, Right != null ? Right.Height : (SByte)0));
            }
            private AVLNode Rebalance()
            {
                var b = this.Balance;
                if(Math.Abs(b) < 2) return this;
                //Left unbalanced
                if(b == -2)
                {
                    //if Left is right unbalanced
                    if(Left.Balance == 1)
                        Left = Left.Right.RotateLeft(Left);
                    return Left.RotateRight(this);
                }
                //Right unbalanced
                if(b == 2)
                {
                    //If Right is left unbalanced
                    if(Right.Balance == -1)
                        Right = Right.Left.RotateRight(Right);
                    return Right.RotateLeft(this);
                }

                //Only occurs when merging
                throw new Exception($"Tree is too unbalanced ({b})");
            }
            private AVLNode RotateLeft(AVLNode father)
            {
                //You are father's right child, so you can give him you Left child as his Right child
                father.Right = this.Left;
                father.Update();
                this.Left = father;
                this.Update();
                return this;                
            }
            private AVLNode RotateRight(AVLNode father)
            {
                //You are father's left child, so you can give him your Right child as his Left child
                father.Left = this.Right;
                father.Update();
                this.Right = father;
                this.Update();
                return this;                
            }
            public AVLNode Insert(T key)
            {
                var z = key.CompareTo(this.Value);
                int next = z < 0 ? 0 : 1;
                if(nodes[next] == null)
                {
                    nodes[next] = new AVLNode(key);
                    this.Update();
                    //No need to rebalance
                    return this;
                }
                nodes[next] = nodes[next].Insert(key);
                this.Update();
                return this.Rebalance();
            }
            //If you extend range query, you should rewrite this
            public AVLNode Remove(T key)
            {
                var z = key.CompareTo(this.Value);
                if(z == 0)
                {
                    //This node must be removed, we swap with either successor or predecessor
                    //Easy case
                    if(Left == null) return Right;
                    if(Right == null) return Left;
                    Left = Left.RemovePredecessor(this);
                    this.Update();
                    return this.Rebalance();                           
                }
                int next = (z < 0 ? 0 : 1);
                if(nodes[next] == null) return this;
                nodes[next] = nodes[next].Remove(key);
                this.Update();
                return this.Rebalance();
            }
            //Subfunction called by Remove
            private AVLNode RemovePredecessor(AVLNode source)
            {
                //We found the predecessor, we swap and return Left
                if(Right == null)
                {
                    source.Value = this.Value;
                    return Left;
                }
                Right = Right.RemovePredecessor(source);
                this.Update();
                return this.Rebalance();
            }
            public void InOrder()
            {
                if(Left != null) Left.InOrder();
                Console.Write(this.Value + " ");
                if(Right != null) Right.InOrder();
            }
            public void PreOrder()
            {
                Console.Write(this.Value + " ");
                if(Left != null) Left.PreOrder();
                if(Right != null) Right.PreOrder();
            }
            public bool CheckTreeBalance()
            {
                if(Math.Abs(Balance) >= 2) return false;
                if(Left != null && Left.CheckTreeBalance() == false) return false;
                if (Right != null) return Right.CheckTreeBalance();
                return true;
            }
        }
        private AVLNode root;

        public AVLMultiset()
        {

        }

        public UInt32 Size
        {
            get => root != null ? root.Size : 0;
        }
        public UInt32 Height
        {
            get => (root != null ? (UInt32)root.Height : 0);
        }

        public void Insert(T key)
        {
            if(root == null) root = new AVLNode(key);
            else root = root.Insert(key);
        }
        public void Remove(T key)
        {
            if(root == null) return;
            root = root.Remove(key);
        }
        public bool CheckTreeBalance()
        {
            if(root != null) return root.CheckTreeBalance();
            return true;
        }
        public void InOrder()
        {
            if(root != null) root.InOrder();
            Console.WriteLine();
        }
        public void PreOrder()
        {
            if(root != null) root.PreOrder();
            Console.WriteLine();
        }
    }
}