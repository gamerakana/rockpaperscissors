using UnityEngine;
using System.Collections;
using System;

namespace Core
{
	public class AnimationEventArgs : EventArgs
	{
		public AnimationEventArgs(string eventName)
		{
			this.EventName = eventName;
		}

		public string EventName { get; private set; }
	}
}