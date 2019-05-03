using System.Collections.Generic;
using System.Text.RegularExpressions;

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
			public string Value { get; set; }

			/// <summary>
			/// Constructor for Var
			/// </summary>
			/// <param name="s">name of var</param>
			/// <param name="value">default value of var</param>
			public Var(string s, string value):this() {
				this.Name = s;
				this.Value = value;
			}

			/// <summary>
			/// Setter for value;
			/// </summary>
			/// <param name="v"></param>
			public void Change(string v) {
				new Debug("b", $@"Changing value from ""{Value}"" to ""{v}""", Debug.Importance.ERROR);
				this.Value = v;
				new Debug("b", $@"Result: ""{Value}""", Debug.Importance.ERROR);
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
				new Debug("a", $@"Attempt to change value of ""{s}"" to ""{value}"".", Debug.Importance.ERROR);
				if( vars.Find(e => e.Name == s).Name != null )
					vars.Find(e => e.Name == s).Change(value);
				else
					vars.Add(new Var(s, value));

				if( this[s] != value )
					throw new System.Exception("Mot changed!");
				new Debug("a", $@"Result: ""{this[s]}""", Debug.Importance.ERROR);
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

		public void LoadFromFile(string str) {
			// TODO: Parse Config String
			/*
				"<ident>":"<value>";
			*/
			string varMatch = @"(?<e1>""|'|`)(?<ident>(?:.(?!\k<e1>))*.)\k<e1>:(?<e2>""|'|`)(?<value>(?:.(?!\k<e2>))*.)\k<e2>;";
			MatchCollection matches = Regex.Matches(str, varMatch);
			foreach( Match m in matches ) {
				string ident = m.Groups["ident"].Value ?? "$";
				string value = m.Groups["value"].Value ?? "undefined";
				if( ident == "$" || string.IsNullOrWhiteSpace(str) ) {
					new Debug("GameVars#LoadFromFile(string)", $@"Variable identificator is illegal! {m.Groups["ident"]}", Debug.Importance.ERROR);
					return;
				}
				new Debug("LoadFromFile", $@"Changed ""{ident}""({this[ident] ?? "null"}, {this.vars.Find(e=>e.Name==ident).Name??"null"}) to: ""{value}"".", Debug.Importance.IMPORTANT_INFO);
				this[ident] = value;

				new Debug("", $"When do i Fire? {this[ident]}", Debug.Importance.ERROR);

				if( this[ident] != value )
					throw new System.Exception("Could not change value!");

			}
		}

	}
}