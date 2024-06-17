using System.Collections;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    protected Animator animator;
    protected HealthSystem healthSystem;

    protected bool isDead = false;

    protected static readonly float magnitudeThreshold = 0.1f;
    protected static readonly int isMovingHash = Animator.StringToHash("isMoving");
    protected static readonly int attackHash = Animator.StringToHash("Attack");
    protected static readonly int isHitHash = Animator.StringToHash("Hit");
    protected static readonly int deadHash = Animator.StringToHash("Dead");

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        healthSystem = GetComponent<HealthSystem>();
    }

    protected virtual void Start()
    {
        if (healthSystem != null)
        {
            healthSystem.OnDamage += Hit;
            healthSystem.OnDeath += Death;
        }
    }

    protected virtual void Hit()
    {
        if (isDead) return;
        animator.SetTrigger(isHitHash);
    }

    protected virtual void Death()
    {
        if (isDead) return;
        isDead = true;

        if (animator != null)
        {
            animator.SetBool(isMovingHash, false);
            animator.SetTrigger(deadHash);
        }
        Invoke("ResetAnimationState", 1f);
    }

    public void ResetAnimationState()
    {
        isDead = false;
        animator.ResetTrigger(attackHash);
        animator.ResetTrigger(isHitHash);
        animator.ResetTrigger(deadHash);
        animator.SetBool(isMovingHash, false);
    }

    protected IEnumerator ResetAttackTrigger()
    {
        yield return new WaitForSeconds(0.1f);
        animator.ResetTrigger(attackHash);
    }

}
