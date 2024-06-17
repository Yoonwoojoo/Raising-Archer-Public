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
        public List<MonsterSpawnInfo> monsters; // 각 스테이지의 몬스터 스폰 정보 리스트
    }

    public List<SpawnArea> spawnAreas;
    public List<StageInfo> stages; // 각 스테이지 정보를 담는 리스트

    private int currentEnemyCount = 0;
    private List<MonsterController> monsters = new List<MonsterController>();

    private int currentStage = 0;
    private Dictionary<string, int> spawnedMonsterCount; // 각 몬스터 태그별 스폰된 몬스터 수를 추적

    private void Start()
    {
        Initialize();
        PlayerController.OnDeathEvent += OnPlayerDeath; // 플레이어 사망 이벤트 구독
    }

    private void OnDestroy()
    {
        PlayerController.OnDeathEvent -= OnPlayerDeath; // 플레이어 사망 이벤트 구독 해제
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
                UpdateStageUI(); // 스테이지 전환 시 UI 업데이트
            }
        }
    }

    private void UpdateStageUI()
    {
        UIManager.Instance.UpdateStageText(currentStage + 1);
    }

    private void OnPlayerDeath()
    {
        // 1초 후에 현재 스테이지를 다시 실행 (플레이어 dead애니메이션 타이밍 맞춘거)_
        Invoke("ResetStageAfterPlayerDeath", 1f);
    }

    private void ResetStageAfterPlayerDeath()
    {
        // 현재 스테이지 초기화
        currentEnemyCount = 0;
        spawnedMonsterCount.Clear();
        foreach (var monster in monsters)
        {
            Destroy(monster.gameObject);
        }
        monsters.Clear();

        // 플레이어를 현재 스테이지에 리젠
        RespawnPlayer();

        // 스테이지 다시 스폰
        SpawnMonstersInCurrentStage();
        UpdateStageUI();
    }

    private void RespawnPlayer()
    {
        // 플레이어 리젠 로직
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
        // 플레이어 리젠 위치 결정 (예시로 첫 스폰 지역의 중앙)
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

