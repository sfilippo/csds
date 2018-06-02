using System;

namespace csds
{
    // without size, about 8T slower than SortedSet.
    public class TreapMultiset <T> : Multiset<T> where T : IComparable
    {
        public static Random rnd = new Random();
        public class TreapNode
        {
            public T val;
            public Int32 y = rnd.Next();
            public UInt32 size = 1;
            // public UInt32 height = 1;
            public TreapNode left = null, right = null;
            public TreapNode (T val)
            {
                this.val = val;
            }
            private void Update()
            {
                // size = 1 + (left != null ? left.size : 0) + (right != null ? right.size : 0);
                // height = 1 + Math.MaT(left != null ? left.height : 0, right != null ? right.height : 0);
            }
            public bool PriorityOver(TreapNode other)
            {
                return this.y < other.y;
            }
            public TreapNode Merge(TreapNode other)
            {
                if(other == null) return this;
                if(PriorityOver(other))
                {
                    if(this.right == null) this.right = other;
                    else this.right = this.right.Merge(other);
                    Update();
                    return this;
                }
                else
                {
                    //no need to check if left is null
                    other.left = this.Merge(other.left);
                    other.Update();
                    return other;
                }
            }
            public Tuple<TreapNode, TreapNode> Split(T k)
            {
                //if this needs to go left
                if(this.val.CompareTo(k) < 0)
                {
                    if(this.right == null) return Tuple.Create(this, (TreapNode)null);
                    var T = this.right.Split(k);
                    this.right = T.Item1;
                    this.Update();
                    return Tuple.Create(this, T.Item2);
                }
                else
                {
                    if(this.left == null) return Tuple.Create((TreapNode)null, this);
                    var T = this.left.Split(k);
                    this.left = T.Item2;
                    this.Update();
                    return Tuple.Create(T.Item1, this);
                }
            }
            public TreapNode Insert(TreapNode other)
            {
                var T = this.Split(other.val);
                return (T.Item1 != null ? T.Item1.Merge(other) : other).Merge(T.Item2);
            }
            public TreapNode Remove(T k)
            {
                int comp = k.CompareTo(this.val);
                if(comp == 0)
                    if(this.left == null)
                        return this.right;
                    else
                        return this.left.Merge(this.right);
                if(comp < 0)
                {
                    if(this.left != null)
                        this.left = this.left.Remove(k);
                }
                else
                {
                    if(this.right != null)
                        this.right = this.right.Remove(k);
                }
                this.Update();
                return this;
            }
            public void InOrder()
            {
                if(left != null) left.InOrder();
                Console.Write(this.val + " ");
                if(right != null) right.InOrder();
            }
            public void PreOrder()
            {
                Console.Write(this.val + " ");
                if(left != null) left.PreOrder();
                if(right != null) right.PreOrder();
            }
        }
        private TreapNode root = null;
        public override UInt32 Size{ get => root == null ? 0 : root.size;}
        public override void Insert(T val)
        {
            if(root == null) root = new TreapNode(val);
            else root = root.Insert(new TreapNode(val));
        }
        public override void Remove(T val)
        {
            if(root == null) return;
            else root = root.Remove(val);
        }
        public override void InOrder()
        {
            if(root == null)
                Console.WriteLine("Multiset is empty.");
            else
            {
                root.InOrder();
                Console.WriteLine();
            }
        }
        public override void PreOrder()
        {
            if(root == null)
                Console.WriteLine("Multiset is empty.");
            else
            {
                root.PreOrder();
                Console.WriteLine();
            }
        }
    }
}