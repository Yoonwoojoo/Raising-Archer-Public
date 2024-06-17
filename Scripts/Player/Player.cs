using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string playerName;
    private TMP_Text playerNameText;

    public PlayerStatsHandler playerStatsHandler;
    private HealthSystem healthSystem;
    private ExpSystem expSystem;

    private WeaponData selectedWeapon;
    private Sprite defaultWeaponSprite; // �⺻ ���� ��������Ʈ �����

    private void Awake()
    {
        playerName = DataManager.Instance.playerName;
        playerNameText = GetComponentInChildren<TMP_Text>();
        if (playerNameText != null)
        {
            playerNameText.text = playerName;
        }

        PlayerManager.Instance.Player = this;
        DontDestroyOnLoad(gameObject);

        playerStatsHandler = GetComponent<PlayerStatsHandler>();
        healthSystem = GetComponent<HealthSystem>();
        expSystem = GetComponent<ExpSystem>();

        healthSystem.isPlayer = true;

        // �⺻ ���� ��������Ʈ �ʱ�ȭ
        defaultWeaponSprite = GetDefaultWeaponSprite();
    }

    private void Start()
    {
        playerStatsHandler.InitializePlayerStats();
        healthSystem.InitializeHealth();

        UpdateAllUI();
    }

    public void UpdateAllUI()
    {
        if (UIManager.Instance != null)
        {
            if (playerStatsHandler != null && healthSystem != null && expSystem != null)
            {
                UIManager.Instance.UpdateLvText(playerStatsHandler.currentStats.level);
                UIManager.Instance.UpdateHpText(healthSystem.currentHP, playerStatsHandler.currentStats.maxHP);
                UIManager.Instance.UpdateExpText(playerStatsHandler.currentStats.experience, expSystem.ExperienceToNextLevel());
                UIManager.Instance.UpdateGoldText(playerStatsHandler.currentStats.gold);
                UIManager.Instance.UpdateCrystalText(playerStatsHandler.currentStats.crystal);
                UIManager.Instance.UpdatePlayerInfo(playerStatsHandler.currentStats);
                UIManager.Instance.UpdateWeaponInfo(selectedWeapon);
            }
        }
    }

    public void EquipWeapon(WeaponData weapon)
    {
        // ���� ���� ����
        if (selectedWeapon != null)
        {
            UnequipWeapon();
        }

        // ���ο� ���� ����
        selectedWeapon = weapon;
        ApplyWeaponStats(weapon);
        UpdateWeaponSprite(weapon.weaponSprite);
        UpdateAllUI();
    }

    private void UnequipWeapon()
    {
        // ���� ���� �ɷ�ġ ����
        if (selectedWeapon != null)
        {
            playerStatsHandler.currentStats.attackSO.attackDamage -= selectedWeapon.weaponAttackDamage;
            playerStatsHandler.currentStats.attackSO.attackDelay -= selectedWeapon.weaponAttackDelay;
            playerStatsHandler.currentStats.moveSpeed -= selectedWeapon.weaponMoveSpeed;
        }

        // �⺻ ���� ��������Ʈ�� ����
        UpdateWeaponSprite(defaultWeaponSprite);
        selectedWeapon = null;
        UpdateAllUI();
    }

    private void ApplyWeaponStats(WeaponData weapon)
    {
        if (weapon == null)
        {
            return;
        }

        if (playerStatsHandler.currentStats.attackSO == null)
        {
            return;
        }

        playerStatsHandler.currentStats.attackSO.attackDamage += weapon.weaponAttackDamage;
        playerStatsHandler.currentStats.attackSO.attackDelay += weapon.weaponAttackDelay;
        playerStatsHandler.currentStats.moveSpeed += weapon.weaponMoveSpeed;
    }

    private void UpdateWeaponSprite(Sprite newSprite)
    {
        Transform weaponPivot = transform.Find("PlayerSprite/WeaponPivot/WeaponSprite");
        if (weaponPivot != null)
        {
            weaponPivot.GetComponent<SpriteRenderer>().sprite = newSprite;
        }
    }

    private Sprite GetDefaultWeaponSprite()
    {
        Transform weaponPivot = transform.Find("PlayerSprite/WeaponPivot/WeaponSprite");
        if (weaponPivot != null)
        {
            return weaponPivot.GetComponent<SpriteRenderer>().sprite;
        }
        return null;
    }

    public void GainExperience(int amount)
    {
        expSystem.AddExperience(amount);
    }

    public void GainGold(int amount)
    {
        playerStatsHandler.currentStats.gold += amount;
        UpdateAllUI();
    }
}

