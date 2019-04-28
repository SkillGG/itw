using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using System.Linq;

namespace ITW.Gameplay {

	public class Language {

		public struct LangOpt{

			public string NAME { get; private set; }
			public string VALUE { get; private set; }

			public LangOpt(string n, string v){
				NAME = n;
				VALUE = v;
			}

			public void Change(string v){
				VALUE = v;
			}

		}

		private int id;
		private string name;
		private List<Sentence> strings;

		private List<LangOpt> options;

		public Sentence GetSentence(string n){
			return strings.Where(e => e.name == n).First( );
		}

		public int ID { get => id; }
		public string NAME { get => name; }

		public Language(int i, string n) {
			id = i;
			name = n;
			strings = new List<Sentence>( );
			options = new List<LangOpt>( );
		}

		public LangOpt GetOption(string n){
			LangOpt o = new LangOpt("null","null");
			foreach(LangOpt l in options){
				if(l.NAME == n && o.NAME == "null"){
					o = l;
				}
			}
			return o;
		}

		public void SetOption(string name, string value){
			if(GetOption(name).NAME == "null"){
				options.Add(new LangOpt(name, value));
			}else{
				options.Where((e) => e.NAME == name).First( ).Change(value);
			}
			RefreshOptions( );
		}

		private void RefreshOptions(){
			options = options.GroupBy(e => e.NAME).Select(g=>g.First()).ToList();
		}

	}

	public struct Sentence {

		public string name;
		private string value;

		public List<SentenceOption> options;

		public struct SentenceOption {
			public string NAME { get; private set; }
			public string VALUE { get; private set; }

			public SentenceOption(string n, string v) {
				NAME = n;
				VALUE = v;
			}

			public void Change(string v) {
				VALUE = v;
			}
		}

		public string GetValue(GameVars gv) {
			return "";
		}

		public Sentence(string name, string value){
			this.name = name;
			this.value = value;
			this.options = new List<SentenceOption>( );
		}

		public SentenceOption GetOption(string n) {
			SentenceOption o = new SentenceOption("null", "null");
			foreach( SentenceOption l in options ) {
				if( l.NAME == n && o.NAME == "null" ) {
					o = l;
				}
			}
			return o;
		}

		public void SetOption(string name, string value) {
			if( GetOption(name).NAME == "null" ) {
				options.Add(new SentenceOption(name, value));
			} else {
				options.Where((e) => e.NAME == name).First( ).Change(value);
			}
			RefreshOptions( );
		}

		private void RefreshOptions() {
			options = options.GroupBy(e => e.NAME).Select(g => g.First( )).ToList( );
		}

	}

	public class LanguageHandler {

		List<Language> languages;

		public LanguageHandler() {
			languages = new List<Language>( );
		}

		public Language GetLanguage(int id) {
			Language r = null;
			for( int i = 0; i < languages.Count; i++ )
				if( languages[i].ID == id )
					r = languages[i];
			return r;
		}

		public Language GetLanguage(string name) {
			Language r = null;
			for( int i = 0; i < languages.Count; i++ )
				if( languages[i].NAME == name )
					r = languages[i];
			return r;
		}

		public int AddFromString(string s) {
			string place = "LanguageHandler#AddFromString(string)";
			s = Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(s, @"\[(?!\n)", "[\n"), @"(?<!\n)];", "\n];"), @"(?:(?<!.)[\t ]*)", ""), @"[\n\r]", "");
			/*
			**
			<langIdent><options><<langID>>=[<br>
					(<ident><options>=<value>;<br>)*
			];<br>

			<br>: place, where you can put any white space
			<langID>: [0-9]+ id of language
			<langIdent>: capital code of language
			<options>: <option>*
			<option>: .<ident>=<value>
			<ident>: name of variable that is [a-zA-Z_][a-zA-Z0-9]*
						Occupied ident's:
							endl
			<value>: value of variable enclosed into '', `` or "".
					 \n\r should be escaped via $<endl>.
					 enclosing characters should be escaped using preceding $.
					 $<<ident>> signifies use of external variable. To escape $ sign use $$ or $<$>
			**
			
			PL:name='$$'<1>=[
				PL="Polski";
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

			/* 
				mainPattern:
				<bp>=[<ap>];

				<bp>:?<langIdent>(?<options>)<?<langID>>
				<ap>:(?<ident>(?<options>)*=?<value>;?<br>)*
			*/
			string mainPattern =
			@"\s*(?<bp>[A-Z]+(?:\.[a-zA-Z_][a-zA-Z0-9_]*[ \t]*:[ \t]*(?<e1>""|'|`)(?:\$\k<e1>|(?!\k<e1>).)*\k<e1>)*[ \t]*\<[0-9]+\>)[ \t]*=[ \t]*\[\s*(?<ap>(?:[a-zA-Z_][a-zA-Z0-9_]*(?:\.[a-zA-Z_][a-zA-Z0-9_]*[ \t]*:[ \t]*(?<e2>""|'|`)(?:\$\k<e2>|(?!\k<e2>).)*\k<e2>)*[ \t]*=[ \t]*(?<e3>""|'|`)(?:\$\k<e3>|(?!\k<e3>).)*\k<e3>[ \t]*;\s*)+)\];\s*";

			// If matches it means there is no semicolon after ] (lang ending)
			string mainPattern_NOLSC_Err =
			@".(?:(?<!]|;).)*\[[^\]]*](?!;)";

			// If matches it means there is no <langID>
			string mainPattern_NOID_Err =
			@"(?<b>.(?:(?<!;).)*)(?<!\d\>)=\[(?:(?!];).)*];";

			// If matches it means there is no = between <langID> and [
			string mainPattern_NOEQ_Err =
			@"(?<b>.(?:(?<!;).)*)(?<!>=)\[(?:(?!];).)*];";

			// If matches means there is quotation not closed properly.
			string mainPattern_MISQT_Err =
			@".(?:(?<!;|\[).)*(?<=:)(?<e1>""|'|`)(?:\$\k<e1>|(?!\k<e1>).)*\k<e1>(?!=|<)(?:(?!;).)*;";

			// If matches means there is semicolon missed.
			string mainPattern_MISSC_Err =
			@".(?:(?< !;|\[).)*(?<==)(?<e1>""|'|`)(?:\$\k<e1>|(?!\k<e1>).)*\k<e1>(?!;)";

			/*
				befPattern:
				<langIdent><options><<langID>>
			*/
			string befPattern = @"(?<langIdent>[A-Z]+)(?<options>(?:\.[A-Za-z][a-zA-Z0-9]*[ \t]*:[ \t]*(?<e1>""|'|`)(?:\$\k<e1>|(?!\k<e1>).)*(?:\k<e1>))*)[ \t]*\<(?<langID>[0-9]+)\>";

			/*
				aftPattern:
				<ident><options>=<value>;
			*/
			string aftPattern = @"(?<ident>[a-zA-Z_][a-zA-Z0-9_]*)(?<options>(?:\.[a-zA-Z_][a-zA-Z0-9_]*[ \t]*:[ \t]*(?<e2>""|'|`)(?:\$\k<e2>|(?!\k<e2>).)*\k<e2>)*)[ \t]*=[ \t]*(?<e3>""|'|`)(?<value>(?:\$\k<e3>|(?!\k<e3>).)*)\k<e3>[ \t]*;\s*";

			/*
			 optionPattern:
			 .<ident>:<value>
			*/
			string optionPattern = @"\.(?<ident>[A-Za-z][a-zA-Z0-9]*)[ \t]*:[ \t]*(?<e1>""|'|`)(?<value>(?:\$\k<e1>|(?!\k<e1>).)*)(?:\k<e1>)";
			

			int errno = 0;

			if( !Regex.IsMatch(s, mainPattern) ) {

				if( Regex.IsMatch(s, mainPattern_NOLSC_Err) ) {
					errno = 1;
					var all_unended_languages = Regex.Matches(s, mainPattern_NOLSC_Err);
					foreach( Match m in all_unended_languages ) {
						int forth = m.Value.Length / 4;
						forth = forth > 10 ? 10 : forth;
						string befp = s.Substring(m.Index, forth);
						string aftp = s.Substring(m.Index + ( m.Value.Length - forth ), forth);
						new Debug(place, $"Expected ';' after ']' in: {befp}...{aftp}", Debug.Importance.ERROR);
					}
				} else if( Regex.IsMatch(s, mainPattern_NOID_Err) ) {
					errno = 2;
					var all_noid_languages = Regex.Matches(s, mainPattern_NOID_Err);
					foreach( Match m in all_noid_languages ) {
						string befp = s.Substring(m.Index, m.Groups["b"].Value.Length + 10);
						new Debug(place, $"Expected <$langID$> before '=[' in: {befp}...", Debug.Importance.ERROR);
					}
				} else if( Regex.IsMatch(s, mainPattern_NOEQ_Err) ) {
					errno = 3;
					var all_noeq_languages = Regex.Matches(s, mainPattern_NOID_Err);
					foreach( Match m in all_noeq_languages ) {
						string befp = s.Substring(m.Index, m.Groups["b"].Value.Length + 10);
						new Debug(place, $"Expected = before '[' in: {befp}...", Debug.Importance.ERROR);
					}
				} else if( Regex.IsMatch(s, mainPattern_MISSC_Err) ) {
					errno = 4;
					var all_missc = Regex.Matches(s, mainPattern_MISSC_Err);
					foreach( Match m in all_missc ) {
						new Debug(place, $"This expression should end with a semicolon: {m.Value}", Debug.Importance.ERROR);
					}
				} else if( Regex.IsMatch(s, mainPattern_MISQT_Err) ) {
					errno = 5;
					var all_misqt = Regex.Matches(s, mainPattern_MISQT_Err);
					foreach( Match m in all_misqt ) {
						new Debug(place, $"String has been not properly closed in: {m.Value}", Debug.Importance.ERROR);
					}
				} else {
					new Debug(place, $"Unexpected error in {s}!", Debug.Importance.ERROR);
				}

			}

			foreach( Match m in Regex.Matches(s, mainPattern) ) {
				string befg = m.Groups["bp"].Value;
				string aftg = Regex.Replace(m.Groups["ap"].Value, @"[\n\r]", "");

				new Debug(place, $"Bef: {befg}, Afg: {aftg}", Debug.Importance.IMPORTANT_INFO);

				Match bm = Regex.Match(befg, befPattern);
				string langIdent = bm.Groups["langIdent"].Value;
				string options = bm.Groups["options"].Value;
				string langID = bm.Groups["langID"].Value;

				new Debug(place, $"ID: {langID}, Ident: {langIdent}, opts: {options}", Debug.Importance.IMPORTANT_INFO);

				Language newLang = new Language(int.Parse(langID), langIdent);

				foreach(Match lo in Regex.Matches(options, optionPattern)){
					newLang.SetOption(lo.Groups["ident"].Value, lo.Groups["value"].Value);
				}

				foreach(Match var in Regex.Matches(aftg, aftPattern)){
					string ident = var.Groups["ident"].Value;
					string opts = var.Groups["options"].Value;
					string value = var.Groups["value"].Value;

					Sentence s = new Sentence(ident, value);

					foreach(Match vo in Regex.Matches(opts, optionPattern)){

						s.SetOption(vo.Groups["ident"].Value, vo.Groups["value"].Value);

					}

				}

			}

			return errno;
		}

	}
}
