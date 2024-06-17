using UnityEngine;

[System.Serializable]
public class PlayerStats
{
    [HideInInspector][Range(1, 5000)] public int maxHP;
    [HideInInspector][Range(1f, 20f)] public float moveSpeed;
    [HideInInspector][Range(1, 1000)] public int level;
    [HideInInspector][Range(0f, 999999999)] public float experience;
    [HideInInspector][Range(0f, 999999999)] public int gold;
    [HideInInspector][Range(0f, 999999)] public int crystal;
    public AttackSO attackSO;

    public void Initialize(AttackSO baseAttackSO)
    {
        attackSO = baseAttackSO;
        attackSO.Initialize();

        level = 1;
        maxHP = 50;
        moveSpeed = 3f;
        experience = 0;
        gold = 0;
        crystal = 0;
    }
}
