using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIImageFadeOut : MonoBehaviour
{
	public float fadeDuration = 0.5f;

	Image image;
	Coroutine fadeRoutine;

	void Awake()
	{
		image = GetComponent<Image>();
	}

	public void FadeOut()
	{
		if (fadeRoutine != null)
			StopCoroutine(fadeRoutine);

		fadeRoutine = StartCoroutine(FadeOutRoutine());
	}

	IEnumerator FadeOutRoutine()
	{
		float t = 0f;
		Color startColor = image.color;

		while (t < fadeDuration)
		{
			t += Time.unscaledDeltaTime;
			float alpha = Mathf.Lerp(startColor.a, 0f, t / fadeDuration);

			image.color = new Color(
				startColor.r,
				startColor.g,
				startColor.b,
				alpha
			);

			yield return null;
		}

		image.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
	}
}