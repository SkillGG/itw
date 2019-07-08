using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ITW.Exts;
using System;

namespace ITW.Gameplay {
	/// <summary>
	/// Class that builds up whole game.
	/// </summary>
	public class MainGame {

		const string CL_Font = "Fira20";

		public VariablesHandler Game { get; private set; }

		private List<StringDrawn> languagesDrawn = new List<StringDrawn>( );
		private List<int> languagesIDs = new List<int>( );

		public MainGame(VariablesHandler game) {
			Game = game;
		}

		public void Start() {

			// Setting up important values
			if( !Game.Vars.IsVar("LANG") )
				Game.Vars["LANG"] = $"{Game.Language.ID}";
			else if( !Game.Vars.IsVar("cl_skip") || Game.Vars?["cl_skip"] == "0" ) {
				Game.Vars["langChoose"] = "false";
				Game.Vars["UpdateState"] = null;
				Game.Vars["DrawState"] = null;
			}

			// Setting up debuging
			Game.Vars.ToggleDebug("UpdateState");
			Game.Vars.ToggleDebug("DrawState");

			ChangeLanguage(() => { new Debug("", $"Language chosen to {Game.Language["full", 0].VALUE}", Debug.Importance.IMPORTANT_INFO); });
		}

		/// <summary>
		/// Change language ending action
		/// </summary>
		private Action CL_Action;

		/// <summary>
		/// "Choose language" title
		/// </summary>
		private StringDrawn cl_title;

		public void ChangeLanguage(Action after) {
			// If not changing aready
			if( Game.Vars["langChoose"] != "true" ) {
				// Debug variables
				Game.Vars.ToggleDebug("LANG");
				Game.Vars.ToggleDebug("langChoose");

				// Choose Language title
				cl_title = new StringDrawn(
					s: Game.Langs.Current["cl_title"].GetValue(Game.Vars),   // Choose language in given language
					dt: new StringDrawn.DrawType( ),
					p: new Vector2(0, 0), c: Color.White                 // Color and position
				);

				// Set flag
				Game.Vars["langChoose"] = "true";

				// init languages
				languagesDrawn = new List<StringDrawn>( );
				languagesIDs = new List<int>( );

				int x = 50; // Horizontal position
				int y = 75;  // Vertical position
				int space = 25; // vertica space

				foreach( Language l in Game.Langs.All ) {
					string rep = "";    // representation as string
					if( l.IsOption("full") ) {    // If there is 'full' lang option
						rep =
						!string.IsNullOrWhiteSpace(l["full", 0].VALUE)   // If is set properely
						? l["full", 0].VALUE            // use it
						: Game.Language[l.NAME + ".lang"].GetValue(null) ?? l.NAME; // otherwise get normally 
					} else {  // Get normally

						// Get from defualt language value <langIdent>.lang or use just <langIdent>
						rep = Game.Language[l.NAME + ".lang"].GetValue(null) ?? l.NAME;
					}

					// Add language
					languagesDrawn.Add(new StringDrawn(rep, p: new Vector2(x, y), c: Game.Langs.Current.ID == l.ID ? Color.Gray : Color.White));
					languagesIDs.Add(l.ID);

					// move to next
					y += (int) Game.Fonts[CL_Font].FONT.MeasureString(rep).Y + space;
				}

				// Show each string
				foreach( StringDrawn s in languagesDrawn )
					s.Show( );
				cl_title.Show( );

				// set ending action
				CL_Action = after;
			}
		}

		public void Update(InputStates bef) {
			// Input Variables to not reinitialize
			InputStates input = new InputStates( );
			MouseClick mr = input.MouseReleased(bef);
			MouseClick mp = input.MousePressed(bef);

			if( Game.Vars["langChoose"] == "true" ) {
				// Check for LANGCHOOSE inputs
				if( input.KeyUp(bef, Keys.Enter) ) {
					// Clicked confirm

					Game.Vars["LANG"] = "1";
					Game.Vars["langChoose"] = "false";
					Game.Vars.ToggleDebug("langChoose");
					Game.RefreshLanguage( );
					CL_Action?.Invoke( );
				}
				if( mr.Button == MouseButton.LEFT ) {
					int z = 0;
					foreach( StringDrawn s in languagesDrawn ) {
						if( input.MouseRectangle.Intersects(s.Border ?? new Rectangle(0, 0, 0, 0)) ) {
							Game.Vars["LANG"] = $"{languagesIDs[z]}";
							Game.Vars["langChoose"] = "false";
							Game.Vars.ToggleDebug("langChoose");
							Game.RefreshLanguage( );
							CL_Action?.Invoke( );
						}
						z++;
					}
				}

			} else {
				// Check Game.Vars["UpdateState"] to check what types of input to check
			}

		}

		public void Draw(SpriteBatch sb) {
			if( Game.Vars["langChoose"] == "true" ) {
				cl_title.Draw(sb, Game.Fonts[CL_Font].FONT);
				foreach( StringDrawn s in languagesDrawn )
					s.Draw(sb, Game.Fonts[CL_Font].FONT);
			} else {
				// Check Game.Vars["DrawState"] to check what to draw
			}

		}

	}
}
