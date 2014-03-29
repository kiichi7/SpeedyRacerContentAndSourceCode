// Project: SpeedyRacer, File: Highscores.cs
// Namespace: SpeedyRacer.GameScreens, Class: Highscores
// Path: C:\code\SpeedyRacer\GameScreens, Author: Abi
// Code lines: 447, Size of file: 13,31 KB
// Creation date: 12.09.2006 07:22
// Last modified: 22.10.2006 18:41
// Generated with Commenter by abi.exDream.com

#region Using directives
#if DEBUG
//using NUnit.Framework;
#endif
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using SpeedyRacer.GameLogic;
using SpeedyRacer.Graphics;
using SpeedyRacer.Helpers;
using SpeedyRacer.Properties;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpeedyRacer.Sounds;
#endregion

namespace SpeedyRacer.GameScreens
{
	/// <summary>
	/// Highscores
	/// </summary>
	/// <returns>IGame screen</returns>
	class Highscores : IGameScreen
	{
		#region Highscore helper class
		/// <summary>
		/// Highscore helper class
		/// </summary>
		private struct HighscoreHelper
		{
			#region Variables
			/// <summary>
			/// Player name
			/// </summary>
			public string name;
			/// <summary>
			/// Highscore points 
			/// </summary>
			public int timeMilliseconds;
			#endregion

			#region Constructor
			/// <summary>
			/// Create highscore
			/// </summary>
			/// <param name="setName">Set name</param>
			/// <param name="setTimeMs">Set time ms</param>
			public HighscoreHelper(string setName, int setTimeMs)
			{
				name = setName;
				timeMilliseconds = setTimeMs;
			} // HighscoreInLevel(setName, setTimeMs)
			#endregion

			#region ToString
			/// <summary>
			/// To string
			/// </summary>
			/// <returns>String</returns>
			public override string ToString()
			{
				return name + ":" + timeMilliseconds;
			} // ToString()
			#endregion
		} // struct HighscoreInLevel

		/// <summary>
		/// Number of highscores displayed in this screen.
		/// </summary>
		private const int NumOfHighscores = 10;

		/// <summary>
		/// List of remembered highscores.
		/// </summary>
		private static HighscoreHelper[] highscores = null;

		/// <summary>
		/// Write highscores to string. Used to save to highscores settings.
		/// </summary>
		private static void WriteHighscoresToSettings()
		{
			string saveString = "";
			for (int num = 0; num < NumOfHighscores; num++)
      {
				saveString += (saveString.Length == 0 ? "" : ", ") +
					highscores[num];
      } // for (num)

			GameSettings.Default.Highscores = saveString;
		} // WriteHighscoresToSettings()

		/// <summary>
		/// Read highscores from settings
		/// </summary>
		/// <returns>True if reading succeeded, false otherwise.</returns>
		private static bool ReadHighscoresFromSettings()
		{
			if (String.IsNullOrEmpty(GameSettings.Default.Highscores))
				return false;

			string highscoreString = GameSettings.Default.Highscores;
			string[] allHighscores = highscoreString.Split(new char[] { ',' });
			for (int num = 0; num < NumOfHighscores; num++)
			{
				string[] oneHighscore =
					allHighscores[num].Split(new char[] { ':' });
				highscores[num] = new HighscoreHelper(
					oneHighscore[0], Convert.ToInt32(oneHighscore[1]));
			} // for (num)

			return true;
		} // ReadHighscoresFromSettings()
		#endregion

		#region Static constructor
		/// <summary>
		/// Create Highscores class, will basically try to load highscore list,
		/// if that fails we generate a standard highscore list!
		/// </summary>
		static Highscores()
		{
			// Init highscores
			highscores = new HighscoreHelper[NumOfHighscores];

			// Get player name from windows computer/user name if not set yet.
			if (String.IsNullOrEmpty(GameSettings.Default.PlayerName))
			{
				// And save it back
				GameSettings.Default.PlayerName = WindowsHelper.GetDefaultPlayerName();
			} // if (String.IsNullOrEmpty)

			if (ReadHighscoresFromSettings() == false)
			{
				// Generate default lists
				highscores[9] = new HighscoreHelper("Kai", 90000);
				highscores[8] = new HighscoreHelper("Viper", 85000);
				highscores[7] = new HighscoreHelper("Netfreak", 80000);
				highscores[6] = new HighscoreHelper("Darky", 75000);
				highscores[5] = new HighscoreHelper("Waii", 70000);
				highscores[4] = new HighscoreHelper("Judge", 65000);
				highscores[3] = new HighscoreHelper("exDreamBoy", 60000);
				highscores[2] = new HighscoreHelper("Master_L", 55000);
				highscores[1] = new HighscoreHelper("Freshman", 50000);
				highscores[0] = new HighscoreHelper("abi", 45000);

				WriteHighscoresToSettings();
			} // if (ReadHighscoresFromSettings)
		} // Highscores()
		#endregion

		#region Get top lap time
		/// <summary>
		/// Get top lap time
		/// </summary>
		/// <returns>Best lap time</returns>
		public static float GetTopLapTime()
		{
			return (float)highscores[0].timeMilliseconds / 1000.0f;
		} // GetTopLapTime()
		#endregion

		#region Get top 5 rank lap times
		/// <summary>
		/// Get top 5 rank lap times
		/// </summary>
		/// <returns>Array of top 5 times</returns>
		public static int[] GetTop5LapTimes()
		{
			return new int[]
				{
					highscores[0].timeMilliseconds,
					highscores[1].timeMilliseconds,
					highscores[2].timeMilliseconds,
					highscores[3].timeMilliseconds,
					highscores[4].timeMilliseconds,
				};
		} // GetTop5LapTimes()
		#endregion

		#region Get rank from current score
		/// <summary>
		/// Get rank from current time.
		/// Used in game to determinate rank while flying around ^^
		/// </summary>
		/// <param name="timeMilisec">Time ms</param>
		/// <returns>Int</returns>
		public static int GetRankFromCurrentTime(int timeMilliseconds)
		{
			// Time must be at least 1 second
			if (timeMilliseconds < 1000)
				// Invalid time, return rank 11 (out of highscore)
				return NumOfHighscores;

			// Just compare with all highscores and return the rank we have reached.
			for (int num = 0; num < NumOfHighscores; num++)
			{
				if (timeMilliseconds <= highscores[num].timeMilliseconds)
					return num;
			} // for (num)

			// No Rank found, use rank 11
			return NumOfHighscores;
		} // GetRankFromCurrentTime(level, timeMilliseconds)
		#endregion

		#region Submit highscore after game
		/// <summary>
		/// Submit highscore. Done after each game is over (won or lost).
		/// New highscore will be added to the highscore screen.
		/// In the future: Also send highscores to the online server.
		/// </summary>
		/// <param name="score">Score</param>
		/// <param name="levelName">Level name</param>
		public static void SetHighscore(int timeMilliseconds)
		{
			// Search which highscore rank we can replace
			for (int num = 0; num < NumOfHighscores; num++)
			{
				if (timeMilliseconds <= highscores[num].timeMilliseconds)
				{
					// Move all highscores up
					for (int moveUpNum = NumOfHighscores - 1; moveUpNum > num;
						moveUpNum--)
					{
						highscores[moveUpNum] = highscores[moveUpNum - 1];
					} // for (moveUpNum)

					// Add this highscore into the local highscore table
					highscores[num].name = GameSettings.Default.PlayerName;
					highscores[num].timeMilliseconds = timeMilliseconds;

					// And save that
					Highscores.WriteHighscoresToSettings();

					break;
				} // if (timeMilisec)
			} // for (num)

			// Else no highscore was reached, we can't replace any rank.
		} // SetHighscore(level, timeMilisec)
		#endregion

		#region Render
		/// <summary>
		/// Render game screen. Called each frame.
		/// </summary>
		/// <returns>Bool</returns>
		public bool Render()
		{
			// This starts both menu and in game post screen shader!
			BaseGame.UI.PostScreenMenuShader.Start();

			// Render background
			BaseGame.UI.RenderMenuBackground();
			BaseGame.UI.RenderBlackBar(160, 498-160);

			// Highscores header
			BaseGame.UI.Logos.RenderOnScreenRelative1600(
#if XBOX360
				10 + 36, 18 + 26, UIRenderer.HeaderHighscoresGfxRect);
#else
				10, 18, UIRenderer.HeaderHighscoresGfxRect);
#endif

			// Simple track
			int xPos = BaseGame.XToRes(512-160*3/2 + 25);
			int yPos = BaseGame.YToRes(182);
			int lineHeight = BaseGame.YToRes(27);
			TextureFont.WriteText(xPos, yPos, "Simple", Color.Yellow);

			int xPos1 = BaseGame.XToRes(300);
			int xPos2 = BaseGame.XToRes(350);
			int xPos3 = BaseGame.XToRes(640);

			// Draw seperation line
			yPos = BaseGame.YToRes(208);
			BaseGame.DrawLine(
				new Point(xPos1, yPos),
				new Point(xPos3+TextureFont.GetTextWidth("5:67:89"), yPos),
				new Color(192, 192, 192, 128));
			// And another one, looks better with 2 pixel height
			BaseGame.DrawLine(
				new Point(xPos1, yPos+1),
				new Point(xPos3 + TextureFont.GetTextWidth("5:67:89"), yPos + 1),
				new Color(192, 192, 192, 128));

			yPos = BaseGame.YToRes(220);

			// Go through all highscores
			for (int num = 0; num < NumOfHighscores; num++)
			{
				Rectangle lineRect = new Rectangle(
					0, yPos, BaseGame.Width, lineHeight);
				Color col = Input.MouseInBox(lineRect) ?
					Color.White : new Color(200, 200, 200);
				TextureFont.WriteText(xPos1, yPos,
					(1 + num) + ".", col);
				TextureFont.WriteText(xPos2, yPos,
					highscores[num].name, col);

				TextureFont.WriteGameTime(xPos3, yPos,
					highscores[num].timeMilliseconds, Color.Yellow);

				yPos += lineHeight;
			} // for (num)

			BaseGame.UI.RenderBottomButtons(true);

			if (Input.KeyboardEscapeJustPressed ||
				Input.GamePadBJustPressed ||
				Input.GamePadBackJustPressed ||
				Input.MouseLeftButtonJustPressed &&
				// Don't allow clicking on the controls to quit
				Input.MousePos.Y > yPos)
				return true;

			return false;
		} // Render()
		#endregion

		#region Unit Testing
#if DEBUG
		/// <summary>
		/// Test Highscores
		/// </summary>
		static public void TestHighscores()
		{
			Highscores highscoresScreen = null;
			TestGame.Start(
				delegate
				{
					highscoresScreen = new Highscores();
					SpeedyRacerManager.AddGameScreen(highscoresScreen);
				},
				delegate
				{
					highscoresScreen.Render();
				});
		} // TestHighscores()
#endif
		#endregion
	} // class Highscores
} // namespace SpeedyRacer.GameScreens