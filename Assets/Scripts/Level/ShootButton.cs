using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class ShootButton : Resetable, IInteractable
{
    public GameObject[] interactables;
    [SerializeField] SpriteRenderer eye;
    [SerializeField] Light2D eyeLight;
    [SerializeField] Light2D hitFX;
    private bool canInteract = true;

    void Awake()
    {
        hitFX.enabled = false;
        eyeLight.enabled = false;
        eye.color = Color.white;
    }
    public void Interact()
    {
        if (!canInteract) return;

        canInteract = false;
        eyeLight.enabled = true;
        eye.color = Color.red;
        StartCoroutine(Blink());
        foreach (var interactable in interactables)
        {
            interactable.GetComponent<IInteractable>().Interact();
        }
    }
    private IEnumerator Blink()
    {
        if (hitFX == null) yield break;
        hitFX.enabled = true;
        yield return new WaitForSeconds(0.1f);
        hitFX.enabled = false;
    }
    public override void OnReset()
    {
        StopAllCoroutines();
        canInteract = true;
        hitFX.enabled = false;
        eyeLight.enabled = false;
        eye.color = Color.white;
    }
}