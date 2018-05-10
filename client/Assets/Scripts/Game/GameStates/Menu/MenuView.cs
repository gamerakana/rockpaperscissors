using UnityEngine;
using System.Collections;
using System;

namespace RPS
{
	/// <summary>
	/// View for the menu state.
	/// </summary>
	public class MenuView : BaseGameStateView 
	{
		public event EventHandler<BeginGameEventArgs> GameModeSelected;	

		/// <summary>
		/// Handles the selected single player event.
		/// </summary>
		public void OnSelectedSinglePlayer() 
		{
			DispatchGameModeSelectedEvent(Globals.GameModes.SinglePlayer);
		}

		/// <summary>
		/// Handles the selected multi player event.
		/// </summary>
		public void OnSelectedMultiPlayer()
		{
			DispatchGameModeSelectedEvent(Globals.GameModes.MultiPlayer);
		}

		/// <summary>
		/// Dispatches the game mode selected event.
		/// </summary>
		/// <param name="gameMode">Game mode.</param>
		private void DispatchGameModeSelectedEvent(Globals.GameModes gameMode)
		{
			EventHandler<BeginGameEventArgs> eventHandler = GameModeSelected;
			if(eventHandler != null)
				eventHandler(this, new BeginGameEventArgs(gameMode));
		}
	}
}

