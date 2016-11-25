using UnityEngine;
using System.Collections;

public class GoapCollectAxeFromBlacksmithAction : GoapAction
{
    private bool m_HasAxe = false;
    private ToolDispenser m_ToolDispenser; // where we get the tool from

    public GoapCollectAxeFromBlacksmithAction()
    {
        problem with this pre condition
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
            print("Collect axe: " + false);
            return false;
        }

        var closest = GetClosestToolDispenser(dispensers, agent);
        if (closest == null)
        {
            print("Collect axe: " + false);
            return false;
        }

        m_ToolDispenser = closest;
        target = m_ToolDispenser.transform;

        print("Collect axe: " + true);
        return true;
    }

    public override bool Perform(GameObject agent)
    {

        if (m_ToolDispenser.woodenAxeCount > 0)
        {
            m_ToolDispenser.RetrievedWoodenAxe();

            agent.GetComponent<Inventory>().IncrementResourceCount(ResourceType.WoodenAxe, 1);

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
            if(dispenser.woodenAxeCount <= 0)
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
