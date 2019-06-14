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

		public VariablesHandler Game { get; private set; }

		private List<StringDrawn> languagesDrawn = new List<StringDrawn>( );
		private List<int> languagesIDs = new List<int>( );

		public MainGame(VariablesHandler game) {
			Game = game;
		}

		public void Start() {
			// Setting up important values
			Game.Vars["LANG"] = $"{Game.Language.ID}";
			Game.Vars["langChoose"] = "false";
			Game.Vars["UpdateState"] = null;
			Game.Vars["DrawState"] = null;

			// Setting up debuging
			Game.Vars.ToggleDebug("UpdateState");
			Game.Vars.ToggleDebug("DrawState");


			ChangeLanguage(() => { new Debug("",$"Language chosen to {Game.Language["full", 0].VALUE}", Debug.Importance.IMPORTANT_INFO); });
		}

		/// <summary>
		/// Change language ending action
		/// </summary>
		private Action CL_Action;

		public void ChangeLanguage(Action after) {
			if( Game.Vars["langChoose"] != "true" ) {
				Game.Vars.ToggleDebug("LANG");
				Game.Vars.ToggleDebug("langChoose");
				Game.Vars["langChoose"] = "true";

				// init languages
				languagesDrawn = new List<StringDrawn>( );
				languagesIDs = new List<int>( );

				int x = 50; // Horizontal position
				int y = 50;  // Vertical position

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
					//new Debug("", $"New Lang to draw: {rep}", Debug.Importance.ERROR);
					languagesDrawn.Add(new StringDrawn(rep, p: new Vector2(x, y), c: Color.White));
					languagesIDs.Add(l.ID);
					y += (int) Game.Fonts["Fira10"].FONT.MeasureString(rep).Y + 50;
				}
				foreach( StringDrawn s in languagesDrawn )
					s.Show( );
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
				foreach( StringDrawn s in languagesDrawn )
					s.Draw(sb, Game.Fonts["Fira10"].FONT);
			} else {
				// Check Game.Vars["DrawState"] to check what to draw
			}

		}

	}
}
