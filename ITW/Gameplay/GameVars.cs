using System.Collections.Generic;
using System.Linq;

namespace ITW.Gameplay {

	public class GameVars {

		public struct Var {
			public string Name { get; private set; }
			public string Value { get; private set; }

			public Var(string s, string value) {
				this.Name = s;
				this.Value = value;
			}

			public void Change(string v) {
				this.Value = v;
			}
		}

		List<Var> vars;

		public string this[string s] {
			get => vars.Find(e=>e.Name == s).Value;
			set {
				if( vars.Find(e => e.Name == s).Name != null )
					vars.Where(e => e.Name == s).First( ).Change(value);
				else
					vars.Add(new Var(s, value));
			}
		}

		public GameVars() {
			vars = new List<Var> {
				new Var("endl", "\n\r"),
				new Var("$", "\\$")
			};
		}
	}
}