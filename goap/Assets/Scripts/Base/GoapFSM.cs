using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Stack-based Finite State Machine.
 * Push and pop states to the FSM.
 * 
 * States should push other states onto the stack 
 * and pop themselves off.
 */
public class GoapFSM {

	public delegate void StateDel (GoapFSM fsm, GameObject gameObject);

	private Stack<StateDel> m_StateStack = new Stack<StateDel> ();

	public void Update (GameObject gameObject) {
		if (m_StateStack.Peek () != null) {
			m_StateStack.Peek ().Invoke (this, gameObject);
		}
	}

	public void pushState(StateDel state) {
		m_StateStack.Push (state);
	}

	public void popState() {
		m_StateStack.Pop ();
	}
}

public interface GoapFSMState 
{

	void Update (GoapFSM fsm, GameObject gameObject);
}

