using System.Collections;
using UnityEngine;

public class TutorialBoatScript : BoatScript
{
    private float baseSpeed;
    protected override void Awake()
    {
        
    }

    void Start()
    {
        baseSpeed = speed;
        speed = 0;
		shipType = ShipType.Viking;

		var main = burstParticles.main;
		var main2 = deathParticles.main;

        rend.sprite = shipSprites[2];
        main.startColor = Color.green;
        main2.startColor = Color.green;
    }

    public override IEnumerator BoostBoat() 
	{
        speed = baseSpeed;
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

		burstParticles.Clear();
		burstParticles.Play();
	}
}
