using UnityEngine;

public class Resetable : MonoBehaviour
{
    protected Vector3 initialPosition;
    protected virtual void Start()
    {
        initialPosition = transform.position;
        LoopManager.Instance.OnGameReset += OnReset;
    }

    protected virtual void OnDestroy()
    {
        if (LoopManager.Instance != null)
        LoopManager.Instance.OnGameReset -= OnReset;
    }

    public virtual void OnReset()
    {
        transform.position = initialPosition;
        transform.rotation = Quaternion.identity;
        StopAllCoroutines();
    }
}