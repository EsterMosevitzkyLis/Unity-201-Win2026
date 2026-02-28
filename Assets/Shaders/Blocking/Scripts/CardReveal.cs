using UnityEngine;

public class CardReveal : MonoBehaviour
{
    [Header("Parents")]
    public GameObject backParent;
    public GameObject frontParent;
    public GameObject auraParent;

    [Header("Tilt")]
    public Transform tiltPivot; // child object (NOT animated)
    public float maxTiltAngle = 15f;
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

        frontParent.SetActive(false);
        auraParent.SetActive(false);

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
        frontParent.SetActive(true);
        auraParent.SetActive(true);
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
        // Get mouse world position
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = tiltPivot.position.z; // match pivot Z

        // X difference from pivot
        float deltaX = mouseWorld.x - tiltPivot.position.x;

        // Optional scaling factor to control max tilt
        float factor = 0.5f; // adjust to taste
        float yRotation = -deltaX * maxTiltAngle / factor;

        // Clamp to maxTiltAngle
        yRotation = Mathf.Clamp(yRotation, -maxTiltAngle, maxTiltAngle);

        // Apply rotation
        targetRotation = Quaternion.Euler(0f, yRotation, 0f);
    }
}