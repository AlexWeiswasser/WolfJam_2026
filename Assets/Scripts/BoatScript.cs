using UnityEngine;

public class BoatScript : MonoBehaviour
{
	public enum ShipType
	{
		Cowboy,
		Robot,
		Viking,
		Skeleton
	}

	[Header ("Stats")]
	[SerializeField] float speed;
	ShipType shipType;
	public ShipType GetShipType() => shipType;

	[Header ("References")]
	[SerializeField] Rigidbody2D rb;
	[SerializeField] SpriteRenderer rend;
	[SerializeField] Collider2D col;

	private void Awake()
	{
		int type = Random.Range(0, 4);
		shipType = (ShipType)type;

		switch (type)
		{
			case 0:
				{
					rend.color = Color.darkKhaki;
				}
				break;
			case 1:
				{
					rend.color = Color.red;
				}
				break;
			case 2:
				{
					rend.color = Color.yellow;
				}
				break;
			case 3:
				{
					rend.color = Color.purple;
				}
				break;
		}
	}

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
		if (collision.CompareTag("Boat")) Obliterate(gameObject);
		else if (collision.CompareTag("Wall")) Obliterate(gameObject);
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

	public void Obliterate(bool success)
	{
		if (success) ReputationTracker.AddReputation();
		else ReputationTracker.LoseReputation();
		Destroy(gameObject);
	}
}
