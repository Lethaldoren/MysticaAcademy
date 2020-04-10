using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class TeleportCircle : MonoBehaviour
{
    float startTime;
    public float beginingDuration;
    public AnimationCurve startRotationMod;

    public Transform runes1;
    public float runes1RotationMod;
    public Transform runes2;
    public float runes2RotationMod;

    public Transform center;
    public float centerRotationMod;

    void OnEnable()
    {
        startTime = Time.time;
        runes1.localRotation = Quaternion.identity;
        runes2.localRotation = Quaternion.identity;
        center.localRotation = Quaternion.identity;
    }

    float prog;
    void Update()
    {
        prog = (Time.time - startTime) / beginingDuration;
        runes1.Rotate(Vector3.forward, Time.deltaTime * runes1RotationMod * (1 + startRotationMod.Evaluate(prog)));
        runes2.Rotate(Vector3.forward, Time.deltaTime * runes2RotationMod * (1 + startRotationMod.Evaluate(prog)));
        center.Rotate(Vector3.forward, Time.deltaTime * centerRotationMod * (1 + startRotationMod.Evaluate(prog)));
    }
}
