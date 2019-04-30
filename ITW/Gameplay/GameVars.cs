using System.Collections.Generic;
using System.Linq;

namespace ITW.Gameplay {

	/// <summary>
	/// Class to store all game variables
	/// </summary>
	public class GameVars {

		/// <summary>
		/// struct to store single variable
		/// </summary>
		public struct Var {

		/// <summary>
		/// Name of variable. Unchangable
		/// </summary>
			public string Name { get; private set; }
			/// <summary>
			/// Value of variable. Can be changed via <see cref="Change(string)"/>.
			/// </summary>
			public string Value { get; private set; }

			/// <summary>
			/// Constructor for Var
			/// </summary>
			/// <param name="s">name of var</param>
			/// <param name="value">default value of var</param>
			public Var(string s, string value) {
				this.Name = s;
				this.Value = value;
			}

			/// <summary>
			/// Setter for value;
			/// </summary>
			/// <param name="v"></param>
			public void Change(string v) {
				this.Value = v;
			}
		}

		/// <summary>
		/// Array of variables
		/// </summary>
		List<Var> vars;

		/// <summary>
		/// Getter and setter for variables
		/// </summary>
		/// <param name="s">name of variable</param>
		/// <returns></returns>
		public string this[string s] {
			get => vars.Find(e => e.Name == s).Value;
			set {
				if( vars.Find(e => e.Name == s).Name != null )
					vars.Find(e => e.Name == s).Change(value);
				else
					vars.Add(new Var(s, value));
			}
		}

		/// <summary>
		/// Constructor for GameVars
		/// </summary>
		public GameVars() {
			vars = new List<Var> {
				new Var("endl", "\n\r"),
				new Var("$", "\\$")
			};
		}
	}
}