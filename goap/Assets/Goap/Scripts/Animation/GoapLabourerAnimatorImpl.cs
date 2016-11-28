using UnityEngine;
using System.Collections;

public interface GoapLabourerAnimator
{
    void PlayPickUp();
    void PlayStandUp();
    void PlaySlash();
    void StopSlash();
    void PlayThrust();
    void StopThrust();
}

[RequireComponent(typeof(GoapMovement), typeof(Animator))]
public class GoapLabourerAnimatorImpl : MonoBehaviour, GoapLabourerAnimator
{
    private static readonly int X_VELOCITY_HASH = Animator.StringToHash("XVelocity");
    private static readonly int Y_VELOCITY_HASH = Animator.StringToHash("YVelocity");
    private static readonly int VELOCITY_HASH = Animator.StringToHash("VelocityNorm");
    private static readonly int PICKUP_HASH = Animator.StringToHash("PickUp");
    private static readonly int STANDUP_HASH = Animator.StringToHash("StandUp");
    private static readonly int SLASHING_HASH = Animator.StringToHash("Slashing");
    private static readonly int THRUSTING_HASH = Animator.StringToHash("Thrusting");

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

    public void PlayPickUp()
    {
        m_Movement.DisableMovement();
        m_Animator.SetTrigger(PICKUP_HASH);
    }

    public void PlayStandUp()
    {
        m_Animator.SetTrigger(STANDUP_HASH);

        Invoke("EnableMovement", 0.5f);
    }

    public void PlaySlash()
    {
        m_Animator.SetBool(SLASHING_HASH, true);
    }

    public void StopSlash()
    {
        m_Animator.SetBool(SLASHING_HASH, false);
    }

    public void PlayThrust()
    {
        m_Animator.SetBool(THRUSTING_HASH, true);
    }

    public void StopThrust()
    {
        m_Animator.SetBool(THRUSTING_HASH, false);
    }

    private void EnableMovement()
    {
        m_Movement.EnableMovement();
    }
}
