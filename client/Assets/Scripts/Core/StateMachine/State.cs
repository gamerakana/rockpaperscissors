using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.StateMachine
{
	using Data = Dictionary<string, object>;

	public class State<T>
	{		
		// Object to manage
		private T targetObject;

		// State name
		protected string stateName;

		// State data
		protected Data stateData;

		// Tracking last state info
		protected string lastStateName;
		protected Data lastStateData;

		// Our state machine
		protected StateMachine<T> stateMachine;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Core.State`1"/> class.
		/// </summary>
		/// <param name="targetObject">Target object.</param>
		public State(T targetObject, Data stateData)
		{
			this.targetObject = targetObject;
			this.stateData = stateData;
		}
		
		/// <summary>
		/// Handles the enter event.
		/// </summary>
		public virtual void OnEnter(string lastStateName, Data lastStateData, StateMachine<T> stateMachine)
		{
			this.lastStateName = lastStateName;
			this.lastStateData = lastStateData;
			this.stateMachine = stateMachine;
		}

		/// <summary>
		/// Update the state
		/// </summary>
		/// <param name="elapsed">Elapsed.</param>
		public virtual void Update(float elapsed){}
		
		/// <summary>
		/// Handles the exit event.
		/// </summary>
		public virtual void OnExit(){}

		/// <summary>
		/// Handles the destroy event.
		/// </summary>
		public virtual void OnDestroy(){}
		
		/// <summary>
		/// Gets the target object.
		/// </summary>
		/// <value>The target object.</value>
		protected T TargetObject 
		{
			get { return targetObject; }
		}
		
		/// <summary>
		/// Gets the last state.
		/// </summary>
		/// <value>The last state.</value>
		public string LastState 
		{
			get { return lastStateName; }
		}

		/// <summary>
		/// Gets the name of the state.
		/// </summary>
		/// <value>The name of the state.</value>
		public string StateName 
		{
			get { return stateName; }
		}

		/// <summary>
		/// Gets the state data.
		/// </summary>
		/// <value>The state data.</value>
		public Data StateData 
		{
			get { return stateData; }
		}

		/// <summary>
		/// Gets the state machine.
		/// </summary>
		/// <value>The state machine.</value>
		public StateMachine<T> StateMachine 
		{
			get { return stateMachine; }
		}
	}
}

