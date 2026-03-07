using UnityEngine;
using System.Collections;

public class SpiralController : MonoBehaviour
{
    public Renderer targetRenderer;
    public string propertyName = "_LifeTime";
    public float duration = 1.5f;

    private Material mat;

    void Start()
    {
        if (targetRenderer != null)
        {
            // This creates the unique instance automatically
            mat = targetRenderer.material;
        }
    }

    public void AnimateLifeTime()
    {
        StopAllCoroutines(); // Prevent overlapping animations
        StartCoroutine(LerpShaderValue(1, 0.091f));
    }

    IEnumerator LerpShaderValue(float start, float end)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float currentValue = Mathf.Lerp(start, end, elapsed / duration);

            // Apply the value
            mat.SetFloat(propertyName, currentValue);
            yield return null;
        }

        // Ensure we hit the exact final value
        mat.SetFloat(propertyName, end);
    }
}