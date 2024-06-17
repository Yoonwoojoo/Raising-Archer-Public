using UnityEngine;

public class MonsterMechanism : MonoBehaviour
{
    private MonsterController monsterController;

    private void Awake()
    {
        monsterController = GetComponent<MonsterController>();
    }

    public void FindTarget()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            monsterController.target = player.transform;
        }
    }
}