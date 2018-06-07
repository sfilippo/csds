using System;

namespace csds
{
    public interface PersistentMap<K, T> where K : IComparable
    {
        PersistentMap<K,T> Assign(K key, T value);
        PersistentMap<K,T> Remove(K key);
        bool Contains(K key);
        T Find(K key);
        T GetKth(K key);
        PersistentMap<K, T> ChangeKth(K key);
        UInt32 Size{get;}
        UInt32 Height{get;}
        void PrintInOrder();
        void PrintPreOrder();
    }
}
