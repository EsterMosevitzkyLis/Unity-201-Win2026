using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    public Transform target;
    public float distance = 3f;
    public float height = 1.6f;
    public float speed = 90f; // מעלות לשנייה

    float angle;

    void LateUpdate()
    {
        if (!target) return;

        angle += speed * Time.deltaTime;
        var rot = Quaternion.Euler(0f, angle, 0f);

        Vector3 offset = rot * new Vector3(0f, 0f, -distance);
        Vector3 pos = target.position + offset + Vector3.up * height;

        transform.position = pos;
        transform.LookAt(target.position + Vector3.up * height);
    }
}
