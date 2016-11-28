using UnityEngine;
using System.Collections;

public class GoapDepositWoodToBlacksmithAction : GoapAction
{
	private bool m_DepositedWood = false;
	private BlacksmithResourceDeposit m_TargetDeposit;
	private Inventory m_Inventory;

	void Awake()
	{
		m_Inventory = GetComponent<Inventory> ();
	}

    protected override void Start()
    {
        base.Start();

		AddPrecondition ("hasLogs", true); // can't drop off firewood if we don't already have some
		AddEffect ("hasLogs", false); // we now have no firewood
		AddEffect ("collectLogs", true); // we collected firewood
	}

	protected override void DoReset ()
	{
		m_DepositedWood = false;
		m_TargetDeposit = null;
	}

	public override bool IsDone ()
	{
		return m_DepositedWood;
	}

	public override bool RequiresInRange ()
	{
		return true;
	}

	public override void SetTarget ()
	{
        BlacksmithResourceDeposit closest = GetClosest();

		if (closest != null) {
			m_TargetDeposit = closest;
			target = m_TargetDeposit.transform;
		}
	}

	public override bool CheckProceduralPrecondition ()
	{
        return COMPONENT_DATABASE.RetrieveComponents<BlacksmithResourceDeposit>().Count > 0;
	}



	public override bool Perform ()
	{
		m_TargetDeposit.logs += m_Inventory.GetResourceCount (ResourceType.Wood);
		m_Inventory.SetResourceCount (ResourceType.Wood, 0);
		m_DepositedWood = true;

		return true;
	}

    private BlacksmithResourceDeposit GetClosest()
    {
        var trees = COMPONENT_DATABASE.RetrieveComponents<BlacksmithResourceDeposit>();

        BlacksmithResourceDeposit closest = null;
        float closestDist = float.MaxValue;

        foreach (var tree in trees)
        {
            float dist = (tree.gameObject.transform.position - transform.position).magnitude;

            if (dist < closestDist)
            {
                closest = (BlacksmithResourceDeposit)tree;
                closestDist = dist;
            }
        }

        return closest;
    }
}
