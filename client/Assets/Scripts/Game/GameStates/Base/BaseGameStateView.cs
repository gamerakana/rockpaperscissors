using UnityEngine;
using System.Collections;

namespace RPS
{
	/// <summary>
	/// Base view for any game state.
	/// </summary>
	public class BaseGameStateView : MonoBehaviour
	{		
		/// <summary>
		/// Handles the show event. Called after being displayed.
		/// </summary>
		public virtual void OnShow() { }

		/// <summary>
		/// Releases all resources. Called before Destroy()
		/// </summary>
		public virtual void Dispose() { }
	}
}

