using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject VictoryCollider;
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
        VictoryCollider.SetActive(false);
    }
    public void Interact()
    {
        anim.SetBool("Open", true);
        VictoryCollider.SetActive(true);
    }
}