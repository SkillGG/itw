using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ITW.Gameplay.MainMenu;

namespace ITW.Gameplay {
	
	public class Menu {

		public LanguageScreen LangScreen { get; private set; }

		public Menu(ITW game){
			LangScreen = new LanguageScreen( );
			LangScreen.Picked((Language lang)=> { /*game.SelectLanguage(lang); Proceed( );*/return; });
		}

		public void Draw(SpriteBatch cb, SpriteFont[] fonts){
			
		}
	}
}
