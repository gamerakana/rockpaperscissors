using UnityEngine;
using System.Collections;
using System;

namespace RPS
{
	public class ChangeGameStateEventArgs : EventArgs
	{
		public ChangeGameStateEventArgs(BaseGameState state)
		{
			this.State = state;
		}

		public BaseGameState State { get; private set; }
	}
}

