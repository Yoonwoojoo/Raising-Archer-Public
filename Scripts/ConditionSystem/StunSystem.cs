using System;
using System.Collections;
using UnityEngine;

public class StunSystem : MonoBehaviour
{
    public bool isStunned { get; private set; } = false;
    public event Action<bool> OnStunChanged;

    private void Awake()
    {
        
    }

    public void ApplyStun(float stunTime)
    {
        if (stunTime > 0f)
        {
            StartCoroutine(StunCoroutine(stunTime));
        }
    }

    private IEnumerator StunCoroutine(float stunTime)
    {
        isStunned = true;
        OnStunChanged?.Invoke(isStunned);

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }

        yield return new WaitForSeconds(stunTime);

        isStunned = false;
        OnStunChanged?.Invoke(isStunned);
    }
}