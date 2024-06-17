using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : GenericSingleton<UIManager>
{
    private TMP_Text[] textArr;
    public Image HpBar;
    public Image ExpBar;

    public TMP_Text userName;
    public TMP_Text userHP;
    public TMP_Text userAttack;
    public TMP_Text userAttackSpeed;
    public TMP_Text userMoveSpeed;

    public GameObject userInvenWindow;
    public Inventory inventory;

    public GameObject weaponInfoPanel;
    public TMP_Text weaponNameText;
    public TMP_Text weaponAttackDamageText;
    public TMP_Text weaponAttackDelayText;
    public TMP_Text weaponMoveSpeedText;

    protected override void Awake()
    {
        base.Awake();
        textArr = GetComponentsInChildren<TMP_Text>();
        StartCoroutine(DisableUserInvenWindowAfterInitialization());
    }

    public void UpdateWeaponInfo(WeaponData weapon)
    {
        if (weapon != null)
        {
            weaponNameText.text = weapon.weaponName;
            weaponAttackDamageText.text = weapon.weaponAttackDamage.ToString();
            weaponAttackDelayText.text = weapon.weaponAttackDelay.ToString("F2");
            weaponMoveSpeedText.text = weapon.weaponMoveSpeed.ToString();
        }
    }


    public void UpdatePlayerInfo(PlayerStats currentStats)
    {
        if (currentStats != null)
        {
            userName.text = PlayerManager.Instance.Player.playerName;
            userHP.text = currentStats.maxHP.ToString();
            userAttack.text = currentStats.attackSO.attackDamage.ToString();
            userAttackSpeed.text = currentStats.attackSO.attackDelay.ToString();
            userMoveSpeed.text = currentStats.moveSpeed.ToString();
        }
    }

    public void ClearWeaponInfo()
    {
        weaponNameText.text = "";
        weaponAttackDamageText.text = "";
        weaponAttackDelayText.text = "";
        weaponMoveSpeedText.text = "";
    }

    public void RefreshUI()
    {
        foreach (var slot in inventory.slots)
        {
            if (slot.weapon != null)
            {
                slot.gameObject.SetActive(true);
                slot.icon.enabled = true;
            }
            else
            {
                slot.gameObject.SetActive(false);
                slot.icon.enabled = false;
            }
        }
        Canvas.ForceUpdateCanvases();
    }

    public void UpdateLvText(int currentLV)
    {
        textArr[1].text = $"{currentLV}";
    }

    public void UpdateHpText(int currentHP, int maxHP)
    {
        textArr[2].text = $"{currentHP} / {maxHP}";
        UpdateHpBar(currentHP, maxHP);
    }

    public void UpdateExpText(float currentEXP, float maxEXP)
    {
        float Exp = (currentEXP / maxEXP) * 100f;
        textArr[3].text = $"{Exp.ToString("F2")} %";
        UpdateExpBar(currentEXP, maxEXP);
    }

    public void UpdateStageText(int currentStage)
    {
        textArr[4].text = $"{"Stage " + currentStage}";
    }

    public void UpdateGoldText(int currentGold)
    {
        textArr[5].text = $"{currentGold}";
    }

    public void UpdateCrystalText(int currentCrystal)
    {
        textArr[6].text = $"{currentCrystal}  °³";
    }

    public void UpdateHpBar(int currentHP, int maxHP)
    {
        HpBar.fillAmount = (float)currentHP / maxHP;
    }

    public void UpdateExpBar(float currentEXP, float maxEXP)
    {
        ExpBar.fillAmount = (float)currentEXP / maxEXP;
    }

    public void UpdateTargetUI(Transform target)
    {
        Player player = PlayerManager.Instance.Player;
        if (player != null)
        {
            TargetSystem targetingSystem = player.GetComponent<TargetSystem>();
            if (targetingSystem != null)
            {
                targetingSystem.UpdateTargetUI(target);
            }
        }
    }

    private IEnumerator DisableUserInvenWindowAfterInitialization()
    {
        yield return new WaitUntil(() => inventory != null && inventory.isInitialized);
        userInvenWindow.SetActive(false);
    }
}
