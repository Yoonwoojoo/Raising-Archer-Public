using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private Transform target;
    private AttackSO attackData;
    private TrailRenderer trailRenderer;

    private void Awake()
    {
        trailRenderer = GetComponent<TrailRenderer>();
    }

    public void Initialize(Transform target, AttackSO attackData)
    {
        this.target = target;
        this.attackData = attackData;
    }

    private void Update()
    {
        if (target == null)
        {
            DestroyProjectile();
            return;
        }

        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * attackData.projectileSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            HitTarget();
        }
    }

    private void HitTarget()
    {
        HealthSystem targetHealthSystem = target.GetComponent<HealthSystem>();
        if (targetHealthSystem != null)
        {
            targetHealthSystem.ChangeHealth(-attackData.attackDamage, attackData.stunTime);
        }
        DestroyProjectile();
    }

    private void DestroyProjectile()
    {
        // TrailRenderer�� null�� �ƴ��� Ȯ���ϰ�, null�� �ƴ� ��쿡�� ����
        if (trailRenderer != null)
        {
            // �ʿ�� TrailRenderer ���� �߰� �۾� ����
        }
        Destroy(gameObject);
    }
}
