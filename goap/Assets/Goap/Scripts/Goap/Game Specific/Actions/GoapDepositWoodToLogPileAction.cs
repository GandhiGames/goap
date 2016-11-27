using UnityEngine;
using System.Collections;

public class GoapDepositWoodToLogPileAction : GoapAction
{
	private bool m_DepositedWood = false;
	private WoodStack m_TargetDeposit;
	private GetClosestComponent m_GetComponent;
	private Inventory m_Inventory;

	void Awake()
	{
		m_Inventory = GetComponent<Inventory> ();
	}

	void Start ()
	{
		m_GetComponent = new GetClosestComponent ();

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
		var closest = m_GetComponent.GetClosest<WoodStack> (gameObject);

		if (closest != null) {
			m_TargetDeposit = closest;
			target = m_TargetDeposit.transform;
		}
	}

	public override bool CheckProceduralPrecondition ()
	{

		var closest = GameObject.FindObjectsOfType<WoodStack> ();

		if (closest.Length == 0) {
			return false;
		}

		return true;
	}


	public override bool Perform ()
	{		
		m_TargetDeposit.count += m_Inventory.GetResourceCount (ResourceType.Wood);
		m_Inventory.SetResourceCount (ResourceType.Wood, 0);
		m_DepositedWood = true;

		return true;
	}
}
