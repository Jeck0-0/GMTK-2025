using UnityEngine;

public class MuzzleFlash : MonoBehaviour // to easily add more FX later
{
    [SerializeField] Animator animator;
    public void Flash()
    {
        animator.SetTrigger("Flash");
    }
}
