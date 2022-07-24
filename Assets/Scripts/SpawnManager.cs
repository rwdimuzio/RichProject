﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    const int SLOWEST_SPAWN_RATE = 10;
    const int FASTEST_SPAWN_RATE = 0;
    bool spawning = false;
    public float startDelay=1f;
    public float repeatRate=1f;
    private int ticksTilSpawn=10;
    float startZ=20;
    float minX=-19;
    float maxX=20;
    public GameObject  heroObject;
    public GameObject[]  goodGuys;
    public GameObject[] badGuys;
    public GameObject[] explosions;
    public float goodToBadRatio=0.1f;
    // Start is called before the first frame update
    void Start()
    {
        countdown = ticksTilSpawn;
        //spawnHero();
        InvokeRepeating("SpawnGameObject",startDelay, repeatRate);
    }
    public void startSpawning(){
        ticksTilSpawn = SLOWEST_SPAWN_RATE;
        spawning = true;
    }
    public void stopSpawning(){
        spawning = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    int ct=0;
    int countdown=0;
    private void SpawnGameObject(){
        if(!spawning) return;
        countdown--;
        if(countdown > 0 ) return;
        countdown = ticksTilSpawn; 
        float x = Random.value;
        if(x>goodToBadRatio){
            spawnBadGuy();
        } else {
            spawnGoodGuy();
        }
        if((++ct % 25) == 0){
            //Debug.Log("New Hero");
 //           spawnHero();
            if(ticksTilSpawn > 1){
                ticksTilSpawn--;
            }
        }

    }
    void spawnBadGuy(){
            //Debug.Log("New Bad guy");
            int animalIdx = Random.Range(0,badGuys.Length-1);
            Vector3 pos = new Vector3(
                Random.Range(minX,maxX),
                1,
                startZ
            );
            Instantiate(
                badGuys[animalIdx],
                pos,
                badGuys[animalIdx].transform.rotation
            );


    }
    void spawnGoodGuy(){
            //Debug.Log("New Good guy");
            int animalIdx = Random.Range(1,goodGuys.Length);
            Vector3 pos = new Vector3(
                Random.Range(minX,maxX),
                1,
                startZ
            );
            Instantiate(
                goodGuys[animalIdx],
                pos,
                goodGuys[animalIdx].transform.rotation
            );
        
    }
    public GameObject getExplosion(){
        return  explosions[0];
    } 
}
