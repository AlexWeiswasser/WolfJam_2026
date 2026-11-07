using UnityEngine;

public class CameraScript : MonoBehaviour
{
	public void ChangeFloor(int floor) 
	{
		transform.position = new Vector3(0, -30 + floor * 10, -10);
	}
}
