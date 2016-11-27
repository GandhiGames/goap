using UnityEngine;
using System.Collections;

public class GoapForgeWoodenAxeAction : GoapAction
{
	public float forgeDurationSecs = 10f;

	private bool m_Forged = false;
	private BlacksmithForge m_TargetForge;
	private float m_StartTime = 0;
	private GoapLabourerAnimator m_Animator;
	private Inventory m_Inventory;
	private ToolDispenser m_ToolDispenser;

	void Awake ()
	{
		m_Animator = GetComponent<GoapLabourerAnimator> ();
		m_Inventory = GetComponent<Inventory> ();
		m_ToolDispenser = GetComponent<ToolDispenser> ();
	}

	void Start ()
	{
		AddPrecondition ("hasLogs", true);
		AddEffect ("hasNewWoodenAxe", true);
	}

	protected override void DoReset ()
	{
		m_Forged = false;
		m_TargetForge = null;
		m_StartTime = 0;
	}

	public override bool IsDone ()
	{
		return m_Forged;
	}

	public override bool RequiresInRange ()
	{
		return true;
	}

	public override bool Perform ()
	{
		if (m_StartTime == 0) {
			m_StartTime = Time.time;
			m_Animator.PlaySlash ();
		}


		if (Time.time - m_StartTime > forgeDurationSecs) {
			m_Animator.StopSlash ();

			m_Inventory.SetResourceCount (ResourceType.Wood, 0);

			m_ToolDispenser.IncrementToolCount (ToolType.WoodenAxe, 1);

			m_Forged = true;
		}

		return true;
	}

	public override void SetTarget ()
	{
		var trees = UnityEngine.GameObject.FindObjectsOfType<BlacksmithForge> ();

		var closest = GetClosestForge (trees, gameObject);
		if (closest != null) {
			m_TargetForge = closest;
			target = m_TargetForge.transform;
		}


	}

	public override bool CheckProceduralPrecondition ()
	{
		var trees = UnityEngine.GameObject.FindObjectsOfType<BlacksmithForge> ();

		if (trees.Length == 0) {
			return false;
		}

		return true;
	}

	private BlacksmithForge GetClosestForge (BlacksmithForge[] trees, GameObject agent)
	{
		BlacksmithForge closest = null;
		float closestDist = float.MaxValue;

		foreach (var tree in trees) {
			float dist = (tree.gameObject.transform.position - agent.transform.position).magnitude;

			if (dist < closestDist) {
				closest = tree;
				closestDist = dist;
			}
		}

		return closest;
	}
}
