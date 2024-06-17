using System;
using UnityEngine;

public class ExpSystem : MonoBehaviour
{
    public event Action OnLevelUp;

    private PlayerStatsHandler statsHandler;

    private void Awake()
    {
        statsHandler = GetComponent<PlayerStatsHandler>();
    }

    private void Start()
    {
        if (UIManager.Instance != null)
        {
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateExpText(statsHandler.currentStats.experience, ExperienceToNextLevel());
        }
    }

    public void AddExperience(int amount)
    {
        statsHandler.currentStats.experience += amount;

        if (statsHandler.currentStats.experience >= ExperienceToNextLevel())
        {
            LevelUp();
        }

        UpdateUI();
    }

    private void LevelUp()
    {
        statsHandler.currentStats.level++;
        statsHandler.currentStats.experience = 0;
        statsHandler.IncreaseStats(); // ������ �� ���� ���� �޼��� ȣ��
        OnLevelUp?.Invoke();
        UpdateUI();
    }

    public int ExperienceToNextLevel() // �������� �ʿ��� ����ġ ��� ����
    {
        return statsHandler.currentStats.level * 500; // ���÷� ������ 500 ����ġ �ʿ�
    }
}
