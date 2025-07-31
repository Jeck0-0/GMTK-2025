using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] GameObject obj;

    private static SceneLoader _instance;

    #region Singleton
    public static SceneLoader Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<SceneLoader>();

                // If no SceneLoader exists, create a new one
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("SceneLoader");
                    _instance = singletonObject.AddComponent<SceneLoader>();
                }
            }

            return _instance;
        }
    }

    void Awake()
    {
        // If the instance is already set, destroy this duplicate object
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            _instance = this;
        }
    }
    #endregion

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName));
    }
    public void RestartScene()
    {
        StartCoroutine(LoadSceneRoutine(SceneManager.GetActiveScene().name));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        if (anim) anim.SetTrigger("Start");

        yield return new WaitForSeconds(1f);

        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName);
        yield return loadOp;

        if (anim) anim.SetTrigger("End");
    }
}