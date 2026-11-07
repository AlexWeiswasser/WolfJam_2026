using UnityEngine;

public class LadderScript : MonoBehaviour
{
	[SerializeField] PlayerScript player;
	[SerializeField] Transform trapDoor;
	[SerializeField] Transform ladder;
	[SerializeField] bool canDownFromMiddle = true;
	[SerializeField] bool canUpFromMiddle = true; 
	public void Interact(bool up) 
	{
		Vector2 pos = up ? trapDoor.position : ladder.position;
		player.ChangeFloor(up, pos, canDownFromMiddle, canUpFromMiddle);
	}
}
