using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.StateMachine
{
	using Data = Dictionary<string, object>;

	public class StateMachine<T> : MonoBehaviour 
	{
		public const string EMPTY_STATE = "none";

		// Target to manage
		private T targetObject;

		// Current state
		private State<T> currentState;
		
		/// <summary>
		/// Changes the state.
		/// </summary>
		/// <param name="newState">New state.</param>
		public virtual void ChangeState(State<T> newState)
		{
			// For passing to new state
			string lastState = EMPTY_STATE;
			Data lastStateData = null;

			// Cleanup current
			if(currentState != null)
			{
				lastState = currentState.StateName;
				lastStateData = currentState.StateData;
				currentState.OnExit();
				currentState.OnDestroy();
				currentState = null;
			}

			// Set up new
			if(newState != null)
			{
				currentState = newState;
				currentState.OnEnter(lastState, lastStateData, this);
			}
		}

		/// <summary>
		/// Update this instance.
		/// </summary>
		void Update()
		{
			if(currentState != null)
			{
				currentState.Update(Time.deltaTime);
			}
		}
		
		/// <summary>
		/// Gets the name of the current state.
		/// </summary>
		/// <value>The name of the current state.</value>
		public string CurrentStateName 
		{
			get { return currentState.StateName; }
		}
		
		/// <summary>
		/// Gets the current state
		/// </summary>
		/// <value>The name of the current state.</value>
		public State<T> CurrentState
		{
			get { return currentState; }
		}
	}
}

