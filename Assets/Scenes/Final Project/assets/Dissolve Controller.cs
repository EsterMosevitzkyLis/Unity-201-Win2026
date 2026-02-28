using UnityEngine;
using UnityEngine.UI;

public class DissolveController : MonoBehaviour
{
    [Range(0f, 1f)]
    public float dissolveAmount;

    private Material runtimeMaterial;

    void Awake()
    {
        Image img = GetComponent<Image>();

        runtimeMaterial = Instantiate(img.material);
        img.material = runtimeMaterial;

        dissolveAmount = runtimeMaterial.GetFloat("_fade");
    }

    void Update()
    {
        runtimeMaterial.SetFloat("_fade", dissolveAmount);
    }
}