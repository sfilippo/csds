using System;

namespace csds
{
    public interface Multiset<T> where T : IComparable
    {
        void Insert(T key);
        void Remove(T key);
        UInt32 Size{get;}
        UInt32 Height{get;}
        void InOrder();
        void PreOrder();
    }
}
