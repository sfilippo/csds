using System;

namespace csds
{
    public abstract class Multiset<T> where T : IComparable
    {
        abstract public void Insert(T key);
        abstract public void Remove(T key);
        abstract public UInt32 Size{get;}
        abstract public void InOrder();
        abstract public void PreOrder();
        public Multiset(){}
    }
}
