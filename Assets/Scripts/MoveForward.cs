using System.Net.NetworkInformation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveForward : MonoBehaviour
{
    SpawnManager gameControl;
    public enum UnitType
    {
        Ufo1,
        Ufo2,
        Ufo3,
        Pew1,
        Pew2,
        Powerup1,
        Powerup2
    };

    public UnitType unitType=UnitType.Ufo1;

    public int points = 100;
    public float hits=1;
    public float speed=1;
    private float lowerZ=-10;
    private float upperZ=30;
    // Start is called before the first frame update
    void Start()
    {
        gameControl = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //speed=0.25f;
        transform.Translate(Vector3.forward * Time.deltaTime * speed);    
        if(transform.position.z < lowerZ || transform.position.z > upperZ){
            Destroy(gameObject);
        } 
    }
    private void OnTriggerEnter(Collider other){
        Debug.Log("triggered "+gameObject.tag+" was hit by "+other.gameObject.tag);
        if(gameObject.CompareTag("Pew")){
            if(other.gameObject.CompareTag("Ufo")){
                GameDirector.Instance.addPoints(points);
                spawnExplosion(other.gameObject);
                UnityEngine.Object.Destroy(other.gameObject,0.10f);
                Destroy(gameObject);
            }
        }
    }
    private void spawnExplosion(GameObject src){
                Vector3 pos = new Vector3(
                    src.transform.position.x,
                    src.transform.position.y,
                    src.transform.position.z
                );
                // TODO add score
                GameObject g = Instantiate(
                    gameControl.getExplosion(),
                    pos,
                    Quaternion.identity
                );
                 MoveForward base_c = src.gameObject.GetComponent<MoveForward>();
                 MoveForward g_c = g.GetComponent<MoveForward>();
                 g_c.speed = -1 * base_c.speed; // they are coming at us, so flip the speed
                UnityEngine.Object.Destroy(g,1.5f);
    }
}
