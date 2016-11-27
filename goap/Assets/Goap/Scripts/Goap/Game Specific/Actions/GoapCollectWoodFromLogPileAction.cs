using UnityEngine;
using System.Collections;
using System;

public class GoapCollectWoodFromLogPileAction : GoapAction
{
	public float workDurationSecs = 2f;
	// seconds
	public int maxWoodToCollect = 2;

	private bool m_Collected = false;
	private float m_StartTime = 0;
	private WoodStack m_TargetStack;
	private GoapLabourerAnimator m_Animator;
	private Inventory m_Inventory;

	void Awake ()
	{
		m_Animator = GetComponent<GoapLabourerAnimator> ();
		m_Inventory = GetComponent<Inventory> ();
	}

	void Start ()
	{
		AddPrecondition ("hasLogs", false); // if we have logs we don't want more
		AddEffect ("hasLogs", true);
	}

	protected override void DoReset ()
	{
		m_Collected = false;
		m_StartTime = 0f;
	}

	public override bool IsDone ()
	{
		return m_Collected;
	}

	public override bool RequiresInRange ()
	{
		return true;
	}

	public override void SetTarget ()
	{
		var stacks = UnityEngine.GameObject.FindObjectsOfType<WoodStack> ();
		var closest = GetClosestWoodStack (stacks, gameObject);
		if (closest != null) {
			m_TargetStack = closest;
			target = m_TargetStack.transform;
		}
	}

	public override bool CheckProceduralPrecondition ()
	{
		// find the nearest wood stack that we can collect
		var stacks = UnityEngine.GameObject.FindObjectsOfType<WoodStack> ();

		if (stacks.Length == 0) {
			return false;
		}

		return true;
	}

	public override bool Perform ()
	{
		if (m_StartTime == 0) {
			m_StartTime = Time.time;
			m_Animator.PlayPickUp ();
		}


		if (Time.time - m_StartTime > workDurationSecs) {
			m_Animator.PlayStandUp ();

			int woodCount = Mathf.Min (maxWoodToCollect, m_TargetStack.count);

			m_Inventory.IncrementResourceCount (ResourceType.Wood, woodCount);

			m_TargetStack.count -= woodCount;

			m_Collected = true;

		}

		return true;
	}


	private WoodStack GetClosestWoodStack (WoodStack[] stacks, GameObject agent)
	{
		WoodStack closest = null;
		float closestDist = float.MaxValue;

		foreach (var stack in stacks) {
			if (stack.count <= 0) {
				continue;
			}

			float dist = (stack.gameObject.transform.position - agent.transform.position).magnitude;

			if (dist < closestDist) {
				closest = stack;
				closestDist = dist;
			}
		}

		return closest;
	}
}
