using UnityEngine;

public class PlayerAnimationController : AnimationController
{
    private PlayerController controller;
    private PlayerStatsHandler playerStatsHandler;
    private float timeSinceLastAttack = 0;

    protected override void Awake()
    {
        base.Awake();
        controller = GetComponent<PlayerController>();
        playerStatsHandler = GetComponent<PlayerStatsHandler>();
    }

    protected override void Start()
    {
        base.Start();
        controller.OnMoveEvent += Moving;
        controller.OnAttackEvent += Attacking;
    }

    private void Update()
    {
        if (timeSinceLastAttack < playerStatsHandler.currentStats.attackSO.attackDelay)
        {
            timeSinceLastAttack += Time.deltaTime;
        }
    }

    public void Moving(Vector2 direction)
    {
        if (isDead) return;
        animator.SetBool(isMovingHash, direction.magnitude > magnitudeThreshold);
    }

    public void Attacking(AttackSO attackSO)
    {
        if (isDead) return;

        if (timeSinceLastAttack >= attackSO.attackDelay)
        {
            animator.SetTrigger(attackHash);
            StartCoroutine(ResetAttackTrigger());

            timeSinceLastAttack = 0f;
        }
    }
}
