using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedActivate : Resetable, IInteractable
{
    public List<GameObject> interactables;

    public float duration = 1;
    private bool active;

    public bool disableInteractablesOnReset = true;
    
    private Coroutine resetCoroutine;
    
    public void Interact()
    {
        if(active)
            return;
        interactables.ForEach(interactable => interactable.GetComponent<IInteractable>()?.Interact());

        StartCoroutine(DisableAfterTime());
    }

    IEnumerator DisableAfterTime()
    {
        yield return new WaitForSeconds(duration);
        active = false;
        interactables.ForEach(x => x.GetComponent<IInteractable>()?.ResetObject());
    }
    
    public void ResetObject()
    {
        if(resetCoroutine != null)
            StopCoroutine(resetCoroutine);
        if(disableInteractablesOnReset)
            interactables.ForEach(x => x.GetComponent<IInteractable>()?.ResetObject());
    }
}