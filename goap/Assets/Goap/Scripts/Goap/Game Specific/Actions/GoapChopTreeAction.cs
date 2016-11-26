using UnityEngine;
using System.Collections;
using System;

public class GoapChopTreeAction : GoapAction
{
    public float workDurationSecs = 4f;

    private bool m_TreeChopped = false;
    private CutableTree m_TargetTree;
    private float m_StartTime;
    private GoapLabourerAnimator m_Animator;

    void Awake()
    {
        m_Animator = GetComponent<GoapLabourerAnimator>();
    }

    public GoapChopTreeAction()
    {
        AddPrecondition("hasAxe", true); // we need a tool to do this
        AddPrecondition("hasLogs", false); // if we have firewood we don't want more
        AddEffect("hasLogs", true);
    }

    protected override void DoReset()
    {
        m_TreeChopped = false;
        m_TargetTree = null;
        m_StartTime = 0f;
    }

    public override bool IsDone()
    {
        return m_TreeChopped;
    }

    public override bool RequiresInRange()
    {
        return true;
    }

    public override bool CheckProceduralPrecondition(GameObject agent)
    {
        var trees = UnityEngine.GameObject.FindObjectsOfType<CutableTree>();

        if (trees.Length == 0)
        {
            return false;
        }

        var closest = GetClosestTree(trees, agent);
        if (closest == null)
        {
            return false;
        }

        m_TargetTree = closest;
        target = m_TargetTree.transform;

        return true;
    }

    public override bool Perform(GameObject agent)
    {
        if (m_StartTime == 0)
        {
            m_StartTime = Time.time;
            m_Animator.PlaySlash();
        }


        if (Time.time - m_StartTime > workDurationSecs)
        {
            m_Animator.StopSlash();

            var inventory = agent.GetComponent<Inventory>();
            inventory.IncrementResourceCount(ResourceType.Wood, 4);

            inventory.equippedTool.Damage();

            if(inventory.equippedTool.IsDestroyed())
            {
                inventory.equippedTool = null;
            }

            m_TreeChopped = true;

            

        }

        return true;
    }

    private CutableTree GetClosestTree(CutableTree[] trees, GameObject agent)
    {
        CutableTree closest = null;
        float closestDist = float.MaxValue;

        foreach (var tree in trees)
        {
            float dist = (tree.gameObject.transform.position - agent.transform.position).magnitude;

            if (dist < closestDist)
            {
                closest = tree;
                closestDist = dist;
            }
        }

        return closest;
    }
}
