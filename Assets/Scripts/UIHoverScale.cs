using UnityEngine;
using UnityEngine.EventSystems;

public class UIHoverScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Scale Settings")]
    public Transform target;
    public float hoverScale = 1.1f;
    public float speed = 10f;

    Vector3 originalScale;
    Vector3 targetScale;

    void Awake()
    {
        if (target == null)
            target = transform;

        originalScale = target.localScale;
        targetScale = originalScale;
    }

    void Update()
    {
        target.localScale = Vector3.Lerp(
            target.localScale,
            targetScale,
            Time.unscaledDeltaTime * speed
        );
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = originalScale * hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = originalScale;
    }
}
