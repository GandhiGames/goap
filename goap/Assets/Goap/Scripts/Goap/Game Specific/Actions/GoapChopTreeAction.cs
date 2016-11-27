using UnityEngine;
using System.Collections.Generic;
using System;

public class GoapChopTreeAction : GoapAction
{
	public float workDurationSecs = 4f;

	private bool m_TreeChopped = false;
	private CutableTree m_TargetTree;
	private float m_StartTime;
	private GoapLabourerAnimator m_Animator;
	private Inventory m_Inventory;
	private WorldComponentDatabase m_Database;

	void Awake ()
	{
		m_Animator = GetComponent<GoapLabourerAnimator> ();
		m_Inventory = GetComponent<Inventory> ();
		m_Database = FindObjectOfType<WorldComponentDatabase> ();
	}

	void Start ()
	{
		AddPrecondition ("hasAxe", true); // we need a tool to do this
		AddPrecondition ("hasLogs", false); // if we have firewood we don't want more
		AddEffect ("hasLogs", true);
	}


	protected override void DoReset ()
	{
		m_TreeChopped = false;
		m_TargetTree = null;
		m_StartTime = 0f;
	}

	public override bool IsDone ()
	{
		return m_TreeChopped;
	}

	public override bool RequiresInRange ()
	{
		return true;
	}

	public override void SetTarget ()
	{
		var closest = GetClosestTree ();

		if (closest != null) {
			m_TargetTree = closest;
			target = m_TargetTree.transform;
		}
	}

	public override bool CheckProceduralPrecondition ()
	{
		return m_Database.RetrieveComponents<CutableTree>().Count > 0;
	}

	public override bool Perform ()
	{
		if (m_StartTime == 0) {
			m_StartTime = Time.time;
			m_Animator.PlaySlash ();
		}


		if (Time.time - m_StartTime > workDurationSecs) {
			m_Animator.StopSlash ();

			m_Inventory.IncrementResourceCount (ResourceType.Wood, 4);

			m_Inventory.equippedTool.Damage ();

			if (m_Inventory.equippedTool.IsDestroyed ()) {
				m_Inventory.equippedTool = null;
			}

			m_TreeChopped = true;

            

		}

		return true;
	}

	private CutableTree GetClosestTree ()
	{
		var trees = m_Database.RetrieveComponents<CutableTree>();

		CutableTree closest = null;
		float closestDist = float.MaxValue;

		foreach (var tree in trees) {
			float dist = (tree.gameObject.transform.position - transform.position).magnitude;

			if (dist < closestDist) {
				closest = (CutableTree)tree;
				closestDist = dist;
			}
		}

		return closest;
	}
}
