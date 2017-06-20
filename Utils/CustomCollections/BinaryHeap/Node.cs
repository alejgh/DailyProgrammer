using System;
namespace CustomCollections
{
    internal class Node<T>
    {
        internal T data;
        internal int priority;
        
        internal Node(T data, int priority)
        {
            this.data = data;
            this.priority = priority;
        }
    }
}
