using UnityEngine;

public class DockScript : MonoBehaviour
{
	[SerializeField] BoatScript.ShipType shipType;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.CompareTag("Boat"))
		{
			if(collision.TryGetComponent<BoatScript>(out var bScript))
			{
				if (bScript.GetShipType() == shipType)
				{
					PointTracker.GainPoints();
					Destroy(collision.gameObject);
				}
			}
		}
	}
}
