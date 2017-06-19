using System;
using System.Collections.Generic;
using Utils;

namespace KnightsMetric
{
    class MainClass
    {
        private static readonly Tuple<int, int>[] movements = {
            new Tuple<int, int>(-1, -2),
            new Tuple<int, int>(1, -2),
            new Tuple<int, int>(-1, 2),
            new Tuple<int, int>(1, 2),
            new Tuple<int, int>(-2, -1),
            new Tuple<int, int>(2, -1),
            new Tuple<int, int>(-2, 1),
            new Tuple<int, int>(2, 1)
        };


        public static void Main(string[] args)
        {
            Point start = new Point(0, 0);
            Point goal = new Point(-2, 5);
            BreadthFirstSearch(start, goal);
        }


        private static void BreadthFirstSearch(Point start, Point goal)
        {
            Queue<Point> queue = new Queue<Point>();
            IDictionary<Point, Point> path = new Dictionary<Point, Point>();

            queue.Enqueue(start);
            path.Add(start, null);
            while (queue.Count != 0)
            {
                Point current = queue.Dequeue();
                if (current.Equals(goal))
                {
                    PrintPath(path, start, current);
                    return;
                }


                // expand point
                foreach (var move in movements)
                {
                    Point neighbour = new Point(current.x + move.Item1, current.y + move.Item2);
                    if (!path.ContainsKey(neighbour))
                    {
                        queue.Enqueue(neighbour);
                        path.Add(neighbour, current);
                    }
                }
            }
        }


        private static void PrintPath(IDictionary<Point, Point> path, Point start, Point goal)
        {
            LinkedList<Point> pathList = new LinkedList<Point>();
            Point current = goal;
            pathList.AddFirst(current);
            int numberOfMoves = 0;

            do
            {
                current = path[current];
                pathList.AddFirst(current);
                numberOfMoves++;
            } while (current != start);

            Console.WriteLine("Start point: {0}. Goal: {1}.", start, goal);
            Console.WriteLine("Path: ");
            foreach (Point p in pathList)
            {
                Console.Write(p);
                if (p != pathList.Last.Value)
                {
                    Console.Write(" -> ");
                }
            }
            Console.WriteLine("\nNumber of moves: {0}", numberOfMoves);
        }
    }
}