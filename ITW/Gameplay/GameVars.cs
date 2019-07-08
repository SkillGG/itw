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
			public Var(string s, string value) {
				Name = s;
				Value = value;
			}
		}

		/// <summary>
		/// Array of variables
		/// </summary>
		private List<Var> vars;

		/// <summary>
		/// Array of names of variables that are being debugged
		/// </summary>
		private List<string> debugged;

		/// <summary>
		/// Returns currently debugged values.
		/// </summary>
		/// <returns></returns>
		public string Debugged() {
			string s = "";
			foreach(string x in debugged){
				s += $@"{x}=""{this[x] ?? "null"}"";";
			}
			return s;
		}

		/// <summary>
		/// Toggles debug on given value
		/// </summary>
		/// <param name="td"></param>
		public void ToggleDebug(string td) {
			if( debugged.Contains(td) )
				debugged.Remove(td);
			else if( IsVar(td) )
				debugged.Add(td);
		}

		/// <summary>
		/// Getter and setter for variables
		/// </summary>
		/// <param name="s">name of variable</param>
		/// <returns></returns>
		public string this[string s] {
			get => vars.Find(e => e.Name == s).Value;
			set {
				if( vars.Find(e => e.Name == s).Name != null )
					vars[vars.FindIndex(e => e.Name == s)] = new Var(s, value);
				else
					vars.Add(new Var(s, value));
			}
		}

		/// <summary>
		/// Checks if Var with given name is set up
		/// </summary>
		/// <param name="name">Name of Variable to check</param>
		/// <returns></returns>
		public bool IsVar(string name) => vars.Find(e => e.Name == name).Name != null;


		/// <summary>
		/// Constructor for GameVars
		/// </summary>
		public GameVars() {
			vars = new List<Var> {
				new Var("endl", "\n\r"),
				new Var("$", "\\$")
			};
			debugged = new List<string> { };
		}

		public void LoadFromFile(string input) {
			// TODO: Parse error handling!
			/*
				[@]"<ident>":"<value>";
				/\
			"debug" this change
			*/

			/** Delete comments */
			string str = input;
			string varMatch = 
			@"(?<dbg>[@])?(?<e1>""|'|`)(?<ident>(?:.(?!\k<e1>))*.)\k<e1>:(?<e2>""|'|`)(?<value>(?:.(?!\k<e2>))*.)\k<e2>;";
			MatchCollection matches = Regex.Matches(str, varMatch);
			foreach( Match m in matches ) {
				string dbg = m.Groups["dbg"].Value ?? "-";
				string ident = m.Groups["ident"].Value ?? "$";
				string value = m.Groups["value"].Value ?? "undefined";
				if( dbg == "@" )
					new Debug($"LoadFromFile({str})", $@"Trying to change ""{ident}"" value(""{this[ident]??"null"}"") to ""{value}""", Debug.Importance.IMPORTANT_INFO);
				if( ident == "$" || string.IsNullOrWhiteSpace(str) ) {
					new Debug("GameVars#LoadFromFile(string)", $@"Variable identificator is illegal! {m.Groups["ident"]}", Debug.Importance.ERROR);
					return;
				}

				this[ident] = value;

			}
		}

	}
}