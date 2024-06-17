using System;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public WeaponData weapon;
    public Button equipButton;

    private void Start()
    {
        ClearSlot();
        equipButton.onClick.AddListener(OnEquipButtonClicked);
    }

    public void AddItem(WeaponData newWeapon)
    {
        weapon = newWeapon;
        icon.sprite = weapon.weaponSprite;
        icon.enabled = true;
        equipButton.gameObject.SetActive(true);
    }

    public void ClearSlot()
    {
        weapon = null;
        icon.sprite = null;
        icon.enabled = false;
        equipButton.gameObject.SetActive(false);
    }

    public void OnSlotClicked()
    {
        Inventory.instance.SelectSlot(this);
    }

    public void OnEquipButtonClicked()
    {
        if (weapon == null)
        {
            return;
        }

        PlayerManager.Instance.Player.EquipWeapon(weapon);
    }
}
