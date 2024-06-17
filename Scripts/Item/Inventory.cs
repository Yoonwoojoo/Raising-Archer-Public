using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public InventorySlot[] slots;
    private InventorySlot selectedSlot;
    public bool isInitialized { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        InitializeSlots();
    }

    private void InitializeSlots()
    {
        foreach (var slot in slots)
        {
            slot.gameObject.SetActive(true);
            slot.ClearSlot();
        }
        isInitialized = true;
    }

    public bool Add(WeaponData weapon)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (!slots[i].icon.enabled)
            {
                slots[i].AddItem(weapon);
                UIManager.Instance.RefreshUI();
                return true;
            }
        }
        return false;
    }

    public void SelectSlot(InventorySlot slot)
    {
        if (selectedSlot != slot)
        {
            selectedSlot = slot;
            UIManager.Instance.ClearWeaponInfo();
            UIManager.Instance.UpdateWeaponInfo(selectedSlot.weapon);
            UIManager.Instance.weaponInfoPanel.SetActive(true);
        }
        else
        {
            bool isActive = UIManager.Instance.weaponInfoPanel.activeSelf;
            UIManager.Instance.weaponInfoPanel.SetActive(!isActive);
        }
    }
}
