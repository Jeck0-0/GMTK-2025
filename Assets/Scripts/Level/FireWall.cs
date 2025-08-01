using UnityEngine;

public class FireWall : MonoBehaviour
{
    public Transform player;
    public float maxSpeed = 3f;
    public float minSpeed = 1f;
    public float slowDownDistance = 10f;
    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        float t = Mathf.Clamp01(distance / slowDownDistance);
        float currentSpeed = Mathf.Lerp(minSpeed, maxSpeed, t);

        transform.position += Vector3.right * currentSpeed * Time.deltaTime;
    }
}