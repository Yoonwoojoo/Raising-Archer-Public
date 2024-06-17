using UnityEngine;

public class MonsterMoveFlip : MonoBehaviour
{
    private MonsterController controller;
    private MonsterStatsHandler monsterStatsHandler;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D _rb;

    private void Awake()
    {
        controller = GetComponent<MonsterController>();
        monsterStatsHandler = GetComponent<MonsterStatsHandler>();
        spriteRenderer =GetComponentInChildren<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (controller == null || monsterStatsHandler == null) return;

        if (controller.healthSystem.currentHP <= 0)
        {
            _rb.velocity = Vector2.zero;
            _rb.simulated = false;
            controller.OnMove(Vector2.zero);
            return;
        }

        if (!controller.stunSystem.isStunned)
        {
            float attackRange = monsterStatsHandler.currentStats.attackSO.attackRange;

            if (controller.DistanceToTarget() < float.MaxValue)
            {
                Vector2 direction = controller.DirectionToTarget();
                MoveFlip(direction);

                if (controller.DistanceToTarget() <= attackRange)
                {
                    controller.isAttacking = true;
                    _rb.velocity = Vector2.zero;
                    controller.OnMove(Vector2.zero);
                }
                else
                {
                    controller.isAttacking = false;
                }
            }
            else
            {
                controller.isAttacking = false;
            }
        }
        else
        {
            _rb.velocity = Vector2.zero;
            controller.OnMove(Vector2.zero);
        }
    }

    public void MoveFlip(Vector2 direction)
    {
        direction.Normalize();

        Vector2 movement = direction * monsterStatsHandler.currentStats.moveSpeed;
        _rb.velocity = movement;

        controller.OnMove(direction);

        if(direction.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if(direction.x > 0)
        {
            spriteRenderer.flipX = false;
        }
    }
}