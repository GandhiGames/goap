﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class GoapAction
{
	/* The cost of performing the action. 
	 * Figure out a weight that suits the action. 
	 * Changing it will affect what actions are chosen during planning.*/
	public float cost = 1f;

	/**
	 * An action often has to perform on an object. This is that object. Can be null. */
	public GameObject target;

	public Dictionary<string, object> preconditions {
		get {
			return m_Preconditions;
		}
	}

	public Dictionary<string, object> effects {
		get {
			return m_Effects;
		}
	}

	private Dictionary<string, object> m_Preconditions;
	private Dictionary<string, object> m_Effects;

	private bool m_InRange = false;

	public GoapAction ()
	{
		m_Preconditions = new Dictionary<string, object> ();
		m_Effects = new Dictionary<string, object> ();
	}

	/**
	 * Reset any variables that need to be reset before planning happens again.
	 */
	public virtual void Reset ()
	{
		m_InRange = false;
		target = null;
		Reset ();
	}


	/**
	 * Is the action done?
	 */
	public abstract bool IsDone ();

	/**
	 * Procedurally check if this action can run. Not all actions
	 * will need this, but some might.
	 */
	public abstract bool CheckProceduralPrecondition (GameObject agent);

	/**
	 * Run the action.
	 * Returns True if the action performed successfully or false
	 * if something happened and it can no longer perform. In this case
	 * the action queue should clear out and the goal cannot be reached.
	 */
	public abstract bool Perform (GameObject agent);

	/**
	 * Does this action need to be within range of a target game object?
	 * If not then the moveTo state will not need to run for this action.
	 */
	public abstract bool RequiresInRange ();


	/**
	 * Are we in range of the target?
	 * The MoveTo state will set this and it gets reset each time this action is performed.
	 */
	public bool IsInRange ()
	{
		return m_InRange;
	}

	public void SetInRange (bool inRange)
	{
		this.m_InRange = inRange;
	}


	public void AddPrecondition (string key, object value)
	{
		m_Preconditions.Add (key, value);
	}


	public void RemovePrecondition (string key)
	{
		if (m_Preconditions.ContainsKey (key)) {
			m_Preconditions.Remove (key);
		}

		/*
		KeyValuePair<string, object> remove = default(KeyValuePair<string,object>);
		foreach (KeyValuePair<string, object> kvp in m_Preconditions) {
			if (kvp.Key.Equals (key))
				remove = kvp;
		}
		if (!default(KeyValuePair<string,object>).Equals (remove))
			m_Preconditions.Remove (remove);
		*/
	}


	public void AddEffect (string key, object value)
	{
		m_Effects.Add (key, value);
	}


	public void RemoveEffect (string key)
	{
		if (m_Effects.ContainsKey (key)) {
			m_Effects.Remove (key);
		}

		/*
		KeyValuePair<string, object> remove = default(KeyValuePair<string,object>);
		foreach (KeyValuePair<string, object> kvp in m_Effects) {
			if (kvp.Key.Equals (key))
				remove = kvp;
		}

		if (!default(KeyValuePair<string,object>).Equals (remove))
			m_Effects.Remove (remove); 
		*/
	}




}