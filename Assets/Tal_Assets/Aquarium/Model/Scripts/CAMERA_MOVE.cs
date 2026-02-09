using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    public Transform target;
    public float distance = 3f;
    public float height = 1.6f;
    public float speed = 90f; // מעלות לשנייה
    
    [Header("Delay")]
    public float startDelay = 4f; // זמן המתנה בשניות

    float angle;
    float elapsedTime;
    bool isRotating;

    void Start()
    {
        elapsedTime = 0f;
        isRotating = false;
    }

    void LateUpdate()
    {
        if (!target) return;

        // ספירת זמן עד שמתחילים לסובב
        if (!isRotating)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= startDelay)
            {
                isRotating = true;
            }
        }

        // סיבוב רק אחרי ה-delay
        if (isRotating)
        {
            angle += speed * Time.deltaTime;
        }

        var rot = Quaternion.Euler(0f, angle, 0f);

        Vector3 offset = rot * new Vector3(0f, 0f, -distance);
        Vector3 pos = target.position + offset + Vector3.up * height;

        transform.position = pos;
        transform.LookAt(target.position + Vector3.up * height);
    }
}