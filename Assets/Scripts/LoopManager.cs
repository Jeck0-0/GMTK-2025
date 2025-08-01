using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;

public class LoopManager : Singleton<LoopManager>
{
    public List<ShotRecord> previousShots = new List<ShotRecord>(); // All bullets from previous runs
    private List<ShotRecord> currentShots = new List<ShotRecord>(); // Bullets from the current run

    [SerializeField] GameObject loopBullet;
    public event Action OnGameReset;

    private float runTime = 0f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        ResetLevel();

        runTime += Time.deltaTime;
    }

    public void RecordShot(Vector2 pos, Vector2 dir)
    {
        currentShots.Add(new ShotRecord
        {
            position = pos,
            direction = dir.normalized,
            timeSinceStart = runTime
        });
    }

    private IEnumerator ReplayShots()
    {
        var replayList = new List<ShotRecord>(previousShots);// to avoid bugs
        float timer = 0f;
        int index = 0;

        while (index < replayList.Count)
        {
            timer += Time.deltaTime; // don't use runTime

            while (index < replayList.Count && replayList[index].timeSinceStart <= timer)
            {
                FireBullet(replayList[index].position, replayList[index].direction);
                index++;
            }

            yield return null;
        }
    }

    public void FireBullet(Vector2 position, Vector2 direction)
    {
        GameObject bullet = Instantiate(loopBullet, position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().linearVelocity = direction * 10f;
    }

    public void ResetLevel()
    {
        runTime = 0f;

        previousShots.AddRange(currentShots);
        currentShots.Clear();

        OnGameReset?.Invoke();
        StartCoroutine(ReplayShots());
    }
}

[System.Serializable]
public class ShotRecord
{
    public Vector2 position;
    public Vector2 direction;
    public float timeSinceStart;
}