// Project: SpeedyRacer, File: RenderToTexture.cs
// Namespace: SpeedyRacer.Shaders, Class: RenderToTexture
// Path: C:\code\SpeedyRacer\Shaders, Author: Abi
// Code lines: 463, Size of file: 12,89 KB
// Creation date: 12.09.2006 07:20
// Last modified: 22.10.2006 18:52
// Generated with Commenter by abi.exDream.com

#region Using directives
using Microsoft.Xna.Framework.Graphics;
#if DEBUG
//using NUnit.Framework;
#endif
using System;
using System.Collections;
using System.Text;
using SpeedyRacer.Graphics;
using SpeedyRacer.Helpers;
using Texture = SpeedyRacer.Graphics.Texture;
using Model = SpeedyRacer.Graphics.Model;
using XnaTexture = Microsoft.Xna.Framework.Graphics.Texture2D;
using Microsoft.Xna.Framework;
using SpeedyRacer.GameLogic;
using Microsoft.Xna.Framework.Input;
#endregion

namespace SpeedyRacer.Shaders
{
	/// <summary>
	/// Render to texture helper class based on the Texture class.
	/// This class allows to render stuff onto textures, if thats not
	/// supported, it will just not work and report an engine log message.
	/// This class is required for most PostScreenShaders.
	/// </summary>
	public class RenderToTexture : Texture
	{
		#region Variables
		/// <summary>
		/// Our render target we are going to render to. Much easier than in MDX
		/// where you have to use Surfaces, etc. Also supports the Xbox360 model
		/// of resolving the render target texture before we can use it, otherwise
		/// the RenderToTexture class would not work on the Xbox360.
		/// </summary>
		RenderTarget2D renderTarget = null;

		/// <summary>
		/// Z buffer surface for shadow mapping render targets that do not
		/// fit in our resolution. Usually unused!
		/// </summary>
		DepthStencilBuffer zBufferSurface = null;
		/// <summary>
		/// ZBuffer surface
		/// </summary>
		/// <returns>Surface</returns>
		public DepthStencilBuffer ZBufferSurface
		{
			get
			{
				return zBufferSurface;
			} // get
		} // ZBufferSurface

		/// <summary>
		/// Posible size types for creating a RenderToTexture object.
		/// </summary>
		public enum SizeType
		{
			/// <summary>
			/// Uses the full screen size for this texture
			/// </summary>
			FullScreen,
			/// <summary>
			/// Uses half the full screen size, e.g. 800x600 becomes 400x300
			/// </summary>
			HalfScreen,
			/// <summary>
			/// Uses a quarter of the full screen size, e.g. 800x600 becomes 200x150
			/// </summary>
			QuarterScreen,
			/// <summary>
			/// Shadow map texture, usually 1024x1024, but can also be better
			/// like 2048x2048 or 4096x4096.
			/// </summary>
			ShadowMap,
		} // enum SizeTypes

		/// <summary>
		/// Size type
		/// </summary>
		private SizeType sizeType;

		/// <summary>
		/// Calc size
		/// </summary>
		private void CalcSize()
		{
			switch (sizeType)
			{
				case SizeType.FullScreen:
					texWidth = BaseGame.Width;
					texHeight = BaseGame.Height;
					break;
				case SizeType.HalfScreen:
					texWidth = BaseGame.Width / 2;
					texHeight = BaseGame.Height / 2;
					break;
				case SizeType.QuarterScreen:
					texWidth = BaseGame.Width / 4;
					texHeight = BaseGame.Height / 4;
					break;
				case SizeType.ShadowMap:
#if XBOX360
					// TODO: try to implement vsm technique!
					if (BaseGame.HighDetail)
					{
						texWidth = 2048;// 512;// 1024;//512
						texHeight = 2048;// 512;// 1024;//512
					} // else
					else
					{
						texWidth = 1024;
						texHeight = 1024;
					} // else
#else
					// Use 2048x2048 if we allow ps30
					if (BaseGame.CanUsePS30 &&
						BaseGame.HighDetail)
					{
						texWidth = 2048;
						texHeight = 2048;
					} // if (BaseGame.CanUsePS30)
					else
					{
						// 512x512 is an option too, but it is very blocky!
						if (BaseGame.CanUsePS20 == false &&
							BaseGame.HighDetail == false)
						{
							texWidth = 512;
							texHeight = 512;
						} // if
						else
						{
							texWidth = 1024;
							texHeight = 1024;
						} // else
					} // else
#endif
					break;
			} // switch
			CalcHalfPixelSize();
		} // CalcSize()

		/// <summary>
		/// Does this texture use some high percision format? Better than 8 bit color?
		/// </summary>
		private bool usesHighPercisionFormat = false;
		#endregion

		#region Properties
		/// <summary>
		/// Render target
		/// </summary>
		/// <returns>Render target 2D</returns>
		public RenderTarget2D RenderTarget
		{
			get
			{
				return renderTarget;
			} // get
		} // RenderTarget

		/// <summary>
		/// Override how to get XnaTexture, we have to resolve the render target
		/// for supporting the Xbox, which requires calling Resolve first!
		/// After that you can call this property to get the current texture.
		/// </summary>
		/// <returns>XnaTexture</returns>
		public override XnaTexture XnaTexture
		{
			get
			{
				if (alreadyResolved)
					internalXnaTexture = renderTarget.GetTexture();
				return internalXnaTexture;
			} // get
		} // XnaTexture

		/// <summary>
		/// Does this texture use some high percision format? Better than 8 bit color?
		/// </summary>
		public bool UsesHighPercisionFormat
		{
			get
			{
				return usesHighPercisionFormat;
			} // get
		} // UsesHighPercisionFormat
		#endregion

		#region Constructors
		/// <summary>
		/// Id for each created RenderToTexture for the generated filename.
		/// </summary>
		private static int RenderToTextureGlobalInstanceId = 0;
		/// <summary>
		/// Creates an offscreen texture with the specified size which
		/// can be used for render to texture.
		/// </summary>
		public RenderToTexture(SizeType setSizeType)
		{
			sizeType = setSizeType;
			CalcSize();

			texFilename = "RenderToTexture instance " +
				RenderToTextureGlobalInstanceId++;

			Create();

			BaseGame.AddRemRenderToTexture(this);
		} // RenderToTexture(setSizeType)
		#endregion

		#region Handle device reset
		/// <summary>
		/// Handle the DeviceReset event, we have to re-create all our render targets.
		/// </summary>
		public void HandleDeviceReset()
		{
			// Just recreate it.
			Create();
			alreadyResolved = false;
		} // HandleDeviceReset()
		#endregion

		#region Create
		/// <summary>
		/// Check if we can use a specific surface format for render targets.
		/// </summary>
		/// <param name="format"></param>
		/// <returns></returns>
		private static bool CheckRenderTargetFormat(SurfaceFormat format)
		{
			return BaseGame.Device.CreationParameters.Adapter.CheckDeviceFormat(
				BaseGame.Device.CreationParameters.DeviceType,
				BaseGame.Device.DisplayMode.Format,
				ResourceUsage.None,//.ResolveTarget,//.WriteOnly,
				QueryUsages.None,//.RenderTarget,
				ResourceType.RenderTarget,//.Texture2D,
				format);
		} // CheckRenderTargetFormat(format)

		/// <summary>
		/// Create
		/// </summary>
		private void Create()
		{
			SurfaceFormat format = SurfaceFormat.Color;
			// Try to use R32F format for shadow mapping if possible (ps20),
			// else just use A8R8G8B8 format for shadow mapping and
			// for normal RenderToTextures too.
			if (sizeType == SizeType.ShadowMap && BaseGame.CanUsePS20)
			{
				// Can do R32F format?
				if (CheckRenderTargetFormat(SurfaceFormat.Single))
					format = SurfaceFormat.Single;
				// Else try R16F format, thats still much better than A8R8G8B8
				else if (CheckRenderTargetFormat(SurfaceFormat.HalfSingle))
					format = SurfaceFormat.HalfSingle;
				// And check a couple more formats (mainly for the Xbox360 support)
				else if (CheckRenderTargetFormat(SurfaceFormat.HalfVector2))
					format = SurfaceFormat.HalfVector2;
				else if (CheckRenderTargetFormat(SurfaceFormat.Luminance16))
					format = SurfaceFormat.Luminance16;
				// Else nothing found, well, then just use the 8 bit Color format.

#if XBOX360
				// Try to force Surface format on the Xbox360, CheckRenderTargetFormat
				// does not work on the Xbox at all!
				format = SurfaceFormat.Single;
#endif
			} // if (sizeType)

			// Create render target of specified size.
			renderTarget = new RenderTarget2D(
				BaseGame.Device,
				texWidth, texHeight, 1,
				format);
			if (format != SurfaceFormat.Color)
				usesHighPercisionFormat = true;

			// Create z buffer surface for shadow map render targets
			// if they don't fit in our current resolution.
			if (sizeType == SizeType.ShadowMap &&
        (texWidth > BaseGame.Width ||
        texHeight > BaseGame.Height))
				zBufferSurface =
					new DepthStencilBuffer(
					BaseGame.Device,
					texWidth, texHeight,
					// Lets use the same stuff as the back buffer.
					BaseGame.BackBufferDepthFormat,
					// Don't use multisampling, render target does not support that.
					MultiSampleType.None, 0);

			loaded = true;
		} // Create()
		#endregion

		#region Clear
		/// <summary>
		/// Clear render target (call SetRenderTarget first)
		/// </summary>
		public void Clear(Color clearColor)
		{
			if (loaded == false ||
				renderTarget == null)
				return;

			BaseGame.Device.Clear(
				ClearOptions.Target | ClearOptions.DepthBuffer,
				clearColor, 1.0f, 0);
		} // Clear(clearColor)
		#endregion

		#region Set render target
		/// <summary>
		/// Set render target to this texture to render stuff on it.
		/// </summary>
		public bool SetRenderTarget()
		{
			if (loaded == false ||
				renderTarget == null)
				return false;

			BaseGame.SetRenderTarget(renderTarget, false);
			return true;
		} // SetRenderTarget()
		#endregion
		
		#region Resolve
		/// <summary>
		/// Make sure we don't call XnaTexture before resolving for the first time!
		/// </summary>
		bool alreadyResolved = false;
		/// <summary>
		/// Resolve render target. For windows developers this method may seem
		/// strange, why not just use the rendertarget's texture? Well, this is
		/// just for the Xbox360 support. The Xbox requires that you call Resolve
		/// first before using the rendertarget texture. The reason for that is
		/// copying the data over from the EPRAM to the video memory, for more
		/// details read the XNA docs.
		/// Note: This method will only work if the render target was set before
		/// with SetRenderTarget, else an exception will be thrown to ensure
		/// correct calling order.
		/// </summary>
		public void Resolve()
		{
			// Make sure this render target is currently set!
			if (BaseGame.CurrentRenderTarget != renderTarget)
				throw new InvalidOperationException(
					"You can't call Resolve without first setting the render target!");

			alreadyResolved = true;
			BaseGame.Device.ResolveRenderTarget(0);
		} // Resolve()
		#endregion

		#region Unit Testing
#if DEBUG
		/// <summary>
		/// Test create render to texture
		/// </summary>
		static public void TestCreateRenderToTexture()
		{
			Model testPlate = null;
			RenderToTexture renderToTexture = null;

			TestGame.Start(
				delegate
				{
					testPlate = new Model("CarSelectionPlate");
					renderToTexture = new RenderToTexture(
						//SizeType.ShadowMap1024);
						//.QuarterScreen);
						SizeType.FullScreen);
						//SizeType.HalfScreen);
				},
				delegate
				{
					bool renderToTextureWay =
						Input.Keyboard.IsKeyUp(Keys.Space) &&
						Input.GamePadAPressed == false;
					if (renderToTextureWay)
					{
						// Set render target to our texture
						renderToTexture.SetRenderTarget();

						// Clear background
						renderToTexture.Clear(Color.Blue);

						// Draw background lines
						BaseGame.DrawLine(new Point(0, 200), new Point(200, 0), Color.Blue);
						BaseGame.DrawLine(new Point(0, 0), new Point(400, 400), Color.Red);
						BaseGame.FlushLineManager2D();

						// And draw object
						testPlate.Render(Matrix.CreateScale(1.5f));
						// And flush render manager to draw all objects
						BaseGame.MeshRenderManager.Render();

						// Resolve
						renderToTexture.Resolve();

						// Reset background buffer
						BaseGame.ResetRenderTarget(true);
						
						// Show render target in a rectangle on our screen
						renderToTexture.RenderOnScreen(
							//tst:
							new Rectangle(100, 100, 256, 256));
							//BaseGame.ResolutionRect);
					} // if (renderToTextureWay)
					else
					{
						// Copy backbuffer way, render stuff normally first
						// Clear background
						BaseGame.Device.Clear(Color.Blue);

						// Draw background lines
						BaseGame.DrawLine(new Point(0, 200), new Point(200, 0), Color.Blue);
						BaseGame.DrawLine(new Point(0, 0), new Point(400, 400), Color.Red);
						BaseGame.FlushLineManager2D();

						// And draw object
						testPlate.Render(Matrix.CreateScale(1.5f));
					} // else

					TextureFont.WriteText(2, 30,
						"renderToTexture.Width=" + renderToTexture.Width);
					TextureFont.WriteText(2, 60,
						"renderToTexture.Height=" + renderToTexture.Height);
					TextureFont.WriteText(2, 90,
						"renderToTexture.Valid=" + renderToTexture.Valid);
					TextureFont.WriteText(2, 120,
						"renderToTexture.XnaTexture=" + renderToTexture.XnaTexture);
					TextureFont.WriteText(2, 150,
						"renderToTexture.ZBufferSurface=" + renderToTexture.ZBufferSurface);
					TextureFont.WriteText(2, 180,
						"renderToTexture.Filename=" + renderToTexture.Filename);
				});
		} // TestCreateRenderToTexture()
#endif
		#endregion
	} // class RenderToTexture
} // namespace SpeedyRacer.Shaders
