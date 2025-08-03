using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class ShootButton : Resetable, IInteractable
{
    public GameObject[] interactables;
    [SerializeField] SpriteRenderer eye;
    [SerializeField] Light2D eyeLight;
    [SerializeField] Light2D hitFX;
    [SerializeField] bool resetAfterTime;
    [SerializeField] float timeToReset;
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

        if (resetAfterTime)
            StartCoroutine(ResetAfterTime());
    }


    private float resetTime;
    private float flickerTime;
    private bool flickerOn;
    IEnumerator ResetAfterTime()
    {
        resetTime = Time.time + timeToReset;
        flickerTime = Time.time + .2f;
        while (Time.time < resetTime)
        {
            if (Time.time >= flickerTime)
            {
                eyeLight.enabled = flickerOn;
                eye.color = flickerOn ? Color.red : Color.gray3;
                flickerOn = !flickerOn;
                flickerTime = Time.time + .2f;
            }
            yield return null;
        }
        
        canInteract = true;
        eyeLight.enabled = false;
        eye.color = Color.white;

        foreach (var interactable in interactables)
        {
            interactable.GetComponent<IInteractable>().ResetObject();
        }
    }
    
    
    private IEnumerator Blink()
    {
        if (hitFX == null) yield break;
        hitFX.enabled = true;
        yield return new WaitForSeconds(0.1f);
        hitFX.enabled = false;
    }
    
    public void ResetObject()
    {
        OnReset();
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