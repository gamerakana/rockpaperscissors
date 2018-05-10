using UnityEngine;
using System.Collections;
using System;

namespace RPS
{
	/// <summary>
	/// The view for each user's hand. Manages all animations for both hands.
	/// </summary>
	public class HandView : MonoBehaviour
	{
		private const string ENTER = "Enter";
		private const string PRESHOOT = "Preshoot";
		private const string EXIT = "Exit";

		public event EventHandler ShootComplete;
		public event EventHandler ExitComplete;

		[SerializeField]
		private Animator animationController;

		/// <summary>
		/// Plays the enter animation
		/// </summary>
		public void Enter()
		{
			SetTrigger(ENTER);
		}

		/// <summary>
		/// Plays the preroll animation
		/// </summary>
		public void Preroll()
		{
			SetTrigger(PRESHOOT);
		}

		/// <summary>
		/// Plays the shoot animation
		/// </summary>
		/// <param name="shot">Shot.</param>
		public void Shoot(string shot)
		{
			SetTrigger(shot);
		}

		/// <summary>
		/// Plays the exit animation
		/// </summary>
		public void Exit()
		{
			SetTrigger(EXIT);
		}

		/// <summary>
		/// Sets an animation trigger.
		/// </summary>
		/// <param name="trigger">Trigger.</param>
		private void SetTrigger(string trigger)
		{
			animationController.SetTrigger(trigger);
		}

		/// <summary>
		/// Handles the shoot complete event.
		/// </summary>
		private void OnShootComplete()
		{
			EventHandler callback = ShootComplete;
			if(callback != null)
				callback(this, EventArgs.Empty);
		}

		/// <summary>
		/// Handles the exit complete event.
		/// </summary>
		private void OnExitComplete()
		{
			EventHandler callback = ExitComplete;
			if(callback != null)
				callback(this, EventArgs.Empty);
		}
	}
}

