using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    private float lastAttackTime = 0f;
    private MonsterController monsterController;
    private MonsterStatsHandler monsterStatsHandler;
    private Transform target;
    private HealthSystem targetHealthSystem;

    private void Awake()
    {
        monsterController = GetComponent<MonsterController>();
        monsterStatsHandler = GetComponent<MonsterStatsHandler>();
    }

    private void Start()
    {
        target = monsterController.target;
    }

    private void Update()
    {
        target = monsterController.target;

        float attackDelay = monsterStatsHandler.currentStats.attackSO.attackDelay;

        if (monsterController.isAttacking && Time.time - lastAttackTime >= attackDelay)
        {
            Attack();
            lastAttackTime = Time.time;
        }
    }

    private void Attack()
    {
        if (target == null) return;

        targetHealthSystem = target.GetComponent<HealthSystem>();
        if (targetHealthSystem != null)
        {
            AttackSO attackSO = monsterStatsHandler.currentStats.attackSO;
            targetHealthSystem.ChangeHealth(-attackSO.attackDamage, attackSO.stunTime);

            monsterController.OnAttack();
        }
    }
}