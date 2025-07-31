using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

public class PostFXManager : Singleton<PostFXManager>
{
    [SerializeField] Volume victoryFX;
    [SerializeField] Volume damageFX;
    [SerializeField] float effectDecaySpeed = 1f;

    public void VictoryFX()
    {
        StartCoroutine(PlayVictoryEffect(victoryFX));
    }
    public void DamageFX()
    {
        StartCoroutine(PlayVictoryEffect(damageFX));
    }

    private IEnumerator PlayVictoryEffect(Volume volume, bool fromTop = true)
    {
        if (volume == null)
        yield break;


        if (fromTop)
        {
            volume.weight = 1f;
            while (volume.weight > 0f)
            {
                volume.weight -= effectDecaySpeed * Time.deltaTime;
                yield return null;
            }
            volume.weight = 0f;
        }
        else
        {
            volume.weight = 0f;
            while (volume.weight < 1f)
            {
                volume.weight += effectDecaySpeed * Time.deltaTime;
                yield return null;
            }
            volume.weight = 1f;
        }
    }
}
