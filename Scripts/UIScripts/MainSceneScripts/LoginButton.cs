using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginButton : MonoBehaviour
{
    public TMP_Text nickname;

    public void OnLoginButtonClick()
    {
        DataManager.Instance.SetPlayerName(nickname.text);
        SceneManager.LoadScene("GameScene");
    }
}