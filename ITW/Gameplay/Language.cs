using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using System.Linq;

namespace ITW.Gameplay {

	/// <summary>
	/// Main Language Class. It stores sentences for given language. 
	/// </summary>
	public class Language {

		/// <summary>
		/// Options for Language class
		/// </summary>
		public struct LangOpt {

			/// <summary>
			/// Returns representation of this LangOpt as string formatted: <code>name:value</code>
			/// </summary>
			/// <returns></returns>
			public override string ToString() => $@"{NAME}:""{VALUE}""";

			/// <summary>
			/// Name(ID) of the option. Cannot be changed after initialization.
			/// </summary>
			public string NAME { get; private set; }

			/// <summary>
			/// Option Value. This is variable, that the option is set to. It can be changed via <see cref="Change(string)"/>
			/// </summary>
			public string VALUE { get; private set; }

			/// <summary>
			/// Constructor for the LangOpt
			/// </summary>
			/// <param name="n">Name of options, that it will be get by.</param>
			/// <param name="v">Value of option</param>
			public LangOpt(string n, string v) {
				NAME = n;       // set name to given
				VALUE = null;   // set default for value
				Change(v);      // change value to given
			}

			/// <summary>
			/// Change value of option
			/// </summary>
			/// <param name="v">Value to change to</param>
			public void Change(string v) {
				if( NAME != null )  // if option initialized properly
					VALUE = v;      // change its value
			}

		}

		/// <summary>
		/// Language's ID
		/// </summary>
		private readonly int id;
		/// <summary>
		/// Language's name
		/// </summary>
		private readonly string name;
		/// <summary>
		/// List of Sentences, this Language is supporting
		/// </summary>
		private List<Sentence> strings;

		/// <summary>
		/// Language's options.
		/// </summary>
		private List<LangOpt> options;

		/// <summary>
		/// Check if option is present in this language.
		/// </summary>
		/// <param name="n">name of option</param>
		/// <returns></returns>
		public bool IsOption(string n) => this[n, true].NAME != null;
		/// <summary>
		/// Check if sentence is present in this language.
		/// </summary>
		/// <param name="n">name of option</param>
		/// <returns></returns>
		public bool IsSentence(string n) => this[n].Plain( ) != null;

		/// <summary>
		/// Getter and setter for Language's options. To distinquish from <see cref="this[string]"/> you need to use second argument.
		/// <para>
		///	VALUE OF <see langword="bool"/> DOES NOT MATTER
		/// </para>
		/// You can use <see cref="this[string, int]"/> for same functionality.
		/// </summary>
		/// <param name="n">name of option</param>
		/// <param name="f">separator from <see cref="this[string]"/></param>
		/// <seealso cref="this[string, int]"/>
		/// <returns></returns>
		public LangOpt this[string n, bool f] {
			get => options.Find(e => e.NAME == n); // get option where NAME equals n
			set {
				if( IsOption(n) )                           // If option already initialized
					options[options.FindIndex(e => e.NAME == n)] = new LangOpt(n, value.VALUE);
				else
					options.Add(new LangOpt(n, value.VALUE));   // else add new one
			}
		}

		/// <summary>
		/// Getter and setter for Language's options. To distinquish from <see cref="this[string]"/> you need to use second argument.
		/// <para>
		///	VALUE OF <see langword="int"/> DOES NOT MATTER
		/// </para>
		/// You can use <see cref="this[string, bool]"/> for same functionality.
		/// </summary>
		/// <param name="n">name of option</param>
		/// <param name="f">separator from <see cref="this[string]"/></param>
		/// <seealso cref="this[string, bool]"/>
		/// <returns></returns>
		public LangOpt this[string n, int f] {
			get => this[n, true];
			set => this[n, true] = value;
		}

		/// <summary>
		/// Getter and Setter for Language's Sentences<para></para>
		/// If there is no such Sentence, returned one has specified name, but null value
		/// </summary>
		/// <param name="n">name of an Sentence to set/get</param>
		/// <returns></returns>
		public Sentence this[string n] {
			get => strings.Find(e => e.name == n).name == null ? new Sentence(n, null, null) : strings.Find(e => e.name == n);  // get sentence where name equals n
			set {
				if( IsSentence(n) ) {             // if sentence already in Language
					new Debug("Language[string]", $"Sentence {n} already exists! ('{this[n].name}', '{this[n].Plain( )}')", Debug.Importance.ERROR);
					return;                     // ABORT
				} else
					strings.Add(new Sentence(n, value.Plain( ), this)); // else add new Sentence
			}
		}

		/// <summary>
		/// Language's ID public property
		/// </summary>
		public int ID { get => id; }
		/// <summary>
		/// Language's NAME public property
		/// </summary>
		public string NAME { get => name; }

		/// <summary>
		/// Constructor for new Language
		/// </summary>
		/// <param name="i">ID of Language</param>
		/// <param name="n">NAME of Language</param>
		public Language(int i, string n) {
			// Init variables
			id = i;
			name = n;
			strings = new List<Sentence>( );
			options = new List<LangOpt>( );
		}

		/// <summary>
		/// Deletes every found option duplicates
		/// </summary>
		private void RemoveOptionsDuplicates() {
			options = options.GroupBy(e => e.NAME).Select(g => g.First( )).ToList( );
		}

	}

	/// <summary>
	/// Single Sentence for Language
	/// </summary>
	public struct Sentence {

		public override string ToString() => GetValue(null);

		/// <summary>
		/// Language from this sentence comes. Allows for backreferencing to its other fields.
		/// </summary>
		private readonly Language l;

		/// <summary>
		/// Name of Sentence it should stay unchanged
		/// </summary>
		public readonly string name;
		/// <summary>
		/// Value of Sentence it cannot be changed
		/// </summary>
		private readonly string value;

		/// <summary>
		/// Returns value as a String without 
		/// </summary>
		/// <returns></returns>
		public string Plain() => value;

		/// <summary>
		/// List of Sentence specific Options
		/// </summary>
		public List<SentenceOption> options;

		/// <summary>
		/// Option Struct for Sentence
		/// </summary>
		public struct SentenceOption {
			/// <summary>
			/// NAME of Option. NAME cannot be changed
			/// </summary>
			public string NAME { get; private set; }
			/// <summary>
			/// VALUE of Option. This can be changed using <see cref="Change(string)"/>.
			/// </summary>
			public string VALUE { get; private set; }

			/// <summary>
			/// SentenceOption Constructor
			/// </summary>
			/// <param name="n">NAME of option</param>
			/// <param name="v">Initial VALUE of option</param>
			public SentenceOption(string n, string v) {
				NAME = n;
				VALUE = v;
			}

			/// <summary>
			/// Checks if options has been initialized properely
			/// </summary>
			/// <returns></returns>
			public bool IsInitialized() {
				if( String.IsNullOrWhiteSpace(NAME) )
					return false;
				return true;
			}

			/// <summary>
			/// Checks if option has been initialized just for <see cref="Sentence.this[string]"/> = new <see cref="SentenceOption"/>(null, "value");
			/// </summary>
			/// <returns></returns>
			public bool IsFastInitialized() {
				if( !String.IsNullOrWhiteSpace(VALUE) && String.IsNullOrWhiteSpace(NAME) )
					return true;
				return IsInitialized( );
			}

			/// <summary>
			/// Changes VALUE of this option
			/// </summary>
			/// <param name="v">new VALUE</param>
			public void Change(string v) {
				VALUE = v;
			}
		}

		/// <summary>
		/// Returns value with every escaped character and variable. If you do not want to replace those use <see cref="Plain()"/>.
		/// </summary>
		/// <param name="gv">Object storing game variables used inside those strings</param>
		/// <returns>string that has parsed value of Sentence</returns>
		public string GetValue(GameVars gv) {
			string place = "Sentence#GetValue(GameVars)";   // set place for errors
			if( String.IsNullOrWhiteSpace(value) ) {    // check if Sentence is assigned
				new Debug(place, $"You try to read from unassigned Sentence '{name}'! Returned null!", Debug.Importance.ERROR);
				return null;
			}
			// reassign variable to not change the original
			string r = value;
			if( gv == null )    // if GameVars not present
				gv = new GameVars( );   // create new one for escaping $$, $<$> and $<endl>
			r = Regex.Replace(r, @".(?<=\$)\$", "$<$>");    // change each $$ to $<$>
			r = Regex.Replace(r, @".(?<=\$)(?<s>""|'|`)", "${s}");  // escape each $", $', $`
			foreach( Match m in Regex.Matches(r, @"(?<whole>\$<(?<var>.*?)>)") ) {  // foreach $<var>
				string vr = ""; // new value
				if( m.Groups["var"].Value == "$" ) {    // if it is $<$>
					r = r.Substring(0, m.Index) + "$" + r.Substring(m.Index + 4);   // replace it properely
					continue;   // go to next one
				}
				if( m.Groups["var"].Value.ElementAt(0) == '.' ) {   // if its backreferenece to same language
					vr = l[m.Groups["var"].Value.Substring(1)].GetValue(gv) ?? "";  // get value from language as changing
				} else {    // if its reference to GameVars
					vr = gv[m.Groups["var"].Value] ?? "";   // get value from GameVars as changing
				}
				// If value returned from language/GameVars is not empty or null ( is present )
				if( !String.IsNullOrWhiteSpace(vr) )
					r = Regex.Replace(r, "\\" + m.Groups["whole"].Value, vr);   // replace $<var> with value of var
				else {
					if( m.Groups["var"].Value.ElementAt(0) == '.' ) // if its backreferenece to same language
						new Debug(place, $"Language({l.NAME}:{l.ID}) does not have field named '{m.Groups["var"].Value.Substring(1)}'", Debug.Importance.ERROR);
					else // if its reference to GameVars
						new Debug(place, $"Given GameVars does not have field named '{m.Groups["var"].Value}'", Debug.Importance.ERROR);
				}
			}
			// return escaped string
			return r ?? null;
		}

		/// <summary>
		/// Constructor of Sentence
		/// </summary>
		/// <param name="name">Name of sentence</param>
		/// <param name="value">value of Sentence</param>
		/// <param name="l">Language to backreference using $&lt;.var></param>
		public Sentence(string name, string value, Language l) {
			this.name = name;
			this.value = value;
			this.l = l;
			this.options = new List<SentenceOption>( );
		}

		/// <summary>
		/// Getter and Setter for SentenceOptions.
		/// </summary>
		/// <param name="n">name of SentenceOption</param>
		/// <returns></returns>
		public SentenceOption this[string n] {
			get => options.Find(e => e.NAME == n); // Returns first occurence 
			set {
				if( this[n].NAME != null )          // if option exists
					options[options.FindIndex(e => e.NAME == n)] = new SentenceOption(n, value.VALUE);  // change its value
				else
					options.Add(value);             // add new option
				RemoveNullOptions( );                       // remove null options
			}
		}

		/// <summary>
		/// Removes all not-initialized variables
		/// </summary>
		private void RemoveNullOptions() => options.RemoveAll(e => !e.IsInitialized( ));
	}

	/// <summary>
	/// Class to handle multiple languages
	/// </summary>
	public class LanguageHandler {

		/// <summary>
		/// List of all languages
		/// </summary>
		public List<Language> All { get; private set; }


		/// <summary>
		/// Constructor
		/// </summary>
		public LanguageHandler() {
			All = new List<Language>( );
			CurrentLang = -1;
		}

		/// <summary>
		/// int storing value of currently chosen language
		/// </summary>
		private int CurrentLang;

		/// <summary>
		/// Changes <see cref="CurrentLang"/> value to given, if Language with given ID exists.
		/// </summary>
		/// <param name="ID">ID of chosen Language</param>
		public void ChangeCurrentLang(int ID) => CurrentLang = IsLanguage(ID) ? ID : CurrentLang;

		/// <summary>
		/// Current Language
		/// </summary>
		public Language Current { get => this[CurrentLang]; }

		/// <summary>
		/// Checking if Language with given ID exists
		/// </summary>
		/// <param name="ID">ID of Language to check</param>
		/// <returns>true if language exists, false if not</returns>
		public bool IsLanguage(int ID) => !( this[ID].NAME == null );

		/// <summary>
		/// Getter for single <see cref="Language"/> via its ID
		/// </summary>
		/// <param name="id">ID of searched <see cref="Language"/></param>
		/// <returns></returns>
		public Language this[int id] {
			get => All.Find(e => e.ID == id);
		}

		/// <summary>
		/// Getter for list of <see cref="Language"/>s via short NAME(&lt;langIdent>)<para>NOTE: Languages can have identical names, ID's cannot be duplicated!</para>
		/// </summary>
		/// <param name="name">NAME of searched <see cref="Language"/>s (&lt;langIdent>)</param>
		/// <returns></returns>
		public List<Language> this[string name] {
			get => All.FindAll(e => e.NAME == name).ToList( );
		}

		/// <summary>
		/// Adds new Language to list if not already present!
		/// </summary>
		/// <param name="l"></param>
		private void AddLanguage(Language l) {
			if( All.Find(e => e.ID == l.ID) == null ) {
				All.Add(l);
				if( CurrentLang == -1 )
					ChangeCurrentLang(l.ID);
			} else
				new Debug("LanguageHandler#AddLanguage", $"Language with ID:{l.ID} has already been added! {All.Find(e => e.ID == l.ID).NAME}", Debug.Importance.ERROR);
		}

		/// <summary>
		/// Language parser from string. Returns error number. Where 0 means succession.
		/// </summary>
		/// <param name="s">string to parse</param>
		/// <param name="errorHandler">Action taking error message as a parameter</param>
		/// <returns></returns>
		public int AddFromString(string s, Action<string> errorHandler) {
			// Delete every not needed \n\r\t
			s = Regex.Replace(
					Regex.Replace(s, @"[\n\r]", ""),                            // Delete each \n\r
				@"((?<e1>""|'|`)(?:\$\k<e1>|(?!\k<e1>).)*\k<e1>)|[\t ]+", "$1");    // Delete each \t outside of quotes

			/*
			**
			<langIdent><options><<langID>>=[(<ident><options>=<value>;)*];
			
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
			@"(?:(?!;|\[).)*(?<==)(?<e1>""|'|`)(?:\$\k<e1>|(?!\k<e1>).)*\k<e1>(?!;)";

			//TODO: Add those errors:
			/*
			 -	ID not number
			 -	Illegal langIdent
			 -	Illegal option ident
			 -	Illegal var ident
			*/

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

			int errno = 0; // error number

			if( !Regex.IsMatch(s, mainPattern) ) {  // error occured!

				if( Regex.IsMatch(s, mainPattern_NOLSC_Err) ) { // is this NOLSC(No Last Semicolon) error?
					errno = 1;
					var all_unended_languages = Regex.Matches(s, mainPattern_NOLSC_Err);    // Raise error for each wrongly ended language
					foreach( Match m in all_unended_languages ) {   // foreach language
																	//	Write error
						int forth = m.Value.Length / 4;
						forth = forth > 10 ? 10 : forth;
						string befp = s.Substring(m.Index, forth);
						string aftp = s.Substring(m.Index + ( m.Value.Length - forth ), forth);
						errorHandler($"Expected ';' after ']' in: {befp}...{aftp}");
					}
				} else if( Regex.IsMatch(s, mainPattern_NOID_Err) ) {   // is this NOID(No Lang ID) error?
					errno = 2;
					var all_noid_languages = Regex.Matches(s, mainPattern_NOID_Err);
					foreach( Match m in all_noid_languages ) {
						string befp = s.Substring(m.Index, m.Groups["b"].Value.Length + 10);
						errorHandler($"Expected <$langID$> before '=[' in: {befp}...");
					}
				} else if( Regex.IsMatch(s, mainPattern_NOEQ_Err) ) {   // is this NOEQ(No Lang '=' sign before []) error?
					errno = 3;
					var all_noeq_languages = Regex.Matches(s, mainPattern_NOID_Err);
					foreach( Match m in all_noeq_languages ) {
						string befp = s.Substring(m.Index, m.Groups["b"].Value.Length + 10);
						errorHandler($"Expected = before '[' in: {befp}...");
					}
				} else if( Regex.IsMatch(s, mainPattern_MISSC_Err) ) {  // is this MISSC(Missing Semicolon) error?
					errno = 4;
					var all_missc = Regex.Matches(s, mainPattern_MISSC_Err);
					foreach( Match m in all_missc ) {
						errorHandler($"This expression should end with a semicolon: {m.Value}");
					}
				} else if( Regex.IsMatch(s, mainPattern_MISQT_Err) ) {  // is this MISQT(Unproperely closed quote) error?
					errno = 5;
					var all_misqt = Regex.Matches(s, mainPattern_MISQT_Err);
					foreach( Match m in all_misqt ) {
						errorHandler($"String has been not properly closed in: {m.Value}");
					}
				} else {    // unexpected error
					errorHandler($"Unexpected error in {s}!");
				}

			}

			foreach( Match m in Regex.Matches(s, mainPattern) ) {   // Get all matches
				string befg = m.Groups["bp"].Value; // before []
				string aftg = Regex.Replace(m.Groups["ap"].Value, @"[\n\r]", ""); // after []

				Match bm = Regex.Match(befg, befPattern);   // get langIdent, options and langID
															// set them respectively
				string langIdent = bm.Groups["langIdent"].Value;
				string options = bm.Groups["options"].Value;
				string langID = bm.Groups["langID"].Value;

				// Create new Lang object
				Language newLang = new Language(int.Parse(langID), langIdent);

				// Set all its options.
				foreach( Match lo in Regex.Matches(options, optionPattern) ) {
					newLang[lo.Groups["ident"].Value, 0] = new Language.LangOpt(lo.Groups["ident"].Value, lo.Groups["value"].Value);
				}

				// Match every sentence from language 'body'
				foreach( Match var in Regex.Matches(aftg, aftPattern) ) {
					string ident = var.Groups["ident"].Value;
					string opts = var.Groups["options"].Value;
					string value = var.Groups["value"].Value;

					// Create new Sentence
					Sentence newSentece = new Sentence(ident, value, newLang);

					// Set all its options
					foreach( Match vo in Regex.Matches(opts, optionPattern) ) {
						newSentece[vo.Groups["ident"].Value] = new Sentence.SentenceOption(null, vo.Groups["value"].Value);
					}

					// Add it to newLang
					newLang[newSentece.name] = newSentece;
				}

				if( CurrentLang == -1 )         // first lang added
					CurrentLang = newLang.ID;   // Set it as default

				// Add newLang to Languages
				AddLanguage(newLang);
			}
			return errno;
		}

		/// <summary>
		/// <see cref="AddFromString(string, Action{string})"/> using <see cref="Debug"/>.
		/// </summary>
		/// <param name="s">string to parse</param>
		/// <returns></returns>
		public int AddFromString(string s) => AddFromString(s, e => new Debug("AddFromString(string)", e, Debug.Importance.ERROR));

	}
}
