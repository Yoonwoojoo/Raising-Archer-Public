using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private PlayerController controller;
    private PlayerStatsHandler playerStatsHandler;
    private Rigidbody2D _rb;
    private bool canMove = true;
    private bool isStunned = false;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        playerStatsHandler = GetComponent<PlayerStatsHandler>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        controller.OnMoveEvent += ApplyMove;
        controller.OnMoveEndEvent += EnableAttack;
        controller.OnAttackEvent += StopMovement;
        controller.OnAttackEndEvent += ResumeMovement;
        controller.stunSystem.OnStunChanged += OnStunChanged;
    }

    public void ApplyMove(Vector2 direction)
    {
        if (!canMove || isStunned || controller.isDead)
        {
            return;
        }

        direction.Normalize();
        Vector2 movement = direction * playerStatsHandler.currentStats.moveSpeed;
        _rb.velocity = movement;
    }

    private void StopMovement(AttackSO attackSO)
    {
        _rb.velocity = Vector2.zero;
        canMove = false;
    }

    private void ResumeMovement()
    {
        if (!isStunned && !controller.isDead)
        {
            canMove = true;
        }
    }

    private void OnStunChanged(bool isStunned)
    {
        this.isStunned = isStunned;
        if (isStunned)
        {
            _rb.velocity = Vector2.zero; // ���� ���¿��� �ӵ��� 0���� ����
            canMove = false; // �̵� �Ұ� ���·� ����
        }
        else
        {
            canMove = true; // ���� ���� �� �ٽ� �̵� ���� ���·� ����
            ResumeMovement();
        }
    }

    private void EnableAttack()
    {
        controller.isMoving = false;
    }
}