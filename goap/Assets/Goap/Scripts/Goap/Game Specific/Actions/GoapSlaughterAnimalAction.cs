using UnityEngine;
using System.Collections;

public class GoapSlaughterAnimalAction : GoapAction 
{
	private AnimalHealth m_TargetAnimal;
	private GetClosestComponent m_GetComponent;
	private GoapLabourerAnimator m_Animator;

	void Awake()
	{
		m_Animator = GetComponent<GoapLabourerAnimator> ();
	}

	void Start()
	{
		m_GetComponent = new GetClosestComponent ();

		//AddPrecondition("spawnMeat", false); 
		AddEffect("spawnMeat", true); 
	}

	protected override void DoReset ()
	{
		m_TargetAnimal = null;
	}

	public override bool IsDone ()
	{
		return m_TargetAnimal == null;
	}

	public override bool RequiresInRange ()
	{
		return true;
	}

	public override void SetTarget ()
	{
		var closest = m_GetComponent.GetClosest<AnimalHealth> (gameObject);

		if (closest != null) {
			m_TargetAnimal = closest;
			target = closest.transform;
		}


	}

	public override bool CheckProceduralPrecondition ()
	{
		var closest = GameObject.FindObjectsOfType<AnimalHealth> ();

		if (closest.Length == 0) {
			return false;
		}

		return true;
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
	}
}
