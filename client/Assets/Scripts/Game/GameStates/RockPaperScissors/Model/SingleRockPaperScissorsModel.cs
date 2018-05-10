using UnityEngine;
using System.Collections;

namespace RPS
{
	/// <summary>
	/// The single player RPS model. Manages all npc behavior and win/loss/tie determination in its implementation of the IRockPaperScissorsModel
	/// </summary>
	public class SingleRockPaperScissorsModel : IRockPaperScissorsModel
	{
		#region IRockPaperScissorsModel implementation

		/// <summary>
		/// Attempts to find a match
		/// </summary>
		/// <param name="success">Success.</param>
		/// <param name="failure">Failure.</param>
		public void Match (System.Action success, System.Action<Globals.RPSModelResultCodes> failure)
		{
			// By default we are already matched
			if(success != null)
				success();
		}

		/// <summary>
		/// Attempts to process a user's shot.
		/// </summary>
		/// <param name="shot">Shot.</param>
		/// <param name="success">Success.</param>
		/// <param name="failure">Failure.</param>
		public void Shoot (string shot, System.Action<RollVO> success, System.Action<Globals.RPSModelResultCodes> failure)
		{
			// Random shoot for opponent
			string theirShot = OpponentShoot();

			RollVO rollVO = new RollVO
			{
				myShot = shot,
				theirShot = theirShot,
				result = DetermineResult(shot, theirShot)
			};
		
			if(success != null)
				success(rollVO);
		}

		/// <summary>
		/// Randomly generates an opponents shot
		/// </summary>
		/// <returns>The shoot.</returns>
		private string OpponentShoot()
		{
			float rand = Random.value;
			float oneThird = 1.0f / 3.0f;
			if(rand < oneThird)
				return Globals.Shots.Rock;
			else if (rand < oneThird * 2)
				return Globals.Shots.Paper;
			return Globals.Shots.Scissors;
		}

		/// <summary>
		/// Determines the result of a roll.
		/// </summary>
		/// <returns>The result.</returns>
		/// <param name="myShot">My shot.</param>
		/// <param name="theirShot">Their shot.</param>
		private string DetermineResult(string myShot, string theirShot)
		{
			if(myShot == theirShot)
			{
				++Ties;
				return Globals.RollResults.Tied;
			}
			else if(myShot == Globals.Shots.Rock)
			{
				if(theirShot == Globals.Shots.Paper)
				{
					++Losses;
					return Globals.RollResults.Lost;
				}
				else
				{
					++Wins;
					return Globals.RollResults.Won;
				}
			}
			else if(myShot == Globals.Shots.Paper)
			{
				if(theirShot == Globals.Shots.Scissors)
				{
					++Losses;
					return Globals.RollResults.Lost;
				}
				else
				{
					++Wins;
					return Globals.RollResults.Won;
				}
			}
			else
			{
				if(theirShot == Globals.Shots.Rock)
				{
					++Losses;
					return Globals.RollResults.Lost;
				}
				else
				{
					++Wins;
					return Globals.RollResults.Won;
				}
			}
		}

		/// <summary>
		/// Quits the game
		/// </summary>
		public void Quit () { }

		/// <summary>
		/// Gets or sets the number of wins.
		/// </summary>
		/// <value>The wins.</value>
		public int Wins { get; set; }

		/// <summary>
		/// Gets or sets the number of losses.
		/// </summary>
		/// <value>The losses.</value>
		public int Losses { get; set; }

		/// <summary>
		/// Gets or sets the number of ties.
		/// </summary>
		/// <value>The ties.</value>
		public int Ties { get; set; }

		#endregion
	}
}

