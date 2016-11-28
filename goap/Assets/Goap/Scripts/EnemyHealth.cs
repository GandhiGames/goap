using UnityEngine;
using System.Collections;

public class EnemyHealth : WorldComponent
{
    public int hitPoints = 3;
    public int currentHealth { get { return m_CurrentHitPoints; } }
    public bool IsDead { get { return m_CurrentHitPoints <= 0; } }

    private int m_CurrentHitPoints;


    void OnEnable()
    {
        COMPONENT_DATABASE.Register<EnemyHealth>(this);

        m_CurrentHitPoints = hitPoints;
    }

    void OnDisable()
    {
        COMPONENT_DATABASE.UnRegister<EnemyHealth>(this);
    }

    public void DoDamage(int amount = 1)
    {
        m_CurrentHitPoints -= amount;

        if(IsDead)
        {
            Kill();
        }
    }

    private void Kill()
    {
        Destroy(gameObject);
    }

}
