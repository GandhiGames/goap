using System;
using System.Collections.Generic;
using UnityEngine;

/**
 * Plans what actions can be completed in order to fulfill a goal state.
 */
public class GoapPlanner
{

	/**
	 * Plan what sequence of actions can fulfill the goal.
	 * Returns null if a plan could not be found, or a list of the actions
	 * that must be performed, in order, to fulfill the goal.
	 */
	public Queue<GoapAction> Plan (HashSet<GoapAction> availableActions, 
	                               Dictionary<string,object> worldState, 
	                               Dictionary<string,object> goal)
	{
		// reset the actions so we can start fresh with them
		foreach (GoapAction a in availableActions) {
			a.Reset ();
		}

		// check what actions can run using their checkProceduralPrecondition
		HashSet<GoapAction> usableActions = new HashSet<GoapAction> ();
		foreach (GoapAction a in availableActions) {
			if (a.CheckProceduralPrecondition ()) {
				usableActions.Add (a);
			}
		}

		// we now have all actions that can run, stored in usableActions

		// build up the tree and record the leaf nodes that provide a solution to the goal.
		List<GoapNode> leaves = new List<GoapNode> ();

		// build graph
		GoapNode start = new GoapNode (null, 0, worldState, null);
		bool success = BuildGraph (start, leaves, usableActions, goal);

		if (!success) {
			// oh no, we didn't get a plan
			return null;
		}

		// get the cheapest leaf
		GoapNode cheapest = null;
		foreach (GoapNode leaf in leaves) {
			if (cheapest == null) {
				cheapest = leaf;
			} else {
				if (leaf.runningCost < cheapest.runningCost) {
					cheapest = leaf;
				}
			}
		}

		// get its node and work back through the parents
		List<GoapAction> result = new List<GoapAction> ();
		GoapNode n = cheapest;
		while (n != null) {
			if (n.action != null) {
				result.Insert (0, n.action); // insert the action in the front
			}
			n = n.parent;
		}
		// we now have this action list in correct order

		Queue<GoapAction> queue = new Queue<GoapAction> ();
		foreach (GoapAction a in result) {
			queue.Enqueue (a);
		}

		// hooray we have a plan!
		return queue;
	}

	/**
	 * Returns true if at least one solution was found.
	 * The possible paths are stored in the leaves list. Each leaf has a
	 * 'runningCost' value where the lowest cost will be the best action
	 * sequence.
	 */
	private bool BuildGraph (GoapNode parent, List<GoapNode> leaves, HashSet<GoapAction> usableActions, Dictionary<string, object> goal)
	{
		bool foundOne = false;

		// go through each action available at this node and see if we can use it here
		foreach (GoapAction action in usableActions) {

			// if the parent state has the conditions for this action's preconditions, we can use it here
			if (InState (action.preconditions, parent.state)) {
				// apply the action's effects to the parent state
				Dictionary<string,object> currentState = PopulateState (parent.state, action.effects);
				//Debug.Log(GoapAgent.prettyPrint(currentState));
				GoapNode node = new GoapNode (parent, parent.runningCost + action.cost, currentState, action);

				if (InState (goal, currentState)) {
					// we found a solution!
					leaves.Add (node);
					foundOne = true;
				} else {
					// not at a solution yet, so test all the remaining actions and branch out the tree
					HashSet<GoapAction> subset = ActionSubset (usableActions, action);
					bool found = BuildGraph (node, leaves, subset, goal);

					if (found) {
						foundOne = true;
					}
				}
			}
		}

		return foundOne;
	}

	/**
	 * Create a subset of the actions excluding the removeMe one. Creates a new set.
	 */
	private HashSet<GoapAction> ActionSubset (HashSet<GoapAction> actions, GoapAction removeMe)
	{
		HashSet<GoapAction> subset = new HashSet<GoapAction> ();
		foreach (GoapAction a in actions) {
			if (!a.Equals (removeMe))
				subset.Add (a);
		}
		return subset;
	}

	/**
	 * Check that all items in 'test' are in 'state'. If just one does not match or is not there
	 * then this returns false.
	 */
	private bool InState (Dictionary<string,object> test, Dictionary<string,object> state)
	{
		foreach (var t in test) {

			object valueObj;

			bool containsKey = state.TryGetValue (t.Key, out valueObj);

			if (!containsKey) {
				return false;
			}

			if (!valueObj.Equals (t.Value)) {
				return false;
			}	
		}

		return true;

		/*
		bool allMatch = true;
		foreach (KeyValuePair<string,object> t in test) {
			bool match = false;

			foreach (KeyValuePair<string,object> s in state) {
				if (s.Equals (t)) {
					match = true;
					break;
				}
			}

			if (!match) {
				allMatch = false;
			}
		}
		return allMatch;
		*/
	}

	/**
	 * Apply the stateChange to the currentState
	 */
	private Dictionary<string,object> PopulateState (Dictionary<string,object> currentState, Dictionary<string,object> stateChange)
	{
		var state = new Dictionary<string,object> (currentState);

		// copy the KVPs over as new objects
		/*
		foreach (KeyValuePair<string,object> s in currentState) {
			state.Add (s.Key, s.Value);
		}
		*/

		foreach (var change in stateChange) {
			if (state.ContainsKey (change.Key)) {
				state.Remove (change.Key);
			} 

			state.Add (change.Key, change.Value);

		}

		/*
		foreach (KeyValuePair<string,object> change in stateChange) {
			// if the key exists in the current state, update the Value
			bool exists = false;

			foreach (KeyValuePair<string,object> s in state) {
				if (s.Equals (change)) {
					exists = true;
					break;
				}
			}

			if (exists) {
				state.RemoveWhere ((KeyValuePair<string,object> kvp) => {
					return kvp.Key.Equals (change.Key);
				});
				KeyValuePair<string, object> updated = new KeyValuePair<string, object> (change.Key, change.Value);
				state.Add (updated);
			}
			// if it does not exist in the current state, add it
			else {
				state.Add (new KeyValuePair<string, object> (change.Key, change.Value));
			}
		}
		*/

		return state;
	}

	/**
	 * Used for building up the graph and holding the running costs of actions.
	 */
	private class GoapNode
	{
		public GoapNode parent;
		public float runningCost;
		public Dictionary<string,object> state;
		public GoapAction action;

		public GoapNode (GoapNode parent, float runningCost, Dictionary<string,object> state, GoapAction action)
		{
			this.parent = parent;
			this.runningCost = runningCost;
			this.state = state;
			this.action = action;
		}
	}

}


