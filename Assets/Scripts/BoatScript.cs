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
	[SerializeField] protected float speed;
	[SerializeField] protected float boostForce;
	protected ShipType shipType;
	public ShipType GetShipType() => shipType;

	[Header ("References")]
	[SerializeField] protected Rigidbody2D rb;
	[SerializeField] protected SpriteRenderer rend;
	[SerializeField] protected ParticleSystem burstParticles;
	[SerializeField] protected ParticleSystem deathParticles; 
	[Tooltip ("Cowboy, Robot, Viking, Skeleton")]
	[SerializeField] protected Sprite[] shipSprites;

	[Header("Sounds")]
	[SerializeField] protected AudioClip[] cowboySounds;
	[SerializeField] protected AudioClip[] robotSounds;
	[SerializeField] protected AudioClip[] vikingSounds;
	[SerializeField] protected AudioClip[] skeletonSounds;
	[SerializeField] protected AudioClip boatCrash;
    [SerializeField] protected AudioClip scorePoint;
	[SerializeField] protected float sfxCooldown;
	[SerializeField] protected AudioClip defaultDash;
	protected bool canPlaySound = true;

    // Movement
    protected Vector2 currentVelocity = Vector2.zero;
	protected Vector2 currentBoostVelocity = Vector2.zero;
	protected float boostDamping = .9f;

	protected virtual void Awake()
	{
		int type = Random.Range(0, 4);
		shipType = (ShipType)type;

		var main = burstParticles.main;
		var main2 = deathParticles.main;

		switch (type)
		{
			case 0:
				{
					rend.sprite = shipSprites[0];
					main.startColor = Color.yellow;
					main2.startColor = Color.yellow;
				}
				break;
			case 1:
				{
					rend.sprite = shipSprites[1];
					main.startColor = Color.red;
					main2.startColor = Color.red;
				}
				break;
			case 2:
				{
					rend.sprite = shipSprites[2];
					main.startColor = Color.green;
					main2.startColor = Color.green;
				}
				break;
			case 3:
				{
					rend.sprite = shipSprites[3];
					main.startColor = Color.purple;
					main2.startColor = Color.purple;
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
	public virtual IEnumerator BoostBoat() 
	{
		yield return new WaitForFixedUpdate(); 
		currentBoostVelocity += (Vector2)transform.right * boostForce;

		if (canPlaySound)
		{
			switch ((int)GetShipType())
			{
				case 0:
					int sound = Random.Range(0, cowboySounds.Length);
					MusicManager.Instance.PlaySfx(cowboySounds[sound]);
					break;
				case 1:
					int sound1 = Random.Range(0, robotSounds.Length);
					MusicManager.Instance.PlaySfx(robotSounds[sound1]);
					break;
				case 2:
					int sound2 = Random.Range(0, vikingSounds.Length);
					MusicManager.Instance.PlaySfx(vikingSounds[sound2]);
					break;
				case 3:
					int sound3 = Random.Range(0, skeletonSounds.Length);
					MusicManager.Instance.PlaySfx(skeletonSounds[sound3]);
					break;
				default:
					break;
			}
			canPlaySound = false;
			StartCoroutine(WaitForSeconds(sfxCooldown));
		}
		else
			MusicManager.Instance.PlaySfx(defaultDash);

		burstParticles.Clear();
		burstParticles.Play();
	}

	protected virtual IEnumerator WaitForSeconds(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		canPlaySound = true;
	}

	protected virtual void OnTriggerEnter2D(Collider2D collision)
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
		if (!success) MusicManager.Instance.PlaySfx(boatCrash);
		else if (success) MusicManager.Instance.PlaySfx(scorePoint);
		if (!success)
		{
			deathParticles.transform.SetParent(null);
			deathParticles.Clear();
			deathParticles.Play();
		}

		Destroy(gameObject);
	}
}
