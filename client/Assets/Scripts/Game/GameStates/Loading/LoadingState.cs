using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RPS
{
	/// <summary>
	/// Loading state controller. Manages the loading view.
	/// </summary>
	public class LoadingState : BaseGameState
	{
		private const string ASSET_PATH = "States/LoadingView";

		public LoadingState(GameView targetObject, Dictionary<string, object> stateData) : base(targetObject, stateData) { }

		/// <summary>
		/// Handles the enter event.
		/// </summary>
		/// <param name="lastStateName">Last state name.</param>
		/// <param name="lastStateData">Last state data.</param>
		/// <param name="stateMachine">State machine.</param>
		public override void OnEnter (string lastStateName, Dictionary<string, object> lastStateData, Core.StateMachine.StateMachine<GameView> stateMachine)
		{
			base.OnEnter (lastStateName, lastStateData, stateMachine);
			Load();
		}

		/// <summary>
		/// Performs any necessary loading before entering the menu state.
		/// </summary>
		private void Load()
		{
			// Since we don't actually have any loading to do, just wait a second before moving to the next state
			LeanTween.delayedCall(1.0f, () => {
				DispatchChangeStateEvent(new MenuState(TargetObject, null));
			});
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

