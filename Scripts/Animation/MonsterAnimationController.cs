using System.Collections;
using UnityEngine;

public class MonsterAnimationController : AnimationController
{
    private MonsterController controller;

    protected override void Awake()
    {
        base.Awake();
        controller = GetComponent<MonsterController>();
    }

    protected override void Start()
    {
        base.Start();
        controller.OnMoveEvent += Moving;
        controller.OnAttackEvent += Attacking;
    }

    private void Moving(Vector2 direction)
    {
        if (isDead) return;
        animator.SetBool(isMovingHash, direction.magnitude > magnitudeThreshold);
    }

    private void Attacking()
    {
        if (isDead) return;
        animator.SetTrigger(attackHash);
        StartCoroutine(ResetAttackTrigger());
    }

    protected override void Death()
    {
        if (isDead) return;
        isDead = true;

        if (animator != null)
        {
            animator.SetBool(isMovingHash, false);
            animator.SetTrigger(deadHash);
        }

        StartCoroutine(DeactivateAfterDelay(2f)); // 2초 후에 비활성화
    }

    private IEnumerator DeactivateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
