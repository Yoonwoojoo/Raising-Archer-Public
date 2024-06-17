using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NicknameField : MonoBehaviour
{
    public TMP_InputField InputNameField;
    public Button loginButton;

    private void Start()
    {
        InputNameField.onValueChanged.AddListener(Check); // 입력 값이 변경될 때마다 이벤트를 호출할 것이다
        loginButton.interactable = false;  // 초기에는 로그인 버튼을 비활성화
    }

    private void Check(string inputText)
    {
        // 텍스트가 비어있는지 확인
        bool hasText = !string.IsNullOrEmpty(inputText);

        // 이름의 길이가 2~8 인지 확인
        bool nameLength = inputText.Length >= 2 && inputText.Length <= 8;

        // 위 두가지 조건이 충족되었다면, 로그인 버튼을 활성화
        loginButton.interactable = hasText && nameLength;
    }
}
