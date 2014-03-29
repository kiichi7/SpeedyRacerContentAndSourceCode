#if DEBUG
// Project: SpeedyRacer, File: UnitTests.cs
// Namespace: SpeedyRacer, Class: UnitTests
// Path: C:\code\SpeedyRacer, Author: Abi
// Code lines: 211, Size of file: 6,79 KB
// Creation date: 03.10.2006 19:00
// Last modified: 07.11.2006 02:25
// Generated with Commenter by abi.exDream.com

#region Using directives
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using SpeedyRacer.GameLogic;
using SpeedyRacer.GameScreens;
using SpeedyRacer.Graphics;
using SpeedyRacer.Helpers;
using SpeedyRacer.Landscapes;
using SpeedyRacer.Properties;
using SpeedyRacer.Shaders;
using SpeedyRacer.Sounds;
using SpeedyRacer.Tracks;
#endregion

namespace SpeedyRacer
{
	/// <summary>
	/// Unit tests. Usually I would use NUnit and especially TestDriven.NET,
	/// but since they don't work in XNA Game Studio Express I use this faked
	/// way of unit testing my static unit tests. Works also quite nice
	/// and is very simple to use, just uncomment the line you want to test
	/// and press F5. In release mode, this all is unused!
	/// Update: Now also supports many command line files to be autogenerated,
	/// which then start the specific unit tests if started.
	/// </summary>
	class UnitTests
	{
		#region Private constructor
		/// <summary>
		/// Create unit tests, private constructor to prevent instantiation.
		/// </summary>
		private UnitTests()
		{
		} // UnitTests()
		#endregion

		#region StartTest
		/// <summary>
		/// Start static test, we just do it this way because we can't
		/// use any professional tools here. In normal projects I would
		/// use VS addins, which are not supported or allowed in
		/// VS Express (say hello and good bye to TestDriven.Net).
		/// We always start this in Debug mode, use Release mode
		/// to start the actual game. You can also just use the
		/// first unit test to start the actual game.
		/// Note: This are ONLY static unit tests, all dynamic unit
		/// tests are covered with NUnit-gui.exe!
		/// </summary>
		public static void StartTest(string[] args)
		{
			#region Start custom test by command line argument
			// Check if the user specified a test he wants to run in the command
			// line arguments.
			if (args != null &&
				args.Length == 1 &&
				args[0].Length > 0)
			{
				string searchForMethodName = args[0];

				// Search for this test with help of reflection :)
				Assembly asm = Assembly.GetExecutingAssembly();

				// Check all classes we can find
				foreach (Type classType in asm.GetTypes())
					// Check all static methods that begin with "Test"
					if (classType.IsClass)
					{
						// Get all static methods
						MethodInfo[] methods = classType.GetMethods(
						BindingFlags.Public | BindingFlags.Static);
						foreach (MethodInfo method in methods)
							if (StringHelper.Contains(method.Name, searchForMethodName))
							{
								// Execute this method!
								method.Invoke(null, null);
								// And get outa here
								return;
							} // foreach if (StringHelper.Contains)
					} // foreach if (classType.IsClass)

				// Not found, just ignore and continue with normal program.
			} // if (args)
			#endregion

			#region SpeedyRacer Unit Tests
			// Just uncomment the line you want to test.
			// Normally you should only test one line at a time.
			// If you are crazy, you can also test everything (lots of tests!)

			Program.StartGame();

			//GenerateUnitTestCommandLineFiles();
			//GameSettings.TestSaveGameSettings();

			//LineManager2D.TestRenderLines();//.TestLineRendering2D();
			//LineManager3D.TestDraw3DLine();
			
			//RenderToTexture.TestCreateRenderToTexture();

			//PostScreenGlow.TestPostScreenGlow();
			//PostScreenGlow.TestPostScreenGlowOnScreenshot();
			//PostScreenMenu.TestPostScreenMenu();
			//PostScreenTest.TestPostScreenTest();
			//PreScreenSkyCubeMapping.TestSkyCubeMapping();

			//ShadowMapShader.TestShadowMapping();

			//Sound.TestPlayClickSound();
			//Sound.TestGearSoundsWithUI();

			//ColladaTrack.TestColladaTrack();
			//UIRenderer.TestUI();
			//UIRenderer.TestGameUI();
			//Texture.TestTextures();
			//Model.TestSingleModel();
			//Model.TestRenderModel();
			//Model.TestCarModel();
			//Model.TestWindmillAnimation();
			//Model.TestManyModelsPerformance();
			//LensFlare.TestLensFlare();

			//TextureFontBigNumbers.TestWriteNumbers();
			//TextureFontBigNumbers.TestFontFadeupEffect();
			//TextureFont.TestTextureFont();
			//Texture.TestRenderingRotatedTexture();

			//Landscape.TestRenderLandscape();

			//PlaneRenderer.TestRenderingPlaneXY();
			//PlaneRenderer.TestRenderingPlaneWithVector111();

			//PhysicsManager.TestCarPhysicsOnAPlane();
			//PhysicsManager.TestCarPhysicsWithGuardRails();

			//CarPhysics.TestCarPhysicsOnPlane();
			//CarPhysics.TestCarPhysicsOnPlaneWithGuardRails();
			
			//ShadowMapShader.TestShadowMapping();
			//Model.TestSingleModel();
			//Options.TestOptions();
			#endregion
		} // StartTest()
		#endregion

		#region Generate unit test command line files
		/// <summary>
		/// Generate unit test command line files. Very useful helper that writes
		/// out command line (.cmd) files for each static unit test we have :)
		/// </summary>
		public static void GenerateUnitTestCommandLineFiles()
		{
			// Search for this test with help of reflection :)
			Assembly asm = Assembly.GetExecutingAssembly();

			// Check all classes we can find
			foreach (Type classType in asm.GetTypes())
				// Check all static methods that begin with "Test"
				if (classType.IsClass)
				{
					// Get all static methods
					MethodInfo[] methods = classType.GetMethods(
						BindingFlags.Public | BindingFlags.Static);
					foreach (MethodInfo method in methods)
						if (method.Name.StartsWith("Test"))
						{
							// Create command line method for this if it does not exist yet.
							string newCmdFile = "SpeedyRacer "+method.Name+".cmd";
							if (File.Exists(newCmdFile) == false)
								FileHelper.CreateTextFile(newCmdFile,
									"echo Autogenerated SpeedyRacer Unit Test cmd file. "+
									"Press Esc to quit Unit Test.\n"+
									"SpeedyRacer.exe "+method.Name,
									// cmd files must be ascii to work correctly!
									Encoding.ASCII);
						} // foreach if (StringHelper.Contains)
				} // foreach if (classType.IsClass)
		} // GenerateUnitTestCommandLineFiles()
		#endregion
	} // class UnitTests
} // namespace SpeedyRacer
#endif