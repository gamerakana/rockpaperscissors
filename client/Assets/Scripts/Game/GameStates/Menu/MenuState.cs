using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RPS
{
	/// <summary>
	/// Menu state controller. Manages the view for the main menu.
	/// </summary>
	public class MenuState : BaseGameState
	{
		private const string ASSET_PATH = "States/MenuView";

		public MenuState(GameView targetObject, Dictionary<string, object> stateData) : base(targetObject, stateData) { }

		/// <summary>
		/// Handles the enter event.
		/// </summary>
		/// <param name="lastStateName">Last state name.</param>
		/// <param name="lastStateData">Last state data.</param>
		/// <param name="stateMachine">State machine.</param>
		public override void OnEnter (string lastStateName, Dictionary<string, object> lastStateData, Core.StateMachine.StateMachine<GameView> stateMachine)
		{
			base.OnEnter (lastStateName, lastStateData, stateMachine);
			View.GameModeSelected += OnGameModeSelected;
		}

		/// <summary>
		/// Handles the exit event.
		/// </summary>
		public override void OnExit ()
		{
			base.OnExit ();
			View.GameModeSelected -= OnGameModeSelected;
		}

		/// <summary>
		/// Handles the game mode selected event.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="args">Arguments.</param>
		private void OnGameModeSelected(object sender, BeginGameEventArgs args)
		{
			DispatchChangeStateEvent(new RockPaperScissorsState(args.GameMode, TargetObject, null));
		}

		/// <summary>
		/// Gets the view.
		/// </summary>
		/// <value>The view.</value>
		private MenuView View
		{
			get { return view as MenuView; }
		}

		/// <summary>
		/// Gets the asset path.
		/// </summary>
		/// <value>The asset path.</value>
		protected override string AssetPath 
		{
			get { return ASSET_PATH; }
		}
	}
}

