using UnityEngine;
using UnityEngine.SceneManagement;


public class DebugShortcuts : MonoBehaviour
{
    float timeScaleBeforePausing;

    private void Update()
    {
        //toggle fullscreen
        if (Input.GetKeyDown(KeyCode.F11))
        {
            bool toggle = !Screen.fullScreen;
            Screen.fullScreen = toggle;
        }

        if (Input.GetKeyDown(KeyCode.F1))
            SceneManager.LoadScene("LoadableLevel");
            
        if (Input.GetKeyDown(KeyCode.F2))
            SceneManager.LoadScene("LevelEditor");

        if (Input.GetKeyDown(KeyCode.F3))
            SceneManager.LoadScene("ProceduralLevel");

        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift))
        {
            
            //lower volume
            if (Input.GetKeyDown(KeyCode.Comma))
            {
                AudioListener.volume = Mathf.Clamp01(AudioListener.volume - .1f);
                Debug.Log("Volume " + AudioListener.volume);
            }

            //increase volume
            if (Input.GetKeyDown(KeyCode.Period))
            {
                AudioListener.volume = Mathf.Clamp01(AudioListener.volume + .1f);
                Debug.Log("Volume " + AudioListener.volume);
            }
            

            //Load main scene
            if (Input.GetKeyDown(KeyCode.W))
            {
                SceneManager.LoadScene(0);
                Debug.Log("Loaded main scene");
            }

            
            //Speed up
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Time.timeScale *= 2;
                Debug.Log($"TimeScale: {Time.timeScale}");
            }
            
            //Slow down
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Time.timeScale /= 2;
                Debug.Log($"TimeScale: {Time.timeScale}");
            }

            //Reset time scale
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Time.timeScale = 1;
                Debug.Log($"TimeScale: {Time.timeScale}");
            }

            //Toggle pause
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (Time.timeScale > 0f)
                {
                    timeScaleBeforePausing = Time.timeScale;
                    Time.timeScale = 0;
                    Debug.Log("Paused");
                }
                else
                {
                    Time.timeScale = timeScaleBeforePausing;
                    Debug.Log("Resumed");
                }
            }
        }
    }
}
