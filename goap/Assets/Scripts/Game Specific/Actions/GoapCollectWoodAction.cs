using UnityEngine;
using System.Collections;
using System;

public class GoapCollectWoodAction : GoapAction
{
    public float workDurationSecs = 2f; // seconds

    private bool m_Collected = false;
    private float m_StartTime = 0;
    private WoodStack m_TargetStack;

    public GoapCollectWoodAction()
    {
        AddPrecondition("hasLogs", false); // if we have logs we don't want more
        AddEffect("hasLogs", true);
    }

    protected override void DoReset()
    {
        m_Collected = false;
        m_StartTime = 0f;
    }

    public override bool IsDone()
    {
        return m_Collected;
    }

    public override bool RequiresInRange()
    {
        return true;
    }

    public override bool CheckProceduralPrecondition(GameObject agent)
    {
        // find the nearest wood stack that we can collect
        var stacks = UnityEngine.GameObject.FindObjectsOfType<WoodStack>();

        if(stacks.Length == 0)
        {
            return false;
        }

        var closest = GetClosestWoodStack(stacks, agent);
        if (closest == null)
        {
            return false;
        }

        m_TargetStack = closest;
        target = m_TargetStack.gameObject;

        return true;
    }

    public override bool Perform(GameObject agent)
    {
        if (m_StartTime == 0)
        {
            m_StartTime = Time.time;
        }

        if (Time.time - m_StartTime > workDurationSecs)
        {
			var inventory = agent.GetComponent<Inventory>();
			inventory.IncrementResourceCount(ResourceType.Wood, 1);
            m_Collected = true;
        }

        return true;
    }

    private WoodStack GetClosestWoodStack(WoodStack[] stacks, GameObject agent)
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
}
