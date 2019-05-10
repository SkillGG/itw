
namespace ITW.Gameplay {
	public class VariablesHandler {

		/// <summary>
		/// private instance of main game
		/// </summary>
		private readonly ITW game;
		/// <summary>
		/// LanguageHandler handle
		/// </summary>
		public LanguageHandler Langs { get => game.Languages; }

		public void RefreshLanguage() => game.RefreshLanguage( );

		public FontHandler Fonts { get => game.Fonts; }

		/// <summary>
		/// GameVars handle
		/// </summary>
		public GameVars Vars { get => game.Variables; }

		/// <summary>
		/// Current language handle
		/// </summary>
		public Language Language { get => Langs.Current; }

		public VariablesHandler(ITW game) {
			this.game = game;
		}

	}
}
