using System;
using UnityEngine;

public class DockScript : MonoBehaviour
{
	[SerializeField] BoatScript.ShipType shipType;
	[SerializeField] bool tutorialDock = false;

	public static event Action<BoatScript.ShipType> DockReached;
	public static event Action TutorialDockReached;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.CompareTag("Boat"))
		{
			Transform boatParent = collision.transform.parent;

			if (boatParent)
			{
				if (boatParent.TryGetComponent<BoatScript>(out var bScript))
				{
					if (bScript.GetShipType() == shipType)
					{
						PointTracker.GainPoints();
						bScript.Obliterate(true);
						DockReached.Invoke(shipType);
						if (tutorialDock)
						{
							TutorialDockReached?.Invoke();
						}
					}
				}
			}
		}
	}
}
