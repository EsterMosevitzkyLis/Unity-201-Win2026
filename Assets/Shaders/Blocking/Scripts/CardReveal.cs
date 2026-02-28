using UnityEngine;

public class CardReveal : MonoBehaviour
{
    [Header("Parents")]
    public GameObject backParent;
    public GameObject frontParent;
    public GameObject auraParent;

    [Header("Tilt")]
    public Transform tiltPivot; // child object (NOT animated)
    public float maxTiltAngle = 30f; // Updated to 30 degrees
    public float tiltSpeed = 10f;

    private Animator anim;
    private bool revealed = false;
    private bool isMouseOver = false;
    private Quaternion targetRotation;

    private Collider2D cardCollider;

    void Awake()
    {
        anim = GetComponent<Animator>();
        cardCollider = GetComponent<Collider2D>();


        if (tiltPivot == null)
            Debug.LogError("TiltPivot not assigned!");

        if (cardCollider == null)
            Debug.LogError("Collider2D missing on this object!");
    }

    void OnMouseEnter()
    {
        isMouseOver = true;

        if (!revealed)
            anim.SetBool("HoverBack", true);
    }

    void OnMouseExit()
    {
        isMouseOver = false;

        if (!revealed)
            anim.SetBool("HoverBack", false);
    }

    void OnMouseDown()
    {
        if (revealed) return;

        revealed = true;

        anim.SetBool("HoverBack", false);
        anim.SetTrigger("RevealCard");
    }

    public void SwapToFront()
    {
        backParent.SetActive(false);
    }

    void Update()
    {
        if (!revealed || tiltPivot == null)
            return;

        if (isMouseOver)
            CalculateMouseTilt();
        else
            targetRotation = Quaternion.identity;

        tiltPivot.localRotation = Quaternion.Lerp(
            tiltPivot.localRotation,
            targetRotation,
            Time.deltaTime * tiltSpeed
        );
    }

    void CalculateMouseTilt()
    {
        // 1. Give the mouse position the correct Z-depth before converting to world space
        Vector3 mouseScreen = Input.mousePosition;
        mouseScreen.z = Mathf.Abs(Camera.main.transform.position.z - tiltPivot.position.z);
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);

        // 2. Measure from the true CENTER of the card, not the pivot transform
        float deltaX = mouseWorld.x - cardCollider.bounds.center.x;

        // 3. Normalize based on the card's half-width (-1 at far left, 1 at far right)
        float normalizedX = deltaX / cardCollider.bounds.extents.x;
        normalizedX = Mathf.Clamp(normalizedX, -1f, 1f);

        // 4. Calculate rotation: Left edge = max tilt (30), Right edge = -max tilt (-30)
        float yRotation = -normalizedX * maxTiltAngle;

        // 5. Apply target rotation
        targetRotation = Quaternion.Euler(0f, yRotation, 0f);
    }
}