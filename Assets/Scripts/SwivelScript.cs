using UnityEngine;

public class SwivelScript : MonoBehaviour, ILight
{
    [SerializeField] Collider2D col;
	Transform player;
	float currentZRotation = 0f;
	float startRot;

	private void Awake()
	{
		startRot = transform.eulerAngles.z;
	}

	public void Interact(PlayerScript pScript)
    {
        pScript.LockInLight(this, col.transform.position);
		player = pScript.transform;
    }

    public void GiveInput(Vector2 input)
    {
		int reverseRot = transform.position.x > player.transform.position.x ? 1 : -1;
		if (input.y > 0)
		{
			currentZRotation += 1f * reverseRot;
		}
		else if (input.y < 0)
		{
			currentZRotation -= 1f * reverseRot;
		}

		currentZRotation = Mathf.Clamp(currentZRotation, -45f, 45f);
		transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, startRot + currentZRotation);
	}
}
