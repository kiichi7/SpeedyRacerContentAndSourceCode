// Project: SpeedyRacer, File: GameScreen.cs
// Namespace: SpeedyRacer.GameScreens, Class: GameScreen
// Path: C:\code\SpeedyRacer\GameScreens, Author: Abi
// Code lines: 206, Size of file: 5,95 KB
// Creation date: 23.10.2006 17:21
// Last modified: 23.10.2006 23:32
// Generated with Commenter by abi.exDream.com

#region Using directives
using System;
using System.Collections.Generic;
using System.Text;
using SpeedyRacer.GameLogic;
using SpeedyRacer.Graphics;
using SpeedyRacer.Helpers;
using SpeedyRacer.Properties;
using SpeedyRacer.Landscapes;
using Microsoft.Xna.Framework;
using SpeedyRacer.Shaders;
using SpeedyRacer.Sounds;
#endregion

namespace SpeedyRacer.GameScreens
{
	/// <summary>
	/// GameScreen, just manages the on screen display for the game.
	/// Controlling is done in ChaseCamera class.
	/// Most graphical stuff is done in AsteroidManager.
	/// </summary>
	class GameScreen : IGameScreen
	{
		#region Constructor
		/// <summary>
		/// Create game screen
		/// </summary>
		public GameScreen()
		{
			// Show car maxSpeed, Acceleration and Mass values.
			// Also show braking, friction and engine values based on that.
			CarPhysics.SetCarVariablesForCarType(
				CarPhysics.DefaultMaxSpeed * 2.22f,//1.05f, // 288 mph
				CarPhysics.DefaultCarMass * 0.67f,//1.015f, // 1015 kg
				CarPhysics.DefaultMaxAccelerationPerSec * 3.5f);//0.85f); // 4 m/s^2

			// Load level
			SpeedyRacerManager.Landscape.ReloadLevel();

			// Reset player variables (start new game, reset time and position)
			SpeedyRacerManager.Player.Reset();
			// Fix light direction (was changed by CarSelection screen!)
			// LightDirection will normalize
			BaseGame.LightDirection = LensFlare.DefaultLightPos;

			// Start gear sound
			Sound.StartGearSound();

			// Start sign sound
			Sound.Play(Sound.Sounds.Startsign);
		} // GameScreen()
    #endregion

		#region Render
		/// <summary>
		/// Render game screen. Called each frame.
		/// </summary>
		public bool Render()
		{
			if (BaseGame.AllowShadowMapping)
			{
				// Generate shadows
				ShaderEffect.shadowMapping.GenerateShadows(
					delegate
					{
						SpeedyRacerManager.Landscape.GenerateShadow();
						SpeedyRacerManager.CarModel.GenerateShadow(
							SpeedyRacerManager.Player.CarRenderMatrix);
					});

				// Render shadows
				ShaderEffect.shadowMapping.RenderShadows(
					delegate
					{
						SpeedyRacerManager.Landscape.UseShadow();
						SpeedyRacerManager.CarModel.UseShadow(
							SpeedyRacerManager.Player.CarRenderMatrix);
					});
			} // if (BaseGame.AllowShadowMapping)

			// This starts both menu and in game post screen shader!
			BaseGame.UI.PostScreenGlowShader.Start();

			// Render background sky and lensflare.
			BaseGame.UI.RenderGameBackground();

			// Render landscape with track and all objects
			SpeedyRacerManager.Landscape.Render();

			// Render car with matrix we got from CarPhysics
			SpeedyRacerManager.CarModel.RenderCar(
				false, SpeedyRacerManager.Player.CarRenderMatrix);

			// And flush all models to be rendered
			BaseGame.MeshRenderManager.Render();

			// Use data from best replay for the shadow car
			Matrix bestReplayCarMatrix =
				SpeedyRacerManager.Landscape.BestReplay.GetCarMatrixAtTime(
				SpeedyRacerManager.Player.GameTimeMilliseconds / 1000.0f);
			// For rendering rotate car to stay correctly on the road
			bestReplayCarMatrix =
				Matrix.CreateRotationX(MathHelper.Pi / 2.0f) *
				Matrix.CreateRotationZ(MathHelper.Pi) *
				bestReplayCarMatrix;

			// Also render the shadow car (if the game has started)!
			if (SpeedyRacerManager.Player.GameTimeMilliseconds > 0)
				SpeedyRacerManager.CarModel.RenderCar(
					true, bestReplayCarMatrix);

			// Show shadows we calculated above
			if (BaseGame.AllowShadowMapping)
			{
				ShaderEffect.shadowMapping.ShowShadows();
			} // if (BaseGame.AllowShadowMapping)

			// Apply post screen shader here before doing the UI
			BaseGame.UI.PostScreenGlowShader.Show();

			// Play motor sound
			Sound.UpdateGearSound(SpeedyRacerManager.Player.Speed,
				SpeedyRacerManager.Player.Acceleration);

			// Show on screen UI for the game.
			// Note: Could be improved by using the latest checkpoints and
			// check times this way!
			BaseGame.UI.RenderGameUI(
				(int)SpeedyRacerManager.Player.GameTimeMilliseconds,
				// Best time and current lap
				(int)SpeedyRacerManager.Player.BestTimeMilliseconds,
				SpeedyRacerManager.Player.CurrentLap+1,
				SpeedyRacerManager.Player.Speed * CarPhysics.MeterPerSecToMph,
				// Gear logic with sound (could be improved ^^)
				1 + (int)(5 * SpeedyRacerManager.Player.Speed /
				CarPhysics.MaxPossibleSpeed),
				// Motormeter
				0.5f * SpeedyRacerManager.Player.Speed /
				CarPhysics.MaxPossibleSpeed +
				// This could be improved
				0.5f*SpeedyRacerManager.Player.Acceleration,
				"Simple", Highscores.GetTop5LapTimes());

			if (Input.KeyboardEscapeJustPressed ||
				Input.GamePadBackJustPressed ||
				(SpeedyRacerManager.Player.GameOver &&
				(Input.KeyboardSpaceJustPressed ||
				Input.GamePadAJustPressed ||
				Input.GamePadBJustPressed ||
				Input.GamePadXJustPressed ||
				Input.GamePadXJustPressed ||
				Input.MouseLeftButtonJustPressed)))
			{
				// Stop motor sound
				Sound.StopGearSound();

				// Play menu music again
				Sound.Play(Sound.Sounds.Music);

				// Return to menu
				return true;
			} // if (Input.KeyboardEscapeJustPressed)

			return false;
		} // Render()
		#endregion
	} // class GameScreen
} // namespace SpeedyRacer.GameScreens
