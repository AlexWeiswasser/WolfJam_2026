using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHandler : MonoBehaviour
{
	[Header ("Object References")]
	[SerializeField] Transform dottedCircle;
	[SerializeField] Transform lightHouse;

	// Tracking 
	Vector3 currentMousePos;
	[SerializeField] float lightCircleSpeed;

	private void Update()
	{
		Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
		mouseWorldPos.z = 0f;
		currentMousePos = mouseWorldPos;
	}

	private void FixedUpdate()
	{
		dottedCircle.transform.position = currentMousePos;

		Vector3 dir = (currentMousePos - lightHouse.position).normalized;
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		lightHouse.transform.rotation = Quaternion.Euler(0f, 0f, angle);
	}
}
