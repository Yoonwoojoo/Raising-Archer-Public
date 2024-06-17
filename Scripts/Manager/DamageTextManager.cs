using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageTextManager : GenericSingleton<DamageTextManager>
{
    public GameObject damageTextPrefab;
    public int poolSize;
    public Color playerDamageColor;
    public Color monsterDamageColor; 

    private Queue<GameObject> textPool;

    private void Start()
    {
        textPool = new Queue<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(damageTextPrefab);
            obj.SetActive(false);
            textPool.Enqueue(obj);
        }
    }

    public void ShowDamage(Vector3 position, int damage, bool isPlayer)
    {
        if (textPool.Count > 0)
        {
            GameObject dmgText = textPool.Dequeue();
            dmgText.transform.position = position;
            TextMeshProUGUI textMesh = dmgText.GetComponentInChildren<TextMeshProUGUI>();
            textMesh.text = damage.ToString();
            textMesh.color = isPlayer ? monsterDamageColor : playerDamageColor; // 플레이어와 몬스터 색상 지정
            dmgText.SetActive(true);
            StartCoroutine(AnimateAndDisable(dmgText, 1.0f));
        }
    }

    private IEnumerator AnimateAndDisable(GameObject obj, float time)
    {
        Vector3 startPosition = obj.transform.position;
        Vector3 endPosition = startPosition + new Vector3(0, 2, 0);

        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            obj.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        obj.SetActive(false);
        textPool.Enqueue(obj);
    }
}
