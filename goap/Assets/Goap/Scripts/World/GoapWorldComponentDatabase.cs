using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class WorldComponent : MonoBehaviour
{
    protected static GoapWorldComponentDatabase COMPONENT_DATABASE;

    protected virtual void Awake()
    {
        if (COMPONENT_DATABASE == null)
        {
            COMPONENT_DATABASE = FindObjectOfType<GoapWorldComponentDatabase>();
        }
    }

}

public class GoapWorldComponentDatabase : MonoBehaviour
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

        print("UnRegister: " + key);

        if (m_Database.ContainsKey (key)) {
			m_Database [key].Remove (component);
		}
	}

	public List<WorldComponent> RetrieveComponents<T> () where T : WorldComponent
	{
		var key = typeof(T);

		if (!m_Database.ContainsKey (key)) {
            m_Database.Add(key, new List<WorldComponent>());
        }

        return m_Database[key];
	}
}
