using System;

namespace csds
{
    // without size, about 8T slower than SortedSet.
    public class PersistentTreapMap <K, T> : PersistentMap<K, T> where K : IComparable where T : new()
    {
        static Random rnd = new Random();

        internal class TreapNode
        {
            public K key;
            public T value;
            public UInt32 size;
            public Byte height;
            public TreapNode left, right;
            //instead of using an update function, we use a constructor each time
            public TreapNode(K key, T value)
            {
                size = height = 1;
                left = right = null;
                this.key = key;
                this.value = value;
            }
            public TreapNode(TreapNode other) : this(other, other.left, other.right)
            {
                //nothing else
            }
            public TreapNode(TreapNode other, TreapNode left, TreapNode right)
            {
                //copy related
                this.key = other.key;
                this.value = other.value;
                //childs related
                this.left = left;
                this.right = right;
                //  """update"""
                this.size = 1 + (left != null ? left.size : 0) + (right != null ? right.size : 0);
                this.height = (byte)(1 + Math.Max(left != null ? left.height : 0, right != null ? right.height : 0));
            }
            public bool HigherPriority(TreapNode other)
            {
                //RBST-like probability based on tree size
                return rnd.Next() % (this.size + other.size) < this.size;
            }
            public TreapNode Merge(TreapNode other)
            {
                //no merge needed
                if(other == null)
                    return new TreapNode(this);
                //priority check
                if(HigherPriority(other))
                {
                    TreapNode r;
                    //if there is nothing to merge with
                    if(this.right == null)  r = new TreapNode(other);
                    else r = this.right.Merge(other);

                    return new TreapNode(this, this.left, r);
                }
                else
                {
                    TreapNode l = this.Merge(other.left);
                    return new TreapNode(other, l, other.right);
                }
            }
            public TreapNode[] Split(K k)
            {
                int res = this.key.CompareTo(k);
                TreapNode[] ans = new TreapNode[]{null, null};
                if(res < 0)
                {
                    if(this.right == null)
                        ans[0] = new TreapNode(this);
                    else{
                        var tmp = this.right.Split(k);
                        ans[0] = new TreapNode(this, this.left, tmp[0]);
                        ans[1] = tmp[1];
                    }
                }
                else
                {
                    if(this.left == null)
                        ans[1] = new TreapNode(this);
                    else
                    {
                        var tmp = this.left.Split(k);
                        ans[1] = new TreapNode(this, tmp[1], this.right);
                        ans[0] = tmp[0];
                    }
                }
                return ans;
            }
            public TreapNode Find(K k)
            {
                int b = k.CompareTo(key);
                if(b == 0) return this;
                if(b < 0) return left != null ? left.Find(k) : null;
                else return right != null ? right.Find(k) : null;
            }
            public TreapNode Change(K key, T value)
            {
                int b = key.CompareTo(this.key);
                if(b == 0){
                    var z = new TreapNode(this);
                    z.value = value;
                    return z;
                }
                if(b < 0) return new TreapNode(this, this.left.Change(key, value), this.right);
                else return new TreapNode(this, this.left, this.right.Change(key, value));
            }
            public TreapNode Insert(TreapNode other)
            {
                var y = this.Split(other.key);
                return (y[0] != null ? y[0].Merge(other) : other).Merge(y[1]);
            }
            public TreapNode Remove(K key)
            {
                int b = key.CompareTo(this.key);
                if(b == 0)
                {
                    return (left == null) ? (right != null ? new TreapNode(right) : null) : left.Merge(right);
                }
                if(b < 0) return new TreapNode(this, left != null ? left.Remove(key) : null, right);
                else return new TreapNode(this, left, right != null ? right.Remove(key) : null);
            }

            internal void InOrder()
            {
                if(left != null) left.InOrder();
                Print();
                if(right != null) right.InOrder();
            }
            internal void PreOrder()
            {
                Print();
                if(left != null) left.PreOrder();
                if(right != null) right.PreOrder();
            }
            internal void Print()
            {
                Console.WriteLine($"{key} : {value}");
            }

            internal TreapNode GetKth(uint k)
            {
                uint pos = left != null ? left.size : 0;
                int b = k.CompareTo(pos);
                if(b == 0) return this;
                if(b < 0) return left.GetKth(k);
                else return right.GetKth(k - pos - 1);
            }
            internal TreapNode ChangeKth(uint k, T value)
            {
                uint pos = left != null ? left.size : 0;
                int b = k.CompareTo(pos);
                if(b == 0)
                {
                    TreapNode tmp = new TreapNode(this);
                    tmp.value = value;
                    return tmp;
                }
                else if(b < 0) return new TreapNode(this, left.ChangeKth(k, value), right);
                else return new TreapNode(this, left, right.ChangeKth(k - pos - 1, value));
            }
        }
        private TreapNode root = null;
        public uint Size => root == null ? 0 : root.size;

        public uint Height => (uint) (root == null ? 0 : root.height);

        public PersistentMap<K, T> Assign(K key, T value)
        {
            if(root == null)
            {
                return new PersistentTreapMap<K, T>(new TreapNode(key, value));
            }
            TreapNode y = root.Find(key);
            if(y == null) return new PersistentTreapMap<K, T>(root.Insert(new TreapNode(key, value)));
            return new PersistentTreapMap<K, T>(root.Change(key, value));
        }

        public PersistentMap<K, T> ChangeKth(uint pos, T value)
        {
            return new PersistentTreapMap<K, T>(root.ChangeKth(pos, value));
        }

        public bool Contains(K key)
        {
            return (root != null) ? root.Find(key) != null : false;
        }

        public T Find(K key)
        {
            if(root == null) return new T();
            var y = root.Find(key);
            return y == null ? new T() : y.value;
        }

        public T GetKth(uint k)
        {
            if(k < 0 || k >= Size) throw new IndexOutOfRangeException();
            return root.GetKth(k).value;
        }

        public void PrintInOrder()
        {
            if(root != null) root.InOrder();
            Console.WriteLine();
        }

        public void PrintPreOrder()
        {
            if(root != null) root.PreOrder();
            Console.WriteLine();
        }

        public PersistentMap<K, T> Remove(K key)
        {
            if(!Contains(key)) return new PersistentTreapMap<K,T>(root);
            return new PersistentTreapMap<K, T>(root.Remove(key));
        }

        private PersistentTreapMap(TreapNode root)
        {
            this.root = new TreapNode(root);
        }
        public PersistentTreapMap()
        {

        }
    }
}