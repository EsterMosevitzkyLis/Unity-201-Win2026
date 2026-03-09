using UnityEngine;

public class CardLookAtCursor : MonoBehaviour
{
    [Header("Hierarchy Setup")]
    [Tooltip("Drag the child GameObject containing your sprites into this slot in the Inspector.")]
    public Transform graphicToTilt;         

    [Tooltip("Check this to allow the card to tilt and pop out. Leave unchecked if it starts face-down.")]
    public bool canTilt = false;

    [Header("Tilt Settings")]
    public float rotationAmount = 10f;      
    public float rotationSpeed = 10f;       
    public float deadzone = 0.1f;           

    [Header("Hover Movement Settings")]
    [Tooltip("The maximum distance the card moves on the Z axis when hovered dead-center.")]
    public float hoverZOffset = -0.2f;
    
    [Tooltip("How fast the card moves to its hovered position.")]
    public float moveSpeed = 10f;
    
    [Tooltip("The distance from the center to calculate the Z falloff.")]
    public float edgeRadius = 2f;           
    
    [Tooltip("If checked, the Z-pop will only flatten out when moving the mouse left/right. Moving up/down keeps it fully popped out.")]
    public bool limitFalloffToXAxis = false; // NEW: The checkbox to isolate the X-axis

    private Quaternion originalLocalRotation;
    private Vector3 originalLocalPosition;  
    
    private Camera mainCamera;
    private bool isMouseOver = false;
    
    private Quaternion targetRotation;
    private Vector3 targetPosition;         

    void Start()
    {
        mainCamera = Camera.main;
        
        if (graphicToTilt == null)
        {
            Debug.LogWarning("Graphic To Tilt is not assigned! Defaulting to the parent transform.");
            graphicToTilt = transform;
        }

        originalLocalRotation = graphicToTilt.localRotation;
        originalLocalPosition = graphicToTilt.localPosition;
        
        targetRotation = originalLocalRotation;
        targetPosition = originalLocalPosition;
    }

    void OnMouseEnter() => isMouseOver = true;
    
    void OnMouseExit()
    {
        isMouseOver = false;
    }

    void Update()
    {
        if (isMouseOver && mainCamera != null && canTilt)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out RaycastHit hit, 100f) && hit.transform == transform)
            {
                Vector3 localPoint = transform.InverseTransformPoint(hit.point);
                Vector2 local2D = new Vector2(localPoint.x, localPoint.y);
                
                // --- UPDATED DYNAMIC Z-OFFSET LOGIC ---
                // Determine which distance we care about based on your new checkbox
                float distanceForFalloff = limitFalloffToXAxis ? Mathf.Abs(local2D.x) : local2D.magnitude;

                float popFactor = Mathf.Clamp01(1f - (distanceForFalloff / edgeRadius));
                targetPosition = originalLocalPosition + new Vector3(0f, 0f, hoverZOffset * popFactor);


                // --- EXISTING TILT LOGIC ---
                if (local2D.magnitude > deadzone)
                {
                    float rotX = Mathf.Clamp(local2D.y, -1f, 1f) * rotationAmount;
                    float rotY = Mathf.Clamp(-local2D.x, -1f, 1f) * rotationAmount;
                    
                    targetRotation = originalLocalRotation * Quaternion.Euler(rotX, rotY, 0f);
                }
                else
                {
                    targetRotation = originalLocalRotation;
                }
            }
            else 
            {
                targetRotation = originalLocalRotation;
                targetPosition = originalLocalPosition;
            }
        }
        else
        {
            targetRotation = originalLocalRotation;
            targetPosition = originalLocalPosition;
        }

        graphicToTilt.localRotation = Quaternion.Slerp(graphicToTilt.localRotation, targetRotation, Time.deltaTime * rotationSpeed);
        graphicToTilt.localPosition = Vector3.Lerp(graphicToTilt.localPosition, targetPosition, Time.deltaTime * moveSpeed);
    }

    // --- ANIMATION EVENT METHODS ---

    public void EnableTilt()
    {
        canTilt = true;
    }

    public void DisableTilt()
    {
        canTilt = false;
        targetRotation = originalLocalRotation; 
        targetPosition = originalLocalPosition; 
    }
}