using System.Collections.Generic;
using UnityEngine;

public class InteractableGroup : Resetable, IInteractable
{
    public int requiredAmount = 2;
    public List<GameObject> interactables = new();
    
    int activated;

    
    public void Interact()
    {
        activated++;
        if (activated >= requiredAmount)
        {
            foreach (var interactable in interactables)
            {
                interactable.GetComponent<IInteractable>().Interact();
            }
        }
    }

    public void ResetObject()
    {
        activated--;
        if(activated < requiredAmount)
            interactables.ForEach(interactable => interactable.GetComponent<IInteractable>()?.ResetObject());
    }

    public override void OnReset()
    {
        activated = 0;
        base.OnReset();
    }
}
