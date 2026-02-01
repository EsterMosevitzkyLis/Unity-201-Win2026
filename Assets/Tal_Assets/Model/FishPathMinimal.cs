using UnityEngine;
using System.Collections;

/// <summary>
/// Fish path script - continuous movement, never stops.
/// Fades happen while swimming.
/// </summary>
public class FishPathMinimal : MonoBehaviour
{
    [Header("X Path (Local Space)")]
    public float startX = 20f;
    public float endX = -15f;
    public float fadeInEndX = 15f;     // Fade in completes here
    public float fadeOutStartX = -10f; // Fade out begins here

    [Header("Speed")]
    public float speed = 3f;

    [Header("Invisible Pause")]
    public float invisiblePause = 0.5f;

    [Header("Shader Opacity Property")]
    public string opacityProperty = "_Opacity";

    Material _mat;
    int _propID;
    Vector3 _basePos;

    void Start()
    {
        _basePos = transform.localPosition;
        
        var rend = GetComponent<Renderer>();
        if (rend != null)
        {
            _mat = rend.material;
            _propID = Shader.PropertyToID(opacityProperty);
        }

        StartCoroutine(Loop());
    }

    IEnumerator Loop()
    {
        float direction = Mathf.Sign(endX - startX);
        if (direction == 0) direction = -1f;

        while (true)
        {
            // Teleport to start, invisible
            float x = startX;
            SetX(x);
            SetOpacity(1f);

            // PHASE 1: Fade in WHILE moving
            while (!ReachedPoint(x, fadeInEndX, direction))
            {
                x += direction * speed * Time.deltaTime;
                SetX(x);

                float fadeProgress = InverseLerp(startX, fadeInEndX, x);
                SetOpacity(1f - fadeProgress);

                yield return null;
            }
            SetOpacity(0f);

            // PHASE 2: Swim fully visible
            while (!ReachedPoint(x, fadeOutStartX, direction))
            {
                x += direction * speed * Time.deltaTime;
                SetX(x);
                yield return null;
            }

            // PHASE 3: Fade out WHILE moving to end
            while (!ReachedPoint(x, endX, direction))
            {
                x += direction * speed * Time.deltaTime;
                SetX(x);

                float fadeProgress = InverseLerp(fadeOutStartX, endX, x);
                SetOpacity(fadeProgress);

                yield return null;
            }
            SetOpacity(1f);

            // PHASE 4: Brief invisible pause before restart
            yield return new WaitForSeconds(invisiblePause);
        }
    }

    bool ReachedPoint(float current, float target, float direction)
    {
        return (direction < 0) ? (current <= target) : (current >= target);
    }

    float InverseLerp(float from, float to, float value)
    {
        if (Mathf.Approximately(from, to)) return 1f;
        return Mathf.Clamp01((value - from) / (to - from));
    }

    void SetX(float x)
    {
        transform.localPosition = new Vector3(x, _basePos.y, _basePos.z);
    }

    void SetOpacity(float value)
    {
        if (_mat != null)
        {
            _mat.SetFloat(_propID, Mathf.Clamp01(value));
        }
    }

    void OnDestroy()
    {
        if (_mat != null)
            Destroy(_mat);
    }
}