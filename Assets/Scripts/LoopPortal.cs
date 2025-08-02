using System.Collections;
using UnityEngine;

public class LoopPortal : MonoBehaviour
{
    [Header("References")]
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] SpriteRenderer portalRenderer;
    [SerializeField] GameObject bulletPrefab;

    [Header("Visuals")]
    [SerializeField] Color colorA = Color.white;
    [SerializeField] Color colorB = Color.red;
    [SerializeField] Sprite[] portalSprites;
    [SerializeField] float timeToChangeSprite = 0.5f;
    [SerializeField] float portalRevealTime = 1f;

    [Header("Pulse Settings")]
    [SerializeField] float scaleMin = 0f;
    [SerializeField] float scaleMax = 0.8f;

    [Header("Audio")]
    [SerializeField] AudioClip shotSound;

    private float totalLifetime;
    private float timeRemaining;
    private float spriteTimer;
    private int currentSpriteIndex;
    private Vector2 direction;

    public void Initialize(float timeUntilShot, Vector2 dir)
    {
        LoopManager.Instance.OnGameReset += OnReset;
        totalLifetime = timeUntilShot;
        timeRemaining = timeUntilShot;
        direction = dir.normalized;

        portalRenderer.enabled = false;
        transform.localScale = Vector3.one;
    }

    private void Update()
    {
        AnimateSprite();

        if (timeRemaining <= 0f)
        return;

        timeRemaining -= Time.deltaTime;
        float t = Mathf.Clamp01(1f - (timeRemaining / totalLifetime));
        spriteRenderer.color = Color.Lerp(colorA, colorB, t);

        if (timeRemaining <= 0f)
        {
            FireBullet();

            StartCoroutine(PulseScale());
        }
    }

    private void AnimateSprite()
    {
        if (portalSprites == null || portalSprites.Length == 0) return;

        spriteTimer += Time.deltaTime;
        if (spriteTimer >= timeToChangeSprite)
        {
            spriteTimer = 0f;
            currentSpriteIndex = (currentSpriteIndex + 1) % portalSprites.Length;
            portalRenderer.sprite = portalSprites[currentSpriteIndex];
        }
    }

    private IEnumerator PulseScale()
    {
        spriteRenderer.enabled = false;
        portalRenderer.enabled = true;

        spriteTimer += Time.deltaTime;
        if (spriteTimer >= timeToChangeSprite)
        {
            spriteTimer = 0f;
            currentSpriteIndex = (currentSpriteIndex + 1) % portalSprites.Length;
            portalRenderer.sprite = portalSprites[currentSpriteIndex];
        }

        float elapsed = 0f;
        float halfDuration = portalRevealTime / 2f;

        // Scale up
        while (elapsed < halfDuration)
        {
            float t = elapsed / halfDuration;
            float scale = Mathf.Lerp(scaleMin, scaleMax, t);
            portalRenderer.transform.localScale = new Vector3(scale, scale, 1f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        elapsed = 0f;

        // Scale down
        while (elapsed < halfDuration)
        {
            float t = elapsed / halfDuration;
            float scale = Mathf.Lerp(scaleMax, scaleMin, t);
            portalRenderer.transform.localScale = new Vector3(scale, scale, 1f);

            // Animate sprite
            spriteTimer += Time.deltaTime;
            if (spriteTimer >= timeToChangeSprite)
            {
                spriteTimer = 0f;
                currentSpriteIndex = (currentSpriteIndex + 1) % portalSprites.Length;
                portalRenderer.sprite = portalSprites[currentSpriteIndex];
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        portalRenderer.transform.localScale = Vector3.zero;
        DestroyPortal();
    }
    private void FireBullet()
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

        GameObject bullet = Instantiate(bulletPrefab, transform.position, rotation);

        if (bullet.TryGetComponent(out Loopbullet proj))
        {
            proj.Initialize(damage: 100f, speed: 16f, duration: 5f);
        }

        if (shotSound)
        AudioManager.Instance.PlaySound(shotSound, 0.7f, transform);

        Shaker.Instance.ShakeCamera(2f, 0.4f);
    }

    private void DestroyPortal()
    {
        if (LoopManager.Instance != null)
        LoopManager.Instance.OnGameReset -= OnReset;
        Destroy(gameObject);
    }

    private void OnReset()
    {
        DestroyPortal();
    }
}