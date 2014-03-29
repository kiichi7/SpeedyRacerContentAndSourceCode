#region Using directive
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using XnaTetris.Sounds;
using XnaTetris.Graphics;
using XnaTetris.Helpers;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace XnaTetris.Game
{
	/// <summary>
	/// Base game class for all the basic game support.
	/// Connects all our helper classes together and makes our live easier!
	/// </summary>
	public class BaseGame : Microsoft.Xna.Framework.Game
	{
		#region Variables
		protected GraphicsDeviceManager graphics;
		protected ContentManager content;

		/// <summary>
		/// Resolution of our game.
		/// </summary>
		protected static int width, height;

		/// <summary>
		/// Font for rendering text
		/// </summary>
		TextureFont font = null;
		#endregion

		#region Properties
		public static int Width
		{
			get
			{
				return width;
			} // get
		} // Width

		public static int Height
		{
			get
			{
				return height;
			} // get
		} // Height
		#endregion

		#region Constructor
		public BaseGame()
		{
			graphics = new GraphicsDeviceManager(this);
			content = new ContentManager(Services);
		} // BaseGame

		protected override void Initialize()
		{
			// Remember resolution
			width = graphics.GraphicsDevice.Viewport.Width;
			height = graphics.GraphicsDevice.Viewport.Height;

			// Create font
			font = new TextureFont(graphics.GraphicsDevice);

			base.Initialize();
		} // Initialize()
		#endregion

		#region Update
		protected override void Update(GameTime gameTime)
		{
			Sound.Update();

			base.Update(gameTime);
		} // Update(gameTime)
		#endregion

		#region Draw
		protected override void Draw(GameTime gameTime)
		{
			// Draw all sprites and fonts
			SpriteHelper.DrawSprites(width, height);
			font.WriteAll();

			base.Draw(gameTime);
		} // Draw(gameTime)
		#endregion
	} // class BaseGame
} // namespace XnaTetris.Game
