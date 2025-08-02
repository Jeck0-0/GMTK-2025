using TarodevController;
using UnityEngine;
public class QuitGameCollider : MonoBehaviour
{ 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.GetComponent<PlayerController>().BlockImput(true);
                Application.Quit();
            }
        }
    }
}
