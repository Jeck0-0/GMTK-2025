using UnityEngine;

public class LoopPortal : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Color colorA = Color.white;
    [SerializeField] Color colorB = Color.red;
    [SerializeField] AudioClip shotSound;

    private float totalLifetime;
    private float timeRemaining;

    private bool active = false;

    public void Initialize(float timeUntilShot)
    {
        LoopManager.Instance.OnGameReset += OnReset;
        totalLifetime = timeUntilShot;
        timeRemaining = timeUntilShot;
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
            if (LoopManager.Instance != null)
            LoopManager.Instance.OnGameReset -= OnReset;

            if (shotSound)
            AudioManager.Instance.PlaySound(shotSound, 0.8f, transform);

            Shaker.Instance.ShakeCamera(2f, 0.4f);

            Destroy(gameObject);
        }
    }
    private void OnReset()
    {
        if (LoopManager.Instance != null)
        LoopManager.Instance.OnGameReset -= OnReset;
        Destroy(gameObject);
    }
}