using UnityEngine;

[CreateAssetMenu(fileName = "AttackSO", menuName = "Raising-Archer/Attacks/Default", order = 0)]
public class AttackSO : ScriptableObject
{
    [Header("Attack Info")]
    public int attackDamage;
    public float attackDelay;
    public float attackRange;
    public float projectileSpeed;

    [Header("KnockBack Info")]
    public float knockbackPower;
    public float knockbackTime;

    [Header("Sturn Info")]
    public float stunTime;

    public void Initialize()
    {
        attackDamage = 3;
        attackDelay = 0.5f;
        attackRange = 2.5f;
        projectileSpeed = 10f;
    }
}
