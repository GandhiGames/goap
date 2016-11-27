using UnityEngine;

public class AnimalMovement : MonoBehaviour, GoapMovement
{
    public float moveSpeed = 1f;
    public float targetMoveOffset = 1f;
    public Vector2 secDelayBeforePositionCalculation = new Vector2(1f, 3f);

    public Vector2 velocity { get { return m_Velocity; } }
    public Vector2 heading { get { return m_Heading; } }

    private static readonly float MIN_DISTANCE = 0.1f;

    private Vector2 m_Heading;
    private Vector2 m_Velocity;
    private Vector2 m_PreviousPos;
    private Vector2 m_TargetPos;
    private bool m_CanMove = true;
    private bool m_RecalculatingTarget = false;
    private Vector2[] m_Directions;
    private AnimalAnimator m_AnimalAnimator;

    void Awake()
    {
        m_AnimalAnimator = GetComponent<AnimalAnimator>();
    }

    void Start()
    {
        m_Heading = Vector2.up;
        m_TargetPos = transform.position;

        m_Directions = new Vector2[] { Vector2.left, Vector2.up, Vector2.right, Vector2.down };

        CalculateNewTarget();
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
        if (!m_RecalculatingTarget)
        {
            m_PreviousPos = transform.position;

            float step = moveSpeed * Time.deltaTime;
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, m_TargetPos, step);

            m_Velocity = ((Vector2)transform.position - m_PreviousPos) / Time.deltaTime;

            if (ReachedTarget())
            {
                m_Velocity = Vector2.zero;
                m_RecalculatingTarget = true;

                if(Random.value >= 0.5f)
                {
                    m_AnimalAnimator.PlayEat();
                }

                Invoke("CalculateNewTarget", 
                    Random.Range(secDelayBeforePositionCalculation.x, 
                        secDelayBeforePositionCalculation.y));
            }
        }
    }

    private bool ReachedTarget()
    {
        return Vector2.Distance(transform.position, m_TargetPos) <= MIN_DISTANCE;
    }

    private void CalculateNewTarget()
    {
        m_RecalculatingTarget = false;
        var pos = m_TargetPos + m_Directions[Random.Range(0, m_Directions.Length - 1)] * targetMoveOffset;
        MoveTowards(pos);
    }
}
