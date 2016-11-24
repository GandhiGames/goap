﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public sealed class GoapAgent : MonoBehaviour
{

	private GoapFSM m_StateMachine;

	private GoapFSM.StateDel m_IdleState;
	// finds something to do
	private GoapFSM.StateDel m_MoveToState;
	// moves to a target
	private GoapFSM.StateDel m_PerformActionState;
	// performs an action

	private HashSet<GoapAction> m_AvailableActions;
	private Queue<GoapAction> m_CurrentActions;

	private Goap m_DataProvider;
	// this is the implementing class that provides our world data and listens to feedback on planning

	private GoapPlanner planner;


	void Start ()
	{
		m_StateMachine = new GoapFSM ();
		m_AvailableActions = new HashSet<GoapAction> ();
		m_CurrentActions = new Queue<GoapAction> ();
		planner = new GoapPlanner ();
		FindDataProvider ();
		CreateIdleState ();
		CreateMoveToState ();
		CreatePerformActionState ();
		m_StateMachine.pushState (m_IdleState);
		LoadActions ();
	}


	void Update ()
	{
		m_StateMachine.Update (this.gameObject);
	}
		
	public void AddAction (GoapAction a)
	{
		m_AvailableActions.Add (a);
	}

	public GoapAction GetAction (Type action)
	{
		foreach (GoapAction g in m_AvailableActions) {
			if (g.GetType ().Equals (action))
				return g;
		}
		return null;
	}

	public void RemoveAction (GoapAction action)
	{
		m_AvailableActions.Remove (action);
	}

	private bool HasActionPlan ()
	{
		return m_CurrentActions.Count > 0;
	}

	private void CreateIdleState ()
	{
		m_IdleState = (fsm, gameObj) => {
			// GOAP planning

			// get the world state and the goal we want to plan for
			Dictionary<string,object> worldState = m_DataProvider.GetWorldState ();
			Dictionary<string,object> goal = m_DataProvider.CreateGoalState ();

			// Plan
			Queue<GoapAction> plan = planner.Plan (gameObject, m_AvailableActions, worldState, goal);

			if (plan != null) {
				// we have a plan, hooray!
				m_CurrentActions = plan;
				m_DataProvider.PlanFound (goal, plan);

				fsm.popState (); // move to PerformAction state
				fsm.pushState (m_PerformActionState);

			} else {
				// ugh, we couldn't get a plan
				Debug.Log ("<color=orange>Failed Plan:</color>" + PrettyPrint (goal));
				m_DataProvider.PlanFailed (goal);
				fsm.popState (); // move back to IdleAction state
				fsm.pushState (m_IdleState);
			}

		};
	}

	private void CreateMoveToState ()
	{
		m_MoveToState = (fsm, gameObj) => {
			// move the game object

			GoapAction action = m_CurrentActions.Peek ();
			if (action.RequiresInRange () && action.target == null) {
				Debug.Log ("<color=red>Fatal error:</color> Action requires a target but has none. Planning failed. You did not assign the target in your Action.checkProceduralPrecondition()");
				fsm.popState (); // move
				fsm.popState (); // perform
				fsm.pushState (m_IdleState);
				return;
			}

			// get the agent to move itself
			if (m_DataProvider.MoveAgent (action)) {
				fsm.popState ();
			}
		};
	}

	private void CreatePerformActionState ()
	{

		m_PerformActionState = (fsm, gameObj) => {
			// perform the action

			if (!HasActionPlan ()) {
				// no actions to perform
				Debug.Log ("<color=red>Done actions</color>");
				fsm.popState ();
				fsm.pushState (m_IdleState);
				m_DataProvider.ActionsFinished ();
				return;
			}

			GoapAction action = m_CurrentActions.Peek ();
			if (action.IsDone ()) {
				// the action is done. Remove it so we can perform the next one
				m_CurrentActions.Dequeue ();
			}

			if (HasActionPlan ()) {
				// perform the next action
				action = m_CurrentActions.Peek ();
				bool inRange = action.RequiresInRange () ? action.IsInRange () : true;

				if (inRange) {
					// we are in range, so perform the action
					bool success = action.Perform (gameObj);

					if (!success) {
						// action failed, we need to plan again
						fsm.popState ();
						fsm.pushState (m_IdleState);
						m_DataProvider.PlanAborted (action);
					}
				} else {
					// we need to move there first
					// push moveTo state
					fsm.pushState (m_MoveToState);
				}

			} else {
				// no actions left, move to Plan state
				fsm.popState ();
				fsm.pushState (m_IdleState);
				m_DataProvider.ActionsFinished ();
			}

		};
	}

	private void FindDataProvider ()
	{
		foreach (Component comp in gameObject.GetComponents(typeof(Component))) {
			if (typeof(Goap).IsAssignableFrom (comp.GetType ())) {
				m_DataProvider = (Goap)comp;
				return;
			}
		}
	}

	private void LoadActions ()
	{
		GoapAction[] actions = gameObject.GetComponents<GoapAction> ();
		foreach (GoapAction a in actions) {
			m_AvailableActions.Add (a);
		}
		Debug.Log ("Found actions: " + PrettyPrint (actions));
	}

	public static string PrettyPrint (Dictionary<string,object> state)
	{
		String s = "";
		foreach (KeyValuePair<string,object> kvp in state) {
			s += kvp.Key + ":" + kvp.Value.ToString ();
			s += ", ";
		}
		return s;
	}

	public static string PrettyPrint (Queue<GoapAction> actions)
	{
		String s = "";
		foreach (GoapAction a in actions) {
			s += a.GetType ().Name;
			s += "-> ";
		}
		s += "GOAL";
		return s;
	}

	public static string PrettyPrint (GoapAction[] actions)
	{
		String s = "";
		foreach (GoapAction a in actions) {
			s += a.GetType ().Name;
			s += ", ";
		}
		return s;
	}

	public static string PrettyPrint (GoapAction action)
	{
		String s = "" + action.GetType ().Name;
		return s;
	}
}
