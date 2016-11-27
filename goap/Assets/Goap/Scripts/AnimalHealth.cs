using UnityEngine;
using System.Collections;

public class AnimalHealth : MonoBehaviour 
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
		
	private IEnumerator DoKill(float seconds)
	{
		yield return new WaitForSeconds (seconds);

		Instantiate (meatPrefab, transform.position, Quaternion.identity);
		Destroy (gameObject);
	}
}
