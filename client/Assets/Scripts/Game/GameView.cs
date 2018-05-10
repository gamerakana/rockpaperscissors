using UnityEngine;
using System.Collections;
using Core;

namespace RPS
{
	/// <summary>
	/// The main game container view. All game state views become children of this view.
	/// </summary>
	public class GameView : MonoBehaviour
	{
		/// <summary>
		/// Shows a game state view.
		/// </summary>
		/// <param name="gameStateView">Game state view.</param>
		public void ShowView(BaseGameStateView gameStateView)
		{
			gameStateView.transform.SetParent(transform, false);
			gameStateView.transform.SetAsLastSibling();
			gameStateView.gameObject.SetActive(true);
			gameStateView.OnShow();
		}
	}
}