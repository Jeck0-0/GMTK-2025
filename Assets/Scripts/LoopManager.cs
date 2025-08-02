using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class LoopManager : Singleton<LoopManager>
{
    public List<ShotRecord> previousShots = new List<ShotRecord>(); // All bullets from previous runs
    private List<ShotRecord> currentShots = new List<ShotRecord>(); // Bullets from the current run

    [SerializeField] GameObject loopBullet;
    [SerializeField] GameObject hintPrefab;

    public event Action OnGameReset;

    private float runTime = 0f;
    private float inputTime = 0f;
    private bool reloadingScene = false;
    void Update()
    {
        
        if (Input.GetKey(KeyCode.R))
        {
            inputTime += Time.deltaTime;
            if (inputTime > 1f && !reloadingScene)
            {
                SceneLoader.Instance.LoadScene(SceneManager.GetActiveScene().name);
                inputTime = 0f;
                reloadingScene = true;
                return;
            }
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            ResetLevel();
            inputTime = 0f;
        }

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

    public void ResetLevel()
    {
        runTime = 0f;

        previousShots.AddRange(currentShots);
        currentShots.Clear();

        OnGameReset?.Invoke();

        SpawnHints();
    }

    private void SpawnHints()
    {
        foreach (var shot in previousShots)
        {
            GameObject hint = Instantiate(hintPrefab, shot.position, Quaternion.identity);

            if (hint.TryGetComponent(out LoopPortal portal))
            {
                portal.Initialize(shot.timeSinceStart, shot.direction);
            }
        }
    }
}

[System.Serializable]
public class ShotRecord
{
    public Vector2 position;
    public Vector2 direction;
    public float timeSinceStart;
}