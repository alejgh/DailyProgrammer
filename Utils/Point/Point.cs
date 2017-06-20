using System;

namespace Utils
{
    public class Point
    {
        public readonly int x;
        public readonly int y;

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

		// GetHashCode implementation from Jon Skeet: 
        // https://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-an-overridden-system-object-gethashcode/263416#263416
		public override int GetHashCode()
		{
			unchecked // Overflow is fine, just wrap
			{
				int hash = 17;
				// Suitable nullity checks etc, of course :)
				hash = hash * 23 + this.x.GetHashCode();
				hash = hash * 23 + this.y.GetHashCode();
				return hash;
			}
		}

        public override string ToString()
        {
            return string.Format("({0}, {1})", x, y);
        }
    }
}
