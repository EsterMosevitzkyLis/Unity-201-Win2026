using UnityEngine;
using System.Collections;

/// <summary>
/// Fish path script - continuous movement with natural swimming motion.
/// Includes vertical bobbing and Y-axis rotation for realistic swimming.
/// Uses MaterialPropertyBlock to avoid breaking shader batching.
/// </summary>
public class FishPathMinimal : MonoBehaviour
{
    [Header("X Path (Local Space)")]
    public float startX = 20f;
    public float endX = -15f;
    public float fadeInEndX = 15f;
    public float fadeOutStartX = -10f;

    [Header("Speed")]
    public float speed = 3f;

    [Header("Invisible Pause")]
    public float invisiblePause = 0.5f;

    [Header("Swimming Motion")]
    [Tooltip("Vertical wave height")]
    public float bobHeight = 0.5f;
    [Tooltip("How fast the fish bobs up and down")]
    public float bobSpeed = 2f;
    [Tooltip("Max rotation angle in Y axis")]
    public float rotationAmount = 15f;
    [Tooltip("How fast the fish rotates")]
    public float rotationSpeed = 1.5f;

    [Header("Shader Opacity Property")]
    public string opacityProperty = "_Opacity";

    MaterialPropertyBlock _propBlock;
    Renderer _renderer;
    int _propID;
    Vector3 _basePos;
    Quaternion _baseRot;
    float _swimTime;

    void Start()
    {
        _basePos = transform.localPosition;
        _baseRot = transform.localRotation;
        
        _renderer = GetComponent<Renderer>();
        if (_renderer != null)
        {
            _propBlock = new MaterialPropertyBlock();
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
            _swimTime = 0f;
            SetPositionAndRotation(x);
            SetOpacity(1f);

            // PHASE 1: Fade in WHILE moving
            while (!ReachedPoint(x, fadeInEndX, direction))
            {
                x += direction * speed * Time.deltaTime;
                _swimTime += Time.deltaTime;
                SetPositionAndRotation(x);

                float fadeProgress = InverseLerp(startX, fadeInEndX, x);
                SetOpacity(1f - fadeProgress);

                yield return null;
            }
            SetOpacity(0f);

            // PHASE 2: Swim fully visible
            while (!ReachedPoint(x, fadeOutStartX, direction))
            {
                x += direction * speed * Time.deltaTime;
                _swimTime += Time.deltaTime;
                SetPositionAndRotation(x);
                yield return null;
            }

            // PHASE 3: Fade out WHILE moving to end
            while (!ReachedPoint(x, endX, direction))
            {
                x += direction * speed * Time.deltaTime;
                _swimTime += Time.deltaTime;
                SetPositionAndRotation(x);

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

    void SetPositionAndRotation(float x)
    {
        // Calculate vertical bobbing (sine wave)
        float yOffset = Mathf.Sin(_swimTime * bobSpeed) * bobHeight;
        
        // Calculate Y rotation (cosine wave for smooth back-and-forth)
        float yRotation = Mathf.Cos(_swimTime * rotationSpeed) * rotationAmount;
        
        // Apply position
        transform.localPosition = new Vector3(x, _basePos.y + yOffset, _basePos.z);
        
        // Apply rotation
        transform.localRotation = _baseRot * Quaternion.Euler(0f, yRotation, 0f);
    }

    void SetOpacity(float value)
    {
        if (_renderer != null && _propBlock != null)
        {
            _propBlock.SetFloat(_propID, Mathf.Clamp01(value));
            _renderer.SetPropertyBlock(_propBlock);
        }
    }
}