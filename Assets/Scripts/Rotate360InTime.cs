using UnityEngine;

public class Rotate360InTime : MonoBehaviour
{
	public float duration = 2f; // Seconds for a full 360 rotation

	float timer;

	void Update()
	{
		timer += Time.deltaTime;

		float t = timer / duration;
		float angle = Mathf.Lerp(0f, 360f, t);

		transform.rotation = Quaternion.Euler(0f, angle, 0f);

		if (t >= 1f)
			timer = 0f;
	}
}