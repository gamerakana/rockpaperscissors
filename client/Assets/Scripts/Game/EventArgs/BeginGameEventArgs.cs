using UnityEngine;
using System.Collections;
using System;

namespace RPS
{
	public class BeginGameEventArgs : EventArgs
	{
		public BeginGameEventArgs(Globals.GameModes gameMode)
		{
			this.GameMode = gameMode;
		}

		public Globals.GameModes GameMode { get; private set; }
	}
}

