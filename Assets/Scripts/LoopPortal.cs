using UnityEngine;

public class LoopPortal : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Color colorA = Color.white;
    [SerializeField] Color colorB = Color.red;
    [SerializeField] AudioClip shotSound;
    [SerializeField] GameObject bulletPrefab;

    private float totalLifetime;
    private float timeRemaining;
    private Vector2 direction;

    private bool active = false;

    public void Initialize(float timeUntilShot, Vector2 dir)
    {
        LoopManager.Instance.OnGameReset += OnReset;

        totalLifetime = timeUntilShot;
        timeRemaining = timeUntilShot;
        direction = dir.normalized;

        active = true;
    }

    void Update()
    {
        if (!active) return;

        timeRemaining -= Time.deltaTime;
        float t = Mathf.Clamp01(1f - (timeRemaining / totalLifetime));
        spriteRenderer.color = Color.Lerp(colorA, colorB, t);

        if (timeRemaining <= 0f)
        {
            LoopManager.Instance.OnGameReset -= OnReset;

            FireBullet();
            Destroy(gameObject);
        }
    }

    private void FireBullet()
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        GameObject bullet = Instantiate(bulletPrefab, transform.position, rotation);

        if (bullet.TryGetComponent(out Loopbullet proj))
        {
            proj.Initialize(damage: 100f, speed: 16f, duration: 5f); // Customize as needed
        }

        if (shotSound)
        AudioManager.Instance.PlaySound(shotSound, 0.7f, transform);

        Shaker.Instance.ShakeCamera(2f, 0.4f);
    }

    private void OnReset()
    {
        LoopManager.Instance.OnGameReset -= OnReset;
        Destroy(gameObject);
    }
}