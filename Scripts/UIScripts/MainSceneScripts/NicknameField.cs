using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NicknameField : MonoBehaviour
{
    public TMP_InputField InputNameField;
    public Button loginButton;

    private void Start()
    {
        InputNameField.onValueChanged.AddListener(Check); // �Է� ���� ����� ������ �̺�Ʈ�� ȣ���� ���̴�
        loginButton.interactable = false;  // �ʱ⿡�� �α��� ��ư�� ��Ȱ��ȭ
    }

    private void Check(string inputText)
    {
        // �ؽ�Ʈ�� ����ִ��� Ȯ��
        bool hasText = !string.IsNullOrEmpty(inputText);

        // �̸��� ���̰� 2~8 ���� Ȯ��
        bool nameLength = inputText.Length >= 2 && inputText.Length <= 8;

        // �� �ΰ��� ������ �����Ǿ��ٸ�, �α��� ��ư�� Ȱ��ȭ
        loginButton.interactable = hasText && nameLength;
    }
}
