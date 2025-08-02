using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject VictoryCollider;
    [SerializeField] bool open;
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
        if (open)
        {
            anim.SetBool("Open", true);
            return;
        }
        VictoryCollider.SetActive(false);
    }
    public void Interact()
    {
        anim.SetBool("Open", true);
        VictoryCollider.SetActive(true);
    }
}