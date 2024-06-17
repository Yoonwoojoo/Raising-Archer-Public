using System;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerController playerController;
    private PlayerStatsHandler playerStatsHandler;
    private float timeSinceLastAttack = 0;

    public GameObject projectilePrefab;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerStatsHandler = GetComponent<PlayerStatsHandler>();
    }

    private void Start()
    {
        playerController.OnAttackEvent += HandleAttack;
    }

    private void Update()
    {
        if (timeSinceLastAttack < playerStatsHandler.currentStats.attackSO.attackDelay)
        {
            timeSinceLastAttack += Time.deltaTime;
        }
    }

    public void HandleAttack(AttackSO attackSO)
    {
        if (playerController.stunSystem.isStunned)
        {
            playerController.EndAttack();
            return;
        }

        if (timeSinceLastAttack < playerStatsHandler.currentStats.attackSO.attackDelay)
        {
            playerController.EndAttack();
            return;
        }

        if (playerController.target == null)
        {
            return;
        }

        if (playerController.IsTargetInLayer() && playerController.IsTargetInRange())
        {
            timeSinceLastAttack = 0f;
            playerController.LookAtTarget();

            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            ProjectileController projectileController = projectile.GetComponent<ProjectileController>();
            if (projectileController != null)
            {
                projectileController.Initialize(playerController.target, attackSO);
            }

            playerController.EndAttack();
        }
    }
}

