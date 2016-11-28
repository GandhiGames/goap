using UnityEngine;
using System.Collections;

public class GoapCollectWoodFromBlacksmithAction : GoapAction
{
	public float workDurationSecs = 2f;
	public int maxWoodToCollect = 5;

	private bool m_Collected = false;
	private float m_StartTime = 0;
	private BlacksmithResourceDeposit m_TargetStack;
	private GoapLabourerAnimator m_Animator;
	private Inventory m_Inventory;

	void Awake ()
	{
		m_Animator = GetComponent<GoapLabourerAnimator> ();
		m_Inventory = GetComponent<Inventory> ();
	}

    protected override void Start()
    {
        base.Start();

        AddPrecondition ("hasLogs", false); 
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
		var closest = GetClosest ();
		if (closest != null) {
			m_TargetStack = closest;
			target = m_TargetStack.transform;
		}


	}

	public override bool CheckProceduralPrecondition ()
	{
        return COMPONENT_DATABASE.RetrieveComponents<BlacksmithResourceDeposit>().Count > 0;
	}

	public override bool Perform ()
	{
		if (m_StartTime == 0) {
			m_StartTime = Time.time;
			m_Animator.PlayPickUp ();
		}


		if (Time.time - m_StartTime > workDurationSecs) {
			m_Animator.PlayStandUp ();

			int woodCount = Mathf.Min (maxWoodToCollect, m_TargetStack.logs);

			m_Inventory.IncrementResourceCount (ResourceType.Wood, woodCount);

			m_TargetStack.logs -= woodCount;

			m_Collected = true;
		}

		return true;
	}


	private BlacksmithResourceDeposit GetClosest ()
	{
        var stacks = COMPONENT_DATABASE.RetrieveComponents<BlacksmithResourceDeposit>();

        if(stacks.Count == 0)
        {
            return null;
        }

		BlacksmithResourceDeposit closest = null;
		float closestDist = float.MaxValue;

		foreach (var s in stacks) {

            var stack = (BlacksmithResourceDeposit)s;

			if (stack.logs <= 0) {
				continue;
			}

			float dist = (stack.gameObject.transform.position - transform.position).magnitude;

			if (dist < closestDist) {
				closest = stack;
				closestDist = dist;
			}
		}

		return closest;
	}
}
