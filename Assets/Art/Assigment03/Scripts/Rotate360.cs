using UnityEngine;

public class Rotate360 : MonoBehaviour
{
    public float rotationDuration = 7f;

    float elapsed = 0f;

    void Update()
    {
        if (elapsed >= rotationDuration) return;

        float anglePerSecond = 360f / rotationDuration;
        transform.Rotate(Vector3.up, anglePerSecond * Time.deltaTime);

        elapsed += Time.deltaTime;
    }
}
