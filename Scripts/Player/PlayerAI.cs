using UnityEngine;

public class PlayerAI : MonoBehaviour
{
    public float wanderRadius; // ���� �̵� �ݰ�
    public float wanderTimer; // �̵� ��� �ð�
    public float detectRange; // ���� Ž�� ����
    public float minDistanceFromMonster; // ���Ϳ��� �ּ� �Ÿ�
    private float timeSinceLastMovedCloserToMonster = 0;
    public float maxTimeWithoutMovingCloser; // �ִ� �������� ���� �ð�

    private float timer;
    private Transform targetMonster;
    private LayerMask monsterLayer;
    private PlayerController controller;
    private PlayerAnimationController animationController;
    private PlayerStatsHandler playerStatsHandler;
    private PlayerMove playerMove;
    private PlayerAttack playerAttack;
    private float timeSinceLastAttack = 0;
    private bool isAutoAttacking = false;
    private bool isPlayerInputActive = false; // �÷��̾� �Է� Ȱ��ȭ ����
    private HealthSystem healthSystem;

    void Awake()
    {
        monsterLayer = LayerMask.GetMask("Monster");
        controller = GetComponent<PlayerController>();
        playerStatsHandler = GetComponent<PlayerStatsHandler>();
        playerMove = GetComponent<PlayerMove>();
        playerAttack = GetComponent<PlayerAttack>();
        animationController = GetComponentInChildren<PlayerAnimationController>();
        healthSystem = GetComponent<HealthSystem>();
        timer = wanderTimer;
    }

    void Update()
    {
        if (isPlayerInputActive || controller.stunSystem.isStunned || controller.isDead || healthSystem.isDead)
        {
            return;
        }

        timer += Time.deltaTime;
        timeSinceLastAttack += Time.deltaTime;
        timeSinceLastMovedCloserToMonster += Time.deltaTime;

        FindClosestMonster();

        if (targetMonster != null)
        {
            controller.SetTarget(targetMonster);
            float distance = Vector2.Distance(transform.position, targetMonster.position);

            if (distance > playerStatsHandler.currentStats.attackSO.attackRange)
            {
                MoveToTarget();
                timeSinceLastMovedCloserToMonster = 0;
            }
            else if (distance < minDistanceFromMonster)
            {
                MoveAwayFromTarget();
            }
            else
            {
                if (timeSinceLastAttack >= playerStatsHandler.currentStats.attackSO.attackDelay)
                {
                    timeSinceLastAttack = 0;
                    AttackMonster();
                }
            }

            // ���� �ð� ���� �������� �������� ���� ���� �õ�
            if (timeSinceLastMovedCloserToMonster >= maxTimeWithoutMovingCloser)
            {
                AttackMonster();
                timeSinceLastMovedCloserToMonster = 0;
            }
        }
        else
        {
            Wander();
        }
    }

    public void SetPlayerInputActive(bool isActive)
    {
        isPlayerInputActive = isActive;
    }

    void FindClosestMonster()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, detectRange, monsterLayer);
        float shortestDistance = Mathf.Infinity;
        foreach (Collider2D collider in hitColliders)
        {
            float distance = Vector2.Distance(transform.position, collider.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                targetMonster = collider.transform;
            }
        }
    }

    void MoveToTarget()
    {
        Vector2 direction = (targetMonster.position - transform.position).normalized;
        playerMove.ApplyMove(direction);
        animationController.Moving(direction);
    }

    void MoveAwayFromTarget()
    {
        Vector2 direction = (transform.position - targetMonster.position).normalized;

        // 360�� ���� ���� ����
        float randomAngle = Random.Range(0f, 360f);
        direction = Quaternion.Euler(0, 0, randomAngle) * direction;

        playerMove.ApplyMove(direction);
        animationController.Moving(direction);
    }

    void AttackMonster()
    {
        if (targetMonster != null && !isAutoAttacking)
        {
            isAutoAttacking = true;
            playerAttack.HandleAttack(playerStatsHandler.currentStats.attackSO);
            animationController.Attacking(playerStatsHandler.currentStats.attackSO);
            Invoke(nameof(ResetAutoAttack), playerStatsHandler.currentStats.attackSO.attackDelay);
        }
    }

    void ResetAutoAttack()
    {
        isAutoAttacking = false;
    }

    void Wander()
    {
        if (timer >= wanderTimer)
        {
            Vector2 newPos = RandomNavSphere(transform.position, wanderRadius);
            Vector2 direction = (newPos - (Vector2)transform.position).normalized;
            playerMove.ApplyMove(direction);
            animationController.Moving(direction);
            timer = 0;
        }
    }

    public static Vector2 RandomNavSphere(Vector2 origin, float dist)
    {
        Vector2 randDirection = UnityEngine.Random.insideUnitCircle * dist;
        randDirection += origin;
        return randDirection;
    }
}
