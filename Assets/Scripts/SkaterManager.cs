using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkaterManager : MonoBehaviour
{

    public GameObject skater;
    public float spawnRadius = 10;
    public int amount = 20;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    public void Initialize(){
        for(int i = 0; i < amount; i++){
            SpawnSkater();
        }

    }
    public void SpawnSkater(){
    
        Vector3 spawnPos = Random.insideUnitCircle * spawnRadius;
        GameObject newSkater = Instantiate(skater, spawnPos, Quaternion.identity);

    }
}
