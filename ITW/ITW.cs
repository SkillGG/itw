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

		InputStates inputs;

		// FONTS
		SpriteFont FiraFont;
		// FONTS

		// SCREEN
		private const int DEFRES = 800;
		private int[] useRes = new int[2] { DEFRES, DEFRES / 12 * 9 };
		private bool fullScreen = false;
		private bool borderLess = true;
		public int WIDTH { get { return this.useRes[0]; } private set { this.useRes[0] = value; } }
		public int HEIGHT { get { return this.useRes[1]; } private set { this.useRes[1] = value; } }
		public bool FULLSCREEN { get { return this.fullScreen; } private set { this.fullScreen = value; } }
		public bool BORDERLESS { get { return this.borderLess; } private set { this.borderLess = value; } }

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
			Window.Position = new Point(( GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - WIDTH ) / 2, 0);

			// Set FPS
			this.IsFixedTimeStep = true;
			this.graphics.SynchronizeWithVerticalRetrace = true;
			this.TargetElapsedTime = new System.TimeSpan(0, 0, 0, 0, 33); // ~30.3FPS

			LanguageHandler z = new LanguageHandler();
			new Debug("",z.AddFromString(@"	EN.yes:'no'<0>=[
	ident.id:`12`=`value`;
	ident.id:`12`=`value`;
	ident.id:`12`=`value`;
];"),Debug.Importance.IMPORTANT_INFO);

		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize() {
			// TODO: Add your initialization logic here

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

			FiraFont = Content.Load<SpriteFont>("fonts/Fira24");

		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// game-specific content.
		/// </summary>
		protected override void UnloadContent() {
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime) {
			if( GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState( ).IsKeyDown(Keys.Escape) )
				Exit( );

			// TODO: Add your update logic here

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime) {
			GraphicsDevice.Clear(Color.CornflowerBlue);

			// TODO: Add your drawing code here
			spriteBatch.Begin( );

			spriteBatch.End( );

			base.Draw(gameTime);
		}
	}
}
