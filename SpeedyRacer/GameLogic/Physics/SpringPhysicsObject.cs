// Project: SpeedyRacer, File: SpringPhysicsObject.cs
// Namespace: SpeedyRacer.Game.Physics, Class: SpringPhysicsObject
// Path: C:\code\SpeedyRacer\Game\Physics, Author: Abi
// Code lines: 116, Size of file: 3,09 KB
// Creation date: 17.10.2006 23:51
// Last modified: 18.10.2006 00:01
// Generated with Commenter by abi.exDream.com

#region Using directives
using System;
using System.Collections;
using System.Text;
#endregion

namespace SpeedyRacer.GameLogic.Physics
{
	/// <summary>
	/// Simple 1D spring formula, used for the car to let pitch value swing out.
	/// </summary>
	public class SpringPhysicsObject
	{
		#region Constants
		/// <summary>
		/// Default mass, friction and spring values for parameterless constructor
		/// </summary>
		public const float
			DefaultMass = 0.5f,
			DefaultFriction = 0.9f,
			DefaultSpringConstant = 1.0f;
		#endregion

		#region Variables
		/// <summary>
		/// Spring constant
		/// </summary>
		private float mass = DefaultMass;
		/// <summary>
		/// Friction
		/// </summary>
		private float friction = DefaultFriction;
		/// <summary>
		/// Spring constant
		/// </summary>
		private float springConstant = DefaultSpringConstant;

		/// <summary>
		/// The distance of the attached object to the spring object.
		/// </summary>
		public float pos = 0.0f;
		/// <summary>
		/// Velocity of spring attached object
		/// </summary>
		public float velocity = 0.0f;
		/// <summary>
		/// Current force, will be set to -pos to let it swing towards the
		/// spring center (in this class its the center).
		/// </summary>
		public float force = 0.0f;
		#endregion

		#region Constructor
		/// <summary>
		/// Create simple 1D spring with default values.
		/// </summary>
		public SpringPhysicsObject()
		{
		} // SpringPhysicsObject()

		/// <summary>
		/// Create simple 1D spring with specified values.
		/// </summary>
		public SpringPhysicsObject(
			float setMass,
			float setFriction,
			float setSpringConstant,
			float setInitialPos)
		{
			mass = setMass;
			friction = setFriction;
			springConstant = setSpringConstant;
			pos = setInitialPos;
			force = 0;
			velocity = 0;
		} // SpringPhysicsObject(setMass, setFriction, setSpringConstant)
		#endregion

		#region Simulate
		/// <summary>
		/// Simulate spring formula using the timeChange.
		/// The velocity is increased by the timeChange * force / mass,
		/// the position is then timeChange * velocity.
		/// </summary>
		public void Simulate(float timeChange)
		{
			// Calculate force again
			force += -pos * springConstant;
			// Calculate velocity
			velocity = force / mass;
			// And apply it to the current position
			pos += timeChange * velocity;
			// Apply friction
			force *= 1.0f - (timeChange * friction);
		} // Simulate(timeChange)
		#endregion

		#region Change pos
		/// <summary>
		/// Change pos
		/// </summary>
		public void ChangePos(float change)
		{
			pos += change;
		} // ChangePos(change)
		#endregion
	} // class SpringPhysicsObject
} // namespace SpeedyRacer.Game.Physics
