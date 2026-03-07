using UnityEngine;
using UnityEngine.EventSystems;

public class UIHoverScaleWithIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("References")]
    public Transform target;
    public RectTransform icon;

    [Header("Scale")]
    public float hoverScale = 1.1f;
    public float speed = 10f;

    [Header("Icon Push")]
    public float iconPushDistance = 20f; // in pixels for UI
    public float iconSpeed = 10f;

    [Header("Rotation")]
    public bool enableHoverRotation = true;
    public float hoverRotationAngle = 5f;
    public float hoverRotationSpeed = 3f;

    [Header("Control")]
    public bool enableHover = false;

    private Vector3 originalScale;
    private Vector3 originalIconPos;
    private Quaternion originalRotation;

    private bool isHovered = false;

    private void Start()
    {
        originalScale = target.localScale;
        originalIconPos = icon.localPosition;
        originalRotation = target.localRotation;
    }

    public void SetOn()
    {
        enableHover = true;
    }

    private void Update()
    {
        if (!enableHover) return;

        // Smooth scale for target
        Vector3 targetScale = isHovered ? originalScale * hoverScale : originalScale;
        target.localScale = Vector3.Lerp(target.localScale, targetScale, Time.deltaTime * speed);

        // Smooth rotation for target
        if (enableHoverRotation)
        {
            float angle = isHovered ? hoverRotationAngle : 0f;
            Quaternion targetRot = Quaternion.Euler(0, 0, angle);
            target.localRotation = Quaternion.Lerp(target.localRotation, targetRot, Time.deltaTime * hoverRotationSpeed);
        }

        Vector3 iconTargetPos = isHovered
           ? originalIconPos + new Vector3(0, 0, -iconPushDistance) // X/Y offset
           : originalIconPos;

        icon.localPosition = Vector3.Lerp(icon.localPosition, iconTargetPos, Time.deltaTime * iconSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!enableHover) return;
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!enableHover) return;
        isHovered = false;
    }
}