using UnityEngine;

public class DelayforHint : MonoBehaviour, IInteractable // lazy script
{
    [SerializeField] float delay = 7f;
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
        Invoke("Delay", delay);
    }

    private void Delay()
    {
        spriteRenderer.enabled = true;
    }

    public void Interact()
    {
        Destroy(gameObject);
    }
    public void ResetObject() { }
}