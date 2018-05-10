using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Core
{
	public class AnimationEventDispatcher : MonoBehaviour
	{
		public event EventHandler<AnimationEventArgs> Event;

		public void DispatchEvent(string eventName)
		{			
			EventHandler<AnimationEventArgs> callback = Event;
			if(callback != null)
				callback(this, new AnimationEventArgs(eventName));
		}
	}
}

