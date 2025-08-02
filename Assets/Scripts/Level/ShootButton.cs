using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ShootButton : MonoBehaviour, IInteractable
{
    [SerializeField] IInteractable[] interactables;
    [SerializeField] SpriteRenderer eye;
    [SerializeField] Light2D eyeLight;

    void Start()
    {
        eyeLight.enabled = false;
        eye.color = Color.white;
    }
    public void Interact()
    {
        eyeLight.enabled = true;
        eye.color = Color.red;
        foreach (var interactable in interactables)
        {
            interactable.Interact();
        }
    }
}