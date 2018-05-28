using System;
using System.Collections.Generic;

namespace csds
{
    class Program
    {
        const int N = (int)1e6;
        const int M = (int)1e9;
        static void TestSet()
        {
            //once Multiset is implemented, pick the best implementation and adapt it
        }
        static void TestMap()
        {
            //refer to TestSet
        }
        //right now, multiset implemented as treap doesn't yield satisfying results.
        static void TestMultiset()
        {
            //validity check
            TreapMultiset<int> set = new TreapMultiset<int>();
            for(int i = 0; i < 20; i++)
                set.Insert(i / 2);
            for(int i = 1; i < 10; i += 2)
                set.Remove(i);
            for(int i = 0; i < 10; i += 3)
                set.Insert(i);             
            Console.WriteLine("0 0 0 1 2 2 3 3 4 4 5 6 6 6 7 8 8 9 9");
            set.Print();
            Console.WriteLine();
            //speed check
            Random rnd = new Random();
            Console.WriteLine($"Inserting {N} items...");
            var y = new System.Diagnostics.Stopwatch();
            y.Start();
            for(int i = 0; i < N; i++)
                set.Insert(rnd.Next() % M);
            y.Stop();
            double time = y.ElapsedMilliseconds;
            Console.WriteLine($"Done in {time} millisecs.");
            Console.WriteLine($"Removing {N} items...");
            y = new System.Diagnostics.Stopwatch();
            y.Start();
            for(int i = 0; i < N; i++)
                set.Remove(rnd.Next() % M);
            y.Stop();
            time = y.ElapsedMilliseconds;
            Console.WriteLine($"Done in {time} millisecs.");
        }
        static void TestArray()
        {
            //implement as set, but with implicit key.
            //worth considering Treaps as Merge/Split might be very interesting
        }

        //Benchmark on OrderedSet to compare Performance.
        //Anything up to 2x slower might be ok, I guess.
        static void TestStdSet()
        {
            SortedSet<int> ss = new SortedSet<int>();
            Random rnd = new Random();
            var y = new System.Diagnostics.Stopwatch();
            y.Start();
            for(int i = 0; i < N; i++)
                ss.Add(rnd.Next() % M);
            y.Stop();
            Console.WriteLine($"STD insertion took {y.ElapsedMilliseconds} ms.");
            y = new System.Diagnostics.Stopwatch();
            y.Start();
            for(int i = 0; i < N; i++)
                ss.Remove(rnd.Next() % M);
            y.Stop();
            Console.WriteLine($"STD removal took {y.ElapsedMilliseconds} ms.");
        }
        static void Main(string[] args)
        {
            TestMultiset();
            TestStdSet();
        }
    }
}
