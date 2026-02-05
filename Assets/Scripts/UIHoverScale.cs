using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIHoverScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Scale Settings")]
    public Transform target;
    public float hoverScale = 1.1f;
    public float speed = 10f;

    [Header("Selection Slider (Target UI Image)")]
    public bool hasSelectionSlider = false;
    public Image targetImage;
    public float sliderSpeed = 5f;
    public string sliderProperty = "_SelectSlider";

    Vector3 originalScale;
    Vector3 targetScale;

    float currentSlider = 0f;
    float targetSlider = 0f;

    Material instanceMaterial;

    void Awake()
    {
        if (target == null)
            target = transform;

        originalScale = target.localScale;
        targetScale = originalScale;

        if (hasSelectionSlider && targetImage != null)
        {
            // Create a UNIQUE material instance for THIS Image
            instanceMaterial = Instantiate(targetImage.material);
            targetImage.material = instanceMaterial;

            if (instanceMaterial.HasProperty(sliderProperty))
            {
                currentSlider = instanceMaterial.GetFloat(sliderProperty);
                targetSlider = currentSlider;
            }
            else
            {
                Debug.LogWarning($"Material does not have property {sliderProperty}");
            }
        }
    }

    void Update()
    {
        // Scale animation
        target.localScale = Vector3.Lerp(
            target.localScale,
            targetScale,
            Time.unscaledDeltaTime * speed
        );

        // Shader slider animation (per UI Image)
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
        targetScale = originalScale * hoverScale;

        if (hasSelectionSlider)
            targetSlider = 1f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = originalScale;

        if (hasSelectionSlider)
            targetSlider = 0f;
    }
}
