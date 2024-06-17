using System;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Camera cam;
    public Transform target;
    public Vector2 moveInput;
    private LayerMask targetLayer;

    private PlayerFlip playerFlip;
    private PlayerStatsHandler playerStatsHandler;
    public HealthSystem healthSystem { get; private set; }
    public StunSystem stunSystem;
    private ExpSystem expSystem;

    private bool isFacingRight = true;
    public bool isMoving { get; set; }
    private bool isAttacking { get; set; }
    public bool isDead { get; set; }

    public event Action<Vector2> OnMoveEvent;
    public event Action OnMoveEndEvent;
    public event Action<AttackSO> OnAttackEvent;
    public event Action OnAttackEndEvent;
    public event Action<Transform> OnTargetChanged;
    public static event Action OnDeathEvent; // 사망 이벤트 추가

    private PlayerAI playerAI; // PlayerAI 참조

    private void Awake()
    {
        cam = Camera.main;
        targetLayer = LayerMask.GetMask("Monster");

        playerFlip = GetComponent<PlayerFlip>();
        playerStatsHandler = GetComponent<PlayerStatsHandler>();
        healthSystem = GetComponent<HealthSystem>();
        expSystem = GetComponent<ExpSystem>();
        stunSystem = GetComponent<StunSystem>();

        playerAI = GetComponent<PlayerAI>(); // PlayerAI 초기화
    }

    private void Start()
    {
        if (healthSystem != null)
        {
            healthSystem.OnDeath += OnDeath;
        }

        if (expSystem != null)
        {
            expSystem.OnLevelUp += OnLevelUp;
        }
    }

    private void OnLevelUp()
    {
        healthSystem.ResetHealth();
        if (PlayerManager.Instance.Player != null)
        {
            PlayerManager.Instance.Player.UpdateAllUI();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (isAttacking || isDead)
        {
            return;
        }

        if (context.phase == InputActionPhase.Started || context.phase == InputActionPhase.Performed)
        {
            moveInput = context.ReadValue<Vector2>();
            isMoving = true;

            if (moveInput.x < 0 && isFacingRight)
            {
                playerFlip.FlipSprite();
                isFacingRight = false;
            }
            else if (moveInput.x > 0 && !isFacingRight)
            {
                playerFlip.FlipSprite();
                isFacingRight = true;
            }

            if (!stunSystem.isStunned)
            {
                playerAI.SetPlayerInputActive(true); // 플레이어 입력 활성화
                OnMoveEvent?.Invoke(moveInput);
            }
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            moveInput = Vector2.zero;
            isMoving = false;

            if (!stunSystem.isStunned)
            {
                OnMoveEndEvent?.Invoke();
            }

            if (!stunSystem.isStunned)
            {
                playerAI.SetPlayerInputActive(false); // 플레이어 입력 비활성화
                OnMoveEvent?.Invoke(moveInput);
            }
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (isMoving || isDead || stunSystem.isStunned)
        {
            return;
        }

        if (context.phase == InputActionPhase.Started || context.phase == InputActionPhase.Performed)
        {
            if (target != null && IsTargetInRange())
            {
                isAttacking = true;
                playerAI.SetPlayerInputActive(true); // 플레이어 입력 활성화
                OnAttackEvent?.Invoke(playerStatsHandler.currentStats.attackSO);
                EndAttack();
                playerAI.SetPlayerInputActive(false); // 플레이어 입력 비활성화
            }
        }
    }

    public void EndAttack()
    {
        isAttacking = false;
        OnAttackEndEvent?.Invoke();
    }

    public void OnTarget(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Vector2 worldPoint = cam.ScreenToWorldPoint(mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero, Mathf.Infinity, targetLayer);

            if (hit.collider != null)
            {
                SetTarget(hit.transform);
            }
            else
            {
                SetTarget(null);
            }
        }
    }

    public void SetTarget(Transform newTarget)
    {
        if (target != newTarget)
        {
            if (target != null)
            {
                HealthSystem targetHealthSystem = target.GetComponent<HealthSystem>();
                if (targetHealthSystem != null)
                {
                    targetHealthSystem.OnDeath -= ClearTarget;
                }
            }

            target = newTarget;

            if (target != null)
            {
                HealthSystem targetHealthSystem = target.GetComponent<HealthSystem>();
                if (targetHealthSystem != null)
                {
                    targetHealthSystem.OnDeath += ClearTarget;
                }
            }

            OnTargetChanged?.Invoke(target);
            UIManager.Instance.UpdateTargetUI(target);
        }
    }

    private void ClearTarget()
    {
        if (target != null)
        {
            HealthSystem targetHealthSystem = target.GetComponent<HealthSystem>();
            if (targetHealthSystem != null)
            {
                targetHealthSystem.OnDeath -= ClearTarget;
            }
        }
        target = null;
        OnTargetChanged?.Invoke(null);
        UIManager.Instance.UpdateTargetUI(null);
    }

    public bool IsTargetInLayer()
    {
        if (target == null) return false;
        return ((1 << target.gameObject.layer) & targetLayer) != 0;
    }

    public bool IsTargetInRange()
    {
        if (target == null) return false;
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        return distanceToTarget <= playerStatsHandler.currentStats.attackSO.attackRange;
    }

    public void LookAtTarget()
    {
        if (target == null) return;

        Vector3 direction = (target.position - transform.position).normalized;
        if (direction.x > 0 && !isFacingRight)
        {
            playerFlip.FlipSprite();
            isFacingRight = true;
        }
        else if (direction.x < 0 && isFacingRight)
        {
            playerFlip.FlipSprite();
            isFacingRight = false;
        }
    }

    private void OnDeath()
    {
        if (isDead) return;
        isDead = true;
        OnDeathEvent?.Invoke(); // 플레이어 전용 사망이벤트
    }
}
