using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHandler : MonoBehaviour
{
	[Header("Object References")]
	[SerializeField] DraggingCircleScript draggingCircle;
	[SerializeField] Transform lampRoom;

	[Header ("Tracking")]
	Vector3 currentMousePos;
	bool circleTracks = true; 
	[SerializeField] float lightBeamSpeed;
	[SerializeField] LayerMask boatLayer;

	// Boat manipulation
	Collider2D[] currentBoats;
	Vector2 mouseStart;
	Vector2 mouseEnd;

	private void Update()
	{
		Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
		mouseWorldPos.z = 0f;
		currentMousePos = mouseWorldPos;
	}

	private void FixedUpdate()
	{
		if (!circleTracks) return;

		// Dotted circles
		draggingCircle.SetPosition(currentMousePos);

		// Light rotation
		Vector3 dir = (currentMousePos - lampRoom.position).normalized;
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);

		lampRoom.transform.rotation = Quaternion.RotateTowards(
			lampRoom.transform.rotation,
			targetRotation,
			lightBeamSpeed * Time.fixedDeltaTime
		);
	}

	public void MousePress(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			if (PauseCanvas.Paused) return;
			circleTracks = false;
			mouseStart = currentMousePos;
			draggingCircle.ToggleArrow(true);
		}
		else if (context.canceled)
		{
			mouseEnd = currentMousePos;
			DirectShips();
			circleTracks = true; 
			draggingCircle.ToggleArrow(false);
		}
	}

	private bool ObtainShips()
	{
		float radius = draggingCircle.GetScale().x / 2;

		if (Vector2.Distance(currentMousePos, draggingCircle.GetPosition()) < radius) 
		return false;

		currentBoats = Physics2D.OverlapCircleAll(draggingCircle.GetPosition(), radius, boatLayer);
		return true;
	}

	private void DirectShips()
	{
		Vector2 dir = (mouseEnd - mouseStart).normalized;
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

		if (!ObtainShips()) return;

		foreach (Collider2D boat in currentBoats)
		{
			BoatScript bScript = boat.GetComponentInParent<BoatScript>();
			if (bScript)
			{
				bScript.RotateBoat(angle);
				bScript.StartCoroutine(bScript.BoostBoat());
			}
		}
	}
}
