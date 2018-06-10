using System;
using System.Collections.Generic;

namespace csds
{
    class Program
    {
        static int N = (int)1e6;
        static int M = (int)1e6;
        static void TestSet()
        {
            //once Multiset is implemented, pick the best implementation and adapt it
        }
        static void TestPTM()
        {
            PersistentTreapMap<int, int> PTM = new PersistentTreapMap<int, int>();

            //* speed check 
            Random rnd = new Random();
            Console.WriteLine($"Inserting {N} items...");
            var y = new System.Diagnostics.Stopwatch();
            long tot = 0;
            y.Start();
            for(int i = 0; i < N; i++)
            {
                int a = rnd.Next() % M;
                int b = rnd.Next() % M;
                // Console.WriteLine($"Insert {a}");
                PTM = (PersistentTreapMap<int,int>)PTM.Assign(a, b);
                //PTM.PrintInOrder();
            }
            y.Stop();
            tot += y.ElapsedMilliseconds;
            double time = y.ElapsedMilliseconds;
            Console.WriteLine($"Done in {time} ms.");
            Console.WriteLine($"Tree height is {PTM.Height}");
            Console.WriteLine($"Removing up to {N} items...");
            y = new System.Diagnostics.Stopwatch();
            y.Start();
            //PTM.PrintPreOrder();
            for(int i = 0; i < N; i++)
            {
                int a = rnd.Next() % M;
                // Console.WriteLine($"Remove {a}");
                PTM = (PersistentTreapMap<int,int>)PTM.Remove(a);
            }
            y.Stop();
            time = y.ElapsedMilliseconds;
            tot += y.ElapsedMilliseconds;
            Console.WriteLine($"Done in {time} ms.");
            Console.WriteLine($"{typeof(PersistentTreapMap<int,int>).ToString()} took {tot} ms.");
            Console.WriteLine($"Tree height is {PTM.Height}");
        }

        static void TestStdDictionary()
        {
            Dictionary<int, int> dict = new Dictionary<int, int>();

            //* speed check 
            Random rnd = new Random();
            Console.WriteLine($"Inserting {N} items...");
            var y = new System.Diagnostics.Stopwatch();
            long tot = 0;
            y.Start();
            for(int i = 0; i < N; i++)
            {
                int a = rnd.Next() % M;
                int b = rnd.Next() % M;
                // Console.WriteLine($"Insert {a}");
                dict[a] = b;
            }
            y.Stop();
            tot += y.ElapsedMilliseconds;
            double time = y.ElapsedMilliseconds;
            Console.WriteLine($"Done in {time} ms.");
            Console.WriteLine($"Removing up to {N} items...");
            y = new System.Diagnostics.Stopwatch();
            y.Start();
            for(int i = 0; i < N; i++)
            {
                int a = rnd.Next() % M;
                // Console.WriteLine($"Remove {a}");
                dict.Remove(a);
            }
            y.Stop();
            time = y.ElapsedMilliseconds;
            tot += y.ElapsedMilliseconds;
            Console.WriteLine($"Done in {time} ms.");
            Console.WriteLine($"{typeof(Dictionary<int,int>).ToString()} took {tot} ms.");
        }

        //Very confused test, where AVL has similar performance to Treap
        static void TestMultiset<T>() where T : Multiset<int>, new()
        {
            T set = new T();

            /* small(est) test that broke AVL
            set.Insert(2);
            set.PreOrder();
            set.Insert(0);
            set.PreOrder();
            set.Insert(6);
            set.PreOrder();
            set.Insert(2);
            set.PreOrder();
            set.Insert(3);
            set.PreOrder();
            set.Insert(3);
            set.PreOrder();
            set.Insert(2);
            set.PreOrder();
            set.Remove(0);
            set.PreOrder();
            set.Remove(0);
            set.PreOrder();
            set.Remove(5);
            set.PreOrder();
            set.Remove(6);
            set.PreOrder();
            set.Remove(4);
            set.PreOrder();
            //*/

            /* tests all possible insert + erase permutations
            if(N >= 7)
            {
                throw new Exception("This would take to much time. Please use smaller N or comment the permutation check");
            }
            List<int> insert = new List<int>();
            List<int> remove = new List<int>();
            for(int i = 0; i < N; i++)
            {
                insert.Add(i);
                remove.Add(i);
            }
            try
            {
                do
                {
                    do
                    {
                        foreach(int x in insert)
                            set.Insert(x);
                        foreach(int x in remove)
                            set.Remove(x);
                    }while(NextPermutation(remove));
                    remove.Reverse();
                } while(NextPermutation(insert));
            }
            catch
            {
                Console.WriteLine($"Error with {insert}, {remove}");
            }
            //*/

            /* validity check 1
            for(int i = 0; i < 20; i++)
                set.Insert(i / 2);
            for(int i = 1; i < 10; i += 2)
                set.Remove(i);
            for(int i = 0; i < 10; i += 3)
                set.Insert(i);             
            Console.WriteLine("0 0 0 1 2 2 3 3 4 4 5 6 6 6 7 8 8 9 9");
            set.Print();
            Console.WriteLine();
            //*/

            //* speed check 
            Random rnd = new Random();
            Console.WriteLine($"Inserting {N} items...");
            var y = new System.Diagnostics.Stopwatch();
            long tot = 0;
            y.Start();
            for(int i = 0; i < N; i++)
            {
                int a = rnd.Next() % M;
                // Console.WriteLine($"Insert {a}");
                set.Insert(a);
            }
            y.Stop();
            tot += y.ElapsedMilliseconds;
            double time = y.ElapsedMilliseconds;
            Console.WriteLine($"Done in {time} ms.");
            Console.WriteLine($"Tree height is {set.Height}");
            Console.WriteLine($"Removing up to {N} items...");
            y = new System.Diagnostics.Stopwatch();
            y.Start();
            for(int i = 0; i < N; i++)
            {
                int a = rnd.Next() % M;
                // Console.WriteLine($"Remove {a}");
                set.Remove(a);
            }
            y.Stop();
            time = y.ElapsedMilliseconds;
            tot += y.ElapsedMilliseconds;
            Console.WriteLine($"Done in {time} ms.");
            Console.WriteLine($"{typeof(T).ToString()} took {tot} ms.");
            Console.WriteLine($"Tree height is {set.Height}");
            //*/

            /* loopify
            TestMultiset();
            //*/
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
            if(args.Length >= 1) N = M = int.Parse(args[0]);
            if(args.Length >= 2) M = int.Parse(args[1]);
            //TestMultiset<AVLMultiset<int>>();
            //TestMultiset<TreapMultiset<int>>();
            //TestStdSet();
            TestPTM();
            TestStdDictionary();
        }

        //source : https://stackoverflow.com/questions/2390954/how-would-you-calculate-all-possible-permutations-of-0-through-n-iteratively/12768718#12768718
        static bool NextPermutation<T>(IList<T> a) where T: IComparable
        {
            if (a.Count < 2) return false;
            var k = a.Count-2;

            while (k >= 0 && a[k].CompareTo( a[k+1]) >=0) k--;
            if(k<0) return false;

            var l = a.Count - 1;
            while (l > k && a[l].CompareTo(a[k]) <= 0) l--;

            var tmp = a[k];
            a[k] = a[l];
            a[l] = tmp;

            var i = k + 1;
            var j = a.Count - 1;
            while(i<j)
            {
                tmp = a[i];
                a[i] = a[j];
                a[j] = tmp;
                i++;
                j--;
            }

            return true;
        }
    }
}
