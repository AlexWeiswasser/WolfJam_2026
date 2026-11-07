using System.Collections.Generic;
using UnityEngine;

public class BoatSpawner : MonoBehaviour
{
	private void OnEnable()
	{
		PointTracker.OnPointsGained += UpdatePoints;
	}

	private void OnDisable()
	{
		PointTracker.OnPointsGained -= UpdatePoints;
	}

	static float SignedAngle(float angle)
	{
		return Mathf.DeltaAngle(0f, angle);
	}

	// Spawn point tracking
	class Spawn
    {
        Vector2 minPos;
        Vector2 maxPos;
        float minRot;
        float maxRot;
        public bool sideWays = false; 

		public void SetMin(Transform min)
		{
			minPos = min.position;
			minRot = SignedAngle(min.eulerAngles.z);
		}
		public void SetMax(Transform max)
		{
			maxPos = max.position;
			maxRot = SignedAngle(max.eulerAngles.z);
		}
		public Vector2 GetSpawnPos()
        {
			if (sideWays)
			{
				return new Vector2(minPos.x, Random.Range(minPos.y, maxPos.y));
			}
			else
			{
				return new Vector2(Random.Range(minPos.x, maxPos.x), minPos.y);
			}
        }
		public Vector3 GetSpawnRot()
		{
			float delta = Mathf.DeltaAngle(minRot, maxRot);
			return new Vector3(0, 0, minRot + delta * Random.value);
		}
	}
    List<Spawn> spawns = new List<Spawn>();
    [SerializeField] Transform topSpawnParent;
    [SerializeField] Transform bottomSpawnParent;
    [SerializeField] Transform RightSpawnParent;

	private void Awake()
	{
        Spawn topSpawn = new Spawn();
        topSpawn.SetMin(topSpawnParent.GetChild(0));
        topSpawn.SetMax(topSpawnParent.GetChild(1));
        spawns.Add(topSpawn);

        Spawn bottomSpawn = new Spawn();
		bottomSpawn.SetMin(bottomSpawnParent.GetChild(0));
        bottomSpawn.SetMax(bottomSpawnParent.GetChild(1));
        spawns.Add(bottomSpawn);

        Spawn rightSpawn = new Spawn();
        rightSpawn.SetMin(RightSpawnParent.GetChild(0));
        rightSpawn.SetMax(RightSpawnParent.GetChild(1));
		rightSpawn.sideWays = true;
        spawns.Add(rightSpawn);
	}

	// Spawning
	[SerializeField] GameObject[] boatPrefabs;
	[SerializeField] GameObject iceBergPrefab;
	[SerializeField] LayerMask boatLayer;
    float timeTilSpawn = 10;
    float currentTime = 10;
	float distanceBetweenSpawnsRadius = 10;
	int amountTilBergSpawn = 6;
	int currentTimesSpawned = 0;

	private void Update()
	{
        currentTime += Time.deltaTime;
        if(currentTime >= timeTilSpawn)
        {
            currentTime -= timeTilSpawn;
            SpawnBoat();
        }
	}

    private void SpawnBoat()
    {
		Collider2D boat;
		Vector2 spawnPos;
		int spawn;
		do
		{
			spawn = Random.Range(0, spawns.Count);
			spawnPos = spawns[spawn].GetSpawnPos();
			boat = Physics2D.OverlapCircle(spawnPos, distanceBetweenSpawnsRadius, boatLayer);
		}
		while (boat != null);
		Quaternion spawnRot = Quaternion.Euler(spawns[spawn].GetSpawnRot());

		if (currentTimesSpawned >= amountTilBergSpawn)
		{
			Instantiate(iceBergPrefab, spawnPos, spawnRot);
			currentTimesSpawned = 0;
		}
		else
		{
			int boatType = Random.Range(0, boatPrefabs.Length);
			Instantiate(boatPrefabs[boatType], spawnPos, spawnRot);
			currentTimesSpawned++;
		}
	}

	void UpdatePoints(int pointsGained, Vector2Int range)
	{
		int mult = range.y / 500;
		float tempTimeToSpawn = 10 - (mult * .5f);
		timeTilSpawn = Mathf.Clamp(tempTimeToSpawn, 1, 10);
	}
}
