/* Attributions
 * Mike DePaul
 * https://github.com/mdepaul/RandomizedList.git
 * **/

using System;
using System.Collections;
using System.Collections.Generic;

namespace MD.RandoCalrissian
{
    /// <summary>
    /// Takes a series of objects and randomizes their order.
    /// </summary>
    /// <typeparam name="T">The type of object, like 'string' or 'int'</typeparam>
    public class RandomizedList<T> : IEnumerable<T>, IList<T>
    {
        IPrng Prng;
        SortedDictionary<int, T> RandomizedDictionary = new SortedDictionary<int, T>();
        List<T> TheList = new List<T>();

        private RandomizedList()
        {
        }

        /// <summary>
        /// Create a new RandomizedList of type T
        /// </summary>
        /// <param name="prng">A class that implements IPrng and can create strong pseduo-random numbers</param>
        public RandomizedList(IPrng prng)
        {
            Prng = prng;
        }

        public int IndexOf(T item)
        {
            return TheList.IndexOf(item);
        }

        /// <summary>
        /// Does nothing. You can not insert into a specific index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Insert(int index, T item)
        {
            //Do nothing
        }


        public void RemoveAt(int index)
        {
            this.Remove(TheList[index]);
            TheList.RemoveAt(index);
        }

        /// <summary>
        /// Since items are ordered randomly, you can not set an item at any index.
        /// Nothing will happen.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index]
        {
            get
            {
                return TheList[index];
            }
            set
            {
                //Do nothing
            }
        }

        public void Add(T item)
        {
            int index = Prng.GetRandomByte().ToInt32() * Prng.GetRandomByte().ToInt32();
            while (RandomizedDictionary.ContainsKey(index))
            {
                //If we get the same key by accident, keep trying until we get a unique one
                index = Prng.GetRandomByte().ToInt32() * Prng.GetRandomByte().ToInt32();
            }
            RandomizedDictionary.Add(index, item);
            CreateList();
        }

        public RandomizedList<T> Add(T[] anArray)
        {
            foreach (var item in anArray)
            {
                this.Add(item);
            }
            return this;
        }

        void CreateList()
        {
            TheList.Clear();
            foreach (var item in RandomizedDictionary)
            {
                TheList.Add(item.Value);
            }
        }

        public void Clear()
        {
            TheList.Clear();
            RandomizedDictionary.Clear();
        }

        public bool Contains(T item)
        {
            return TheList.Contains(item);
        }

        /// <summary>
        /// Copies all the elements of the current one-dimensional array to the specified one-dimensional array starting at the specified destination array index.
        /// <para>The index is specified as a 32-bit integer.</para>
        /// </summary>
        /// <param name="array">The one-dimensional array that is the destination of the elements copied from the current array.</param>
        /// <param name="arrayIndex">A 32-bit integer that represents the index in array at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            int pos = 0;
            for (int i = arrayIndex; i < TheList.Count; i++)
            {
                array[pos] = TheList[i];
                pos++;
            }
        }

        public int Count
        {
            get { return TheList.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            return TheList.Remove(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return TheList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return TheList.GetEnumerator();
        }

        public override string ToString()
        {
            return String.Format("Type: {0}; Length: {1}", typeof(T), this.Count);
        }
    }
}
