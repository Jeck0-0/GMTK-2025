using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopManager : Singleton<LoopManager>
{
    public static List<ShotRecord> previousShots = new();
    public Loopgun loopgun;

    protected List<ShotRecord> shotsLeft = new();

    protected override void Awake()
    {
        base.Awake();
        shotsLeft = new List<ShotRecord>(previousShots);
    }

    void Update()
    {
        // fire shots
        for (int i = shotsLeft.Count - 1; i > 0; i--)
        {
            if (Time.timeSinceLevelLoad > shotsLeft[i].timeSinceStart)
            {
                FireBullet(shotsLeft[i].position, shotsLeft[i].direction);
                shotsLeft.RemoveAt(i);
            }
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