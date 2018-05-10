using UnityEngine;
using System.Collections;
using System;

namespace RPS
{
	/// <summary>
	/// The value object containing the result of a roll.
	/// </summary>
	[Serializable]
	public class RollVO
	{
		public string myShot = string.Empty;

		public string theirShot = string.Empty;

		public string result = Globals.RollResults.Pending;
	}
}

