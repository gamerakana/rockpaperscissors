using UnityEngine;
using System.Collections;
using System;

namespace RPS
{
	/// <summary>
	/// RPS model interface. Core definition for any RPS models.
	/// </summary>
	public interface IRockPaperScissorsModel
	{
		/// <summary>
		/// Attempts to find a match
		/// </summary>
		/// <param name="success">Success.</param>
		/// <param name="failure">Failure.</param>
		void Match(Action success, Action<Globals.RPSModelResultCodes> failure);

		/// <summary>
		/// Attempts to process a user's shot.
		/// </summary>
		/// <param name="shot">Shot.</param>
		/// <param name="success">Success.</param>
		/// <param name="failure">Failure.</param>
		void Shoot(string shot, Action<RollVO> success, Action<Globals.RPSModelResultCodes> failure);

		/// <summary>
		/// Quits the game
		/// </summary>
		void Quit();

		/// <summary>
		/// Gets or sets the number of wins.
		/// </summary>
		/// <value>The wins.</value>
		int Wins { get; set; }

		/// <summary>
		/// Gets or sets the number of losses.
		/// </summary>
		/// <value>The losses.</value>
		int Losses { get; set; }

		/// <summary>
		/// Gets or sets the number of ties.
		/// </summary>
		/// <value>The ties.</value>
		int Ties { get; set; }
	}
}

