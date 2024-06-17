using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagerManager : GenericSingleton<StagerManager>
{
    [System.Serializable]
    public class SpawnArea
    {
        public Vector2 minPlace;
        public Vector2 maxPlace;
    }

    [System.Serializable]
    public class MonsterSpawnInfo
    {
        public string tag;
        public int count;
        public GameObject prefab;
    }

    [System.Serializable]
    public class StageInfo
    {
        public List<MonsterSpawnInfo> monsters; // �� ���������� ���� ���� ���� ����Ʈ
    }

    public List<SpawnArea> spawnAreas;
    public List<StageInfo> stages; // �� �������� ������ ��� ����Ʈ

    private int currentEnemyCount = 0;
    private List<MonsterController> monsters = new List<MonsterController>();

    private int currentStage = 0;
    private Dictionary<string, int> spawnedMonsterCount; // �� ���� �±׺� ������ ���� ���� ����

    private void Start()
    {
        Initialize();
        PlayerController.OnDeathEvent += OnPlayerDeath; // �÷��̾� ��� �̺�Ʈ ����
    }

    private void OnDestroy()
    {
        PlayerController.OnDeathEvent -= OnPlayerDeath; // �÷��̾� ��� �̺�Ʈ ���� ����
    }

    public void Initialize()
    {
        spawnedMonsterCount = new Dictionary<string, int>();
        StartCoroutine(SpawnMonster());
        UpdateStageUI();
    }

    private IEnumerator SpawnMonster()
    {
        while (true)
        {
            SpawnMonstersInCurrentStage();
            yield return new WaitForSeconds(1f);
        }
    }

    private void SpawnMonstersInCurrentStage()
    {
        if (spawnAreas.Count == 0 || stages.Count <= currentStage)
        {
            return;
        }

        StageInfo currentStageInfo = stages[currentStage];

        foreach (var monsterInfo in currentStageInfo.monsters)
        {
            if (!spawnedMonsterCount.ContainsKey(monsterInfo.tag))
            {
                spawnedMonsterCount[monsterInfo.tag] = 0;
            }

            while (spawnedMonsterCount[monsterInfo.tag] < monsterInfo.count)
            {
                SpawnMonster(monsterInfo);
                spawnedMonsterCount[monsterInfo.tag]++;
                currentEnemyCount++;
            }
        }

        CheckStageTransition();
    }

    private void SpawnMonster(MonsterSpawnInfo monsterInfo)
    {
        int areaIndex = Random.Range(0, spawnAreas.Count);
        SpawnArea fixedArea = spawnAreas[areaIndex];
        Vector2 randomPosition = new Vector2(
            Random.Range(fixedArea.minPlace.x, fixedArea.maxPlace.x),
            Random.Range(fixedArea.minPlace.y, fixedArea.maxPlace.y)
        );

        GameObject stageMonster = Instantiate(monsterInfo.prefab, randomPosition, Quaternion.identity);
        stageMonster.name = monsterInfo.tag;

        MonsterController monsterController = stageMonster.GetComponent<MonsterController>();
        if (monsterController == null)
        {
            return;
        }

        monsterController.OnObjectSpawn();
        AddMonster(monsterController);
    }

    private void AddMonster(MonsterController monsterController)
    {
        if (!monsters.Contains(monsterController))
        {
            monsters.Add(monsterController);
            monsterController.OnDeathEvent += RemoveMonster;
        }
    }

    private void RemoveMonster(MonsterController monsterController)
    {
        if (monsters.Contains(monsterController))
        {
            monsters.Remove(monsterController);
            currentEnemyCount--;
            CheckStageTransition();
        }
    }

    private void CheckStageTransition()
    {
        bool allMonstersSpawned = true;

        foreach (var monsterInfo in stages[currentStage].monsters)
        {
            if (spawnedMonsterCount[monsterInfo.tag] < monsterInfo.count)
            {
                allMonstersSpawned = false;
                break;
            }
        }

        if (allMonstersSpawned && currentEnemyCount == 0)
        {
            currentStage++;
            if (currentStage >= stages.Count)
            {
                StopAllCoroutines();
            }
            else
            {
                currentEnemyCount = 0;
                spawnedMonsterCount.Clear();
                UpdateStageUI(); // �������� ��ȯ �� UI ������Ʈ
            }
        }
    }

    private void UpdateStageUI()
    {
        UIManager.Instance.UpdateStageText(currentStage + 1);
    }

    private void OnPlayerDeath()
    {
        // 1�� �Ŀ� ���� ���������� �ٽ� ���� (�÷��̾� dead�ִϸ��̼� Ÿ�̹� �����)_
        Invoke("ResetStageAfterPlayerDeath", 1f);
    }

    private void ResetStageAfterPlayerDeath()
    {
        // ���� �������� �ʱ�ȭ
        currentEnemyCount = 0;
        spawnedMonsterCount.Clear();
        foreach (var monster in monsters)
        {
            Destroy(monster.gameObject);
        }
        monsters.Clear();

        // �÷��̾ ���� ���������� ����
        RespawnPlayer();

        // �������� �ٽ� ����
        SpawnMonstersInCurrentStage();
        UpdateStageUI();
    }

    private void RespawnPlayer()
    {
        // �÷��̾� ���� ����
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.transform.position = GetPlayerRespawnPosition();
            player.healthSystem.ResetHealth();
            player.isDead = false;
        }
    }

    private Vector2 GetPlayerRespawnPosition()
    {
        // �÷��̾� ���� ��ġ ���� (���÷� ù ���� ������ �߾�)
        if (spawnAreas.Count > 0)
        {
            SpawnArea firstArea = spawnAreas[0];
            return new Vector2(
                (firstArea.minPlace.x + firstArea.maxPlace.x) / 2,
                (firstArea.minPlace.y + firstArea.maxPlace.y) / 2
            );
        }

        return Vector2.zero;
    }
}

