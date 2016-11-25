using UnityEngine;
using System.Collections;

public class GoapForgeWoodenAxeAction : GoapAction
{
    public float forgeDurationSecs = 10f;

    private bool m_Forged = false;
    private BlacksmithForge m_TargetForge; 
    private float m_StartTime = 0;
    private GoapLabourerAnimator m_Animator;

    void Awake()
    {
        m_Animator = GetComponent<GoapLabourerAnimator>();
    }

    public GoapForgeWoodenAxeAction()
    {
        AddPrecondition("hasLogs", true);
        AddEffect("hasNewWoodenAxe", true);
    }

    protected override void DoReset()
    {
        m_Forged = false;
        m_TargetForge = null;
        m_StartTime = 0;
    }

    public override bool IsDone()
    {
        return m_Forged;
    }

    public override bool RequiresInRange()
    {
        return true;
    }

    public override bool Perform(GameObject agent)
    {
        if (m_StartTime == 0)
        {
            m_StartTime = Time.time;
            m_Animator.PlaySlash();
        }


        if (Time.time - m_StartTime > forgeDurationSecs)
        {
            m_Animator.StopSlash();

            var inventory = agent.GetComponent<Inventory>();

            inventory.SetResourceCount(ResourceType.Wood, 0);
            inventory.IncrementResourceCount(ResourceType.WoodenAxe, 1);

            m_Forged = true;
        }

        return true;
    }
    public override bool CheckProceduralPrecondition(GameObject agent)
    {
        var trees = UnityEngine.GameObject.FindObjectsOfType<BlacksmithForge>();

        if (trees.Length == 0)
        {
            return false;
        }

        var closest = GetClosestForge(trees, agent);
        if (closest == null)
        {
            return false;
        }

        m_TargetForge = closest;
        target = m_TargetForge.transform;

        return true;
    }

    private BlacksmithForge GetClosestForge(BlacksmithForge[] trees, GameObject agent)
    {
        BlacksmithForge closest = null;
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
