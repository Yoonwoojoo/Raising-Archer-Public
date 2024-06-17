using UnityEngine;

public class MonsterStatsHandler : MonoBehaviour
{
    [SerializeField] public MonsterStats baseStats;
    [SerializeField] public int minGold; 
    [SerializeField] public int maxGold;
    public MonsterStats currentStats { get; private set; }

    private void Awake()
    {
        InitializeMonsterStats();
    }

    public void InitializeMonsterStats()
    {
        if (baseStats == null)
        {
            return;
        }

        AttackSO attackSO = Instantiate(baseStats.attackSO);
        int randomGold = Random.Range(minGold, maxGold + 1);

        currentStats = new MonsterStats
        {
            monsterName = baseStats.monsterName,
            maxHP = baseStats.maxHP,
            moveSpeed = baseStats.moveSpeed,
            experienceReward = baseStats.experienceReward,
            gold = randomGold,
            attackSO = attackSO,
        };
    }
}