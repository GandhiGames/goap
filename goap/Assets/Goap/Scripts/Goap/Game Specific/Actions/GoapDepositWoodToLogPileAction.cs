using UnityEngine;
using System.Collections;

public class GoapDepositWoodToLogPileAction : GoapAction
{
    private bool m_DepositedWood = false;
    private WoodStack m_TargetDeposit;

    public GoapDepositWoodToLogPileAction()
    {
        AddPrecondition("hasLogs", true); // can't drop off firewood if we don't already have some
        AddEffect("hasLogs", false); // we now have no firewood
        AddEffect("collectLogs", true); // we collected firewood
    }

    protected override void DoReset()
    {
        m_DepositedWood = false;
        m_TargetDeposit = null;
    }

    public override bool IsDone()
    {
        return m_DepositedWood;
    }

    public override bool RequiresInRange()
    {
        return true;
    }

    public override bool CheckProceduralPrecondition(GameObject agent)
    {
        // find the nearest wood stack that we can collect
        var stacks = UnityEngine.GameObject.FindObjectsOfType<WoodStack>();

        if (stacks.Length == 0)
        {
            return false;
        }

        var closest = GetClosestWoodDeposit(stacks, agent);
        if (closest == null)
        {
            return false;
        }

        m_TargetDeposit = closest;
        target = m_TargetDeposit.transform;

        return true;
    }

    private WoodStack GetClosestWoodDeposit(WoodStack[] stacks, GameObject agent)
    {
        WoodStack closest = null;
        float closestDist = float.MaxValue;

        foreach (var stack in stacks)
        {
            float dist = (stack.gameObject.transform.position - agent.transform.position).magnitude;

            if (dist < closestDist)
            {
                closest = stack;
                closestDist = dist;
            }
        }

        return closest;
    }

    public override bool Perform(GameObject agent)
    {
        var inventory = agent.GetComponent<Inventory>();
        m_TargetDeposit.count += inventory.GetResourceCount(ResourceType.Wood);
        inventory.SetResourceCount(ResourceType.Wood, 0);
        m_DepositedWood = true;

        return true;
    }
}
