using System;
namespace CustomCollections
{
    public class BinaryHeap<T>
    {
        private Node<T>[] elements;
        private int size;

        public int Count
        {
            get { return size; }
        }

        private const int INITIAL_CAPACITY = 10;


        public BinaryHeap()
        {
            elements = new Node<T>[INITIAL_CAPACITY];
            size = 0;
        }

        public void Insert(T element, int priority)
        {
            if (this.IsEmpty())
            {
                elements[0] = new Node<T>(element, priority);
                size++;
                return;
            }
            else if (this.size >= elements.Length)
                Resize();

            elements[size] = new Node<T>(element, priority);
            bool finished = false;
            int currentPosition = size;

            while (currentPosition != 0 && !finished)
            {
                int parentPos = (currentPosition - 1) / 2;
                if (elements[currentPosition].priority < elements[parentPos].priority)
                {
                    Swap(parentPos, currentPosition);
                }
                else finished = true;

                currentPosition = parentPos;
            }
            size++;
        }

        public T Pop()
        {
            if (this.IsEmpty())
                throw new InvalidOperationException("The heap is empty.");
            T ret = elements[0].data;
            elements[0] = elements[size - 1];
            int currentPosition = 0;
            while (currentPosition < size/2)
            {
                int child1Pos = currentPosition * 2 + 1;
                int child2Pos = currentPosition * 2 + 2;
                int minPos;
                if (child2Pos < this.size)
                {
                    minPos = (elements[child1Pos].priority < elements[child2Pos].priority)
                        ? child1Pos : child2Pos;
                }
                else minPos = child1Pos;

                if (elements[currentPosition].priority > elements[minPos].priority)
                {
                    Swap(currentPosition, minPos);
                    currentPosition = minPos;
                }
                else break;
            }

            size--;
            return ret;
        }

        public T Peek()
		{
			if (this.IsEmpty())
				throw new InvalidOperationException("The heap is empty.");

            return elements[0].data;
        }

        public bool IsEmpty()
        {
            return this.size == 0;
        }

        private void Swap(int posA, int posB)
        {
			Node<T> temp = elements[posA];
			elements[posA] = elements[posB];
			elements[posB] = temp;
        }

        private void Resize()
        {
            Node<T>[] newElements = new Node<T>[elements.Length * 2];
            elements.CopyTo(newElements, 0);
            this.elements = newElements;
        }

    }
}
