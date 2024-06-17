using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    void Awake()
    {
        if (PlayerManager.Instance == null)
        {
            GameObject newGameObject = new GameObject("PlayerManager");
            newGameObject.AddComponent<PlayerManager>();
        }
    }
}

