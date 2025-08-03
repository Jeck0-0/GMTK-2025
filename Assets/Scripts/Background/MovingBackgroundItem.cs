using System;
using Unity.Mathematics;
using UnityEngine;

public class BackgroundItem : MonoBehaviour
{
    [SerializeField] public SpriteRenderer icon;
    public float multiplier = 1f;

    [SerializeField] public Gradient colorGradient;
    [SerializeField] public AnimationCurve alphaCurve;
    [SerializeField] public AnimationCurve scaleCurve;
    [SerializeField] public AnimationCurve positionCurve;
    [SerializeField] public AnimationCurve rotationCurve;

    public float colorGradientDistance = 10;

    public void LateUpdate()
    {
        var cursorPos = Player.instance.transform.position;
        
        var dir = (cursorPos - transform.position).normalized;
        dir = Quaternion.Inverse(transform.rotation) * dir;
        /*var deg2 = transform.rotation.eulerAngles.z;
        var rot = deg - deg2;
        dir = new Vector3(Mathf.Cos(rot / 360 * multiply), Mathf.Sin(rot/360 * multiply), 0);

        if (test)
        {
            Debug.Log($"d {deg} - d2 {deg2} - r {rot} - d[{dir}]");
            
        }*/
        
        var dist = Vector3.Distance(transform.position, cursorPos) * multiplier;

        icon.transform.localScale = Vector3.one * scaleCurve.Evaluate(dist);
        icon.transform.localRotation = Quaternion.Euler(0, 0, rotationCurve.Evaluate(dist) * 90);
        icon.transform.localPosition = dir * positionCurve.Evaluate(dist);
        icon.color = colorGradient.Evaluate(dist / colorGradientDistance);
    }
}
