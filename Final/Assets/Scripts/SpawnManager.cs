using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject objectPrefab;
    private Vector3 spawnPos = new Vector3(25, 0, 0);
    private float startDelay = 1f;
    private float initialRepeatRate = 2f;
    private float minimumRepeatRate = 0.5f;
    private float repeatRateDecrease = 0.1f;
    private float currentRepeatRate;
    private float difficultyIncreaseInterval = 10f;
    private float nextDifficultyIncreaseTime;

    public float spawnRangeX = 5f;
    public float spawnRangeZ = 5f;
    public float spawnRangeY = 2f;

    private float timeSinceLastIncrease = 0f;
    private float increaseInterval = 10f;
    private int objectsToSpawn = 1;
    private int maxObjectsToSpawn = 3;

    private List<GameObject> spawnedObjects = new List<GameObject>();

    void Start()
    {
        currentRepeatRate = initialRepeatRate;
        nextDifficultyIncreaseTime = Time.time + difficultyIncreaseInterval;
        InvokeRepeating("SpawnObject", startDelay, currentRepeatRate);
    }

    void Update()
    {
        timeSinceLastIncrease += Time.deltaTime;

        if (timeSinceLastIncrease >= increaseInterval)
        {
            IncreaseObjectSpawnAmount();
            timeSinceLastIncrease = 0f;
        }

        if (Time.time >= nextDifficultyIncreaseTime)
        {
            IncreaseDifficulty();
            nextDifficultyIncreaseTime = Time.time + difficultyIncreaseInterval;
        }
    }

    void SpawnObject()
    {
        for (int i = 0; i < objectsToSpawn; i++)
        {
            Vector3 spawnPos = GetValidSpawnPosition();
            GameObject newObject = Instantiate(objectPrefab, spawnPos, objectPrefab.transform.rotation);
            spawnedObjects.Add(newObject);
        }
    }

    Vector3 GetValidSpawnPosition()
    {
        Vector3 randomSpawnPos;
        bool positionValid = false;

        int attempts = 0;
        do
        {
            float randomOffsetX = Random.Range(-spawnRangeX, spawnRangeX);
            float randomOffsetZ = Random.Range(-spawnRangeZ, spawnRangeZ);
            float randomOffsetY = Random.Range(-spawnRangeY, spawnRangeY);

            randomSpawnPos = new Vector3(spawnPos.x + randomOffsetX, spawnPos.y + randomOffsetY, spawnPos.z + randomOffsetZ);

            positionValid = IsPositionValid(randomSpawnPos);
            attempts++;
        } while (!positionValid && attempts < 10);

        return randomSpawnPos;
    }

    bool IsPositionValid(Vector3 position)
    {
        foreach (GameObject obj in spawnedObjects)
        {
            if (Vector3.Distance(position, obj.transform.position) < 2f)
            {
                return false;
            }
        }
        return true;
    }

    void IncreaseObjectSpawnAmount()
    {
        if (objectsToSpawn < maxObjectsToSpawn)
        {
            objectsToSpawn++;
            Debug.Log("More objects spawning! Now spawning " + objectsToSpawn + " objects.");
        }
        else
        {
            Debug.Log("Maximum spawn limit reached: " + objectsToSpawn + " objects.");
        }
    }

    void IncreaseDifficulty()
    {
        if (currentRepeatRate > minimumRepeatRate)
        {
            currentRepeatRate -= repeatRateDecrease;
            CancelInvoke("SpawnObject");
            InvokeRepeating("SpawnObject", 0f, currentRepeatRate);
        }
    }
}
