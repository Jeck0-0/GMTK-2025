using UnityEngine;


public class RotateOverTime : MonoBehaviour
{
    public float speed;
    public float offset;

    void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, Time.time * speed + offset);
    }
}