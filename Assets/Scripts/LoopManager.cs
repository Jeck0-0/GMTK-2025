using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class LoopManager : Singleton<LoopManager>
{
    [ShowInInspector] public static List<ShotRecord> previousShots = new();
    public Loopgun loopgun;
    public GameObject gunGfx;
    
    public float previewTime = 1;
    
    [ShowInInspector] protected List<ShotRecord> shotsLeft = new();
    protected ShotRecord nextShot;
    protected ShotRecord currentlyPreviewing;
    
    protected override void Awake()
    {
        base.Awake();
        shotsLeft = new List<ShotRecord>(previousShots);
        nextShot = shotsLeft.OrderBy(x => x.timeSinceStart).FirstOrDefault();
    }

    private void Start()
    {
        gunGfx.SetActive(false);
    }

    void Update()
    {
        // fire shots
        for (int i = shotsLeft.Count - 1; i >= 0; i--)
        {
            if (Time.timeSinceLevelLoad > shotsLeft[i].timeSinceStart)
            {
                FireBullet(shotsLeft[i].position, shotsLeft[i].direction);
                shotsLeft.RemoveAt(i);
                nextShot = shotsLeft.OrderBy(x => x.timeSinceStart).FirstOrDefault();
                gunGfx.SetActive(false);
            }
        }
        if (nextShot != null && Time.timeSinceLevelLoad + previewTime > nextShot.timeSinceStart)
        {
            PreviewShot(nextShot);
        }
    }

    public void RecordShot(Vector2 pos, Quaternion dir)
    {
        previousShots.Add(new ShotRecord
        {
            position = pos,
            direction = dir,
            timeSinceStart = Time.timeSinceLevelLoad
        });
    }

    void PreviewShot(ShotRecord shot)
    {
        loopgun.transform.position= shot.position;
        loopgun.transform.rotation = shot.direction;
        gunGfx.SetActive(true);
    }


    public void FireBullet(Vector2 position, Quaternion direction)
    {
        loopgun.transform.position= position;
        loopgun.transform.rotation = direction;
        loopgun.Attack();
    }
}

[System.Serializable]
public class ShotRecord
{
    public Vector2 position;
    public Quaternion direction;
    public float timeSinceStart;
}