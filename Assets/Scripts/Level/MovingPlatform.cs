using System;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : Resetable, IInteractable
{
    public Transform moveObject;
    
    public List<Transform> targets = new();

    private int currentIndex;
    public bool activated = false;
    public bool loop = true;
    public float stopDuration;
    public float speed;

    private void Awake()
    {
        if(!moveObject)
            moveObject = transform;
    }

    private void Update()
    {
        if (activated)
        {
            Move();
        }
    }


    private float resumeAt;
    protected void Move()
    {
        if (currentIndex >= targets.Count)
            return;
        
        if(Time.time < resumeAt)
            return;
            
        var dir = (targets[currentIndex].position - moveObject.position).normalized;
        var moveBy = speed * Time.deltaTime;
        var dist = Vector3.Distance(moveObject.position, targets[currentIndex].position);

        if (dist <= moveBy)
        {
            moveObject.position = targets[currentIndex].position;

            if (loop)
                currentIndex = (currentIndex + 1) % targets.Count;
            else
                currentIndex++;
            
            resumeAt = Time.time + stopDuration;
            return;
        }

        moveObject.Translate(dir * moveBy, Space.World);
    }


    public void Interact()
    {
        activated = true;
    }

    public void ResetObject()
    {
        activated = false;
        currentIndex = 0;
    }
}
