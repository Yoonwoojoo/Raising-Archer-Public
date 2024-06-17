using System;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public Transform target { get; set; }
    private MonsterMechanism monsterMechanism;
    public MonsterStatsHandler monsterStatsHandler;
    public HealthSystem healthSystem;
    public StunSystem stunSystem;

    public bool isAttacking = false;

    public event Action<Vector2> OnMoveEvent;
    public event Action OnAttackEvent;
    public event Action<MonsterController> OnDeathEvent;

    protected virtual void Awake()
    {
        monsterMechanism = GetComponent<MonsterMechanism>();
        monsterStatsHandler = GetComponent<MonsterStatsHandler>();
        healthSystem = GetComponent<HealthSystem>();
        stunSystem = GetComponent<StunSystem>();
    }

    protected virtual void Start()
    {
        monsterMechanism.FindTarget();
        healthSystem.OnDeath += OnDeath;
    }

    protected virtual void FixedUpdate()
    {
        if (healthSystem == null) return;

        if (stunSystem.isStunned || healthSystem.currentHP <= 0) return;
    }

    public void OnMove(Vector2 direction)
    {
        OnMoveEvent?.Invoke(direction);
    }

    public void OnAttack()
    {
        OnAttackEvent?.Invoke();
    }

    public void OnDeath()
    {
        isAttacking = false;

        if (monsterStatsHandler != null)
        {
            int goldReward = monsterStatsHandler.currentStats.gold;
            PlayerManager.Instance.Player.GainExperience(monsterStatsHandler.currentStats.experienceReward);
            PlayerManager.Instance.Player.GainGold(goldReward);
        }

        OnDeathEvent?.Invoke(this);
    }

    public void OnObjectSpawn()
    {
        healthSystem.ResetHealth();
        healthSystem.OnDeath -= OnDeath;
        healthSystem.OnDeath += OnDeath;
    }

    public float DistanceToTarget()
    {
        if (target == null)
        {
            Debug.LogWarning("Target is null");
            return float.MaxValue;
        }
        return Vector3.Distance(transform.position, target.position);
    }

    public Vector2 DirectionToTarget()
    {
        if(target == null)
        {
            return Vector2.zero;
        }
        else
        {
            return (target.position - transform.position).normalized;
        }
    }
}