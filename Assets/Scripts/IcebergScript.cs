using UnityEngine;

public class IcebergScript : MonoBehaviour
{
	[SerializeField] float speed;
	[SerializeField] Rigidbody2D rb;
	Vector2 currentVelocity = Vector2.zero;

	private void FixedUpdate()
	{
		currentVelocity = rb.position + (Vector2)transform.right * speed * Time.fixedDeltaTime;
		rb.MovePosition(currentVelocity);
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Boat")) Destroy(gameObject);
		else if (collision.CompareTag("Wall")) Destroy(gameObject);
		else if (collision.CompareTag("Iceberg")) Destroy(gameObject);
		else if (collision.CompareTag("Edge"))
		{
			Vector2 closest = collision.ClosestPoint(rb.position);
			Vector2 normal = (rb.position - closest).normalized;
			Vector2 heading = transform.right;

			if (normal != Vector2.zero && Vector2.Dot(heading, normal) < 0f)
			{
				Vector2 newDirection = Vector2.Reflect(heading, normal);
				float newAngle = Mathf.Atan2(newDirection.y, newDirection.x) * Mathf.Rad2Deg;
				rb.rotation = newAngle;
			}
		}
	}
}
