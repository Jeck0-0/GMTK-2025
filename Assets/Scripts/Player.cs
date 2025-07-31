using System;
using UnityEngine;

public class Player : Unit
{
    public static Player instance;

    protected override void Awake()
    {
        if (instance)
        {
            Debug.LogError("Duplicate player instance");
            Destroy(gameObject);
            return;
        }
        instance = this;
        
        base.Awake();
    }

    private void Update()
    {
        weapon.transform.right = (Helpers.Camera.ScreenToWorldPoint(
            Input.mousePosition).XY() - transform.position.XY()).normalized;
        
        if(Input.GetMouseButton(0))
            TryAttacking();
    }

    public override void Die()
    {
        gameObject.SetActive(false);
    }
}
