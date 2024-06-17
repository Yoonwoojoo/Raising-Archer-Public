using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TargetSystem : MonoBehaviour
{
    public PlayerController playerController;
    public TMP_Text targetInfoText; // Ÿ�� ������ ǥ���� UI �ؽ�Ʈ
    public RectTransform targetSignal; // Ÿ���� ��ȣ
    public GameObject targetData; // Ÿ�� �����͸� ǥ���� ������Ʈ
    public TMP_Text targetName; // Ÿ�� �̸� ǥ��
    public Image targetHpBar; // ü�¹� �̹���

    private void Awake()
    {
        if (playerController != null)
        {
            playerController.OnTargetChanged += UpdateTargetUI;
        }
    }

    private void OnDestroy()
    {
        if (playerController != null)
        {
            playerController.OnTargetChanged -= UpdateTargetUI;
        }
    }

    private void Update()
    {
        Transform target = playerController.target;

        if (target != null)
        {
            UpdateTargetUI(target);
        }
        else
        {
            ClearTargetUI();
        }
    }

    public void UpdateTargetUI(Transform target)
    {
        if (target == null)
        {
            ClearTargetUI();
            return;
        }

        targetInfoText.text = target.name;

        Vector3 targetPosition = target.position;
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetPosition);

        targetSignal.position = screenPosition + new Vector3(0, -100, 0);
        targetSignal.gameObject.SetActive(true);

        targetData.SetActive(true);
        targetData.transform.position = screenPosition + new Vector3(0, 20, 0);

        targetName.text = target.name;

        HealthSystem targetHealth = target.GetComponent<HealthSystem>();
        if (targetHealth != null)
        {
            targetHpBar.fillAmount = (float)targetHealth.currentHP / targetHealth.maxHP;
            targetHpBar.gameObject.SetActive(true);
        }
    }

    private void ClearTargetUI()
    {
        targetInfoText.text = "";
        targetSignal.gameObject.SetActive(false);
        targetData.SetActive(false);
    }
}