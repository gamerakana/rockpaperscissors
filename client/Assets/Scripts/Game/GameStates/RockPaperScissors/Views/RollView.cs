using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using Core;

namespace RPS
{
	/// <summary>
	/// Container class for all views relating to the users' rolls. 
	/// </summary>
	public class RollView : MonoBehaviour
	{
		private const string SHOW = "Show";

		public event EventHandler RollComplete;

		[SerializeField]
		private HandView leftHand;

		[SerializeField]
		private HandView rightHand;

		[SerializeField]
		private Text resultLabel;

		[SerializeField]
		private Animator resultAnimator;

		[SerializeField]
		private AnimationEventDispatcher resultAnimationEventDispatcher;

		private string resultString;

		/// <summary>
		/// Plays the enter animations on both hands.
		/// </summary>
		public void Enter()
		{
			leftHand.Enter();
			rightHand.Enter();
		}

		/// <summary>
		/// Plays the preroll animations on both hands.
		/// </summary>
		public void Preroll()
		{
			leftHand.Preroll();
			rightHand.Preroll();
		}

		/// <summary>
		/// Plays the roll animations on both hands.
		/// </summary>
		public void Roll(RollVO rollVO)
		{
			resultString = GetResultText(rollVO);
			leftHand.ShootComplete += OnShootComplete;
			leftHand.Shoot(rollVO.myShot);
			rightHand.Shoot(rollVO.theirShot);
		}

		/// <summary>
		/// Gets the result text.
		/// </summary>
		/// <returns>The result text.</returns>
		/// <param name="rollVO">Roll V.</param>
		private string GetResultText(RollVO rollVO)
		{
			if(rollVO.result == Globals.RollResults.Won)
			{
				return string.Format("{0} beats {1}!\n\nYou won!", rollVO.myShot, rollVO.theirShot);
			}
			else if(rollVO.result == Globals.RollResults.Lost)
			{
				return string.Format("{0} beats {1}\n\nYou lost!", rollVO.theirShot, rollVO.myShot);
			}
			else
			{
				return "You tied!";
			}
		}

		/// <summary>
		/// Handels the shoot animtion complete event.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="args">Arguments.</param>
		private void OnShootComplete(object sender, EventArgs args)
		{
			leftHand.ShootComplete -= OnShootComplete;
			resultLabel.text = resultString;
			resultAnimationEventDispatcher.Event += OnShootTextComplete;
			resultAnimator.SetTrigger(SHOW);
		}

		/// <summary>
		/// Handles the shoot text animation complete event.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="args">Arguments.</param>
		private void OnShootTextComplete(object sender, AnimationEventArgs args)
		{
			resultAnimationEventDispatcher.Event -= OnShootTextComplete;
			leftHand.ExitComplete += OnHandExitComplete;
			leftHand.Exit();
			rightHand.Exit();
		}

		/// <summary>
		/// Handles the hand exit animation complete event.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="args">Arguments.</param>
		public void OnHandExitComplete(object sender, EventArgs args)
		{
			leftHand.ExitComplete -= OnHandExitComplete;
			EventHandler callback = RollComplete;
			if(callback != null)
				callback(this, args);
		}
	}
}

