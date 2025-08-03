using UnityEngine;

public class WASD : MonoBehaviour
{
    [SerializeField] float delay = 7f;
    [SerializeField] GameObject obj;
    void Start()
    {
        obj.SetActive(false);
        Invoke("Delay", delay);
    }

    private void Delay()
    {
        obj.SetActive(true);
    }

}
