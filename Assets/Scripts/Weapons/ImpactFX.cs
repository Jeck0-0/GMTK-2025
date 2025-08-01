using UnityEngine;

public class ImpactFX : MonoBehaviour
{
    [SerializeField] float timeToChangeSprite = 0.05f;
    [SerializeField] Sprite[] sprites;
    [SerializeField] SpriteRenderer spriteRenderer;
    private int currentSpriteIndex = 0;
    private float spriteTimer = 0f;
    void Update()
    {
        if (sprites == null || sprites.Length == 0 || spriteRenderer == null) return;

        spriteTimer += Time.deltaTime;
        if (spriteTimer >= timeToChangeSprite)
        {
            spriteTimer = 0f;
            currentSpriteIndex = (currentSpriteIndex + 1);
            if (currentSpriteIndex >= sprites.Length)
            {
                Destroy(gameObject);
                return;
            }
            spriteRenderer.sprite = sprites[currentSpriteIndex];
        }
    }
}
