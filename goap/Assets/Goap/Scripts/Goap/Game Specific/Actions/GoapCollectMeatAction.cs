using UnityEngine;
using System.Collections;

public class GoapCollectMeatAction : GoapAction
{
	private bool m_HasMeat;
	private Meat m_TargetMeat;
	private Inventory m_Inventory;

	void Awake ()
	{
		m_Inventory = GetComponent<Inventory> ();
	}

    protected override void Start()
    {
        base.Start();

		AddPrecondition ("hasMeat", false); 
		AddPrecondition ("spawnMeat", true);
		AddEffect ("hasMeat", true);
	}

	protected override void DoReset ()
	{
		m_HasMeat = false;
		m_TargetMeat = null;
	}

	public override bool IsDone ()
	{
		return m_HasMeat;
	}

	public override bool RequiresInRange ()
	{
		return true;
	}

	public override void SetTarget ()
	{
        var closestMeat = GetClosest();

		if (closestMeat != null) {
			m_TargetMeat = closestMeat;
			target = closestMeat.transform;
		}

	}

	public override bool CheckProceduralPrecondition ()
	{
		/*
		var closestMeat = m_GetComponent.GetClosest<Meat> (gameObject);

		if (closestMeat == null) {
			return false;
		}

		m_TargetMeat = closestMeat;
		target = closestMeat.transform;
		*/

		return true;
	}

	public override bool Perform ()
	{
		if (m_TargetMeat != null) {
			Destroy (m_TargetMeat.gameObject);

			m_Inventory.IncrementResourceCount (ResourceType.Meat, 1);

			m_HasMeat = true;

			return true;
		}

		return false;
	}

    private Meat GetClosest()
    {
        var meat = COMPONENT_DATABASE.RetrieveComponents<Meat>();

        Meat closest = null;
        float closestDist = float.MaxValue;

        foreach (var tree in meat)
        {
            float dist = (tree.gameObject.transform.position - transform.position).magnitude;

            if (dist < closestDist)
            {
                closest = (Meat)tree;
                closestDist = dist;
            }
        }

        return closest;
    }
}
