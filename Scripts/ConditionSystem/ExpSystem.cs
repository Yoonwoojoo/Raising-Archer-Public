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
        statsHandler.IncreaseStats(); // 레벨업 시 스탯 증가 메서드 호출
        OnLevelUp?.Invoke();
        UpdateUI();
    }

    public int ExperienceToNextLevel() // 레벨업에 필요한 경험치 계산 로직
    {
        return statsHandler.currentStats.level * 500; // 예시로 레벨당 500 경험치 필요
    }
}
