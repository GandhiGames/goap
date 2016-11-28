using UnityEngine;
using System.Collections.Generic;

public class GoapCollectAxeFromBlacksmithAction : GoapAction
{
    public bool startWithAxe = true;
    private bool m_HasAxe = false;
    private ToolDispenser m_ToolDispenser; // where we get the tool from
	private Inventory m_Inventory;

	void Awake()
	{
		m_Inventory = GetComponent<Inventory> ();
	}

    protected override void Start()
    {
        base.Start();

        if (startWithAxe)
        {
            m_Inventory.equippedTool = new WoodenAxe();
        }

		AddPrecondition("hasAxe", false); // don't get a tool if we already have one
		AddEffect("hasAxe", true); // we now have a tool
    }

    protected override void DoReset()
    {
        m_HasAxe = false;
        m_ToolDispenser = null;
    }

    public override bool IsDone()
    {
        return m_HasAxe;
    }

    public override bool RequiresInRange()
    {
        return true; 
    }

	public override void SetTarget ()
	{
		var closest = GetClosest();

		if (closest != null)
		{
			m_ToolDispenser = closest;
			target = m_ToolDispenser.transform;
		}

	}

    public override bool CheckProceduralPrecondition()
    {
        return COMPONENT_DATABASE.RetrieveComponents<ToolDispenser>().Count > 0;
    }

    public override bool Perform()
    {
        var axe = m_ToolDispenser.RetrieveWoodenAxe();

        if (axe != null)
        {
			m_Inventory.equippedTool = axe;

            m_HasAxe = true;

            return true;
        }
        else
        {
            // we got there but there was no tool available! Someone got there first. Cannot perform action
            return false;
        }
    }

    private ToolDispenser GetClosest()
    {
        var dispensers = COMPONENT_DATABASE.RetrieveComponents<ToolDispenser>();

        if(dispensers.Count == 0)
        {
            return null;
        }

        ToolDispenser closest = null;
        float closestDist = float.MaxValue;

        foreach (var d in dispensers)
        {
            var dispenser = (ToolDispenser)d;
            if(dispenser.GetResourceCount(ToolType.WoodenAxe) <= 0)
            {
                continue;
            }

            float dist = (dispenser.gameObject.transform.position - transform.position).magnitude;

            if (dist < closestDist)
            {
                closest = dispenser;
                closestDist = dist;
            }
        }

        return closest;
    }
}
