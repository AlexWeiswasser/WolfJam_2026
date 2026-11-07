using UnityEngine;

public class BoatScript : MonoBehaviour
{
	[Header ("Stats")]
	[SerializeField] float speed;

	private void FixedUpdate()
	{
		transform.position += transform.right * speed * Time.fixedDeltaTime;
	}

	public void RotateBoat(float angle)
	{
		transform.rotation = Quaternion.Euler(0f, 0f, angle);
	}
}
