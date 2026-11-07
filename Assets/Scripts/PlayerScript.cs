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
    bool canMove = true; 
    Vector2 inputVector;

    [Header ("Interaction Data")]
    [SerializeField] float interactionRadius = .75f;
    [SerializeField] LayerMask interactableLayer;
    [SerializeField] LayerMask ladderLayer; 
    float interactionCooldown = .1f;
    float interactCDTracker = 0;
    bool canInteract = true; 

    [Header("References")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] CameraScript camScript;

    // Light data
    ILight currentLight = null;

    // Lifetime functions
    public void ReceiveInput(InputAction.CallbackContext context)
    {
		inputVector = context.ReadValue<Vector2>();

        // Ladder movement
        if (!canMove) return;
		if (inputVector.y > 0) { AttemptLadder(up: true); }
        else if (inputVector.y < 0) { AttemptLadder(up: false); }
    }

    public void AttemptInteraction(InputAction.CallbackContext context)
    {
        if(context.performed)
            AttemptInteract();
    }

	private void FixedUpdate()
	{
        if (currentLight != null) currentLight.GiveInput(inputVector);
		if (!canMove) return;

        if(Mathf.Abs(inputVector.x) > .1f)
		    rb.linearVelocityX = Mathf.Sign(inputVector.x) * moveSpeed;
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
	private void AttemptInteract()
    {
		if (!canInteract) return;

		Collider2D nearbyInteractible = Physics2D.OverlapCircle(transform.position, interactionRadius, interactableLayer);
        if (nearbyInteractible)
        {
            ILight inter = nearbyInteractible.GetComponentInParent<ILight>();
            if (inter != null)
            {
                inter.Interact(this);
                canInteract = false; 
            }
        }
    }
    public void LockInLight(ILight iLight, Vector2 pos)
    {
        // If already locked
        if(!canMove) 
        {
			canMove = true;
			currentLight = null;
            return;
		}

        canMove = false;
        rb.linearVelocityX = 0;
		transform.position = pos;
		currentLight = iLight;
	}

	// Ladder things
	private void AttemptLadder(bool up)
    {
		if (!canInteract) return;

		Collider2D nearbyLadder = Physics2D.OverlapCircle(transform.position, interactionRadius, ladderLayer);
		if (nearbyLadder)
		{
			LadderScript inter = nearbyLadder.GetComponentInParent<LadderScript>();
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

public interface ILight
{
	public void Interact(PlayerScript pScript);
    public void GiveInput(Vector2 input);
}
