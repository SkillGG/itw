using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Threading;
using System;
using System.Linq;

namespace ITW {
	public class StringDrawn {

		private string full;
		private string visible;
		private DrawType type;

		private int walkerPeriod;
		private Timer walker;

		/// <summary>
		/// A class, that defines how StringDrawn will be drawn
		/// </summary>
		public class DrawType {

			public class Stint {
				public override string ToString() => $"{i},{s}";
				public string s;
				public int i;
				public Stint(string x, int z) { s = x; i = z; }
			}

			/// <summary>
			/// Type of draw:
			/// 0 - all at once
			/// 1 - every character each 'pace' ms
			/// 2 - Point array where X indicates index where to start section (first X in array **IS** always 0) and Y indicates pace
			/// </summary>
			private byte type;
			private int pace;
			private int step = 0;
			private Point[] steps;

			public DrawType() {
				type = 0;
				step = 0;
			}

			public DrawType(int pc) {
				type = 1;
				pace = pc;
			}

			public DrawType(Point[] pace) {
				type = 2;
				steps = pace;
				this.pace = steps[0].Y;
				for( int i = 1; i < steps.Length; i++ )
					steps[i].X--;
				steps = steps.OrderBy((o) => o.X).ToArray( );
			}

			public Stint GetFirstStep(string s) {
				if( type == 2 ) {
					return new Stint("", pace);
				}
				if( type == 1 )
					return new Stint("", pace);
				return new Stint(s, -1);
			}

			private Point getPointFromStep(int n) {
				steps = steps.OrderBy(i => i.X).ToArray( );
				if( this.steps == null || this.steps.Length == 0 )
					throw new NullReferenceException("Points are null! Set at least one!");
				int r = 0;
				for( int i = 0; i < this.steps.Length - 1 ; i++ ) {
					if( this.steps[i].X == this.steps[i + 1].X )
						continue;
					if( n == this.steps[i + 1].X || ( n < steps[i + 1].X && n > steps[i].X ) )
						r = i;
					else if( n == steps[i].X )
						r = i - 1;
					else if( i == steps.Length - 2 && n > steps[i].X )
						r = i + 1;
				}
				return this.steps[r < 0 ? 0 : r];
			}

			public Stint GetNextStep(string s) {
				if( type == 2 ) {
					if( steps.Length <= 0 )
						throw new NullReferenceException("Points are null! Set at least one!");
					step++;
					var np = getPointFromStep(step);
					pace = np.Y;
					return new Stint(s.Substring(0, step), pace);
				}
				if( type == 1 )
					return new Stint(s.Substring(0, ++step), pace);
				return new Stint(s, -1);
			}

		}

		public void ShowNext(object o) {
			var nextStep = type.GetNextStep(full);
			new Debug("ShowNext", $"{nextStep.ToString( )}", Debug.Importance.IMPORTANT_INFO);
			if( nextStep.i != walkerPeriod ) {
				walkerPeriod = nextStep.i;
				walker.Change(walkerPeriod, walkerPeriod);
			}
			visible = nextStep.s;
			if( nextStep.s == full ) {
				walker.Dispose( );
				walker = null;
				walker = new Timer((object ox) => { walker.Dispose( ); walker = null; if( endFunc != null ) this.endFunc( ); }, null, afterWait, 1);
			}
		}

		public Vector2 position { get; private set; }

		public void MoveTo(Vector2? p) => position = p ?? new Vector2(0);

		public Color color { get; private set; }

		public void Recolor(Color? c = null) => color = c ?? Color.White;

		/// <summary>
		/// This class is a class that 'types' a string as defined in <paramref name="dt"/>.
		/// </summary>
		/// <param name="s">A string to show</param>
		/// <param name="p">Position where to draw (left-top corner)</param>
		/// <param name="dt">A <code>DrawType</code> (how to type string)</param>
		/// <param name="c">Color of string</param>
		public StringDrawn(string s, Vector2? p = null, StringDrawn.DrawType dt = null, Color? c = null) {
			this.full = s;
			this.MoveTo(p);
			this.type = dt ?? new DrawType( );
			this.color = c ?? Color.White;
		}

		private Action endFunc;

		public void End(Action f) {
			this.endFunc = f;
		}

		public bool first = false;

		private int afterWait;

		public void Show(int bef = 0, int aft = 0) {
			first = true;
			afterWait = aft;
			this.walkerPeriod = type.GetFirstStep(full).i;
			if( walkerPeriod != -1 )
				this.walker = new Timer(ShowNext, null, bef, walkerPeriod);
			else
				this.walker = new Timer((object x) => {
					if( visible == full ) { walker.Dispose( ); walker = null; if( endFunc != null ) endFunc( ); return; }
					visible = full;
					walker.Change(0, aft);
				}, null, bef, 1);
		}

		public void Draw(SpriteBatch sb, SpriteFont font) {
			sb.DrawString(font, visible ?? " ", position, this.color);
		}

	}
}

/*

//INIT
// #1=0,10 #2=10,18 #3=18+
StringDrawn sd = new StringDrawn("This 10ms, this 20, this 100", new DrawType(new Point[3]{new Point(0,10),new Point(10,20), new Point(18,100)}))
sd.End(shownext);
//DRAW
if(!sd.first)
	sd.Show();
sd.Draw(sb);
 */
