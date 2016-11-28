using UnityEngine;
using System.Collections;

public class AnimalHealth : WorldComponent 
{
	public GameObject meatPrefab;

	private bool m_Dying;

	public void Kill(float seconds)
	{
		m_Dying = true;

		StartCoroutine (DoKill (seconds));
	}

	public bool IsDying()
	{
		return m_Dying;
	}

    void OnEnable()
    {
        COMPONENT_DATABASE.Register<AnimalHealth>(this);
    }

    void OnDisable()
    {
        COMPONENT_DATABASE.UnRegister<AnimalHealth>(this);
    }

    private IEnumerator DoKill(float seconds)
	{
		yield return new WaitForSeconds (seconds);

		Instantiate (meatPrefab, transform.position, Quaternion.identity);
		Destroy (gameObject);
	}
}
