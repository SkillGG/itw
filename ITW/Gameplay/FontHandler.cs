using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace ITW.Gameplay {
	public class FontHandler {

		public class Font {
			public string NAME { get; private set; }
			public SpriteFont FONT { get; private set; }
			public Font(SpriteFont sf, string name) {
				NAME = name;
				if( sf != null )
					FONT = sf;
			}
			~Font(){
				FONT = null;
			}
		}

		List<FontHandler.Font> fonts;

		public FontHandler() {
			fonts = new List<Font>( );
		}

		private void AddNew(Font f){
			if( !fonts.Exists(e => e.NAME == f.NAME) )
				fonts.Add(f);
		}

		public Font this[string s]{
			get => fonts.Find(e => e.NAME == s);
			set{
				if( !fonts.Exists(e => e.NAME == s) )
					AddNew(new Font(value.FONT, s));
			}
		}

		public void Unload(){
			this.fonts.Clear( );
			this.fonts = null;
		}

	}
}
