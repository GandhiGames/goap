using UnityEngine;
using System.Collections;

public class GoapCollectWoodFromBlacksmithAction : GoapAction
{

    public float workDurationSecs = 2f; // seconds
    public int maxWoodToCollect = 5;

    private bool m_Collected = false;
    private float m_StartTime = 0;
    private BlacksmithResourceDeposit m_TargetStack;

    private GoapLabourerAnimator m_Animator;

    void Awake()
    {
        m_Animator = GetComponent<GoapLabourerAnimator>();
    }

    public GoapCollectWoodFromBlacksmithAction()
    {
        AddPrecondition("hasLogs", false); 
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
        var stacks = UnityEngine.GameObject.FindObjectsOfType<BlacksmithResourceDeposit>();

        if (stacks.Length == 0)
        {
            return false;
        }

        var closest = GetClosestBlacksmithDeposit(stacks, agent);
        if (closest == null)
        {
            return false;
        }

        m_TargetStack = closest;
        target = m_TargetStack.transform;

        return true;
    }

    public override bool Perform(GameObject agent)
    {
        if (m_StartTime == 0)
        {
            m_StartTime = Time.time;
            m_Animator.PlayPickUp();
        }


        if (Time.time - m_StartTime > workDurationSecs)
        {
            m_Animator.PlayStandUp();

            int woodCount = Mathf.Min(maxWoodToCollect, m_TargetStack.logs);

            var inventory = agent.GetComponent<Inventory>();
            inventory.IncrementResourceCount(ResourceType.Wood, woodCount);

            m_TargetStack.logs -= woodCount;

            m_Collected = true;
        }

        return true;
    }


    private BlacksmithResourceDeposit GetClosestBlacksmithDeposit(BlacksmithResourceDeposit[] stacks, GameObject agent)
    {
        BlacksmithResourceDeposit closest = null;
        float closestDist = float.MaxValue;

        foreach (var stack in stacks)
        {
            if (stack.logs <= 0)
            {
                continue;
            }

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
