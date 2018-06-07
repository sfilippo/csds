using System;

namespace csds
{
    // without size, about 8T slower than SortedSet.
    public class PersistentTreapMap <K, T> : PersistentMap<K, T> where K : IComparable where T : new()
    {
        static Random rnd = new Random();

        class TreapNode
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
                this.height = (byte)(1 + Math.Max(left != null ? left.size : 0, right != null ? right.size : 0));
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
        }

        //to do
        
        public uint Size => throw new NotImplementedException();

        public uint Height => throw new NotImplementedException();

        public PersistentMap<K, T> Assign(K key, T value)
        {
            throw new NotImplementedException();
        }

        public PersistentMap<K, T> ChangeKth(K key)
        {
            throw new NotImplementedException();
        }

        public bool Contains(K key)
        {
            throw new NotImplementedException();
        }

        public T Find(K key)
        {
            throw new NotImplementedException();
        }

        public T GetKth(K key)
        {
            throw new NotImplementedException();
        }

        public void PrintInOrder()
        {
            throw new NotImplementedException();
        }

        public void PrintPreOrder()
        {
            throw new NotImplementedException();
        }

        public PersistentMap<K, T> Remove(K key)
        {
            throw new NotImplementedException();
        }
    }
}