using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeCreator : MonoBehaviour
{
    public float spawnDelay;
    public GameObject spawnee;
    private GameObject birdSpawner;
    private Vector3 spawnPos;
    public bool isLoaded = false;

    // Start is called before the first frame update
    void Start()
    {
        TriggerSpawn();
    }

    public void TriggerSpawn()
    {
        InvokeRepeating("SpawnPipe", 0.0f, spawnDelay);
    }

    public void TriggerSpawnDelayed()
    {
        InvokeRepeating("SpawnPipe", 5.0f, spawnDelay);
    }

    public void SpawnPipe()
    {
        float pipeHeight = Random.Range(-3.0f, 1.8f);
        spawnPos = new Vector3(12.0f, pipeHeight);
        Instantiate(spawnee, spawnPos, transform.rotation);
    }
}
