using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public bool isPlayer;
    private PlayerStatsHandler playerStatsHandler;
    private MonsterStatsHandler monsterStatsHandler;
    private TargetSystem targetSystem;
    public bool isDead = false;
    private StunSystem stunSystem;

    public event Action OnDamage;
    public event Action OnHeal;
    public event Action OnDeath;

    public int currentHP { get; private set; }
    public int maxHP
    {
        get
        {
            if (playerStatsHandler != null)
                return playerStatsHandler.currentStats.maxHP;
            else if (monsterStatsHandler != null)
                return monsterStatsHandler.currentStats.maxHP;
            return 0;
        }
    }

    private void Awake()
    {
        playerStatsHandler = GetComponent<PlayerStatsHandler>();
        monsterStatsHandler = GetComponent<MonsterStatsHandler>();
        targetSystem = GetComponent<TargetSystem>();
        stunSystem = GetComponent<StunSystem>();
    }

    public void InitializeHealth()
    {
        if (playerStatsHandler != null)
        {
            playerStatsHandler.UpdatePlayerStats();
            currentHP = playerStatsHandler.currentStats.maxHP;
        }
        else if (monsterStatsHandler != null)
        {
            monsterStatsHandler.InitializeMonsterStats();
            currentHP = monsterStatsHandler.currentStats.maxHP;
        }

        UpdateUI();
    }

    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (UIManager.Instance != null)
        {
            if (isPlayer)
            {
                UIManager.Instance.UpdateHpText(currentHP, maxHP);
            }
        }
    }

    public bool ChangeHealth(int change, float stunTime)
    {
        if (isDead) return false;

        int oldHP = currentHP;
        currentHP += change;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        int damageAmount = Mathf.Abs(change);

        if (change < 0)
        {
            OnDamage?.Invoke();
            if (DamageTextManager.Instance != null)
            {
                DamageTextManager.Instance.ShowDamage(transform.position, damageAmount, isPlayer);
            }

            if (stunTime > 0f && stunSystem != null)
            {
                stunSystem.ApplyStun(stunTime);
            }
        }

        if (currentHP <= 0)
        {
            currentHP = 0;
            UpdateUI();
            CallDeath();
            return true;
        }

        if (change >= 0)
        {
            OnHeal?.Invoke();
        }

        UpdateUI();

        if (targetSystem != null && targetSystem.playerController != null)
        {
            if (targetSystem.playerController.target == transform)
            {
                targetSystem.UpdateTargetUI(transform);
            }
        }

        return true;
    }


    private void CallDeath()
    {
        if (isDead) return;
        isDead = true;
        OnDeath?.Invoke();
    }

    public void ResetHealth()
    {
        currentHP = maxHP;
        isDead = false;
        UpdateUI();
    }
}
