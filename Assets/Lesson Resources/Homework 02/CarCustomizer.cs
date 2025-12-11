using UnityEngine;
using UnityEngine.UI;

public class CarCustomizer : MonoBehaviour
{
    [SerializeField] private Image carImage;

    [Header("Shader Property Names")]
    [SerializeField] private string primaryColorProperty = "_PrimaryColor";
    [SerializeField] private string secondaryColorProperty = "_SecondaryColor";

    [Header("Color Palettes")]
    [SerializeField] private Color[] primaryColors;
    [SerializeField] private Color[] secondaryColors;

    private Material matInstance;

    private void Awake()
    {
        if (carImage == null)
        {
            Debug.LogError("CarCustomizer: carImage is NOT assigned on " + gameObject.name, this);
            return;
        }

        if (carImage.material == null)
        {
            Debug.LogError("CarCustomizer: carImage has NO material. Assign your Shader Graph material to the Image.", carImage);
            return;
        }

        // Create unique instance of the material
        matInstance = Instantiate(carImage.material);
        carImage.material = matInstance;
    }

    public void SetPrimaryColorIndex(int index)
    {
        if (!CanApplyColor(primaryColors, index, "Primary")) return;
        matInstance.SetColor(primaryColorProperty, primaryColors[index]);
    }

    public void SetSecondaryColorIndex(int index)
    {
        if (!CanApplyColor(secondaryColors, index, "Secondary")) return;
        matInstance.SetColor(secondaryColorProperty, secondaryColors[index]);
    }

    private bool CanApplyColor(Color[] palette, int index, string label)
    {
        if (matInstance == null)
        {
            Debug.LogError($"CarCustomizer: matInstance is null. Did Awake fail? Check carImage & material on {gameObject.name}.", this);
            return false;
        }

        if (palette == null || palette.Length == 0)
        {
            Debug.LogError($"CarCustomizer: {label} palette is empty or null on {gameObject.name}. Fill it in the inspector.", this);
            return false;
        }

        if (index < 0 || index >= palette.Length)
        {
            Debug.LogWarning($"CarCustomizer: {label} index {index} is out of range (0–{palette.Length - 1}).", this);
            return false;
        }

        return true;
    }

    public void SetCarSprite(Sprite sprite)
    {
        if (carImage == null)
        {
            Debug.LogError("CarCustomizer: carImage is null in SetCarSprite.", this);
            return;
        }

        if (sprite == null)
        {
            Debug.LogWarning("CarCustomizer: SetCarSprite was called with a null sprite.", this);
            return;
        }

        carImage.sprite = sprite;
    }
}
