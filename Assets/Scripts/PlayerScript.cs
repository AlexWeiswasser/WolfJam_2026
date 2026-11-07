using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerScript : MonoBehaviour
{
    // Public data
    int currentFloor = 3; // 3 is top, 2 is medium, 1 is ground
    public int GetFloor() => currentFloor; 

	[Header("Movement Stats")]
    [SerializeField] float moveSpeed = 5f;
    Vector2 movementDir;

    [Header ("Interaction Data")]
    [SerializeField] float interactionRadius = 1f;
    [SerializeField] LayerMask interactableLayer;
    float interactionCooldown = .1f;
    float interactCDTracker = 0;
    bool canInteract = true; 

    [Header("References")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] CameraScript camScript;

    // Lifetime functions
    public void ReceiveInput(InputAction.CallbackContext context)
    {
		movementDir = context.ReadValue<Vector2>().normalized;

		// Y based input
		if (movementDir.y > 0) { AttemptInteract(up: true); }
        else if (movementDir.y < 0) { AttemptInteract(up: false); }
    }

	private void FixedUpdate()
	{
		rb.linearVelocityX = movementDir.x * moveSpeed;
	}

    private void Update()
    {
        if (!canInteract)
        {
            interactCDTracker += Time.deltaTime;
            if(interactCDTracker > interactionCooldown)
            {
                canInteract = true;
                interactCDTracker = 0;
            }
        }
    }

    // Interaction functions
	private void AttemptInteract(bool up)
    {
        Collider2D nearbyInteractible = Physics2D.OverlapCircle(transform.position, interactionRadius, interactableLayer);
        if (nearbyInteractible)
        {
            IInteractable inter = nearbyInteractible.GetComponentInParent<IInteractable>();
            if (inter != null)
            {
                inter.Interact(up);
                canInteract = false; 
            }
        }
    }

    public void ChangeFloor(bool up, Vector2 pos, bool canDownFromMiddle, bool canUpFromMiddle)
    {
        if (!canInteract) return;

        if (up && currentFloor == 3) return;
        else if (!up && currentFloor == 1) return;
        else if (!up && currentFloor == 2 && !canDownFromMiddle) return;
        else if (up & currentFloor == 2 && !canUpFromMiddle) return;

        transform.position = pos;
		currentFloor = up ? ++currentFloor : --currentFloor;
		camScript.ChangeFloor(currentFloor);
	}
}

public interface IInteractable
{
	public void Interact(bool up);
}
