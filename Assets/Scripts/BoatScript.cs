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

	private void Awake()
	{
		int type = Random.Range(0, 4);
		shipType = (ShipType)type;

		switch (type)
		{
			case 0:
				{
					rend.color = Color.brown;
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
		if (collision.CompareTag("Boat")) Destroy(gameObject);
	}
}
