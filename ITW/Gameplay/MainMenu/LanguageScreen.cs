using System;

namespace ITW.Gameplay.MainMenu {
	public class LanguageScreen {

		private Action<Language> pickAction;

		public LanguageScreen() {

		}

		public void Picked(Action<Language> pa) {
			this.pickAction = pa;
		}

	}
}
