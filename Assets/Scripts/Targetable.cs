using Sirenix.OdinInspector;
using UnityEngine;

public class Targetable : MonoBehaviour
{
    [DisableInEditorMode] public float currentHealth;
    public float maxHealth = 100;
    public bool isDead;
    public bool isVulnerable = true;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
    }

    //deals damage to this unit and checks if it's dead
    public void Damage(float amount)
    {
        if (isDead || !isVulnerable)
        return;

        currentHealth -= amount;

        if (currentHealth <= 0)
        Die();
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }
}
