using UnityEngine;
using System.Collections;
using System;

namespace RPS
{
	/// <summary>
	/// View for displaying and selecting of the user's shot.
	/// </summary>
	public class ShotSelectionView : MonoBehaviour
	{
		private const string ENTER = "Enter";
		private const string EXIT = "Exit";

		public event EventHandler<ShotSelectedEventArgs> ShotSelected;

		private string selectedShot = null;

		[SerializeField]
		private Animator animationController;

		/// <summary>
		/// Plays the enter animation
		/// </summary>
		public void Show()
		{
			animationController.SetTrigger(ENTER);
		}

		/// <summary>
		/// Handles the shot selected event.
		/// </summary>
		/// <param name="shot">Shot.</param>
		public void OnShotSelected(string shot)
		{
			// Prevent repeat shots.
			if(!string.IsNullOrEmpty(selectedShot))
				return;

			selectedShot = shot;
			animationController.SetTrigger(EXIT);
		}

		/// <summary>
		/// Handles the exit complete event.
		/// </summary>
		private void OnExitComplete()
		{
			EventHandler<ShotSelectedEventArgs> callback = ShotSelected;
			if(callback != null)
				callback(this, new ShotSelectedEventArgs(selectedShot));
			selectedShot = null;
		}
	}
}
