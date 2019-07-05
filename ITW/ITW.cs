using ITW.Exts;
using ITW.Gameplay;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ITW {
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class ITW : Game {
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		private InputStates inputs;

		public GameVars Variables { get; private set; }
		public LanguageHandler Languages { get; private set; }

		public MainGame Main { get; private set; }

		public StringDrawn VersionDraw;
		public StringDrawn DebugDraw;

		// FONTS
		public FontHandler Fonts { get; private set; }
		// FONTS

		// SCREEN
		private const int DEFRES = 800;
		private readonly int[] useRes = new int[2] { DEFRES, DEFRES / 12 * 9 };
		private bool fullScreen = false;
		private bool borderLess = true;
		public int WIDTH { get => this.useRes[0]; private set { this.useRes[0] = value; } }
		public int HEIGHT { get => this.useRes[1]; private set { this.useRes[1] = value; } }
		public bool FULLSCREEN { get => this.fullScreen; private set { this.fullScreen = value; } }
		public bool BORDERLESS { get => this.borderLess; private set { this.borderLess = value; } }

		public Color BACKGROUND { get; set; }
		
		private bool FirstUpdate { get; set; }

		public void ChangeGameResolution(int? w, int? h, bool? FS, bool? BL) {
			string pl = "Multris#ChangeGameResolution";
			new Debug(pl, "Changing Resolution with: {w:" + w + ",h:" + h + ",FS:" + FS + ",BL:" + BL + "}", Debug.Importance.IMPORTANT_INFO);
			this.WIDTH = w ?? this.WIDTH;
			this.HEIGHT = h ?? this.HEIGHT;
			this.FULLSCREEN = FS ?? this.FULLSCREEN;
			this.BORDERLESS = BL ?? this.BORDERLESS;
			new Debug("Multris#ChangeGameResolution", "Successfully changed resolution", Debug.Importance.NOTIFICATION);

			new Debug(pl, "Saving changes", Debug.Importance.INIT_INFO);
			// Save game res
			this.graphics.PreferredBackBufferHeight = HEIGHT;
			this.graphics.PreferredBackBufferWidth = WIDTH;
			this.graphics.IsFullScreen = FULLSCREEN;
			Window.IsBorderless = BORDERLESS;
			this.graphics.ApplyChanges( );
			new Debug(pl, "Successfully saved new resolution", Debug.Importance.NOTIFICATION);
		}
		// SCREEN

		public ITW() {
			Debug.MinImp = Debug.Importance.IMPORTANT_INFO;
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			// show the mouse
			this.IsMouseVisible = true;

			// Initialize (set for first time) all window-related stuff
			this.graphics.PreferredBackBufferWidth = WIDTH;
			this.graphics.PreferredBackBufferHeight = HEIGHT;
			this.graphics.IsFullScreen = FULLSCREEN;
			Window.IsBorderless = BORDERLESS;
			BACKGROUND = Color.Black;
			// screen center
			Window.Position = new Point(( GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - WIDTH ) / 2, 0); 

			// Set FPS
			this.IsFixedTimeStep = true;
			this.graphics.SynchronizeWithVerticalRetrace = true;
			this.TargetElapsedTime = new System.TimeSpan(0, 0, 0, 0, 33); // ~30.3FPS

			// setup flags
			FirstUpdate = false;

			// setup fonts
			Fonts = new FontHandler( );

			// setup language related stuff
			Languages = new LanguageHandler( );
			Variables = new GameVars( );

			// Init all basic variables and load configs
			Variables.LoadFromFile(ReadFile.Read("./Config/Startup.cfg"));
			Variables.LoadFromFile(ReadFile.Read("./Config/Game.cfg"));

			// Create MainGame instance
			this.Main = new MainGame(new VariablesHandler(this));
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize() {
			// TODO: Add your initialization logic here
			
			// start gathering inputs
			inputs = new InputStates( );

			base.Initialize( );
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent() {
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			// TODO: use this.Content to load your game content here

			// Load fonts
			Fonts["Fira24"] = new FontHandler.Font(Content.Load<SpriteFont>("fonts/Fira24"), null);
			Fonts["Fira20"] = new FontHandler.Font(Content.Load<SpriteFont>("fonts/Fira20"), null);
			Fonts["Fira10"] = new FontHandler.Font(Content.Load<SpriteFont>("fonts/Fira10"), null);

			// Load languages
			Languages.AddFromString(ReadFile.Read("./Langs/EN.lang"));
			Languages.AddFromString(ReadFile.Read("./Langs/PL.lang"));

			// Player's config
			if( !string.IsNullOrWhiteSpace(Variables["Player.cfg"]) ) {
				Variables.LoadFromFile(ReadFile.Read(Variables["Player.cfg"]));
			}
			RefreshLanguage( );

			// debug text
			Vector2 s = Fonts["Fira10"].FONT.MeasureString(Languages.Current?["version"].GetValue(Variables) ?? "undefined");
			VersionDraw = new StringDrawn(
				s: Languages.Current?["version"].GetValue(Variables),
				p: new Vector2(WIDTH - s.X, HEIGHT - s.Y),
				c: Color.Red
			);
			VersionDraw.Show( );
			DebugDraw = new StringDrawn(
				s: Variables.Debugged( ),
				p: new Vector2(5),
				c: Color.Orange
			);
			DebugDraw.Show( );
			// debug text

		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// game-specific content.
		/// </summary>
		protected override void UnloadContent() {
			// TODO: Unload any non ContentManager content here
			Fonts.Unload( );
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime) {
			if( GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState( ).IsKeyDown(Keys.Escape) )
				Exit( );

			if( !FirstUpdate ) {
				Main.Start( );
				FirstUpdate = true;
			}

			// debug text

			if( this.VersionDraw.Text != ( Languages.Current?["version"].GetValue(Variables) ?? "undefined" ) ) {
				Vector2 s = Fonts["Fira10"].FONT.MeasureString(Languages.Current?["version"].GetValue(Variables) ?? "undefined");
				VersionDraw.Reset(
					s: Languages.Current?["version"].GetValue(Variables),
					p: new Vector2(WIDTH - s.X, HEIGHT - s.Y)
				);
				VersionDraw.Show( );
			}

			if( this.DebugDraw.Text != Variables.Debugged( ) ) {
				DebugDraw.Reset(Variables.Debugged( ));
				DebugDraw.Show( );
			}

			// debug text

			Main.Update(inputs);

			base.Update(gameTime);

			inputs.Update( );
		}

		/// <summary>
		/// Refreshes language in whole game
		/// </summary>
		public void RefreshLanguage() {
			if( Variables?["LANG"] != $"{Languages.Current.ID}" && Variables?["LANG"] != null ) {
				Languages.ChangeCurrentLang(int.Parse(Variables["LANG"]));
			}
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime) {
			GraphicsDevice.Clear(BACKGROUND);

			// TODO: Add your drawing code here
			spriteBatch.Begin( );
			Main.Draw(spriteBatch);

			// draw debug text
			VersionDraw.Draw(spriteBatch, Fonts["Fira10"].FONT);
			DebugDraw.Draw(spriteBatch, Fonts["Fira10"].FONT);
			//draw debug text

			spriteBatch.End( );

			base.Draw(gameTime);
		}
	}
}
