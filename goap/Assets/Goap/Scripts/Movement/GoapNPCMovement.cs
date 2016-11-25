using UnityEngine;
using System.Collections;

public interface GoapMovement
{
    void EnableMovement();
    void DisableMovement();
    Vector2 velocity { get; }
    Vector2 heading { get; }
    void MoveTowards(Vector2 target);
}

public class GoapNPCMovement : MonoBehaviour, GoapMovement
{
    public float moveSpeed = 1f;
    public Vector2 velocity { get { return m_Velocity; } }

    public Vector2 heading { get { return m_Heading; } }

    private Vector2 m_Heading;
    private Vector2 m_Velocity;
    private Vector2 m_PreviousPos;
    private Vector2 m_TargetPos;
    private bool m_CanMove = true;

    void Start()
    {
        m_Heading = Vector2.up;
        m_TargetPos = transform.position;
    }

    public void MoveTowards(Vector2 targetPos)
    {
        m_Heading = (Vector2)transform.position - targetPos;

        if (m_CanMove)
        {
            m_TargetPos = targetPos;
        }
    }

    public void EnableMovement()
    {
        m_CanMove = true;
    }

    public void DisableMovement()
    {
        m_CanMove = false;
    }

    void Update()
    {
        m_PreviousPos = transform.position;

        float step = moveSpeed * Time.deltaTime;
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, m_TargetPos, step);

        m_Velocity = ((Vector2)transform.position - m_PreviousPos) / Time.deltaTime;
    }

}
