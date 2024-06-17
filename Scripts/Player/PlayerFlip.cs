using UnityEngine;

public class PlayerFlip : MonoBehaviour
{
    [SerializeField] private Transform spriteOfTransform;

    public void FlipSprite()
    {
        if (spriteOfTransform != null)
        {
            Vector2 newScale = spriteOfTransform.localScale;
            newScale.x *= -1;
            spriteOfTransform.localScale = newScale;
        }
    }
}
