using UnityEngine;
using System.Collections;
using System;

namespace RPS
{
	public class ShotSelectedEventArgs : EventArgs
	{
		public ShotSelectedEventArgs(string shot)
		{
			this.Shot = shot;
		}

		public string Shot { get; private set; }
	}
}