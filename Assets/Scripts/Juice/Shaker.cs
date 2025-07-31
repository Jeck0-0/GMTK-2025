using System.Collections;
using Unity.Cinemachine;
using UnityEngine;


public class Shaker : Singleton<Shaker>
{
    private CinemachineCamera cam;
    private CinemachineBasicMultiChannelPerlin noise;
    private Coroutine shakeCoroutine;

    private void Start()
    {
        cam = GetComponent<CinemachineCamera>();
        if (cam != null)
            noise = cam.GetComponent<CinemachineBasicMultiChannelPerlin>();

        if (noise == null)
            Debug.LogError("CinemachineBasicMultiChannelPerlin component missing on the camera!");
    }

    public void ShakeCamera(float intensity, float duration)
    {
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        shakeCoroutine = StartCoroutine(Shake(intensity, intensity, duration));
    }

    private IEnumerator Shake(float amplitude, float frequency, float duration)
    {
        if (noise == null) yield break;

        noise.AmplitudeGain = amplitude;
        noise.FrequencyGain = frequency;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float fadeOutFactor = 1f - (elapsedTime / duration);
            noise.AmplitudeGain = Mathf.Lerp(0, amplitude, fadeOutFactor);
            noise.FrequencyGain = Mathf.Lerp(0, frequency, fadeOutFactor);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        noise.AmplitudeGain = 0f;
        noise.FrequencyGain = 0f;
    }
}