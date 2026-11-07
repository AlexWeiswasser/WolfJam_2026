using System.Collections;
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
	[SerializeField] float boostForce;
	ShipType shipType;
	public ShipType GetShipType() => shipType;

	[Header ("References")]
	[SerializeField] Rigidbody2D rb;
	[SerializeField] SpriteRenderer rend;
	[SerializeField] ParticleSystem burstParticles; 
	[Tooltip ("Cowboy, Robot, Viking, Skeleton")]
	[SerializeField] Sprite[] shipSprites;

	// Movement
	Vector2 currentVelocity = Vector2.zero;
	Vector2 currentBoostVelocity = Vector2.zero;
	float boostDamping = .9f;

	private void Awake()
	{
		int type = Random.Range(0, 4);
		shipType = (ShipType)type;

		var main = burstParticles.main;

		switch (type)
		{
			case 0:
				{
					rend.sprite = shipSprites[0];
					main.startColor = Color.yellow;
					rend.color = Color.green;
				}
				break;
			case 1:
				{
					rend.sprite = shipSprites[1];
					main.startColor = Color.red;
					rend.color = Color.red;
				}
				break;
			case 2:
				{
					rend.sprite = shipSprites[2];
					main.startColor = Color.green;
					rend.color = Color.green;
				}
				break;
			case 3:
				{
					rend.sprite = shipSprites[3];
					main.startColor = Color.purple;
					rend.color = Color.purple;
				}
				break;
		}
	}

	private void FixedUpdate()
	{
		currentBoostVelocity *= boostDamping;
		currentVelocity = rb.position + (Vector2)transform.right * speed * Time.fixedDeltaTime;
		currentVelocity += currentBoostVelocity * Time.fixedDeltaTime;

		rb.MovePosition(currentVelocity);
	}

	public void RotateBoat(float angle) { rb.MoveRotation(angle); }
	public IEnumerator BoostBoat() 
	{
		yield return new WaitForFixedUpdate(); 
		currentBoostVelocity += (Vector2)transform.right * boostForce;

		burstParticles.Clear();
		burstParticles.Play();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Boat")) Obliterate(false);
		else if (collision.CompareTag("Wall")) Obliterate(false);
		else if (collision.CompareTag("Iceberg")) Obliterate(false);
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
