using System;

namespace csds
{
    public interface PersistentMap<K, T> where K : IComparable
    {
        PersistentMap<K,T> Assign(K key, T value);
        PersistentMap<K,T> Remove(K key);
        bool Contains(K key);
        T Find(K key);
        T GetKth(uint pos);
        PersistentMap<K, T> ChangeKth(uint pos, T value);
        UInt32 Size{get;}
        UInt32 Height{get;}
        void PrintInOrder();
        void PrintPreOrder();
    }
}
