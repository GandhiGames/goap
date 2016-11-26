using UnityEngine;
using System.Collections;

public class GoapCollectAxeFromBlacksmithAction : GoapAction
{
    public bool startWithAxe = true;
    private bool m_HasAxe = false;
    private ToolDispenser m_ToolDispenser; // where we get the tool from

    void Start()
    {
        if (startWithAxe)
        {
            GetComponent<Inventory>().equippedTool = new WoodenAxe();
        }
    }

    public GoapCollectAxeFromBlacksmithAction()
    {
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

    public override bool CheckProceduralPrecondition(GameObject agent)
    {
        var dispensers = GameObject.FindObjectsOfType<ToolDispenser>();

        if (dispensers.Length == 0)
        {
            return false;
        }

        var closest = GetClosestToolDispenser(dispensers, agent);
        if (closest == null)
        {
            return false;
        }

        m_ToolDispenser = closest;
        target = m_ToolDispenser.transform;

        return true;
    }

    public override bool Perform(GameObject agent)
    {
        var axe = m_ToolDispenser.RetrieveWoodenAxe();

        if (axe != null)
        {
            agent.GetComponent<Inventory>().equippedTool = axe;

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
