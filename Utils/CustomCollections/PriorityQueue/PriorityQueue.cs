using System;

namespace CustomCollections
{
    /// <summary>
    /// Really Simple implementation of a priority queue
    /// to solve challenge #316 using best first search.
    /// It uses a custom binary heap to store the elements and
    /// perform the operations.
    /// </summary>
    public class PriorityQueue<T>
    {
        private BinaryHeap<T> elements;

        public int Count
        {
            get { return elements.Count; }
        }

        public PriorityQueue()
        {
            elements = new BinaryHeap<T>();
        }

        public void Enqueue(T element, int priority)
        {
            elements.Insert(element, priority);
        }

        public T Dequeue()
        {
            return elements.Pop();
        }
    }
}
