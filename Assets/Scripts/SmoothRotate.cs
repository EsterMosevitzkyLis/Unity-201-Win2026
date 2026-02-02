using UnityEngine;
using System.Collections;

public class SmoothRotate : MonoBehaviour
{
    public float duration = 0.4f;
    public AnimationCurve ease = AnimationCurve.EaseInOut(0, 0, 1, 1);

    Coroutine rotateRoutine;

    public void Rotate180Y(Transform target)
    {
        if (target == null) return;

        if (rotateRoutine != null)
            StopCoroutine(rotateRoutine);

        rotateRoutine = StartCoroutine(RotateRoutine(target));
    }

    IEnumerator RotateRoutine(Transform target)
    {
        Quaternion startRot = target.rotation;

        Vector3 euler = startRot.eulerAngles;
        euler.y += 180f;

        Quaternion endRot = Quaternion.Euler(euler);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / duration;
            float eased = ease.Evaluate(t);

            target.rotation = Quaternion.Slerp(startRot, endRot, eased);
            yield return null;
        }

        target.rotation = endRot;
    }
}
