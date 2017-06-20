using System;
using System.Collections.Generic;
using CustomCollections;
using Utils;

namespace KnightsMetric
{
    class MainClass
    {
        // NOTE: The problem states that we can move freely
        // through the board (i.e. the board extends infinitely
        // in all directions). That is why I didn't create an array
        // representing the board nor checked bounds in the methods
        // and instead used custom points.

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
            Point goal = new Point(525, 157);
            Console.WriteLine("Using Breadth First Search:");
            BreadthFirstSearch(start, goal);
            Console.WriteLine("\nUsing Best First Search:");
			BestFirstSearch(start, goal);
			Console.WriteLine("\nUsing A*:");
            AStar(start, goal);
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

        /// <summary>
        /// Uses best first seach to solve the problem.
        /// It is faster than breadth first seach, however
        /// it doesn't always ensure an optimal solution.
        /// </summary>
        /// <param name="start">Start.</param>
        /// <param name="goal">Goal.</param>
        private static void BestFirstSearch(Point start, Point goal)
        {
            PriorityQueue<Point> queue = new PriorityQueue<Point>();
            IDictionary<Point, Point> path = new Dictionary<Point, Point>();

            queue.Enqueue(start, 0);
            path.Add(start, null);
            while (queue.Count != 0)
            {
                Point current = queue.Dequeue();
                if (current.Equals(goal))
                {
                    PrintPath(path, start, current);
                    return;
                }

                foreach (var move in movements)
                {
                    Point neighbour = new Point(current.x + move.Item1, current.y + move.Item2);
                    if (!path.ContainsKey(neighbour))
                    {
                        int priority = ManhattanDistance(neighbour, goal);
                        queue.Enqueue(neighbour, priority);
                        path.Add(neighbour, current);
                    }
                }
            }
        }

        private static void AStar(Point start, Point goal)
        {
			PriorityQueue<Point> queue = new PriorityQueue<Point>();
            // used to reconstruct the path once we find a solution
			IDictionary<Point, Point> previous = new Dictionary<Point, Point>();
            // stores the 'cost' (number of positions moved) from the start to that point
            IDictionary<Point, int> costs = new Dictionary<Point, int>();

			queue.Enqueue(start, 0);
			previous.Add(start, null);
            costs.Add(start, 0);
			while (queue.Count != 0)
			{
				Point current = queue.Dequeue();
				if (current.Equals(goal))
				{
					PrintPath(previous, start, current);
					return;
				}

				foreach (var move in movements)
				{
					Point neighbour = new Point(current.x + move.Item1, current.y + move.Item2);
					if (!previous.ContainsKey(neighbour))
					{
                        int cost = costs[current] + 3;
						int priority = cost + ManhattanDistance(neighbour, goal);
						queue.Enqueue(neighbour, priority);
						previous.Add(neighbour, current);
                        costs.Add(neighbour, cost);
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

        private static int ManhattanDistance(Point origin, Point destination)
        {
            return Math.Abs(origin.x - destination.x) + Math.Abs(origin.y - destination.y);
        }
    }
}