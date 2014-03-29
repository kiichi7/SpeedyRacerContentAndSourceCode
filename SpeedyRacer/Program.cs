// Project: SpeedyRacer, File: Program.cs
// Namespace: SpeedyRacer, Class: Program
// Path: C:\code\SpeedyRacer, Author: Abi
// Code lines: 65, Size of file: 1,50 KB
// Creation date: 07.09.2006 03:58
// Last modified: 20.10.2006 15:54
// Generated with Commenter by abi.exDream.com

#region Using directives
using System;
using SpeedyRacer.Helpers;
using SpeedyRacer.Properties;
#endregion

namespace SpeedyRacer
{
	/// <summary>
	/// Program
	/// </summary>
	static class Program
	{
		#region RestartGameAfterOptionsChange
#if !XBOX360
		/// <summary>
		/// Restart if user changes something in options.
		/// </summary>
		public static bool RestartGameAfterOptionsChange = false;
#endif
		#endregion

		#region Main
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		/// <param name="args">Arguments</param>
#if DEBUG
		static void Main(string[] args)
		{
			UnitTests.StartTest(args);
#else
		static void Main()
		{
			StartGame();
#endif

			// Make sure settings are saved (will only be executed if any setting
			// changed).
			GameSettings.Save();

#if !XBOX360
			// Restarting does only work on the windows platform, isn't required
			// for the Xbox 360 anyways.
			if (RestartGameAfterOptionsChange)
				System.Diagnostics.Process.Start("SpeedyRacer.exe");
#endif
		} // Main(args)
		#endregion

		#region StartGame
		/// <summary>
		/// Start game, is in a seperate method for 2 reasons: We want to catch
		/// any exceptions here, but not for the unit tests and we also allow
		/// the unit tests to call this method if we don't want to unit test
		/// in debug mode.
		/// </summary>
		public static void StartGame()
		{
			// Normal start without exception checking in debug mode
#if DEBUG
			using (SpeedyRacerManager game = new SpeedyRacerManager())
			{
				game.Run();
			} // using (game)
#elif XBOX360
			// On the Xbox 360 we can't display message boxes.
			using (SpeedyRacerManager game = new SpeedyRacerManager())
			{
				game.Run();
			} // using (game)
#else
			try
			{
				using (SpeedyRacerManager game = new SpeedyRacerManager())
				{
					game.Run();
				} // using (game)
			} // try
			catch (Exception ex)
			{
				Log.Write("Fatal error, application crashed: " + ex.ToString());
			} // catch
#endif
		} // StartGame()
		#endregion				
	} // class Program
} // namespace SpeedyRacer
