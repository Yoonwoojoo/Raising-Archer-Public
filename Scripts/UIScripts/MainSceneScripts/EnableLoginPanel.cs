using UnityEngine;
using TMPro;

public class EnableLoginPanel : MonoBehaviour
{
    public GameObject gameboject;

    public void EnableField()
    {
        gameboject.SetActive(true);
    }
}