using UnityEngine;

public class BoatScript : MonoBehaviour
{
	[Header ("Stats")]
	[SerializeField] float speed;

	[Header ("References")]
	[SerializeField] Rigidbody2D rb;

	private void FixedUpdate()
	{
		Vector3 newPosition = rb.position + (Vector2)transform.right * speed * Time.fixedDeltaTime;
		rb.MovePosition(newPosition);
	}

	public void RotateBoat(float angle)
	{
		rb.MoveRotation(angle);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Boat")) Destroy(gameObject);
	}
}
