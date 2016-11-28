using UnityEngine;
using System.Collections;

public class GoapSlaughterAnimalAction : GoapAction 
{
	private AnimalHealth m_TargetAnimal;
	private GoapLabourerAnimator m_Animator;
    private bool m_Killed = false;

	void Awake()
	{
		m_Animator = GetComponent<GoapLabourerAnimator> ();
	}

    protected override void Start()
    {
        base.Start();

		//AddPrecondition("spawnMeat", false); 
		AddEffect("spawnMeat", true); 
	}

	protected override void DoReset ()
	{
		m_TargetAnimal = null;
        m_Killed = false;
	}

	public override bool IsDone ()
	{
        return m_Killed;
	}

	public override bool RequiresInRange ()
	{
		return true;
	}

	public override void SetTarget ()
	{
        AnimalHealth closest = GetClosest();

        if (closest != null) {
			m_TargetAnimal = closest;
			target = closest.transform;
		}
	}

	public override bool CheckProceduralPrecondition ()
	{
        return COMPONENT_DATABASE.RetrieveComponents<AnimalHealth>().Count > 0;
	}

	public override bool Perform ()
	{
		if (m_TargetAnimal != null && m_TargetAnimal.IsDying()) {
			return true;
		}
		
		if (m_TargetAnimal != null) {
			m_Animator.PlaySlash ();
			m_TargetAnimal.Kill (0.5f);
			Invoke ("StopSlashing", 0.5f);

			return true;
		}

		return false;
	}

	private void StopSlashing()
	{
		m_Animator.StopSlash ();

        m_Killed = true;
	}

    private AnimalHealth GetClosest()
    {
        var health = COMPONENT_DATABASE.RetrieveComponents<AnimalHealth>();

        if (health.Count == 0)
        {
            return null;
        }

        AnimalHealth closest = null;
        float closestDist = float.MaxValue;

        foreach (var ah in health)
        {
            float dist = (ah.gameObject.transform.position - transform.position).magnitude;

            if (dist < closestDist)
            {
                closest = (AnimalHealth)ah;
                closestDist = dist;
            }
        }

        return closest;
    }
}
