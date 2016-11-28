using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(GoapMovement))]
public abstract class GoapLabourer : MonoBehaviour, Goap
{
	public float minDistanceToTarget = 0.1f;
	
	private Inventory m_Inventory;
    private GoapMovement m_Movement;

    void Awake()
    {
		m_Inventory = GetComponent<Inventory>();
        m_Movement = GetComponent<GoapMovement>();
    }

    public Dictionary<string, object> GetWorldState()
    {
        var worldData = new Dictionary<string, object>();

		worldData.Add("hasLogs", m_Inventory.HasResource(ResourceType.Wood));
        worldData.Add("hasAxe", m_Inventory.HasToolEquipped(ToolType.WoodenAxe));
		worldData.Add("hasMeat", m_Inventory.HasResource(ResourceType.Meat));
        worldData.Add("hasCookedMeat", m_Inventory.HasResource(ResourceType.CookedMeat));

        return worldData;
    }

   /**
    * Implement in subclasses
    */
    public abstract Dictionary<string, object> CreateGoalState();

    public void PlanFailed(Dictionary<string, object> failedGoal)
    {
        // Not handling this here since we are making sure our goals will always succeed.
        // But normally you want to make sure the world state has changed before running
        // the same goal again, or else it will just fail.
    }

    public void PlanFound(Dictionary<string, object> goal, Queue<GoapAction> actions)
    {
        // Yay we found a plan for our goal
        Debug.Log("<color=green>Plan found</color> " + GoapAgent.PrettyPrint(actions));
    }

    public void ActionsFinished()
    {
        // Everything is done, we completed our actions for this gool. Hooray!
        Debug.Log("<color=blue>Actions completed</color>");
    }

    public void PlanAborted(GoapAction aborter)
    {
        // An action bailed out of the plan. State has been reset to plan again.
        // Take note of what happened and make sure if you run the same goal again
        // that it can succeed.
        Debug.Log("<color=red>Plan Aborted</color> " + GoapAgent.PrettyPrint(aborter));
    }

    public bool MoveAgent(GoapAction nextAction)
    {
    
       
		if (nextAction.IsInRange())
        {
            // we are at the target location, we are done
            return true;
        }
        else
        {
            m_Movement.MoveTowards(nextAction.target.position);
            return false;
        }




    }
}
