using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

namespace RPS
{
	/// <summary>
	/// Main view for RPS. Manages all subviews of the game itself.
	/// </summary>
	public class RockPaperScissorsView : BaseGameStateView 
	{ 
		public event EventHandler<ShotSelectedEventArgs> ShotSelected;
		public event EventHandler RollCompleted;
		public event EventHandler Quit;

		[SerializeField]
		private RollView rollView;

		[SerializeField]
		private ShotSelectionView shotSelectionView;

		[SerializeField]
		private GameObject lookingForMatchView;

		[SerializeField]
		private Text tallyLabel;

		[SerializeField]
		private Text failureLabel;

		/// <summary>
		/// Handles the show event. Called after being displayed.
		/// </summary>
		public override void OnShow ()
		{
			base.OnShow();

			failureLabel.text = string.Empty;

			if(!lookingForMatchView.activeSelf)
				lookingForMatchView.SetActive(true);

			UpdateTally(0, 0, 0);
		}

		/// <summary>
		/// Shows the selection view.
		/// </summary>
		public void ShowSelectionView()
		{
			if(lookingForMatchView.activeSelf)
				lookingForMatchView.SetActive(false);

			shotSelectionView.ShotSelected += OnShotSelected;
			shotSelectionView.Show();
		}

		/// <summary>
		/// Updates the tally.
		/// </summary>
		/// <param name="wins">Wins.</param>
		/// <param name="losses">Losses.</param>
		/// <param name="ties">Ties.</param>
		public void UpdateTally(int wins, int losses, int ties)
		{
			tallyLabel.text = string.Format("{0}-{1}-{2}", wins, losses, ties);
		}

		/// <summary>
		/// Plays the enter animation
		/// </summary>
		public void Enter()
		{
			rollView.Enter();
		}

		/// <summary>
		/// Plays the preroll animation
		/// </summary>
		public void Preroll()
		{
			rollView.Preroll();
		}

		/// <summary>
		/// Plays the roll animation
		/// </summary>
		public void Roll(RollVO rollVO)
		{
			rollView.RollComplete += OnRollComplete;
			rollView.Roll(rollVO);
		}

		/// <summary>
		/// Handles the roll complete event.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="args">Arguments.</param>
		private void OnRollComplete(object sender, EventArgs args)
		{
			rollView.RollComplete -= OnRollComplete;
			ShowSelectionView();

			EventHandler callback = RollCompleted;
			if(callback != null)
				callback(this, args);
		}

		/// <summary>
		/// Shows a failure message
		/// </summary>
		/// <param name="message">Message.</param>
		public void ShowFailure (string message) 
		{
			failureLabel.text = message;

			if(lookingForMatchView.activeSelf)
				lookingForMatchView.SetActive(false);

			if(rollView.gameObject.activeSelf)
				rollView.gameObject.SetActive(false);

			if(shotSelectionView.gameObject.activeSelf)
				shotSelectionView.gameObject.SetActive(false);

			if(tallyLabel.gameObject.activeSelf)
				tallyLabel.gameObject.SetActive(false);
		}

		/// <summary>
		/// Releases all resources. Called before Destroy()
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="RPS.RockPaperScissorsView"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="RPS.RockPaperScissorsView"/> in an unusable state. After
		/// calling <see cref="Dispose"/>, you must release all references to the <see cref="RPS.RockPaperScissorsView"/> so
		/// the garbage collector can reclaim the memory that the <see cref="RPS.RockPaperScissorsView"/> was occupying.</remarks>
		public override void Dispose ()
		{
			base.Dispose ();
			shotSelectionView.ShotSelected -= OnShotSelected;
		}

		/// <summary>
		/// Handles the shot selected event.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="args">Arguments.</param>
		private void OnShotSelected(object sender, ShotSelectedEventArgs args)
		{
			shotSelectionView.ShotSelected -= OnShotSelected;
			EventHandler<ShotSelectedEventArgs> callback = ShotSelected;
			if(callback != null)
				callback(this, args);
		}

		/// <summary>
		/// Handles the quit event.
		/// </summary>
		public void OnQuit()
		{
			EventHandler callback = Quit;
			if(callback != null)
				callback(this, EventArgs.Empty);
		}
	
	}
}

