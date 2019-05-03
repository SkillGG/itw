using Microsoft.Xna.Framework.Graphics;
using ITW.Exts;

namespace ITW.Gameplay {
	/// <summary>
	/// Class that builds up whole game.
	/// </summary>
	public class MainGame {

		public VariablesHandler Game { get; private set; }



		public MainGame(VariablesHandler game){
			Game = game;
		}


		public void ChangeLanguage() {
			if( Game.Vars["langChoose"] != "true" )
				Game.Vars["langChoose"] = "true";
		}

		public void Update(InputStates bef){
			InputStates input = new InputStates( );

			if(Game.Vars["langChoose"] == "true"){
				// Check for LANGCHOOSE inputs
			}
			else{
				// Check Game.Vars["UpdateState"] to check what types of input to check
			}

		}

		public void Draw(SpriteBatch sb){

			if(Game.Vars["langChoose"] == "true"){
				// Draw LANGCHOOSE
			}
			else {
				// Check Game.Vars["DrawState"] to check what to draw
			}

		}

	}
}
