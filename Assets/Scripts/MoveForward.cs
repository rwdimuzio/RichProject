using System.Net.NetworkInformation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveForward : MonoBehaviour
{
    public enum UnitType
    {
        EasyUfo,
        MediumUfo,
        HardUfo,
        LowPowerShot,
        HighPowerShot,
        PowerupLife,
        PowerupGun
    };

    public UnitType unitType=UnitType.EasyUfo;

    public int points = 100;
    public float hits=1;
    public float speed=1;
    private float lowerZ=-10;
    private float upperZ=30;

    // Update is called once per frame
    public void Update()
    {
        //speed=0.25f;
        transform.Translate(Vector3.forward * Time.deltaTime * speed);    
        if(transform.position.z < lowerZ || transform.position.z > upperZ){
            Destroy(gameObject);
        } 
    }

    public virtual void fieryDeath(){
        
    }
}
