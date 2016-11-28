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

    protected override void Start()
    {
        base.Start();

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
		var closest = GetClosest ();

		if (closest != null) {
			m_TargetForge = closest;
			target = m_TargetForge.transform;
		}
	}

	public override bool CheckProceduralPrecondition ()
	{
        return COMPONENT_DATABASE.RetrieveComponents<BlacksmithForge>().Count > 0;
	}

    private BlacksmithForge GetClosest()
    {
        var trees = COMPONENT_DATABASE.RetrieveComponents<BlacksmithForge>();

        if (trees.Count == 0)
        {
            return null;
        }

        BlacksmithForge closest = null;
        float closestDist = float.MaxValue;

        foreach (var tree in trees)
        {
            float dist = (tree.gameObject.transform.position - transform.position).magnitude;

            if (dist < closestDist)
            {
                closest = (BlacksmithForge)tree;
                closestDist = dist;
            }
        }

        return closest;
    }
}
