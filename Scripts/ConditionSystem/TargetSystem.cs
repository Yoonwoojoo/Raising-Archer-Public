using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TargetSystem : MonoBehaviour
{
    public PlayerController playerController;
    public TMP_Text targetInfoText; // 타겟 정보를 표시할 UI 텍스트
    public RectTransform targetSignal; // 타겟팅 신호
    public GameObject targetData; // 타겟 데이터를 표시할 오브젝트
    public TMP_Text targetName; // 타겟 이름 표시
    public Image targetHpBar; // 체력바 이미지

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