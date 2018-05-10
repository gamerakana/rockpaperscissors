using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace RPS
{
	/// <summary>
	/// Main game controller for RPS. Manages the model and view.
	/// </summary>
	public class RockPaperScissorsState : BaseGameState
	{
		private const string ASSET_PATH = "States/RockPaperScissorsView";

		private IRockPaperScissorsModel model;

		/// <summary>
		/// Initializes a new instance of the <see cref="RPS.RockPaperScissorsState"/> class.
		/// </summary>
		/// <param name="gameMode">Game mode.</param>
		/// <param name="targetObject">Target object.</param>
		/// <param name="stateData">State data.</param>
		public RockPaperScissorsState(Globals.GameModes gameMode, GameView targetObject, Dictionary<string, object> stateData) : base(targetObject, stateData) 
		{ 			
			// Create our model based on game mode
			model = (gameMode == Globals.GameModes.SinglePlayer ? model = new SingleRockPaperScissorsModel() : new MultiRockPaperScissorsModel()) as IRockPaperScissorsModel;
		}

		/// <summary>
		/// Handles the enter event.
		/// </summary>
		/// <param name="lastStateName">Last state name.</param>
		/// <param name="lastStateData">Last state data.</param>
		/// <param name="stateMachine">State machine.</param>
		public override void OnEnter (string lastStateName, Dictionary<string, object> lastStateData, Core.StateMachine.StateMachine<GameView> stateMachine)
		{
			base.OnEnter (lastStateName, lastStateData, stateMachine);

			View.Quit += OnQuit;

			// Attempt to match
			model.Match(OnMatchSuccess, OnMatchFailed);
		}			

		/// <summary>
		/// Handles the match success event.
		/// </summary>
		private void OnMatchSuccess()
		{
			// Let the user select their weapon
			View.ShowSelectionView();

			// Add all view listeners
			View.ShotSelected += OnShotSelected;
			View.RollCompleted += OnRollCompleted;
		}			

		/// <summary>
		/// Handles the match failed event.
		/// </summary>
		private void OnMatchFailed(Globals.RPSModelResultCodes result)
		{
			View.ShowFailure(GetFailureMessage(result));
		}

		/// <summary>
		/// Gets the failure message.
		/// </summary>
		/// <returns>The failure message.</returns>
		/// <param name="result">Result.</param>
		private string GetFailureMessage(Globals.RPSModelResultCodes result)
		{
			switch(result)
			{
			case Globals.RPSModelResultCodes.GenericError:
				return "An error has ocurred. Please try again later.";
			case Globals.RPSModelResultCodes.InvalidReturnData:
				return "Unexpected return data. Please try again later.";
			case Globals.RPSModelResultCodes.NetworkError:
				return "A network error has ocurred. Please check your network and try again.";
			case Globals.RPSModelResultCodes.OpponentQuit:
				return "Your opponent has left the game.";
			case Globals.RPSModelResultCodes.Timeout:
				return "Request timed out. Please try again later.";
			}

			return "An unknown error has ocurred. Please try again later.";
		}

		/// <summary>
		/// Handles the exit event.
		/// </summary>
		public override void OnExit ()
		{
			base.OnExit ();
			View.ShotSelected -= OnShotSelected;
			View.RollCompleted -= OnRollCompleted;
			View.Quit -= OnQuit;
		}

		/// <summary>
		/// Handles the shot selected event.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="args">Arguments.</param>
		private void OnShotSelected(object sender, ShotSelectedEventArgs args)
		{
			View.Enter();
			View.Preroll();
			model.Shoot(args.Shot, OnRollSuccess, OnRollFailure);
		}

		/// <summary>
		/// Handles the roll success event.
		/// </summary>
		/// <param name="rollVO">Roll V.</param>
		private void OnRollSuccess(RollVO rollVO)
		{
			View.Roll(rollVO);
		}

		/// <summary>
		/// Handles the roll failure event.
		/// </summary>
		private void OnRollFailure(Globals.RPSModelResultCodes result)
		{
			View.ShowFailure(GetFailureMessage(result));
		}

		/// <summary>
		/// Handles the roll completed event.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="args">Arguments.</param>
		private void OnRollCompleted(object sender, EventArgs args)
		{
			View.UpdateTally(model.Wins, model.Losses, model.Ties);
		}

		/// <summary>
		/// Handles the quit event.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="args">Arguments.</param>
		private void OnQuit(object sender, EventArgs args)
		{
			ReturnToMenu();
		}

		/// <summary>
		/// Handles the application quit event by leaving the current game.
		/// </summary>
		public override void OnApplicationQuit ()
		{
			base.OnApplicationQuit ();
			ReturnToMenu();
		}

		/// <summary>
		/// Returns to the main menu.
		/// </summary>
		private void ReturnToMenu()
		{
			model.Quit();
			DispatchChangeStateEvent(new MenuState(TargetObject, null));
		}
			
		private RockPaperScissorsView View
		{
			get { return view as RockPaperScissorsView; }
		}

		protected override string AssetPath 
		{
			get { return ASSET_PATH; }
		}
	}
}

