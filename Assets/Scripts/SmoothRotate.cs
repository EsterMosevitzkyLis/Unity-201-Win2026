using UnityEngine;
using System.Collections;

public class SmoothRotate : MonoBehaviour
{
    public float duration = 0.4f;
    public float rotateDegrees = 180f;

    public bool enableJump = true;
    public float jumpHeight = 15f;
    public float jumpDuration = 0.15f;

    public AnimationCurve ease = AnimationCurve.EaseInOut(0, 0, 1, 1);

    Coroutine rotateRoutine;
    UIHoverScale hover;

    void Awake()
    {
        hover = GetComponent<UIHoverScale>();
    }

    public void Rotate(Transform target)
    {
        if (target == null) return;

        if (rotateRoutine != null)
            StopCoroutine(rotateRoutine);

        rotateRoutine = StartCoroutine(RotateRoutine(target));
    }

    IEnumerator RotateRoutine(Transform target)
    {
        if (hover != null)
            hover.MuteHover();

        Vector3 startPos = target.localPosition;

        Quaternion startRot = target.rotation;
        Vector3 euler = startRot.eulerAngles;
        euler.y += rotateDegrees;
        Quaternion endRot = Quaternion.Euler(euler);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / duration;
            float eased = ease.Evaluate(t);

            target.rotation = Quaternion.Slerp(startRot, endRot, eased);

            if (enableJump)
            {
                float jumpT = Mathf.Sin(eased * Mathf.PI);
                target.localPosition = startPos + Vector3.up * jumpT * jumpHeight;
            }

            yield return null;
        }

        target.rotation = endRot;
        target.localPosition = startPos;

        if (hover != null)
            hover.UnmuteHover();
    }
}
