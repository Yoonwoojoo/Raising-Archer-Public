using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponGachaSystem : MonoBehaviour
{
    public WeaponList weaponList;
    public Button gachaButton;
    public Image[] resultImages;
    public Sprite ggwangSprite;
    public int gachaCost = 2000;
    public float spinDuration = 3.0f;
    public float revealDelay = 0.5f;
    public TMP_Text feedbackText;

    private Player player;

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => UIManager.Instance.inventory != null);

        gachaButton.onClick.AddListener(OnGachaButtonClicked);
        player = PlayerManager.Instance.Player;

        ClearInventorySlots();
        feedbackText.gameObject.SetActive(false);
    }

    private void ClearInventorySlots()
    {
        Inventory inventory = UIManager.Instance.inventory;
        foreach (var slot in inventory.slots)
        {
            slot.ClearSlot();
        }
    }

    private void OnGachaButtonClicked()
    {
        if (player.playerStatsHandler.SpendGold(gachaCost))
        {
            StartCoroutine(SpinAndDisplayResults());
        }
    }

    private IEnumerator SpinAndDisplayResults()
    {
        List<WeaponData> selectedWeapons = GetRandomWeapons(6);
        float elapsedTime = 0f;

        while (elapsedTime < spinDuration)
        {
            elapsedTime += Time.deltaTime;
            for (int i = 0; i < resultImages.Length; i++)
            {
                resultImages[i].transform.Rotate(0, 0, 360 * Time.deltaTime);
            }
            yield return null;
        }

        bool inventoryFull = false;
        for (int i = 0; i < resultImages.Length; i++)
        {
            yield return new WaitForSeconds(revealDelay);
            resultImages[i].transform.rotation = Quaternion.identity;
            if (selectedWeapons[i] != null)
            {
                resultImages[i].sprite = selectedWeapons[i].weaponSprite;
                resultImages[i].enabled = true;

                bool added = UIManager.Instance.inventory.Add(selectedWeapons[i]);
                if (!added && !inventoryFull)
                {
                    inventoryFull = true;
                }
            }
            else
            {
                resultImages[i].sprite = ggwangSprite;
                resultImages[i].enabled = true;
            }
        }

        if (inventoryFull)
        {
            feedbackText.text = "인벤토리가 가득 찼습니다!";
            feedbackText.gameObject.SetActive(true);
        }

        UIManager.Instance.RefreshUI();
        player.UpdateAllUI();
    }

    private List<WeaponData> GetRandomWeapons(int count)
    {
        List<WeaponData> selectedWeapons = new List<WeaponData>();

        for (int i = 0; i < count; i++)
        {
            bool weaponSelected = false;

            foreach (var weapon in weaponList.weapons)
            {
                if (Random.value <= weapon.gachaRate / 100f)
                {
                    if (weapon.weaponSprite != null)
                    {
                        selectedWeapons.Add(weapon);
                        weaponSelected = true;
                    }
                    break;
                }
            }
            if (!weaponSelected)
            {
                selectedWeapons.Add(null);
            }
        }

        return selectedWeapons;
    }
}
