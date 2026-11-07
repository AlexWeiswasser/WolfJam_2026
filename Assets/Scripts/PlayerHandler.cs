using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHandler : MonoBehaviour
{
	[Header ("Object References")]
	[SerializeField] Transform dottedCircle;
	[SerializeField] Transform lampRoom;

	// Tracking 
	Vector3 currentMousePos;
	[SerializeField] float lightBeamSpeed;

	private void Update()
	{
		Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
		mouseWorldPos.z = 0f;
		currentMousePos = mouseWorldPos;
	}

	private void FixedUpdate()
	{
		dottedCircle.transform.position = currentMousePos;

		Vector3 dir = (currentMousePos - lampRoom.position).normalized;
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);

		lampRoom.transform.rotation = Quaternion.RotateTowards(
			lampRoom.transform.rotation,
			targetRotation,
			lightBeamSpeed * Time.fixedDeltaTime
		);

	}
}
