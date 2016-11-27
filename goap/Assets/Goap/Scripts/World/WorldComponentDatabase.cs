using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class WorldComponent : MonoBehaviour
{

}

public class WorldComponentDatabase : MonoBehaviour
{

	private Dictionary<System.Type, List<WorldComponent>> m_Database = new Dictionary<System.Type, List<WorldComponent>> ();

	public void Register<T> (T component) where T : WorldComponent
	{
		var key = component.GetType ();

		print ("Register: " + key);

		if (!m_Database.ContainsKey (key)) {
			m_Database.Add (key, new List<WorldComponent> ());
		} 

		m_Database [key].Add (component);
	}

	public void UnRegister<T> (T component) where T : WorldComponent
	{
		var key = component.GetType ();

		if (m_Database.ContainsKey (key)) {
			m_Database [key].Remove (component);
		}
	}

	public List<WorldComponent> RetrieveComponents<T> () where T : WorldComponent
	{
		var key = typeof(T);

		if (m_Database.ContainsKey (key)) {
			return m_Database [key];
		}

		return new List<WorldComponent> ();
	}
}
