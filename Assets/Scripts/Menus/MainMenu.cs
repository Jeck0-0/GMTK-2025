using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject mainBlock;

    public void StartGame()
    {
        SceneLoader.Instance.LoadScene("Level1");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}