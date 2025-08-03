using UnityEngine;

public class MusicCollider : MonoBehaviour
{
    [SerializeField] bool turnMusicOn = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if(turnMusicOn)
                {
                    MusicManager.Instance.StartMusic();
                }
                else
                {
                    MusicManager.Instance.StopMusic(2f);
                }
            }
        }
    }
}
