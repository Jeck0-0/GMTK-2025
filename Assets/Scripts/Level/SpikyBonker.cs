using UnityEngine;
using System.Collections;

public class SpikyBonker : Resetable, IInteractable
{
    [SerializeField] float moveDistance = 10f;
    [SerializeField] float moveDownDuration = 0.4f;
    [SerializeField] float moveUpDuration = 1f;
    [SerializeField] float pauseDuration = 0.5f;

    private Vector3 startPos;
    private Vector3 targetPos;

    public void Interact()
    {
        StartCoroutine(Move());
    }
    public void ResetObject()
    {
        // nothing
    }
    protected override void Start()
    {
        LoopManager.Instance.OnGameReset += OnReset;
        startPos = transform.position;
        targetPos = startPos + Vector3.down * moveDistance;
    }
    IEnumerator Move()
    {
        yield return StartCoroutine(SmoothMove(startPos, targetPos, moveDownDuration));

        yield return new WaitForSeconds(pauseDuration);

        yield return StartCoroutine(SmoothMove(targetPos, startPos, moveUpDuration));
    }

    IEnumerator SmoothMove(Vector3 from, Vector3 to, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(t / duration);
            float eased = Mathf.SmoothStep(0f, 1f, normalizedTime);
            transform.position = Vector3.Lerp(from, to, eased);
            yield return null;
        }

        transform.position = to;
    }

    public override void OnReset()
    {
        transform.position = startPos;
        StopAllCoroutines();
    }
}