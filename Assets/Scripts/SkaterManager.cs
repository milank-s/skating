﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkaterManager : MonoBehaviour
{

    [Header("boid tuning")]
    public float rotateAroundRinkSpeed = 25;
    public float followRinkWeight = 5;
	public float minSeparationLength;
	public float separationSphereRadius;
	public float separationWeight;
	public float cohesionSphereRadius;
	public float cohesionWeight;
	public float alignmentSphereRadius;
	public float alignmentWeight;
	public float speed;
	public float boidFOV;

    [Space(10)]

    public float minSpawnDistance = 3f;
    public SkaterBoid skater;
    public float spawnRadius = 10;
    public int amount = 20;

    [HideInInspector]
    public List<SkaterBoid> skaters;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        SkaterBoid s = skaters[0];
        rotateAroundRinkSpeed = s.rotateSpeed;
        followRinkWeight =  s.followCircleWeight;
        minSeparationLength =  s.minSeparationLength;
        separationSphereRadius = s.separationSphereRadius;
        separationWeight = s.separationWeight;
        alignmentSphereRadius = s.alignmentSphereRadius;
        alignmentWeight = s.alignmentWeight;
        cohesionSphereRadius =  s.cohesionSphereRadius;
        cohesionWeight =  s.cohesionWeight;
        boidFOV = s.angle;
    }

    public void Initialize(){
        for(int i = 0; i < amount; i++){
            SpawnSkater();
            
        }
    }

    public void SpawnSkater(){
        Vector3 spawnPos = Random.insideUnitCircle.normalized * minSpawnDistance;
        spawnPos += spawnPos.normalized * Random.Range(0f, spawnRadius-minSpawnDistance);
        SkaterBoid newSkater = Instantiate(skater, spawnPos, Quaternion.identity);
        newSkater.transform.parent = transform;
        skaters.Add(newSkater);
        
    }

    public void SaveValuesToPrefab(){
        SkaterBoid s = skater;
            s.rotateSpeed = rotateAroundRinkSpeed;
            s.followCircleWeight = followRinkWeight;
            s.minSeparationLength = minSeparationLength;
            s.separationWeight = separationWeight;
            s.alignmentSphereRadius =alignmentSphereRadius;
            s.separationSphereRadius = separationSphereRadius;
            s.alignmentWeight = alignmentWeight;
            s.cohesionSphereRadius = cohesionSphereRadius;
            s.cohesionWeight = cohesionWeight;
            s.angle = boidFOV;
    }
    
    public void Update(){
        foreach(SkaterBoid s in skaters){
            s.rotateSpeed = rotateAroundRinkSpeed;
            s.followCircleWeight = followRinkWeight;
            s.minSeparationLength = minSeparationLength;
            s.separationWeight = separationWeight;
            s.separationSphereRadius = separationSphereRadius;
            s.alignmentSphereRadius =alignmentSphereRadius;
            s.alignmentWeight = alignmentWeight;
            s.cohesionSphereRadius = cohesionSphereRadius;
            s.cohesionWeight = cohesionWeight;
            s.angle = boidFOV;
        }
    }
}
