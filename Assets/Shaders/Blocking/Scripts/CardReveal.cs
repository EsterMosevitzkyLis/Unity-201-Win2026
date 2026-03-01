using UnityEngine;

public class CardReveal : MonoBehaviour
{
    [Header("Parents")]
    public GameObject backParent;
    public GameObject frontParent;
    public GameObject auraParent;

    [Header("Tilt")]
    public Transform tiltPivot;
    public float maxTiltAngle = 30f;
    public float tiltSpeed = 10f;

    [Header("Back Shake")]
    public float hoverShakeAmount = 1f;
    public float hoverShakeSpeed = 20f;

    public float clickShakeAmount = 4f;
    public float clickShakeDuration = 0.25f;
    public float clickShakeSpeed = 40f;

    private Animator anim;
    private bool revealed = false;
    private bool isMouseOver = false;
    private Quaternion targetRotation;

    private Collider2D cardCollider;
    private float clickShakeTimer = 0f;
    private Quaternion originalRotation;

    void Awake()
    {
        anim = GetComponent<Animator>();
        cardCollider = GetComponent<Collider2D>();
        auraParent.SetActive(false);
        originalRotation = transform.localRotation;

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

        transform.localRotation = originalRotation;

        clickShakeTimer = clickShakeDuration;

        anim.SetBool("HoverBack", false);
        anim.SetTrigger("RevealCard");
    }

    public void SwapToFront()
    {
        backParent.SetActive(false);
        auraParent.SetActive(true);
    }

    void Update()
    {
        if (!revealed)
        {
            if (isMouseOver)
            {
                float shakePower = hoverShakeAmount;
                float shakeSpeed = hoverShakeSpeed;

                if (clickShakeTimer > 0f)
                {
                    shakePower = clickShakeAmount;
                    shakeSpeed = clickShakeSpeed;
                    clickShakeTimer -= Time.deltaTime;
                }

                float shake = Mathf.Sin(Time.time * shakeSpeed) * shakePower;

                transform.localRotation = originalRotation * Quaternion.Euler(0f, 0f, shake);
            }
            else
            {
                transform.localRotation = Quaternion.Lerp(
                    transform.localRotation,
                    originalRotation,
                    Time.deltaTime * 8f
                );
            }

            return;
        }

        if (tiltPivot == null)
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
        Vector3 mouseScreen = Input.mousePosition;
        mouseScreen.z = Mathf.Abs(Camera.main.transform.position.z - tiltPivot.position.z);
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);

        float deltaX = mouseWorld.x - cardCollider.bounds.center.x;

        float normalizedX = deltaX / cardCollider.bounds.extents.x;
        normalizedX = Mathf.Clamp(normalizedX, -1f, 1f);

        float yRotation = -normalizedX * maxTiltAngle;

        targetRotation = Quaternion.Euler(0f, yRotation, 0f);
    }
}