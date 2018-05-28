using System;

namespace csds
{
    //about 10x slower than ordered set, probably not worth to continue
    //height is currently commented but supported (and behaves as intended)
    //as size is mandatory, it implements GetKth / [] operator 
    public class RbstMultiset <X> where X : IComparable
    {
        public static Random rnd = new Random();
        public class RbstNode
        {
            public X val;
            public UInt32 size = 1;
            // public UInt32 height = 1;
            public RbstNode left = null, right = null;
            public RbstNode (X val)
            {
                this.val = val;
            }
            public void Update()
            {
                size = 1 + (left != null ? left.size : 0) + (right != null ? right.size : 0);
                // height = 1 + Math.Max(left != null ? left.height : 0, right != null ? right.height : 0);
            }
            public bool PriorityOver(RbstNode other)
            {
                return (rnd.Next() % (this.size + other.size)) < this.size;
            }
            public RbstNode Merge(RbstNode other)
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
            public Tuple<RbstNode, RbstNode> Split(X k)
            {
                //if this needs to go left
                if(this.val.CompareTo(k) < 0)
                {
                    if(this.right == null) return Tuple.Create(this, (RbstNode)null);
                    var x = this.right.Split(k);
                    this.right = x.Item1;
                    this.Update();
                    return Tuple.Create(this, x.Item2);
                }
                else
                {
                    if(this.left == null) return Tuple.Create((RbstNode)null, this);
                    var x = this.left.Split(k);
                    this.left = x.Item2;
                    this.Update();
                    return Tuple.Create(x.Item1, this);
                }
            }
            public RbstNode Insert(RbstNode other)
            {
                var x = this.Split(other.val);
                return (x.Item1 != null ? x.Item1.Merge(other) : other).Merge(x.Item2);
            }
            public RbstNode Find(X k)
            {
                int b = k.CompareTo(this.val);
                if(b == 0) return this;
                if(b < 0) return this.left != null ? this.left.Find(k) : null;
                return this.right != null ? this.right.Find(k) : null;
            }
            public RbstNode GetKth(int i)
            {
                int pos = (left != null ? (int)left.size : 0);
                if(i == pos) return this;
                if(i < pos) return left != null ? left.GetKth(i) : null;
                return right != null ? right.GetKth(i - pos - 1) : null;
            }
            public RbstNode Remove(X k)
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
            public void Print()
            {
                if(left != null) left.Print();
                Console.Write(this.val + " ");
                if(right != null) right.Print();
            }
        }
        private RbstNode root = null;
        public void Insert(X val)
        {
            if(root == null) root = new RbstNode(val);
            else root = root.Insert(new RbstNode(val));
        }
        public bool Find(X val)
        {
            return root != null ? root.Find(val) != null ? true : false : false;
        }
        public RbstNode GetKth(int i)
        {
            return root != null ? root.GetKth(i) : null;
        }
        public X this[int i]
        {
            get
            {
                var y = root.GetKth(i);
                if(y != null) return y.val;
                throw new ArgumentOutOfRangeException();
            }
        }
        public void Remove(X val)
        {
            if(root == null) return;
            else root = root.Remove(val);
        }
        public void Print()
        {
            if(root == null)
                Console.WriteLine("Multiset is empty.");
            else
            {
                root.Print();
                Console.WriteLine();
            }
        }
        // public void Status()
        // {
        //     Console.WriteLine($"{(root != null ? root.size : 0)} / {(root != null ? root.height : 0)}");
        // }
    }
}