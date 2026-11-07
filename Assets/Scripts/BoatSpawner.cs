using System.Collections.Generic;
using UnityEngine;

public class BoatSpawner : MonoBehaviour
{
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
            return new Vector2(Random.Range(minPos.x, maxPos.x), minPos.y);  
        }
        public Vector3 GetSpawnRot()
        {
            return new Vector3(0, 0, Random.Range(minRot, maxRot));
        }
    }
    List<Spawn> spawns = new List<Spawn>();
    [SerializeField] Transform topSpawnParent;

	private void Awake()
	{
        Spawn topSpawn = new Spawn();
        topSpawn.SetMin(topSpawnParent.GetChild(0));
        topSpawn.SetMax(topSpawnParent.GetChild(1));
        spawns.Add(topSpawn);
	}

    // Spawning
    [SerializeField] GameObject[] boatPrefabs;
    float timeTilSpawn = 5;
    float currentTime = 0;

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
        int boatType = Random.Range(0, boatPrefabs.Length);
        int spawn = Random.Range(0, spawns.Count);
        Vector2 spawnPos = spawns[spawn].GetSpawnPos();
        Quaternion spawnRot = Quaternion.Euler(spawns[spawn].GetSpawnRot());
        Instantiate(boatPrefabs[boatType], spawnPos, spawnRot);
    }
}
