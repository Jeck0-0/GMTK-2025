using UnityEngine;

public class Door : Resetable, IInteractable
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
    public void ResetObject()
    {
        anim.SetBool("Open", false);
        VictoryCollider.SetActive(false);
    }
    public override void OnReset()
    {
        base.OnReset();
        if (open) return;

        VictoryCollider.SetActive(false);
        anim.SetBool("Open", false);
        anim.SetTrigger("InstaClose");
        StopAllCoroutines();
    }
}