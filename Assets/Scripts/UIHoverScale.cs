using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIHoverScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Transform target;
    public float hoverScale = 1.1f;
    public float speed = 10f;

    public bool hasSelectionSlider = false;
    public Image targetImage;
    public float sliderSpeed = 5f;
    public string sliderProperty = "_SelectSlider";

    public bool enableHoverRotation = true;
    public float hoverRotationAngle = 5f;
    public float hoverRotationSpeed = 3f;

    public bool hoverMuted = false;

    Vector3 baseScale;
    Vector3 targetScale;

    Quaternion baseRotation;
    bool isHovered;

    float currentSlider;
    float targetSlider;

    Material instanceMaterial;

    void Awake()
    {
        if (target == null)
            target = transform;

        baseScale = target.localScale;
        targetScale = baseScale;
        baseRotation = target.localRotation;

        if (hasSelectionSlider && targetImage != null)
        {
            instanceMaterial = Instantiate(targetImage.material);
            targetImage.material = instanceMaterial;

            if (instanceMaterial.HasProperty(sliderProperty))
            {
                currentSlider = instanceMaterial.GetFloat(sliderProperty);
                targetSlider = currentSlider;
            }
        }
    }

    void Update()
    {
        if (hoverMuted) return;

        target.localScale = Vector3.Lerp(
            target.localScale,
            targetScale,
            Time.unscaledDeltaTime * speed
        );

        if (enableHoverRotation)
        {
            if (isHovered)
            {
                float angle =
                    Mathf.Sin(Time.unscaledTime * hoverRotationSpeed)
                    * hoverRotationAngle;

                target.localRotation =
                    baseRotation * Quaternion.Euler(0f, 0f, angle);
            }
            else
            {
                target.localRotation = Quaternion.Lerp(
                    target.localRotation,
                    baseRotation,
                    Time.unscaledDeltaTime * speed
                );
            }
        }

        if (hasSelectionSlider && instanceMaterial != null)
        {
            currentSlider = Mathf.MoveTowards(
                currentSlider,
                targetSlider,
                Time.unscaledDeltaTime * sliderSpeed
            );

            instanceMaterial.SetFloat(sliderProperty, currentSlider);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverMuted) return;

        isHovered = true;
        targetScale = baseScale * hoverScale;
        targetSlider = 1f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (hoverMuted) return;

        isHovered = false;
        targetScale = baseScale;
        targetSlider = 0f;
    }

    public void SyncBaseState()
    {
        baseRotation = target.localRotation;
        baseScale = target.localScale;
        targetScale = baseScale;
    }

    public void MuteHover()
    {
        hoverMuted = true;
        isHovered = false;

        targetScale = baseScale;
        target.localScale = baseScale;
        target.localRotation = baseRotation;

        targetSlider = 0f;
        currentSlider = 0f;

        if (instanceMaterial != null && instanceMaterial.HasProperty(sliderProperty))
        {
            instanceMaterial.SetFloat(sliderProperty, 0f);
        }
    }

    public void UnmuteHover()
    {
        SyncBaseState();
        hoverMuted = false;
    }
}
