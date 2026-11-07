using UnityEngine;

public class BoatScript : MonoBehaviour
{
	[Header ("Stats")]
	[SerializeField] float speed;

	private void FixedUpdate()
	{
		transform.position += transform.up * speed * Time.fixedDeltaTime;
	}
}
