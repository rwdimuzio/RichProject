using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

// INHERITANCE
// parent

public class MoveForward : MonoBehaviour
{
    private float lowerZ = -10.0f;
    private float upperZ = 30;
    public enum UnitType
    {
        EasyUfo,
        MediumUfo,
        HardUfo,
        LowPowerShot,
        HighPowerShot,
        PowerupLife,
        PowerupGun
    }

    public UnitType unitType = UnitType.EasyUfo;

    // ABSTRACTION
    // these details can be used by child objects

    public int points = 100;

    public float strength = 1; // good guys the power of your punch, bad guys how much of a beating you can take

    public float speed = 1;


    private float hitsLeft = 1; // how much more you can take before you die

    public void start()
    {
        hitsLeft =  strength; // take what you can dish out
    }

    // Update is called once per frame
    public void Update()
    {
        //speed=0.25f;
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        if (transform.position.z < lowerZ || transform.position.z > upperZ)
        {
            ObjectExitsArena();
            Destroy (gameObject);
        }
    }

    public virtual void ObjectExitsArena(){

    }

    // POLYMORPHISM
    public virtual void fieryDeath()
    {
    }

    // ENCAPSULATION
    public int getPoints()
    {
        return points * 10;
    }

    // ENCAPSULATION
    public float punch()
    {
        return strength;
    }

    // ENCAPSULATION
    // reduce your hits by the amount of the punch
    // return true = you died, false = still alive
    public bool takeHit(float punch)
    {
        hitsLeft -= punch;
        return hitsLeft <= 0;
    }
}
