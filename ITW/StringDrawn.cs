using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Threading;
using System;

namespace ITW {
	public class StringDrawn {

		private string full;
		private string visible;
		private DrawType type;

		private Timer walker;

		public class DrawType {

			public class Stint {
				public string s;
				public int i;
				public Stint(string x, int z) { s = x, i = z; }
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

			public DrawType(int pace) {
				type = 1;
				step = pace;
			}

			public DrawType(Point[] pace) {
				type = 2;
				steps = pace;
			}

			public Stint GetNextStep(string s) {
				if( type == 2 ) {
				}
				if( type == 1 )
					return new Stint(s.Substring(0, ++step), pace);
				return new Stint(s, -1);
			}

		}

		public void ShowNext(object o) {

		}

		public Vector2 position { get; private set; }

		public void MoveTo(Vector2? p) => position = p ?? new Vector2(0);

		public Color color { get; private set; }

		public void Recolor(Color? c = null) => color = c ?? Color.White;

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
			this.walker = new Timer(ShowNext, null, bef, type.GetNextStep(full).i);
		}

		public void Draw(SpriteBatch sb, SpriteFont font) {
			sb.DrawString(font, visible, position, this.color);
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
