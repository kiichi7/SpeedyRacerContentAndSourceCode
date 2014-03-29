// Project: SpeedyRacer, File: SpeedyRacer.cs
// Namespace: SpeedyRacer, Class: SpeedyRacer
// Path: C:\code\SpeedyRacer, Author: Abi
// Code lines: 410, Size of file: 10,56 KB
// Creation date: 07.09.2006 05:56
// Last modified: 20.10.2006 16:21
// Generated with Commenter by abi.exDream.com

#region Using directives
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using System;
using System.Collections.Generic;
using SpeedyRacer.GameLogic;
using SpeedyRacer.GameScreens;
using SpeedyRacer.Graphics;
using SpeedyRacer.Helpers;
using SpeedyRacer.Landscapes;
using SpeedyRacer.Sounds;
using Model = SpeedyRacer.Graphics.Model;
using Texture = SpeedyRacer.Graphics.Texture;
using SpeedyRacer.Properties;
using SpeedyRacer.Shaders;
#endregion

namespace SpeedyRacer
{
	/// <summary>
	/// This is the main entry class our game. Handles all game screens,
	/// which themself handle all the game logic.
	/// As you can see this class is very simple, which is really cool.
	/// </summary>
	public class SpeedyRacerManager : BaseGame
	{
		#region Variables
		/// <summary>
		/// Game screens stack. We can easily add and remove game screens
		/// and they follow the game logic automatically. Very cool.
		/// </summary>
		private static Stack<IGameScreen> gameScreens = new Stack<IGameScreen>();

		/// <summary>
		/// Player for the game, also allows us to control the car and contains
		/// all the required code for the car physics, chase camera and basic
		/// player values and the game time because this is the top class
		/// of many derived classes. Player, car and camera position is set
		/// when the game starts depending on the selected level.
		/// </summary>
		private static Player player = new Player(new Vector3(0, 0, 0));

		/// <summary>
		/// Car model and selection plate for the car selection screen.
		/// </summary>
		private static Model carModel = null;

		/// <summary>
		/// Material for brake tracks on the road.
		/// </summary>
		private static Material brakeTrackMaterial = null;

		/// <summary>
		/// Landscape we are currently using.
		/// </summary>
		private static Landscape landscape = null;
		#endregion

		#region Properties
		/// <summary>
		/// In menu
		/// </summary>
		/// <returns>Bool</returns>
		public static bool InMenu
		{
			get
			{
				return gameScreens.Count > 0 &&
					gameScreens.Peek().GetType() != typeof(GameScreen);
			} // get
		} // InMenu

		/// <summary>
		/// In game?
		/// </summary>
		public static bool InGame
		{
			get
			{
				return gameScreens.Count > 0 &&
					gameScreens.Peek().GetType() == typeof(GameScreen);
			} // get
		} // InGame

		/// <summary>
		/// ShowMouseCursor
		/// </summary>
		/// <returns>Bool</returns>
		public static bool ShowMouseCursor
		{
			get
			{
				// Only if not in Game, not in logo or splash screen!
				return gameScreens.Count > 0 &&
					gameScreens.Peek().GetType() != typeof(GameScreen);
			} // get
		} // ShowMouseCursor

		/// <summary>
		/// Player for the game, also allows us to control the car and contains
		/// all the required code for the car physics, chase camera and basic
		/// player values and the game time because this is the top class
		/// of many derived classes.
		/// Easy access here with a static property in case we need the player
		/// somewhere in the game.
		/// </summary>
		/// <returns>Player</returns>
		public static Player Player
		{
			get
			{
				return player;
			} // get
		} // Player

		/// <summary>
		/// Car model
		/// </summary>
		/// <returns>Model</returns>
		public static Model CarModel
		{
			get
			{
				return carModel;
			} // get
		} // CarModel

		/// <summary>
		/// Brake track material
		/// </summary>
		/// <returns>Material</returns>
		public static Material BrakeTrackMaterial
		{
			get
			{
				return brakeTrackMaterial;
			} // get
		} // BrakeTrackMaterial

		/// <summary>
		/// Landscape we are currently using, used for several things (menu
		/// background, the game, some other classes outside the landscape class).
		/// </summary>
		/// <returns>Landscape</returns>
		public static Landscape Landscape
		{
			get
			{
				return landscape;
			} // get
		} // Landscape
		#endregion

		#region Constructor
		/// <summary>
		/// Create Racing game
		/// </summary>
		public SpeedyRacerManager()
			: base("SpeedyRacer")
		{
			// Start playing the menu music
			Sound.Play(Sound.Sounds.Music);

			// Create main menu as our main entry point
			gameScreens.Push(new MainMenu());
		} // SpeedyRacerManager()

		/// <summary>
		/// Create Racing game for unit tests, not used for anything else.
		/// </summary>
		public SpeedyRacerManager(string unitTestName)
			: base(unitTestName)
		{
			// Don't add game screens here
		} // SpeedyRacerManager(unitTestName)

		/// <summary>
		/// Load car stuff
		/// </summary>
		protected override void Initialize()
		{
 			base.Initialize();

			// Load car model
			carModel = new Model("SpeedyRacer");

			// Load landscape
			landscape = new Landscape();

			// Load extra textures
			brakeTrackMaterial = new Material("track");
		} // LoadCarStuff()
		#endregion

		#region Add game screen
		/// <summary>
		/// Add game screen
		/// </summary>
		/// <param name="gameScreen">Game screen</param>
		public static void AddGameScreen(IGameScreen gameScreen)
		{
			// Play sound for screen click
			Sound.Play(Sound.Sounds.ScreenClick);

			// Add the game screen
			gameScreens.Push(gameScreen);
		} // AddGameScreen(gameScreen)
		#endregion

		#region Update
		/// <summary>
		/// Update
		/// </summary>
		protected override void Update(GameTime gameTime)
		{
			// Update game engine
			base.Update(gameTime);

			// Update player and game logic
			player.Update();
		} // Update()
		#endregion

		#region Render
		/// <summary>
		/// Render
		/// </summary>
		protected override void Render()
		{
			// No more game screens?
			if (gameScreens.Count == 0)
			{
				// Before quiting, stop music and play crash sound :)
				Sound.PlayCrashSound(true);
				Sound.StopMusic();

				// Then quit
				Exit();
				return;
			} // if (gameScreens.Count)

			// Handle current screen
			if (gameScreens.Peek().Render())
			{
				// If this was the options screen and the resolution has changed,
				// restart the game!
				if (gameScreens.Peek().GetType() == typeof(Options) &&
					(BaseGame.Width != GameSettings.Default.ResolutionWidth ||
					BaseGame.Height != GameSettings.Default.ResolutionHeight))
				{
#if !XBOX360
					// Restart if resolution was changed!
					Program.RestartGameAfterOptionsChange = true;
					this.Exit();
#endif
				} // if

				// Play sound for screen back
				Sound.Play(Sound.Sounds.ScreenBack);

				gameScreens.Pop();
			} // if (gameScreens.Peek)
		} // Render()

		/// <summary>
		/// Post user interface rendering, in case we need it.
		/// Used for rendering the car selection 3d stuff after the UI.
		/// </summary>
		protected override void PostUIRender()
		{
			// Enable depth buffer again
			BaseGame.Device.RenderState.DepthBufferEnable = true;

			// Do menu shader after everything
			if (SpeedyRacerManager.InMenu &&
				PostScreenMenu.Started)
				UI.PostScreenMenuShader.Show();
		} // PostUIRender()
		#endregion
	} // class SpeedyRacerManager
} // namespace SpeedyRacer
