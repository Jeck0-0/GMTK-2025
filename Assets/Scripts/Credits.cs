using UnityEngine;

public class Credits : MonoBehaviour
{
    public void OpenJeck()
    {
        Application.OpenURL("https://jeck0-0.itch.io/");
    }
    public void OpenSeva()
    {
        Application.OpenURL("https://famona.itch.io/");
    }
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
}
