using UnityEngine;
using System.Collections;

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

    void Start()
    {
        if (startWithAxe)
        {
            GetComponent<Inventory>().equippedTool = new WoodenAxe();
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
		var dispensers = GameObject.FindObjectsOfType<ToolDispenser>();
		var closest = GetClosestToolDispenser(dispensers, gameObject);
		if (closest != null)
		{
			m_ToolDispenser = closest;
			target = m_ToolDispenser.transform;
		}

	}

    public override bool CheckProceduralPrecondition()
    {
        var dispensers = GameObject.FindObjectsOfType<ToolDispenser>();

        if (dispensers.Length == 0)
        {
            return false;
        }

        return true;
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

    private ToolDispenser GetClosestToolDispenser(ToolDispenser[] dispensers, GameObject agent)
    {
        ToolDispenser closest = null;
        float closestDist = float.MaxValue;

        foreach (var dispenser in dispensers)
        {
            if(dispenser.GetResourceCount(ToolType.WoodenAxe) <= 0)
            {
                continue;
            }

            float dist = (dispenser.gameObject.transform.position - agent.transform.position).magnitude;

            if (dist < closestDist)
            {
                closest = dispenser;
                closestDist = dist;
            }
        }

        return closest;
    }
}
