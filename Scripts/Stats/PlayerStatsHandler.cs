using UnityEngine;

public class PlayerStatsHandler : MonoBehaviour
{
    public PlayerStats baseStats;
    public PlayerStats currentStats { get; private set; }

    public void InitializePlayerStats()
    {
        if(baseStats == null)
        {
            return;
        }
        else
        {
            baseStats.Initialize(baseStats.attackSO);
            UpdatePlayerStats();
        }
    }
    public void UpdatePlayerStats()
    {
        currentStats = new PlayerStats
        {
            maxHP = baseStats.maxHP,
            moveSpeed = baseStats.moveSpeed,
            level = baseStats.level,
            experience = baseStats.experience,
            gold = baseStats.gold,
            crystal = baseStats.crystal,
            attackSO = baseStats.attackSO
        };

        currentStats.attackSO.Initialize();
    }

    public void IncreaseStats()
    {
        currentStats.maxHP += 12;
        currentStats.attackSO.attackDamage += 1;
    }

    public bool SpendGold(int amount)
    {
        if (currentStats.gold >= amount)
        {
            currentStats.gold -= amount;
            return true;
        }
        else
        {
            return false;
        }
    }
}