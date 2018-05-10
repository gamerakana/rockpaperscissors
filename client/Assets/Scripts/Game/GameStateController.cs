using UnityEngine;
using System.Collections;
using Core.StateMachine;

namespace RPS
{
	/// <summary>
	/// Main controller for the entire game. The container for all game states.
	/// </summary>
	public class GameStateController : StateMachine<GameView>
	{
		[SerializeField]
		private GameView gameView;

		/// <summary>
		/// Handles the awake call
		/// </summary>
		void Awake() 
		{
			ChangeState(new LoadingState(gameView, null));
		}

		/// <summary>
		/// Changes the state.
		/// </summary>
		/// <param name="newState">New state.</param>
		public override void ChangeState (State<GameView> newState)
		{
			if(CurrentGameState != null)
				CurrentGameState.ChangeState -= OnChangeState;
			base.ChangeState (newState);
			if(CurrentGameState != null)
				CurrentGameState.ChangeState += OnChangeState;
		}

		/// <summary>
		/// Handles the application quit event.
		/// </summary>
		public void OnApplicationQuit()
		{
			if(CurrentGameState != null)
				CurrentGameState.OnApplicationQuit();
		}

		/// <summary>
		/// Handles the application focus event.
		/// </summary>
		/// <param name="hasFocus">If set to <c>true</c> has focus.</param>
		public void OnApplicationFocus(bool hasFocus)
		{
			if(CurrentGameState != null)
				CurrentGameState.OnApplicationFocus(hasFocus);
		}

		/// <summary>
		/// Handles the change state event.
		/// </summary>
		/// <param name="args">Arguments.</param>
		private void OnChangeState(object sender, ChangeGameStateEventArgs args)
		{
			ChangeState(args.State);
		}

		private BaseGameState CurrentGameState
		{
			get { return CurrentState as BaseGameState; }
		}
	}
}

