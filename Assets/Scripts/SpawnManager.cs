using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public float startDelay=1f;
    public float repeatRate=1f;
    float startZ=20;
    float minX=-19;
    float maxX=20;
    public GameObject[]  goodGuys;
    public GameObject[] badGuys;
    public GameObject[] explosions;
    public float goodToBadRatio=0.1f;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnGameObject",startDelay, repeatRate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnGameObject(){
        float x = Random.value;
        if(x>goodToBadRatio){
            spwanBadGuy();
        } else {
            spwanGoodGuy();
        }

    }
    void spwanBadGuy(){
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
    void spwanGoodGuy(){
            //Debug.Log("New Good guy");
            int animalIdx = Random.Range(0,goodGuys.Length);
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
