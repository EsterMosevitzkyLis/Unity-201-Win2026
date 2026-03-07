using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Image))]
public class CardCoverDissolveAnimation : MonoBehaviour
{
    [Header("Shader Property Name")]
    [SerializeField] private string floatPropertyName = "_DissolveYPosition";

    [Header("Lerp Settings")]
    public float startValue = 0f;
    public float endValue = 1f;
    public float duration = 0.5f;

    [Header("Delay Settings")]
    public float delay = 0f;

    private Image image;
    private Material runtimeMaterial;
    private Coroutine currentRoutine;

    void Awake()
    {
        image = GetComponent<Image>();

        // Create unique material instance so it doesn't affect other UI
        runtimeMaterial = Instantiate(image.material);
        image.material = runtimeMaterial;

        runtimeMaterial.SetFloat(floatPropertyName, startValue);
    }

    // ⭐ This is the method you trigger from the UnityEvent
    public void PlayDissolve()
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(LerpRoutine());
    }

    private IEnumerator LerpRoutine()
    {
        if (delay > 0f)
            yield return new WaitForSecondsRealtime(delay);

        float time = 0f;
        runtimeMaterial.SetFloat(floatPropertyName, startValue);

        while (time < duration)
        {
            time += Time.unscaledDeltaTime;

            float t = time / duration;
            float value = Mathf.Lerp(startValue, endValue, t);

            runtimeMaterial.SetFloat(floatPropertyName, value);

            yield return null;
        }

        runtimeMaterial.SetFloat(floatPropertyName, endValue);
    }
}