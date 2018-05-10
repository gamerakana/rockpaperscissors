using UnityEngine;
using System.Collections;
using Core.StateMachine;
using System.Collections.Generic;
using System;
using Core.Log;

namespace RPS
{
	/// <summary>
	/// Acts as a controller for any game state.
	/// </summary>
	public abstract class BaseGameState : State<GameView>
	{		
		public event EventHandler<ChangeGameStateEventArgs> ChangeState;

		protected BaseGameStateView view;

		public BaseGameState(GameView targetObject, Dictionary<string, object> stateData) : base(targetObject, stateData) { }

		/// <summary>
		/// Handles the enter event.
		/// </summary>
		/// <param name="lastStateName">Last state name.</param>
		/// <param name="lastStateData">Last state data.</param>
		/// <param name="stateMachine">State machine.</param>
		public override void OnEnter (string lastStateName, Dictionary<string, object> lastStateData, StateMachine<GameView> stateMachine)
		{
			base.OnEnter (lastStateName, lastStateData, stateMachine);

			if(!string.IsNullOrEmpty(AssetPath))
				LoadAndShowView(AssetPath);
		}
			
		/// <summary>
		/// Handles the destroy event.
		/// </summary>
		public override void OnDestroy ()
		{
			base.OnDestroy ();
			view.Dispose();
			GameObject.Destroy(view.gameObject);
		}

		/// <summary>
		/// Handles the application quit event.
		/// </summary>
		public virtual void OnApplicationQuit() { }

		/// <summary>
		/// Handles the application focus event.
		/// </summary>
		/// <param name="hasFocus">If set to <c>true</c> has focus.</param>
		public virtual void OnApplicationFocus(bool hasFocus) { }

		/// <summary>
		/// Loads and show's the view
		/// </summary>
		/// <param name="path">Path.</param>
		protected void LoadAndShowView (string path) 
		{ 
			if(string.IsNullOrEmpty(path))
			{
				SimpleLogger.LogError(this, "Attempting to load view at invalid path.");
				return;
			}

			GameObject prefab = Resources.Load<GameObject>(path);
			if(prefab == null)
			{
				SimpleLogger.LogError(this, "Attempting to load view at invalid path.");
				return;
			}

			view = GameObject.Instantiate(prefab).GetComponent<BaseGameStateView>();
			if(view == null)
			{
				SimpleLogger.LogError(this, "View is missing the required component.");
				return;
			}

			TargetObject.ShowView(view);
		}

		/// <summary>
		/// Dispatches the change state event.
		/// </summary>
		/// <param name="state">State.</param>
		protected void DispatchChangeStateEvent(BaseGameState state)
		{
			EventHandler<ChangeGameStateEventArgs> eventHandler = ChangeState;
			if(eventHandler != null)
				eventHandler(this, new ChangeGameStateEventArgs(state));
		}

		/// <summary>
		/// Gets the asset path.
		/// </summary>
		/// <value>The asset path.</value>
		protected abstract string AssetPath
		{
			get;
		}
	}
}

