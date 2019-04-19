using Microsoft.Xna.Framework;

namespace ITW.Exts {
	public class Line1D {

		public int X;
		public int Y;
		private int L;

		public Point Location => new Point(X, Y);
		public int Length { get => L; set => L = value; }

		public Line1D(int x, int y, int l){
			this.X = x;
			this.Y = y;
			this.Length = l;
		}

		public Line1D(Point p, int l){
			this.X = p.X;
			this.Y = p.Y;
			this.Length = l;
		}

	}

	public class Line2D {
		public Point Start;
		public Point End;

		public Line2D(Point s, Point e){
			this.Start = s;
			this.End = e;
		}

		public Line2D(int x1, int y1, int x2, int y2){
			Start = new Point(x1, y1);
			End = new Point(x2, y2);
		}

	}
}
