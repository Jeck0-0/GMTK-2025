using TarodevController;
using UnityEngine;

public class VictoryCollider : MonoBehaviour
{
    [SerializeField] string sceneToLoad;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.GetComponent<PlayerController>().BlockImput(true);
                SceneLoader.Instance.LoadScene(sceneToLoad);
            }
        }
    }
}