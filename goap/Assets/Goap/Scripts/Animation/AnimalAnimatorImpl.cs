using UnityEngine;
using System.Collections;

public interface AnimalAnimator
{
    void PlayEat();
}

[RequireComponent(typeof(GoapMovement), typeof(Animator))]
public class AnimalAnimatorImpl : MonoBehaviour, AnimalAnimator
{
    private static readonly int X_VELOCITY_HASH = Animator.StringToHash("XVelocity");
    private static readonly int Y_VELOCITY_HASH = Animator.StringToHash("YVelocity");
    private static readonly int VELOCITY_HASH = Animator.StringToHash("VelocityNorm");
    private static readonly int EAT_HASH = Animator.StringToHash("Eat");

    private GoapMovement m_Movement;
    private Animator m_Animator;

    void Awake()
    {
        m_Movement = GetComponent<GoapMovement>();
        m_Animator = GetComponent<Animator>();
    }

    void Update()
    {
        m_Animator.SetFloat(X_VELOCITY_HASH, -m_Movement.heading.x);
        m_Animator.SetFloat(Y_VELOCITY_HASH, -m_Movement.heading.y);
        m_Animator.SetFloat(VELOCITY_HASH, m_Movement.velocity.magnitude);
    }

    public void PlayEat()
    {
        m_Animator.SetTrigger(EAT_HASH);
    }

}
