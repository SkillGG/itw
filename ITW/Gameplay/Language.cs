using System.Collections.Generic;
using System.Text.RegularExpressions;

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

		public void FromString(string s){

			/*
			.txt:
			Yes["EN","PL",2]:[Yes][Tak][Ja];
			Only_this["PL","EN"]:[Tylko to][Only this];
			*/
			
			MatchCollection matches = // $1 - name, $2 - languages, $3 - strings
			new Regex(@"(?:([a-zA-Z][a-zA-Z0-9]*)\[((?:(?:(?:[A-Z]+)|(?:[0-9]+))[,]?)*)\]:((?:\[[^\]]+\])+))+")
			.Matches(s);
			foreach( Match m in matches ){
				string name = m.Groups[0].Value;
				string[] languages = m.Groups[1].Value.Split(',');
				string[] values = m.Groups[2].Value.Split(',');
			}

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
