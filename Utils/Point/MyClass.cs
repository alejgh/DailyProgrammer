using System;

namespace Utils
{
    public class Point
    {
        public int x;
        public int y;

        public Point(int x, int y) 
        {
            this.x = x;
            this.y = y;
        }

        public override bool Equals(Object obj) {
            Point otherPoint = obj as Point;
            if (otherPoint == null) return false;

            return this.x == otherPoint.x && this.y == otherPoint.y;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("({0}, {1})", x, y);
        }
    }
}
