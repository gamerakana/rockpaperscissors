using UnityEngine;
using System.Collections;

namespace RPS
{
	public static class Globals
	{
		public enum GameModes
		{
			SinglePlayer,
			MultiPlayer
		}

		public enum RPSModelResultCodes
		{
			Success,
			NetworkError,
			GenericError,
			InvalidReturnData,
			Timeout,
			OpponentQuit
		}

		public static class Shots
		{
			public const string None = "None";
			public const string Rock = "Rock";
			public const string Paper = "Paper";
			public const string Scissors = "Scissors";
		}

		public static class RollResults
		{
			public const string Pending = "Pending";
			public const string Tied = "Tied";
			public const string Won = "Won";
			public const string Lost = "Lost";
		}			
	}
}

