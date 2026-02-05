using UnityEngine;
using UnityEngine.EventSystems;

public class HoverScaleButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[Header("Scale Settings")]
	public float hoverScale = 1.1f;
	public float scaleSpeed = 10f;

	Vector3 defaultScale;
	Vector3 targetScale;

	void Awake()
	{
		defaultScale = transform.localScale;
		targetScale = defaultScale;
	}

	void Update()
	{
		transform.localScale = Vector3.Lerp(
			transform.localScale,
			targetScale,
			Time.unscaledDeltaTime * scaleSpeed
		);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		targetScale = defaultScale * hoverScale;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		targetScale = defaultScale;
	}
}