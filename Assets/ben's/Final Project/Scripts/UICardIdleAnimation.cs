using UnityEngine;
using UnityEngine.EventSystems;

public class DevilCardAnimation : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [Header("Float")]
    public float floatAmplitude = 8f;
    public float floatSpeed = 1f;

    [Header("Idle Shake")]
    public float idleShakePos = 0.8f;
    public float idleShakeRot = 0.6f;
    public float idleSpeedMult = 1f;

    [Header("Hover Shake")]
    public float hoverShakePos = 4f;
    public float hoverShakeRot = 5f;
    public float hoverSpeedMult = 1.8f;

    [Header("Click Rage Shake")]
    public float rageShakePos = 12f;
    public float rageShakeRot = 18f;
    public float rageSpeedMult = 3.5f;
    public float rageDuration = 1f;

    [Header("Base Shake Speed")]
    public float baseShakeSpeed = 35f;

    [Header("Scale")]
    public float hoverScale = 1.1f;
    public float rageScale = 1.3f;
    public float scaleSpeed = 12f;

    RectTransform rect;
    Vector2 startPos;
    Quaternion startRot;
    Vector3 startScale;

    bool hovering;
    bool isRaged = false;
    bool rageDone = false;
    float rageTimer = 0f;
    float seed;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        startPos = rect.anchoredPosition;
        startRot = rect.localRotation;
        startScale = rect.localScale;
        seed = Random.value * 10f;
    }

    void Update()
    {
        float t = Time.unscaledTime + seed;

        if (rageTimer > 0f)
        {
            rageTimer -= Time.unscaledDeltaTime;
            if (rageTimer <= 0f)
            {
                rageTimer = 0f;
                isRaged = false;
                rageDone = true;
                hovering = false; // forcibly disable hover after rage ends
            }
        }

        // Float (always active)
        float idleY = Mathf.Sin(t * floatSpeed) * floatAmplitude;
        Vector2 idlePos = startPos + new Vector2(0, idleY);

        float posShake = 0f;
        float rotShake = 0f;
        float speedMult = 0f;

        if (isRaged)
        {
            float rage01 = rageTimer / rageDuration;

            posShake = Mathf.Lerp(0f, rageShakePos, rage01);
            rotShake = Mathf.Lerp(0f, rageShakeRot, rage01);
            speedMult = Mathf.Lerp(0f, rageSpeedMult, rage01);
        }
        else if (!rageDone)
        {
            // Before pressing and rage ends, idle or hover shake allowed
            if (hovering)
            {
                posShake = hoverShakePos;
                rotShake = hoverShakeRot;
                speedMult = hoverSpeedMult;
            }
            else
            {
                posShake = idleShakePos;
                rotShake = idleShakeRot;
                speedMult = idleSpeedMult;
            }
        }
        // else after rage done -> no shaking at all

        float shakeT = Time.unscaledTime * baseShakeSpeed * speedMult;

        float shakeX = Mathf.Sin(shakeT * 2.1f) * posShake;
        float shakeY = Mathf.Sin(shakeT * 2.7f) * posShake;
        float shakeRot = Mathf.Sin(shakeT * 3.3f) * rotShake;

        rect.anchoredPosition = idlePos + new Vector2(shakeX, shakeY);
        rect.localRotation = startRot * Quaternion.Euler(0, 0, shakeRot);

        // Scale logic - NO hover scale allowed after rageDone

        Vector3 targetScale;

        if (isRaged)
        {
            float rageProgress = 1f - (rageTimer / rageDuration);
            targetScale = Vector3.Lerp(startScale, startScale * rageScale, rageProgress);
        }
        else
        {
            targetScale = startScale; // Always normal scale after rage done, no hover scale
        }

        rect.localScale = Vector3.Lerp(rect.localScale, targetScale, Time.unscaledDeltaTime * scaleSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isRaged || rageDone) return; // No hover allowed after rage
        hovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isRaged || rageDone) return; // No hover allowed after rage
        hovering = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isRaged || rageDone) return; // Ignore clicks during and after rage

        isRaged = true;
        rageTimer = rageDuration;
        rageDone = false;
    }
}
