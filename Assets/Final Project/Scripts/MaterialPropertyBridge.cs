using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode] // This lets you see the changes in the Scene view without hitting Play
public sealed class MaterialPropertyBridge : MonoBehaviour
{
    [Range(0f, 1f)]
    public float blindIntensity = 0f; // This is what you will animate!

    private Graphic _graphic;
    private MaterialPropertyBlock _propBlock;
    private static readonly int BlindIntensityId = Shader.PropertyToID("_Blind_Intensity");

    void OnEnable()
    {
        _graphic = GetComponent<Graphic>();
    }

    // Update is called every frame
    void Update()
    {
        if (_graphic == null) return;

        // This creates a "local" version of the material so you don't break the original file
        _graphic.material.SetFloat(BlindIntensityId, blindIntensity);
    }
}