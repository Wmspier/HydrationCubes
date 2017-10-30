using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeIndex : MonoBehaviour {

    public class Point
    {
        public int X;
        public int Y;
        public int Z;

        public Point(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }
		public static Point operator -(Point p1, Point p2)
		{
			return new Point(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
		}
		public static Point operator +(Point p1, Point p2)
		{
            return new Point(p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z );
		}
        public static bool operator ==(Point p1, Point p2)
        {
            return (p1.X == p2.X &&
                    p1.Y == p2.Y &&
                    p1.Z == p2.Z);
        }
		public static bool operator !=(Point p1, Point p2)
		{
			return !(p1.X == p2.X &&
				     p1.Y == p2.Y &&
                     p1.Z == p2.Z);
		}
		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
				return false;
            var p1 = this;
            var p2 = obj as Point;

			return (p1.X == p2.X &&
					p1.Y == p2.Y &&
					p1.Z == p2.Z);
		}
		public override int GetHashCode()
		{
			return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
		}
    }
		
    public Point Index;
    public CubeIndex()
    {
        Index = new Point(0, 0, 0);
    }
    public CubeIndex(int x, int y, int z)
    {
        Index = new Point(x, y, z);
    }
}
