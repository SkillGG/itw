using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using System.Linq;

namespace ITW.Gameplay {

	public class Sentence {

		private readonly Language lng;
		private readonly string s;

		public Sentence(Language lang, string str) {
			this.lng = lang;
		}

		public bool IsFrom(Language l) {
			if( l.ID == lng.ID ) {
				return true;
			}
			return false;
		}

	}

	public class Language {
		private int id;
		private string name;
		private List<Sentence> strings;

		public int ID { get => id; }
		public string NAME { get => name; }

		public int FromString(string s) {

			/*
			**
			<langIdent>[:<options>]<<langID>>=[
				<ident>[:<options>]=<value>;
			];
			<langIdent>: capital code of language
			<options>: <option>*
			<option>: <ident>=<value>
			<ident>: name of variable that is [a-zA-Z_][a-zA-Z0-9_+-?!@#$%^&*()]
			<value>: value of variable enclosed into '', `` or "".
					 enclosing characters should be escaped using preceding $.
					 $<<ident>> signifies use of external variable. To escape $ sign use $$ or $<$>
			**
			PL:name='$$'<1>=[
				PL="Polski"
				Yes="Tak";
				Why?="Dlaczego?";
			];
			DE<2>=[Yes="Ja";Why?="Warum?"];
			*/

			// ("|'|`)((?:\\\1|(?!\1).)*)(\1) - value (pcre)
			// \.(?:([A-Za-z]+):(?:("|'|`)((?:\2|(?!\2).)*)(?:\2))) - option (pcre)

			// ((?<enc>"|'|`)((?:\\\k<enc>|(?!\k<enc>).)*)(?:\k<enc>)) - value (.net)
			// \.(?:([A-Za-z]+):(?:(?<enc>"|'|`)((?:\k<enc>|(?!\k<enc>).)*)(?:\k<enc>))) - option (.net)

			// [A-Z]+ - langIdent
			// [0-9]+ - langID
			// [a-zA-Z_][a-zA-Z0-9]+ - ident


			// FULL
			// ([A-Z]+(?:\.(?:(?:[A-Za-z]+):(?:(?<e1>"|'|`)(?:(?:\\\k<e1>|(?!\k<e1>).)*)(?:\k<e1>))))+\<[0-9]+\>)=\[\s*((?:[a-zA-Z_][a-zA-Z0-9]+(?:\.(?:(?:[A-Za-z]+):(?:(?<e2>"|'|`)(?:(?:\\\k<e2>|(?!\k<e2>).)*)(?:\k<e2>))))+=(?:(?<e3>"|'|`)(?:(?:\\\k<e3>|(?!\k<e3>).)*)(?:\k<e3>));\s*)+)\];


			string pattern = @"\s*(?<bp>[A-Z]+(?:\.(?:(?:[A-Za-z]+):(?:(?<e1>""|'|`)(?:(?:\\\k<e1>|(?!\k<e1>).)*)(?:\k<e1>))))+\<[0-9]+\>)=\[\s*(?<ap>(?:[a-zA-Z_][a-zA-Z0-9]+(?:\.(?:(?:[A-Za-z]+):(?:(?<e2>""|'|`)(?:(?:\\\k<e2>|(?!\k<e2>).)*)(?:\k<e2>))))+=(?:(?<e3>""|'|`)(?:(?:\\\k<e3>|(?!\k<e3>).)*)(?:\k<e3>));\s*)+)\];\s*";

			int r = 0;

			foreach(Match m in Regex.Matches(s, pattern)){
				string befg = m.Groups["bp"].Value;
				string aftg = m.Groups["ap"].Value;
			}

			return r;
		}

		public Language(int ID, string NAME) {
			this.id = ID;
			this.name = NAME;
		}

		public void AddWord(Sentence s) {
			if( s.IsFrom(this) ) {
				this.strings.Add(s);
			}
		}

	}
}
